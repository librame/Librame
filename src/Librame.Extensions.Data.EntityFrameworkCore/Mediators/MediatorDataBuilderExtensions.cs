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
    using Core;

    static class MediatorDataBuilderExtensions
    {
        public static IDataBuilder AddMediators(this IDataBuilder builder)
        {
            builder.Services.AddTransient<INotificationHandler<DataAuditNotification>, DataAuditNotificationHandler>();
            builder.Services.AddTransient<INotificationHandler<DataEntityNotification>, DataEntityNotificationHandler>();
            builder.Services.AddTransient<INotificationHandler<DataMigrationNotification>, DataMigrationNotificationHandler>();

            return builder;
        }

    }
}
