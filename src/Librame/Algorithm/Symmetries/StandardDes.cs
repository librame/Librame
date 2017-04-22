﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Linq;
using System.Security.Cryptography;

namespace Librame.Algorithm.Symmetries
{
    /// <summary>
    /// 标准 DES 对称算法。
    /// </summary>
    public class StandardDes : AbstractStandardSymmetryAlgorithm
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractByteCodec"/> 实例。
        /// </summary>
        /// <param name="algoSettings">给定的 <see cref="AlgorithmSettings"/>。</param>
        public StandardDes(AlgorithmSettings algoSettings)
            : base(algoSettings)
        {
        }


        /// <summary>
        /// 生成密钥。
        /// </summary>
        /// <param name="guid">给定的全局唯一标识符（可选；默认使用 <see cref="Adaptation.AdapterSettings.AuthId"/>）。</param>
        /// <returns>返回用于当前算法的 192 位密钥字节数组。</returns>
        public override byte[] GenerateKey(string guid = null)
        {
            var key128 = GuidToKey128Bit(guid);

            // 192bit
            return key128.Concat(key128.Take(8).Reverse()).ToArray();
        }

        /// <summary>
        /// 生成向量。
        /// </summary>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回用于当前算法的 64 位向量字节数组。</returns>
        public override byte[] GenerateIV(byte[] key)
        {
            // 64bit
            return key.Take(8).Reverse().ToArray();
        }


        /// <summary>
        /// 创建 DES 引擎。
        /// </summary>
        /// <returns>返回 <see cref="SymmetricAlgorithm"/>。</returns>
        protected override SymmetricAlgorithm CreateEngine()
        {
            return DES.Create();
        }

    }
}
