﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Aspects
{
    using Accessors;
    using Core.Services;
    using Core.Threads;
    using Stores;

    /// <summary>
    /// 数据库上下文访问器截面接口。
    /// </summary>
    /// <typeparam name="TAudit">指定的审计类型。</typeparam>
    /// <typeparam name="TAuditProperty">指定的审计属性类型。</typeparam>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <typeparam name="TMigration">指定的迁移类型。</typeparam>
    /// <typeparam name="TTenant">指定的租户类型。</typeparam>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    /// <typeparam name="TIncremId">指定的增量式标识类型。</typeparam>
    public interface IDbContextAccessorAspect<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> : ISortableService
        where TAudit : DataAudit<TGenId>
        where TAuditProperty : DataAuditProperty<TIncremId, TGenId>
        where TEntity : DataEntity<TGenId>
        where TMigration : DataMigration<TGenId>
        where TTenant : DataTenant<TGenId>
        where TGenId : IEquatable<TGenId>
        where TIncremId : IEquatable<TIncremId>
    {
        /// <summary>
        /// 时钟服务。
        /// </summary>
        IClockService Clock { get; }

        /// <summary>
        /// 标识符。
        /// </summary>
        IStoreIdentifier Identifier { get; }

        /// <summary>
        /// 锁定器。
        /// </summary>
        IMemoryLocker Locker { get; }


        /// <summary>
        /// 启用截面。
        /// </summary>
        bool Enabled { get; }


        /// <summary>
        /// 前置处理。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IDbContextAccessor{TAudit, TAuditProperty, TEntity, TMigration, TTenant}"/>。</param>
        void Preprocess(IDbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant> accessor);

        /// <summary>
        /// 异步前置处理。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IDbContextAccessor{TAudit, TAuditProperty, TEntity, TMigration, TTenant}"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个异步操作。</returns>
        Task PreprocessAsync(IDbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant> accessor, CancellationToken cancellationToken = default);


        /// <summary>
        /// 后置处理。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IDbContextAccessor{TAudit, TAuditProperty, TEntity, TMigration, TTenant}"/>。</param>
        void Postprocess(IDbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant> accessor);

        /// <summary>
        /// 异步后置处理。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IDbContextAccessor{TAudit, TAuditProperty, TEntity, TMigration, TTenant}"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个异步操作。</returns>
        Task PostprocessAsync(IDbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant> accessor, CancellationToken cancellationToken = default);
    }
}
