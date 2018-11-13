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

namespace Librame.Extensions
{
    using Encryption;

    /// <summary>
    /// 加密缓冲区静态扩展。
    /// </summary>
    public static class EncryptionBufferExtensions
    {
        
        #region ICiphertextAlgorithmBuffer

        /// <summary>
        /// 转换为内部密文算法缓冲区。
        /// </summary>
        /// <param name="str">给定的密文字符串。</param>
        /// <param name="serviceProvider">给定的 <see cref="IServiceProvider"/>。</param>
        /// <returns>返回算法缓冲区。</returns>
        public static IEncryptionBuffer<ICiphertextAlgorithmConverter, string> AsCiphertextBuffer(this string str,
            IServiceProvider serviceProvider)
        {
            var converter = serviceProvider.GetRequiredService<ICiphertextAlgorithmConverter>();

            return str.AsCiphertextBuffer(converter).ApplyServiceProvider(serviceProvider);
        }

        /// <summary>
        /// 转换为内部密文算法缓冲区。
        /// </summary>
        /// <param name="str">给定的密文字符串。</param>
        /// <param name="converter">给定的 <see cref="ICiphertextAlgorithmConverter"/>。</param>
        /// <returns>返回算法缓冲区。</returns>
        public static IEncryptionBuffer<ICiphertextAlgorithmConverter, string> AsCiphertextBuffer(this string str,
            ICiphertextAlgorithmConverter converter)
        {
            return converter.AsEncryptionBuffer(str);
        }

        #endregion


        #region IPlaintextAlgorithmBuffer

        /// <summary>
        /// 转换为内部明文算法缓冲区。
        /// </summary>
        /// <param name="str">给定的明文字符串。</param>
        /// <param name="serviceProvider">给定的 <see cref="IServiceProvider"/>。</param>
        /// <returns>返回算法缓冲区。</returns>
        public static IEncryptionBuffer<IPlaintextAlgorithmConverter, string> AsPlaintextBuffer(this string str,
            IServiceProvider serviceProvider)
        {
            var converter = serviceProvider.GetRequiredService<IPlaintextAlgorithmConverter>();

            return str.AsPlaintextBuffer(converter).ApplyServiceProvider(serviceProvider);
        }

        /// <summary>
        /// 转换为内部明文算法缓冲区。
        /// </summary>
        /// <param name="str">给定的明文字符串。</param>
        /// <param name="converter">给定的 <see cref="IPlaintextAlgorithmConverter"/>。</param>
        /// <returns>返回算法缓冲区。</returns>
        public static IEncryptionBuffer<IPlaintextAlgorithmConverter, string> AsPlaintextBuffer(this string str,
            IPlaintextAlgorithmConverter converter)
        {
            return converter.AsEncryptionBuffer(str);
        }

        #endregion


        /// <summary>
        /// 转换为内部加密缓冲区。
        /// </summary>
        /// <typeparam name="TConverter">指定的转换器类型。</typeparam>
        /// <typeparam name="TSource">指定的来源类型。</typeparam>
        /// <param name="converter">给定的转换器。</param>
        /// <param name="source">给定的来源实例。</param>
        /// <returns>返回 <see cref="IEncryptionBuffer{TConverter, TSource}"/>。</returns>
        public static IEncryptionBuffer<TConverter, TSource> AsEncryptionBuffer<TConverter, TSource>(this TConverter converter, TSource source)
            where TConverter : IAlgorithmConverter<TSource>
        {
            return new InternalEncryptionBuffer<TConverter, TSource>(converter, source);
        }

    }
}