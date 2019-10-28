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
    /// 扩展构建器封装器接口。
    /// </summary>
    /// <typeparam name="TSource">指定的源类型。</typeparam>
    public interface IExtensionBuilderWrapper<out TSource> : IExtensionBuilder, IWrapper<TSource>
        where TSource : class
    {
    }
}
