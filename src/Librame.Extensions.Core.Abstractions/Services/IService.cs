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
using System;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 服务接口。
    /// </summary>
    public interface IService : IDisposable
    {
        /// <summary>
        /// 日志工厂。
        /// </summary>
        /// <value>返回 <see cref="ILoggerFactory"/>。</value>
        ILoggerFactory LoggerFactory { get; }
    }
}
