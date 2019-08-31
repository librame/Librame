﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 存储中心接口。
    /// </summary>
    /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
    public interface IStoreHub<TAccessor> : IStoreHub<TAccessor, Audit, Tenant>
        where TAccessor : IAccessor
    {
    }


    /// <summary>
    /// 存储中心接口。
    /// </summary>
    /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
    /// <typeparam name="TAudit">指定的审计类型。</typeparam>
    /// <typeparam name="TTenant">指定的租户类型。</typeparam>
    public interface IStoreHub<TAccessor, TAudit, TTenant> : IStoreHub, IAuditStore<TAccessor, TAudit>, ITenantStore<TAccessor, TTenant>
        where TAccessor : IAccessor
        where TAudit : class
        where TTenant : class
    {
        /// <summary>
        /// 初始化器。
        /// </summary>
        IStoreInitializer<TAccessor> Initializer { get; }
    }


    /// <summary>
    /// 存储中心接口。
    /// </summary>
    public interface IStoreHub
    {
    }
}
