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
using System.Linq;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 抽象算法标识符。
    /// </summary>
    public abstract class AbstractAlgorithmIdentifier : IAlgorithmIdentifier
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractAlgorithmIdentifier"/>。
        /// </summary>
        /// <param name="memory">给定的 <see cref="ReadOnlyMemory{Byte}"/>。</param>
        /// <param name="converter">给定的 <see cref="IAlgorithmConverter"/>。</param>
        public AbstractAlgorithmIdentifier(ReadOnlyMemory<byte> memory, IAlgorithmConverter converter)
        {
            Memory = memory;
            Converter = converter.NotNull(nameof(converter));
        }

        /// <summary>
        /// 构造一个 <see cref="AbstractAlgorithmIdentifier"/>。
        /// </summary>
        /// <param name="identifier">给定的算法字符串。</param>
        /// <param name="converter">给定的 <see cref="IAlgorithmConverter"/>。</param>
        public AbstractAlgorithmIdentifier(string identifier, IAlgorithmConverter converter)
        {
            Converter = converter.NotNull(nameof(converter));
            Memory = converter.From(identifier);
        }


        /// <summary>
        /// 只读的连续内存区域。
        /// </summary>
        public ReadOnlyMemory<byte> Memory { get; }

        /// <summary>
        /// 算法转换器。
        /// </summary>
        public IAlgorithmConverter Converter { get; }


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="other">给定的 <see cref="IAlgorithmIdentifier"/>。</param>
        /// <returns>返回布尔值。</returns>
        public virtual bool Equals(IAlgorithmIdentifier other)
            => Memory.ToArray().SequenceEqual(other?.Memory.ToArray());

        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
            => (obj is IAlgorithmIdentifier other) ? Equals(other) : false;


        /// <summary>
        /// 定义比较相等静态方法需强制重写此方法。
        /// </summary>
        /// <returns>返回 32 位带符号整数。</returns>
        public override int GetHashCode()
            => ToString().GetHashCode();


        /// <summary>
        /// 转换为 BASE64 字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
            => Converter.To(Memory);


        /// <summary>
        /// 转换为短字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public virtual string ToShortString()
        {
            var i = 1L;
            foreach (var b in Memory.ToArray())
                i *= b + 1;

            // 8d7225f69933e15
            return string.Format("{0:x}", _ = DateTimeOffset.UtcNow.Ticks);
        }


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="a">给定的 <see cref="AbstractAlgorithmIdentifier"/>。</param>
        /// <param name="b">给定的 <see cref="AbstractAlgorithmIdentifier"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator ==(AbstractAlgorithmIdentifier a, AbstractAlgorithmIdentifier b)
            => a.Equals(b);

        /// <summary>
        /// 是否不等。
        /// </summary>
        /// <param name="a">给定的 <see cref="AbstractAlgorithmIdentifier"/>。</param>
        /// <param name="b">给定的 <see cref="AbstractAlgorithmIdentifier"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator !=(AbstractAlgorithmIdentifier a, AbstractAlgorithmIdentifier b)
            => !a.Equals(b);
    }
}
