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
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Librame.Extensions.Encryption
{
    using Core;

    /// <summary>
    /// 内部签名证书服务。
    /// </summary>
    internal class InternalSigningCredentialsService : AbstractService<InternalSigningCredentialsService, EncryptionBuilderOptions>, ISigningCredentialsService
    {
        private readonly ConcurrentDictionary<string, SigningCredentials> _credentials;


        /// <summary>
        /// 构造一个 <see cref="InternalSigningCredentialsService"/> 实例。
        /// </summary>
        /// <param name="credentials">给定的签名证书集合。</param>
        /// <param name="options">给定的 <see cref="IOptions{EncryptionBuilderOptions}"/>。</param>
        /// <param name="logger">给定的 <see cref="ILogger{InternalSigningCredentialsService}"/>。</param>
        public InternalSigningCredentialsService(IEnumerable<KeyValuePair<string, SigningCredentials>> credentials,
            IOptions<EncryptionBuilderOptions> options, ILogger<InternalSigningCredentialsService> logger)
            : base(options, logger)
        {
            _credentials = new ConcurrentDictionary<string, SigningCredentials>(credentials);
        }


        /// <summary>
        /// 获取全局签名证书。
        /// </summary>
        /// <returns>返回 <see cref="SigningCredentials"/>。</returns>
        public SigningCredentials GetGlobalSigningCredentials()
        {
            return GetSigningCredentials(EncryptionBuilderOptions.GLOBAL_KEY);
        }

        /// <summary>
        /// 获取签名证书。
        /// </summary>
        /// <param name="key">给定的键名。</param>
        /// <returns>返回 <see cref="SigningCredentials"/>。</returns>
        public SigningCredentials GetSigningCredentials(string key)
        {
            return _credentials[key];
        }

    }
}
