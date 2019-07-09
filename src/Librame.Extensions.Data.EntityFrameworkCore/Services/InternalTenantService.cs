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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 内部租户服务。
    /// </summary>
    internal class InternalTenantService : AbstractDataService, ITenantService
    {
        /// <summary>
        /// 构造一个 <see cref="InternalTenantService"/> 实例。
        /// </summary>
        /// <param name="options">给定的 <see cref="IOptions{DataBuilderOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILogger{InternalTenantService}"/>。</param>
        public InternalTenantService(IOptions<DataBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(options, loggerFactory)
        {
        }


        /// <summary>
        /// 异步获取租户。
        /// </summary>
        /// <typeparam name="TTenant">指定的租户类型。</typeparam>
        /// <param name="queryable">给定的 <see cref="IQueryable{TTenant}"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="ITenant"/> 的异步操作。</returns>
        public Task<ITenant> GetTenantAsync<TTenant>(IQueryable<TTenant> queryable, CancellationToken cancellationToken = default)
            where TTenant : ITenant
        {
            return cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
                var tenant = Options.DefaultTenant;
                Logger.LogInformation($"Get Tenant: Name={tenant?.Name}, Host={tenant.Host}");

                return tenant;
            });
        }
    }
}