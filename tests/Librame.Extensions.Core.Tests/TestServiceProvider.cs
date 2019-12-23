﻿using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Core.Tests
{
    using Mediators;

    internal static class TestServiceProvider
    {
        static TestServiceProvider()
        {
            Current = Current.EnsureSingleton(() =>
            {
                var services = new ServiceCollection();

                services.AddLibrame();

                services.AddSingleton<IRequestHandler<Ping, Pong>, PingHandler>();
                services.AddSingleton<IRequestPreProcessor<Ping>, PingPreProcessor>();
                services.AddSingleton<IRequestPostProcessor<Ping, Pong>, PingPongPostProcessor>();

                services.AddScoped<TestInjectionService>();

                services.AddScoped<CoreDecoratorTests.TestService>();
                services.AddScoped<CoreDecoratorTests.TestServiceDecorator>();
                services.AddScoped<CoreDecoratorTests.TestImplementation>();
                services.AddScoped<CoreDecoratorTests.TestImplementationDecorator>();

                return services.BuildServiceProvider();
            });
        }


        public static IServiceProvider Current { get; }
    }
}
