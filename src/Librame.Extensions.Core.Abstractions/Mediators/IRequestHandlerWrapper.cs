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
    using Services;

    /// <summary>
    /// 请求处理程序封装接口。
    /// </summary>
    /// <typeparam name="TRequest">指定的请求类型。</typeparam>
    /// <typeparam name="TResponse">指定的响应类型。</typeparam>
    public interface IRequestHandlerWrapper<in TRequest, TResponse> : IRequestHandlerWrapperIndication
        where TRequest : IRequest<TResponse>
    {
        /// <summary>
        /// 异步处理请求。
        /// </summary>
        /// <param name="request">给定的 <see cref="IRequest{TResponse}"/>。</param>
        /// <param name="serviceFactory">给定的 <see cref="ServiceFactory"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TResponse"/> 异步操作。</returns>
        Task<TResponse> HandleAsync(IRequest<TResponse> request, ServiceFactory serviceFactory,
            CancellationToken cancellationToken = default);
    }
}
