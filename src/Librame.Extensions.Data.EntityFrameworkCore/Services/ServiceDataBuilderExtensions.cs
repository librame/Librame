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

namespace Librame.Extensions.Data
{
    static class ServiceDataBuilderExtensions
    {
        public static IDataBuilder AddServices(this IDataBuilder builder)
        {
            builder.Services.AddScoped<IClockService, ClockService>();
            builder.Services.AddScoped<ITenantService, TenantService>();

            builder.Services.AddScoped<IAuditService, AuditServiceBase>();
            builder.Services.AddScoped<IIdentifierService, IdentifierServiceBase>();
            builder.Services.AddScoped(typeof(IInitializerService<>), typeof(InitializerServiceBase<>));
            builder.Services.AddScoped(typeof(IInitializerService<,>), typeof(InitializerServiceBase<,>));

            return builder;
        }

    }
}