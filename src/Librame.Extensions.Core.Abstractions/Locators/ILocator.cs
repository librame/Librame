﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 定位器接口。
    /// </summary>
    /// <typeparam name="TSource">指定的源类型。</typeparam>
    public interface ILocator<TSource>
        where TSource : class
    {
        /// <summary>
        /// 原始源。
        /// </summary>
        TSource RawSource { get; }

        /// <summary>
        /// 获取源。
        /// </summary>
        /// <returns>返回源实例。</returns>
        TSource GetSource();
    }
}