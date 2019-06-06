﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 缓冲区。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    public class Buffer<T> : AbstractBuffer<T>
    {
        /// <summary>
        /// 构造一个 <see cref="Buffer{T}"/> 实例。
        /// </summary>
        /// <param name="memory">给定的 <see cref="Memory{T}"/>。</param>
        public Buffer(Memory<T> memory)
            : base(memory)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="Buffer{T}"/> 实例。
        /// </summary>
        /// <param name="array">给定的数组。</param>
        public Buffer(T[] array)
            : base(array)
        {
        }


        /// <summary>
        /// 创建副本。
        /// </summary>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        public override IBuffer<T> Copy()
        {
            return new Buffer<T>(Memory);
        }

    }
}