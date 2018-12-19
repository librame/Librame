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

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 租户标识接口。
    /// </summary>
    /// <remarks>
    /// 主要用于实体关联租户标识。
    /// </remarks>
    public interface ITenantId<TId> : ITenantId
        where TId : IEquatable<TId>
    {
        /// <summary>
        /// 租户标识。
        /// </summary>
        TId TenantId { get; set; }
    }


    /// <summary>
    /// 租户标识接口。
    /// </summary>
    /// <remarks>
    /// 主要用于实体关联租户标识的验证。
    /// </remarks>
    public interface ITenantId
    {
    }
}
