﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 访问器数据构建器静态扩展。
    /// </summary>
    public static class AccessorDataBuilderExtensions
    {
        /// <summary>
        /// 添加数据库访问器（即上下文）集合。
        /// </summary>
        /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <param name="setupAction">给定的 <see cref="Action{DataBuilderOptions, DbContextOptionsBuilder}"/>。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddAccessor<TAccessor>(this IDataBuilder builder,
            Action<DataBuilderOptions, DbContextOptionsBuilder> setupAction)
            where TAccessor : DbContext, IAccessor
        {
            return builder.AddAccessor<IAccessor, TAccessor>(setupAction);
        }

        /// <summary>
        /// 添加数据库访问器（即上下文）集合。
        /// </summary>
        /// <typeparam name="TAccessorService">指定的访问器服务类型。</typeparam>
        /// <typeparam name="TAccessorImplementation">指定的访问器实现类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <param name="setupAction">给定的 <see cref="Action{DataBuilderOptions, DbContextOptionsBuilder}"/>。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddAccessor<TAccessorService, TAccessorImplementation>(this IDataBuilder builder,
            Action<DataBuilderOptions, DbContextOptionsBuilder> setupAction)
            where TAccessorService : IAccessor
            where TAccessorImplementation : DbContext, TAccessorService
        {
            setupAction.NotNull(nameof(setupAction));

            builder.Services.AddDbContext<TAccessorService, TAccessorImplementation>((serviceProvider, optionsBuilder) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<DataBuilderOptions>>().Value;
                setupAction.Invoke(options, optionsBuilder);
            });

            return builder;
        }

    }
}