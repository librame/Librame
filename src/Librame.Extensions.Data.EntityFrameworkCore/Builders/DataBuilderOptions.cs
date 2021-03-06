﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;

namespace Librame.Extensions.Data.Builders
{
    using Data.Stores;

    /// <summary>
    /// 数据构建器选项（默认使用 <see cref="Guid"/> 作为生成式标识和创建者类型）。
    /// </summary>
    public class DataBuilderOptions : DataBuilderOptions<Guid, Guid>
    {
    }


    /// <summary>
    /// 数据构建器选项。
    /// </summary>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
    public class DataBuilderOptions<TGenId, TCreatedBy> : DataBuilderOptionsBase
        where TGenId : IEquatable<TGenId>
        where TCreatedBy : IEquatable<TCreatedBy>
    {
        /// <summary>
        /// 构造一个 <see cref="DataBuilderOptions{TGenId, TCreatedBy}"/>。
        /// </summary>
        protected DataBuilderOptions()
            : base(new DataTenant<TGenId, TCreatedBy>())
        {
        }

    }
}
