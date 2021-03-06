﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions;
using Librame.Extensions.Core;
using Librame.Extensions.Core.Builders;
using Librame.Extensions.Core.Options;
using Librame.Extensions.Data.Builders;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 数据构建器静态扩展。
    /// </summary>
    public static class DataBuilderExtensions
    {
        /// <summary>
        /// 添加数据扩展。
        /// </summary>
        /// <param name="parentBuilder">给定的父级 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="configureDependency">给定的配置依赖动作方法（可选）。</param>
        /// <param name="builderFactory">给定创建数据构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddData(this IExtensionBuilder parentBuilder,
            Action<DataBuilderDependency> configureDependency = null,
            Func<IExtensionBuilder, DataBuilderDependency, IDataBuilder> builderFactory = null)
            => parentBuilder.AddData<DataBuilderDependency>(configureDependency, builderFactory);

        /// <summary>
        /// 添加数据扩展。
        /// </summary>
        /// <typeparam name="TDependency">指定的依赖类型。</typeparam>
        /// <param name="parentBuilder">给定的父级 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="configureDependency">给定的配置依赖动作方法（可选）。</param>
        /// <param name="builderFactory">给定创建数据构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static IDataBuilder AddData<TDependency>(this IExtensionBuilder parentBuilder,
            Action<TDependency> configureDependency = null,
            Func<IExtensionBuilder, TDependency, IDataBuilder> builderFactory = null)
            where TDependency : DataBuilderDependency
        {
            // Clear Options Cache
            ConsistencyOptionsCache.TryRemove<DataBuilderOptions>();

            // Add Builder Dependency
            var dependency = parentBuilder.AddBuilderDependency(out var dependencyType, configureDependency);
            parentBuilder.Services.TryAddReferenceBuilderDependency<DataBuilderDependency>(dependency, dependencyType);

            // Add Dependencies
            if (dependency.SupportsEntityFrameworkDesignTimeServices)
            {
                parentBuilder.Services
                    .AddEntityFrameworkDesignTimeServices();

                // TypeMappingSourceDependencies 默认被重复注册导致依赖异常，须移除后重新注册
                parentBuilder.Services.TryRemoveAll<TypeMappingSourceDependencies>();
                parentBuilder.Services.AddSingleton<TypeMappingSourceDependencies>();
            }

            // Create Builder
            return builderFactory.NotNullOrDefault(()
                => (b, d) => new DataBuilder(b, d)).Invoke(parentBuilder, dependency);
        }


        /// <summary>
        /// 添加数据库设计时。
        /// </summary>
        /// <typeparam name="TDesignTime">指定的设计时类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static IDataBuilder AddDatabaseDesignTime<TDesignTime>(this IDataBuilder builder)
            where TDesignTime : class, IDesignTimeServices
        {
            builder.NotNull(nameof(builder));

            var designTimeType = typeof(TDesignTime);
            builder.SetProperty(p => p.DatabaseDesignTimeType, designTimeType);

            var designTime = designTimeType.EnsureCreate<TDesignTime>();
            designTime.ConfigureDesignTimeServices(builder.Services);

            builder.Services.Configure<DataBuilderOptions>(options =>
            {
                var reference = AssemblyDescriptor.Create(designTimeType.Assembly);
                if (!options.MigrationAssemblyReferences.Contains(reference))
                    options.MigrationAssemblyReferences.Add(reference);
            });

            return builder;
        }

    }
}
