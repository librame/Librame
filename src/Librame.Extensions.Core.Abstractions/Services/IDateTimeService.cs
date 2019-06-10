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

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 日期与时间服务接口。
    /// </summary>
    public interface IDateTimeService : IService
    {
        /// <summary>
        /// 异步人性化。
        /// </summary>
        /// <param name="dateTime">给定的 <see cref="DateTime"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含字符串的异步操作。</returns>
        Task<string> HumanizeAsync(DateTime dateTime, CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步人性化。
        /// </summary>
        /// <param name="dateTimeOffset">给定的 <see cref="DateTimeOffset"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含字符串的异步操作。</returns>
        Task<string> HumanizeAsync(DateTimeOffset dateTimeOffset, CancellationToken cancellationToken = default);
    }
}
