﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Core.Mediators
{
    /// <summary>
    /// 通知处理程序接口。
    /// </summary>
    /// <typeparam name="TNotification">指定的通知类型。</typeparam>
    public interface INotificationHandler<in TNotification>
        where TNotification : INotificationIndication
    {
        /// <summary>
        /// 异步处理通知。
        /// </summary>
        /// <param name="notification">给定的通知。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个异步操作。</returns>
        Task HandleAsync(TNotification notification, CancellationToken cancellationToken = default);
    }
}
