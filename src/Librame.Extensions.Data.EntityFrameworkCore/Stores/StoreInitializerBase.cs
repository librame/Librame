﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Logging;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 存储初始化器基类。
    /// </summary>
    /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
    /// <typeparam name="TIdentifier">指定的标识符类型。</typeparam>
    public class StoreInitializerBase<TAccessor, TIdentifier> : StoreInitializerBase<TAccessor>, IStoreInitializer<TAccessor, TIdentifier>
        where TAccessor : DbContextAccessor
        where TIdentifier : IStoreIdentifier
    {
        /// <summary>
        /// 构造一个 <see cref="StoreInitializerBase{TAccessor, TIdentifier}"/>。
        /// </summary>
        /// <param name="identifier">给定的 <see cref="IAccessor"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public StoreInitializerBase(IStoreIdentifier identifier, ILoggerFactory loggerFactory)
            : base(identifier, loggerFactory)
        {
            // Cast
            Identifier = identifier.CastTo<IStoreIdentifier, TIdentifier>(nameof(identifier));
        }

        /// <summary>
        /// 构造一个 <see cref="StoreInitializerBase{TAccessor, TIdentifier}"/>。
        /// </summary>
        /// <param name="identifier">给定的 <typeparamref name="TAccessor"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public StoreInitializerBase(TIdentifier identifier, ILoggerFactory loggerFactory)
            : base(identifier, loggerFactory)
        {
            // Override
            Identifier = identifier;
        }


        /// <summary>
        /// 覆盖标识符接口实例。
        /// </summary>
        /// <value>返回 <typeparamref name="TAccessor"/>。</value>
        public new TIdentifier Identifier { get; }
    }


    /// <summary>
    /// 存储初始化器基类。
    /// </summary>
    /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
    public class StoreInitializerBase<TAccessor> : AbstractStoreInitializer<TAccessor>
        where TAccessor : DbContextAccessor
    {
        /// <summary>
        /// 构造一个 <see cref="StoreInitializerBase{TAccessor}"/>。
        /// </summary>
        /// <param name="identifier">给定的 <see cref="IStoreIdentifier"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public StoreInitializerBase(IStoreIdentifier identifier, ILoggerFactory loggerFactory)
            : base(identifier, loggerFactory)
        {
        }


        /// <summary>
        /// 初始化核心。
        /// </summary>
        /// <param name="stores">给定的 <see cref="IStoreHub{TAccessor}"/>。</param>
        protected override void InitializeCore(IStoreHub<TAccessor> stores)
        {
            InitializeTenants(stores);
        }

        /// <summary>
        /// 初始化租户集合。
        /// </summary>
        /// <param name="stores">给定的 <see cref="IStoreHub{TAccessor}"/>。</param>
        protected virtual void InitializeTenants(IStoreHub<TAccessor> stores)
        {
            // 初始化默认租户
            var defaultTenant = stores.Accessor.BuilderOptions.Tenants.Default;
            if (defaultTenant.IsNotNull()
                && !stores.ContainTenantAsync(defaultTenant.Name, defaultTenant.Host).Result)
            {
                Tenant tenant;

                // 添加默认租户到数据库
                if (defaultTenant is Tenant _tenant)
                {
                    tenant = _tenant;
                }
                else
                {
                    tenant = new Tenant();
                    defaultTenant.EnsurePopulate(tenant);
                }

                tenant.Id = Identifier.GetTenantIdAsync().Result;

                stores.TryCreateAsync(default, tenant).Wait();
                Logger.LogTrace($"Add default tenant (name={tenant.Name}, host={tenant.Host}) to database.");
            }
        }

    }
}