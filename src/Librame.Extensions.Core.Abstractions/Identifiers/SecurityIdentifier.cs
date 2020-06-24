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
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Core.Identifiers
{
    using Combiners;

    /// <summary>
    /// 安全标识符。
    /// </summary>
    public class SecurityIdentifier
    {
        private readonly byte[] _buffer;


        /// <summary>
        /// 构造一个 <see cref="SecurityIdentifier"/>。
        /// </summary>
        /// <param name="identifier">给定的标识符字符串。</param>
        public SecurityIdentifier(string identifier)
        {
            if (!TryParseIdentifier(identifier, out var buffer))
                throw new ArgumentException($"The identifier '{identifier}' is not a valid security identifier.");

            _buffer = buffer;
        }

        /// <summary>
        /// 构造一个 <see cref="SecurityIdentifier"/>。
        /// </summary>
        /// <param name="g">给定的 <see cref="Guid"/>。</param>
        public SecurityIdentifier(Guid g)
            : this(g.ToByteArray())
        {
        }

        private SecurityIdentifier(byte[] buffer)
        {
            _buffer = buffer;
        }


        /// <summary>
        /// 转换为全局唯一标识符。
        /// </summary>
        /// <returns>返回 <see cref="Guid"/>。</returns>
        public Guid ToGuid()
            => new Guid(_buffer);


        /// <summary>
        /// 转换为只读内存字节。
        /// </summary>
        /// <returns>返回 <see cref="ReadOnlyMemory{Byte}"/>。</returns>
        public ReadOnlyMemory<byte> ToReadOnlyMemory()
            => _buffer;


        /// <summary>
        /// 转换为长度为 15 的短字符串（不可解析还原，可当作标识）。
        /// </summary>
        /// <param name="timestamp">给定的 <see cref="DateTime"/>。</param>
        /// <returns>返回字符串。</returns>
        public string ToShortString(DateTime timestamp)
            => _buffer.FormatString(timestamp.Ticks);

        /// <summary>
        /// 转换为长度为 15 的短字符串（不可解析还原，可当作标识）。
        /// </summary>
        /// <param name="timestamp">给定的 <see cref="DateTimeOffset"/>。</param>
        /// <returns>返回字符串。</returns>
        public string ToShortString(DateTimeOffset timestamp)
            => _buffer.FormatString(timestamp.Ticks);


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="other">给定的 <see cref="SecurityIdentifier"/>。</param>
        /// <returns>返回布尔值。</returns>
        public bool Equals(SecurityIdentifier other)
            => ToString() == other?.ToString();

        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
            => obj is SecurityIdentifier other && Equals(other);


        /// <summary>
        /// 定义比较相等静态方法需强制重写此方法。
        /// </summary>
        /// <returns>返回 32 位带符号整数。</returns>
        public override int GetHashCode()
            => ToString().CompatibleGetHashCode();


        /// <summary>
        /// 转换为 BASE64 字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
        {
            return new SignedTokenCombiner(new string[]
            {
                CoreSettings.Preference.SecurityIdentifierConverter.ConvertTo(_buffer)
            },
            s => s.Sha256HexString());
        }

        [SuppressMessage("Design", "CA1031:不捕获常规异常类型")]
        private static bool TryParseIdentifier(string identifier, out byte[] buffer)
        {
            try
            {
                var signedToken = new SignedTokenCombiner(identifier, s => s.Sha256HexString());
                buffer = CoreSettings.Preference.SecurityIdentifierConverter.ConvertFrom(signedToken.DataSegments[0]);
                return true;
            }
            catch (Exception)
            {
                buffer = null;
                return false;
            }
        }


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="a">给定的 <see cref="SecurityIdentifier"/>。</param>
        /// <param name="b">给定的 <see cref="SecurityIdentifier"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator ==(SecurityIdentifier a, SecurityIdentifier b)
            => (a?.Equals(b)).Value;

        /// <summary>
        /// 是否不等。
        /// </summary>
        /// <param name="a">给定的 <see cref="SecurityIdentifier"/>。</param>
        /// <param name="b">给定的 <see cref="SecurityIdentifier"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator !=(SecurityIdentifier a, SecurityIdentifier b)
            => !(a?.Equals(b)).Value;


        /// <summary>
        /// 隐式转换为全局唯一标识符。
        /// </summary>
        /// <param name="identifier">给定的 <see cref="SecurityIdentifier"/>。</param>
        public static implicit operator Guid(SecurityIdentifier identifier)
            => identifier.NotNull(nameof(identifier)).ToGuid();

        /// <summary>
        /// 隐式转换为字符串形式。
        /// </summary>
        /// <param name="identifier">给定的 <see cref="SecurityIdentifier"/>。</param>
        public static implicit operator string(SecurityIdentifier identifier)
            => identifier?.ToString();


        /// <summary>
        /// 空安全标识符。
        /// </summary>
        public static readonly SecurityIdentifier Empty
            = new SecurityIdentifier(Guid.Empty);


        /// <summary>
        /// 新建安全标识符。
        /// </summary>
        /// <returns>返回 <see cref="SecurityIdentifier"/>。</returns>
        public static SecurityIdentifier New()
            => new SecurityIdentifier(Guid.NewGuid());

        /// <summary>
        /// 新建安全标识符数组。
        /// </summary>
        /// <param name="count">给定要生成的实例数量。</param>
        /// <returns>返回 <see cref="SecurityIdentifier"/> 数组。</returns>
        public static SecurityIdentifier[] NewArray(int count)
        {
            var identifiers = new SecurityIdentifier[count];
            for (var i = 0; i < count; i++)
                identifiers[i] = New();

            return identifiers;
        }


        /// <summary>
        /// 尝试获取安全标识符。
        /// </summary>
        /// <param name="identifier">给定的标识符字符串。</param>
        /// <param name="result">输出 <see cref="SecurityIdentifier"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool TryGetIdentifier(string identifier,
            out SecurityIdentifier result)
        {
            if (TryParseIdentifier(identifier, out var buffer))
            {
                result = new SecurityIdentifier(buffer);
                return true;
            }

            result = null;
            return false;
        }

    }
}
