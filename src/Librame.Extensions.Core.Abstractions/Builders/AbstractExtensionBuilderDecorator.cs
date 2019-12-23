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

namespace Librame.Extensions.Core.Builders
{
    /// <summary>
    /// 抽象扩展构建器装饰器。
    /// </summary>
    /// <typeparam name="TSource">指定的来源类型。</typeparam>
    public abstract class AbstractExtensionBuilderDecorator<TSource> : AbstractExtensionBuilder, IExtensionBuilderDecorator<TSource>
        where TSource : class
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractExtensionBuilder"/>。
        /// </summary>
        /// <param name="source">给定的装饰 <typeparamref name="TSource"/>。</param>
        /// <param name="parentBuilder">给定的父级 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="dependency">给定的 <see cref="IExtensionBuilderDependency"/>。</param>
        protected AbstractExtensionBuilderDecorator(TSource source,
            IExtensionBuilder parentBuilder, IExtensionBuilderDependency dependency)
            : base(parentBuilder, dependency)
        {
            Source = source.NotNull(nameof(source));
        }

        /// <summary>
        /// 构造一个 <see cref="AbstractExtensionBuilder"/>。
        /// </summary>
        /// <param name="source">给定的装饰 <typeparamref name="TSource"/>。</param>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <param name="dependency">给定的 <see cref="IExtensionBuilderDependency"/>。</param>
        protected AbstractExtensionBuilderDecorator(TSource source,
            IServiceCollection services, IExtensionBuilderDependency dependency)
            : base(services, dependency)
        {
            Source = source.NotNull(nameof(source));
        }


        /// <summary>
        /// 源构建器。
        /// </summary>
        public TSource Source { get; }
    }
}
