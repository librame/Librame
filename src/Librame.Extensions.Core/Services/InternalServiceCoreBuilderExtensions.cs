﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.DependencyInjection;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 内部服务核心构建器静态扩展。
    /// </summary>
    internal static class InternalServiceCoreBuilderExtensions
    {
        /// <summary>
        /// 添加服务集合扩展。
        /// </summary>
        /// <param name="builder">给定的 <see cref="ICoreBuilder"/>。</param>
        /// <returns>返回 <see cref="ICoreBuilder"/>。</returns>
        public static ICoreBuilder AddServices(this ICoreBuilder builder)
        {
            builder.Services.AddSingleton(typeof(IServicesManager<>), typeof(InternalServicesManager<>));
            builder.Services.AddSingleton(typeof(IServicesManager<,>), typeof(InternalServicesManager<,>));

            builder.Services.AddScoped<IHumanizationService, InternalHumanizationService>();
            builder.Services.AddScoped<IInjectionService, InternalInjectionService>();
            builder.Services.AddScoped<IPlatformService, InternalPlatformService>();

            return builder;
        }

    }
}
