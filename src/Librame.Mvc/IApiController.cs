﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Data;

namespace System.Web.Http
{
    /// <summary>
    /// API 控制器接口。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    public interface IApiController<T>
        where T : class
    {
        /// <summary>
        /// 数据仓库。
        /// </summary>
        IRepository<T> Repository { get; }
    }
}
