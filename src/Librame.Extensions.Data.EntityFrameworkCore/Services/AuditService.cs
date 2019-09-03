﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data
{
    using Core;

    /// <summary>
    /// 审计服务。
    /// </summary>
    public class AuditService : AbstractService, IAuditService
    {
        /// <summary>
        /// 构造一个 <see cref="AuditService"/>。
        /// </summary>
        /// <param name="options">给定的 <see cref="IOptions{DataBuilderOptions}"/>。</param>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="identifier">给定的 <see cref="IStoreIdentifier"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public AuditService(IOptions<DataBuilderOptions> options, IClockService clock,
            IStoreIdentifier identifier, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Options = options.NotNull(nameof(options)).Value;
            Clock = clock.NotNull(nameof(clock));
            Identifier = identifier.NotNull(nameof(identifier));
        }


        /// <summary>
        /// 构建器选项。
        /// </summary>
        protected DataBuilderOptions Options { get; }

        /// <summary>
        /// 时钟服务。
        /// </summary>
        protected IClockService Clock { get; }

        /// <summary>
        /// 标识符服务。
        /// </summary>
        protected IStoreIdentifier Identifier { get; }


        /// <summary>
        /// 异步审计。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="List{Audit}"/> 的异步操作。</returns>
        public virtual async Task<List<Audit>> AuditAsync(IAccessor accessor,
            CancellationToken cancellationToken = default)
        {
            if (accessor.IsNotNull() && accessor is DbContextAccessor dbContextAccessor)
            {
                var audits = GetAudits(dbContextAccessor.ChangeTracker, cancellationToken);

                await ProcessAuditsAsync(dbContextAccessor, audits, cancellationToken);

                return audits;
            }

            return null;
        }

        /// <summary>
        /// 异步处理审计。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="DbContextAccessor"/>。</param>
        /// <param name="audits">给定要处理的审计列表。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个异步操作。</returns>
        protected virtual async Task ProcessAuditsAsync(DbContextAccessor accessor, List<Audit> audits,
            CancellationToken cancellationToken = default)
        {
            await accessor.Audits.AddRangeAsync(audits, cancellationToken);

            var mediator = accessor.ServiceFactory.GetRequiredService<IMediator>();
            await mediator.Publish(new AuditNotification { Audits = audits });
        }


        #region GetAudits

        /// <summary>
        /// 异步获取审计。
        /// </summary>
        /// <param name="changeTracker">给定的 <see cref="ChangeTracker"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回 <see cref="List{Audit}"/>。</returns>
        protected virtual List<Audit> GetAudits(ChangeTracker changeTracker, CancellationToken cancellationToken)
        {
            var entityStates = Options.Audits.AuditEntityStates;

            // 得到变化的实体集合
            var entityEntries = changeTracker.Entries()
                .Where(m => m.Entity.IsNotNull() && entityStates.Contains(m.State)).ToList();

            var audits = new List<Audit>();

            if (entityEntries.IsNullOrEmpty())
                return audits;

            foreach (var entry in entityEntries)
            {
                if (entry.Metadata.ClrType.IsDefined<NotAuditedAttribute>())
                    continue; // 如果不审计，则忽略

                var audit = ToAudit(entry, cancellationToken);
                audits.Add(audit);
            }

            return audits;
        }

        /// <summary>
        /// 转换为审计。
        /// </summary>
        /// <param name="entry">给定的 <see cref="EntityEntry"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回审计。</returns>
        protected virtual Audit ToAudit(EntityEntry entry, CancellationToken cancellationToken)
        {
            var audit = new Audit
            {
                Id = Identifier.GetAuditIdAsync(cancellationToken).Result,
                TableName = GetTableName(entry),
                EntityTypeName = entry.Metadata.ClrType.GetCustomFullName(),
                State = (int)entry.State,
                StateName = entry.State.ToString()
            };

            foreach (var property in entry.CurrentValues.Properties)
            {
                if (property.IsConcurrencyToken)
                    continue;

                if (property.IsPrimaryKey())
                    audit.EntityId = GetEntityId(entry.Property(property.Name));

                var auditProperty = new AuditProperty()
                {
                    Id = Identifier.GetAuditPropertyIdAsync(cancellationToken).Result,
                    PropertyName = property.Name,
                    PropertyTypeName = property.ClrType.GetCustomFullName()
                };

                switch (entry.State)
                {
                    case EntityState.Added:
                        auditProperty.NewValue = entry.Property(property.Name).CurrentValue?.ToString();
                        break;

                    case EntityState.Deleted:
                        auditProperty.OldValue = entry.Property(property.Name).OriginalValue?.ToString();
                        break;

                    case EntityState.Modified:
                        {
                            var currentValue = entry.Property(property.Name).CurrentValue?.ToString();
                            var originalValue = entry.Property(property.Name).OriginalValue?.ToString();

                            if (currentValue != originalValue)
                            {
                                auditProperty.NewValue = currentValue;
                                auditProperty.OldValue = originalValue;
                            }
                        }
                        break;
                }

                audit.Properties.Add(auditProperty);
            }

            if (entry.State == EntityState.Modified && entry.Entity is IUpdation updation)
            {
                audit.CreatedTime = ToDateTime(updation.GetUpdatedTime(), cancellationToken);
                audit.CreatedBy = ToBy(updation.GetUpdatedBy());
            }
            else if (entry.Entity is ICreation creation)
            {
                audit.CreatedTime = ToDateTime(creation.GetCreatedTime(), cancellationToken);
                audit.CreatedBy = ToBy(creation.GetCreatedBy());
            }
            else
            {
                audit.CreatedTime = Clock.GetUtcNowAsync(cancellationToken).Result;
            }

            return audit;
        }

        /// <summary>
        /// 获取表名。
        /// </summary>
        /// <param name="entry">给定的 <see cref="EntityEntry"/>。</param>
        /// <returns>返回字符串。</returns>
        protected virtual string GetTableName(EntityEntry entry)
        {
            var relational = entry.Metadata?.Relational();

            return new TableSchema(relational?.TableName, relational?.Schema).ToString();
        }

        /// <summary>
        /// 获取实体标识。
        /// </summary>
        /// <param name="property">给定的 <see cref="PropertyEntry"/>。</param>
        /// <returns>返回字符串。</returns>
        protected virtual string GetEntityId(PropertyEntry property)
        {
            if (property.EntityEntry.State == EntityState.Deleted)
                return property.OriginalValue?.ToString();

            return property.CurrentValue?.ToString();
        }

        /// <summary>
        /// 转换为日期时间格式。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回 <see cref="DateTimeOffset"/>。</returns>
        protected virtual DateTimeOffset ToDateTime(object obj, CancellationToken cancellationToken)
        {
            if (obj.IsNull())
                return Clock.GetUtcNowAsync(cancellationToken).Result;

            if (obj is DateTimeOffset dateTimeOffset)
                return dateTimeOffset;

            if (obj is DateTime dateTime)
                return new DateTimeOffset(dateTime);

            return DateTimeOffset.Parse(obj.ToString());
        }

        /// <summary>
        /// 转换为人员内容。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回字符串。</returns>
        protected virtual string ToBy(object obj)
        {
            if (obj is string str)
                return str;

            return obj?.ToString();
        }

        #endregion

    }
}