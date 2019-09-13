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
    using Core;

    /// <summary>
    /// 存储标识符基类。
    /// </summary>
    public class StoreIdentifierBase : AbstractStoreIdentifier
    {
        /// <summary>
        /// 构造一个 <see cref="StoreIdentifierBase"/>。
        /// </summary>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public StoreIdentifierBase(IClockService clock, ILoggerFactory loggerFactory)
            : base(clock, loggerFactory)
        {
        }

    }
}
