﻿using global::DotNetty.Common.Internal.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Cryptography.X509Certificates;

namespace Librame.Extensions.Network.DotNetty
{
    using Core;
    using Encryption;
    using Network;

    public static class DotNettyServiceProvider
    {
        static DotNettyServiceProvider()
        {
            Current = Current.EnsureSingleton(() =>
            {
                var locator = "dotnetty.com.pfx".AsFileLocator(AppContext.BaseDirectory.CombinePath(@"..\..\..\..\..\resources"));

                var services = new ServiceCollection();

                services.AddLibrame(dependency => dependency.LoggingSetupAction = logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);

                    logging.AddConsole(logger => logger.IncludeScopes = false);
                    logging.AddFilter((str, level) => true);
                })
                .AddEncryption().AddGlobalSigningCredentials(new X509Certificate2(locator.ToString(), "password"))
                .AddNetwork().AddDotNetty();

                // Use DotNetty LoggerFactory
                services.TryReplace(InternalLoggerFactory.DefaultFactory);

                return services.BuildServiceProvider();
            });
        }


        public static IServiceProvider Current { get; }
    }
}
