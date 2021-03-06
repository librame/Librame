﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Logging;
using System.Text;

namespace Librame.Extensions.Network.Services
{
    using Core.Services;
    using Network.Builders;

    /// <summary>
    /// 网络服务基类。
    /// </summary>
    public class NetworkServiceBase : AbstractExtensionBuilderService<NetworkBuilderOptions>, INetworkService
    {
        /// <summary>
        /// 构造一个 <see cref="NetworkServiceBase"/>。
        /// </summary>
        /// <param name="serviceBase">给定的 <see cref="NetworkServiceBase"/>。</param>
        protected NetworkServiceBase(NetworkServiceBase serviceBase)
            : base(serviceBase?.Options, serviceBase?.LoggerFactory)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="NetworkServiceBase"/>。
        /// </summary>
        /// <param name="dependency">给定的 <see cref="NetworkBuilderDependency"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        protected NetworkServiceBase(NetworkBuilderDependency dependency, ILoggerFactory loggerFactory)
            : base(dependency?.Options, loggerFactory)
        {
        }


        /// <summary>
        /// 字符编码。
        /// </summary>
        public Encoding Encoding
            => ExtensionSettings.Preference.DefaultEncoding;
    }
}
