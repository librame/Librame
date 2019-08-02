﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.FileProviders;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Storage
{
    using Core;

    /// <summary>
    /// 文件服务接口。
    /// </summary>
    public interface IFileService : IService
    {
        /// <summary>
        /// 异步获取内容。
        /// </summary>
        /// <param name="root">给定的根。</param>
        /// <param name="subpath">给定的子路径。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含字符串的异步操作。</returns>
        Task<string> GetContentAsync(string root, string subpath, CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步获取提供程序。
        /// </summary>
        /// <param name="root">给定的根。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="IFileProvider"/> 的异步操作。</returns>
        Task<IFileProvider> GetProviderAsync(string root, CancellationToken cancellationToken = default);
    }
}
