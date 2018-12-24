﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions;
using Librame.Extensions.Data;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// 实体类型构建器静态扩展。
    /// </summary>
    public static class DataEntityTypeBuilderExtensions
    {

        /// <summary>
        /// 映射表名。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="entityTypeBuilder">给定的 <see cref="EntityTypeBuilder{TEntity}"/>。</param>
        /// <param name="table">给定的 <see cref="ITableSchema"/>。</param>
        /// <returns>返回 <see cref="EntityTypeBuilder{TEntity}"/>。</returns>
        public static EntityTypeBuilder<TEntity> ToTable<TEntity>(this EntityTypeBuilder<TEntity> entityTypeBuilder, ITableSchema table)
            where TEntity : class
        {
            table.NotDefault(nameof(table));

            if (table.Schema.IsNotEmpty())
                return entityTypeBuilder.ToTable(table.Name);

            return entityTypeBuilder.ToTable(table.Name, table.Schema);
        }

    }
}
