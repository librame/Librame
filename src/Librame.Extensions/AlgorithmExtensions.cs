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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Librame.Extensions
{
    /// <summary>
    /// 算法静态扩展。
    /// </summary>
    public static class AlgorithmExtensions
    {
        /// <summary>
        /// 26 个小写字母。
        /// </summary>
        public const string LOWER = "abcdefghijklmnopqrstuvwxyz";

        /// <summary>
        /// 26 个大写字母。
        /// </summary>
        public const string UPPER = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// 10 个数字。
        /// </summary>
        public const string NUMBER = "0123456789";

        /// <summary>
        /// 9 个部分特殊符号。
        /// </summary>
        public const string SPECIAL = "~!@#$%^&*";

        /// <summary>
        /// 52 个大小写字母。
        /// </summary>
        public const string LETTER = LOWER + UPPER;

        /// <summary>
        /// 62 个大小写字母和数字。
        /// </summary>
        public const string LETTER_NUMBER = LETTER + NUMBER;

        /// <summary>
        /// 71 个大小写字母、数字和特殊符号。
        /// </summary>
        public const string LETTER_NUMBER_SPECIAL = LETTER + NUMBER + SPECIAL;


        /// <summary>
        /// 随机字符串字典。
        /// </summary>
        /// <param name="number">给定要生成的随机字符串个数（如 100 个）。</param>
        /// <param name="length">给定单个随机字符串的长度（可选；默认 8 位长度）。</param>
        /// <param name="encodeFactory">给定单个随机字符串的编码工厂方法（可选；默认使用 MD5 编码）。</param>
        /// <param name="hasSpecial">是否包含部分特殊符号（可选；默认不包含）。</param>
        /// <returns>返回 <see cref="Dictionary{String, String}"/>。</returns>
        public static Dictionary<string, string> RandomStrings(this int number, int length = 8,
            Func<string, string> encodeFactory = null, bool hasSpecial = false)
        {
            if (encodeFactory.IsNull())
                encodeFactory = Md5Base64String;

            var pairs = new Dictionary<string, string>();
            var chars = hasSpecial ? LETTER_NUMBER_SPECIAL : LETTER_NUMBER;
            var random = new Random((int)DateTime.Now.Ticks);

            var offset = 0;
            for (int j = 0; j < number + offset; j++)
            {
                var str = string.Empty;

                for (int i = 0; i < length; i++)
                {
                    str += chars[random.Next(chars.Length)];
                }

                if (str.IsLetter())
                {
                    offset++;
                    continue; // 如果全是字母则重新生成
                }

                if (str.IsDigit())
                {
                    offset++;
                    continue; // 如果全是数字则重新生成
                }

                if (hasSpecial && (!str.HasSpecial() || str.IsSpecial()))
                {
                    offset++;
                    continue; // 如果没有或全是特殊符号则重新生成
                }

                pairs.Add(str, encodeFactory.Invoke(str));
            }

            return pairs;
        }


        #region Base and Hex

        private static readonly string _base32Chars = UPPER + "234567";

        /// <summary>
        /// 转换 BASE32 字符串。
        /// </summary>
        /// <param name="bytes">给定的字节数组。</param>
        /// <returns>返回字符串。</returns>
        public static string AsBase32String(this byte[] bytes)
        {
            bytes.NotNullOrEmpty(nameof(bytes));

            var sb = new StringBuilder();
            for (var offset = 0; offset < bytes.Length;)
            {
                var numCharsToOutput = GetNextGroup(bytes, ref offset,
                    out byte a, out byte b, out byte c, out byte d, out byte e, out byte f, out byte g, out byte h);

                sb.Append((numCharsToOutput >= 1) ? _base32Chars[a] : '=');
                sb.Append((numCharsToOutput >= 2) ? _base32Chars[b] : '=');
                sb.Append((numCharsToOutput >= 3) ? _base32Chars[c] : '=');
                sb.Append((numCharsToOutput >= 4) ? _base32Chars[d] : '=');
                sb.Append((numCharsToOutput >= 5) ? _base32Chars[e] : '=');
                sb.Append((numCharsToOutput >= 6) ? _base32Chars[f] : '=');
                sb.Append((numCharsToOutput >= 7) ? _base32Chars[g] : '=');
                sb.Append((numCharsToOutput >= 8) ? _base32Chars[h] : '=');
            }

            return sb.ToString();

            int GetNextGroup(byte[] buffer, ref int offset,
                out byte a, out byte b, out byte c, out byte d, out byte e, out byte f, out byte g, out byte h)
            {
                uint b1, b2, b3, b4, b5;

                int retVal;
                switch (offset - buffer.Length)
                {
                    case 1: retVal = 2; break;
                    case 2: retVal = 4; break;
                    case 3: retVal = 5; break;
                    case 4: retVal = 7; break;
                    default: retVal = 8; break;
                }

                b1 = (offset < buffer.Length) ? buffer[offset++] : 0U;
                b2 = (offset < buffer.Length) ? buffer[offset++] : 0U;
                b3 = (offset < buffer.Length) ? buffer[offset++] : 0U;
                b4 = (offset < buffer.Length) ? buffer[offset++] : 0U;
                b5 = (offset < buffer.Length) ? buffer[offset++] : 0U;

                a = (byte)(b1 >> 3);
                b = (byte)(((b1 & 0x07) << 2) | (b2 >> 6));
                c = (byte)((b2 >> 1) & 0x1f);
                d = (byte)(((b2 & 0x01) << 4) | (b3 >> 4));
                e = (byte)(((b3 & 0x0f) << 1) | (b4 >> 7));
                f = (byte)((b4 >> 2) & 0x1f);
                g = (byte)(((b4 & 0x3) << 3) | (b5 >> 5));
                h = (byte)(b5 & 0x1f);

                return retVal;
            }
        }

        /// <summary>
        /// 还原 BASE32 字符串。
        /// </summary>
        /// <param name="base32String">给定的 BASE32 字符串。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] FromBase32String(this string base32String)
        {
            base32String.NotNullOrEmpty(nameof(base32String));

            base32String = base32String.TrimEnd('=');
            if (base32String.Length == 0)
                return new byte[0];

            if (base32String.HasLower())
                base32String = base32String.ToUpperInvariant();

            var bytes = new byte[base32String.Length * 5 / 8];
            var bitIndex = 0;
            var inputIndex = 0;
            var outputBits = 0;
            var outputIndex = 0;

            while (outputIndex < bytes.Length)
            {
                var byteIndex = _base32Chars.IndexOf(base32String[inputIndex]);
                if (byteIndex < 0)
                    throw new FormatException();

                var bits = Math.Min(5 - bitIndex, 8 - outputBits);
                bytes[outputIndex] <<= bits;
                bytes[outputIndex] |= (byte)(byteIndex >> (5 - (bitIndex + bits)));

                bitIndex += bits;
                if (bitIndex >= 5)
                {
                    inputIndex++;
                    bitIndex = 0;
                }

                outputBits += bits;
                if (outputBits >= 8)
                {
                    outputIndex++;
                    outputBits = 0;
                }
            }

            // 因字符串强制以“\0”结尾，故需手动移除数组末尾的“0”字节，才能正确还原源数组
            bytes = bytes.TrimEnd(byte.MinValue).ToArray();

            return bytes;
        }


        /// <summary>
        /// 转换 BASE64 字符串。
        /// </summary>
        /// <param name="bytes">给定的字节数组。</param>
        /// <returns>返回字符串。</returns>
        public static string AsBase64String(this byte[] bytes)
            => Convert.ToBase64String(bytes);

        /// <summary>
        /// 还原 BASE64 字符串。
        /// </summary>
        /// <param name="base64String">给定的 BASE64 字符串。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] FromBase64String(this string base64String)
            => Convert.FromBase64String(base64String);


        /// <summary>
        /// 转换 16 进制字符串。
        /// </summary>
        /// <param name="bytes">给定的字节数组。</param>
        /// <returns>返回字符串。</returns>
        public static string AsHexString(this byte[] bytes)
            => BitConverter.ToString(bytes).Replace("-", string.Empty);

        /// <summary>
        /// 还原 16 进制字符串。
        /// </summary>
        /// <param name="hexString">给定的 16 进制字符串。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] FromHexString(this string hexString)
        {
            hexString.NotNullOrEmpty(nameof(hexString));

            if (!hexString.Length.IsMultiples(2))
                throw new ArgumentException("Hex length must be in multiples of 2.");

            //var memory = hexString.AsMemory();

            var length = hexString.Length / 2;
            var buffer = new byte[length];

            for (int i = 0; i < length; i++)
                buffer[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16); // memory.Slice(i * 2, 2)

            return buffer;
        }

        #endregion


        #region Hash Algorithm

        /// <summary>
        /// 计算 MD5。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回字符串。</returns>
        public static string Md5Base64String(this string str)
            => str.FromEncodingString().Md5().AsBase64String();

        /// <summary>
        /// 计算 SHA1。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回字符串。</returns>
        public static string Sha1Base64String(this string str)
            => str.FromEncodingString().Sha1().AsBase64String();

        /// <summary>
        /// 计算 SHA256。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回字符串。</returns>
        public static string Sha256Base64String(this string str)
            => str.FromEncodingString().Sha256().AsBase64String();

        /// <summary>
        /// 计算 SHA384。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回字符串。</returns>
        public static string Sha384Base64String(this string str)
            => str.FromEncodingString().Sha384().AsBase64String();

        /// <summary>
        /// 计算 SHA512。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回字符串。</returns>
        public static string Sha512Base64String(this string str)
            => str.FromEncodingString().Sha512().AsBase64String();


        /// <summary>
        /// 计算 MD5。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="rsa">给定的 <see cref="RSA"/>（可选；默认不签名）。</param>
        /// <param name="padding">给定的 <see cref="RSASignaturePadding"/>（可选；如果启用签名，则默认使用 <see cref="RSASignaturePadding.Pkcs1"/>）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] Md5(this byte[] buffer, RSA rsa = null, RSASignaturePadding padding = null)
            => buffer.Hash(HashAlgorithmName.MD5, rsa, padding);

        /// <summary>
        /// 计算 SHA1。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="rsa">给定的 <see cref="RSA"/>（可选；默认不签名）。</param>
        /// <param name="padding">给定的 <see cref="RSASignaturePadding"/>（可选；如果启用签名，则默认使用 <see cref="RSASignaturePadding.Pkcs1"/>）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] Sha1(this byte[] buffer, RSA rsa = null, RSASignaturePadding padding = null)
            => buffer.Hash(HashAlgorithmName.SHA1, rsa, padding);

        /// <summary>
        /// 计算 SHA256。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="rsa">给定的 <see cref="RSA"/>（可选；默认不签名）。</param>
        /// <param name="padding">给定的 <see cref="RSASignaturePadding"/>（可选；如果启用签名，则默认使用 <see cref="RSASignaturePadding.Pkcs1"/>）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] Sha256(this byte[] buffer, RSA rsa = null, RSASignaturePadding padding = null)
            => buffer.Hash(HashAlgorithmName.SHA256, rsa, padding);

        /// <summary>
        /// 计算 SHA384。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="rsa">给定的 <see cref="RSA"/>（可选；默认不签名）。</param>
        /// <param name="padding">给定的 <see cref="RSASignaturePadding"/>（可选；如果启用签名，则默认使用 <see cref="RSASignaturePadding.Pkcs1"/>）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] Sha384(this byte[] buffer, RSA rsa = null, RSASignaturePadding padding = null)
            => buffer.Hash(HashAlgorithmName.SHA384, rsa, padding);

        /// <summary>
        /// 计算 SHA512。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="rsa">给定的 <see cref="RSA"/>（可选；默认不签名）。</param>
        /// <param name="padding">给定的 <see cref="RSASignaturePadding"/>（可选；如果启用签名，则默认使用 <see cref="RSASignaturePadding.Pkcs1"/>）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] Sha512(this byte[] buffer, RSA rsa = null, RSASignaturePadding padding = null)
            => buffer.Hash(HashAlgorithmName.SHA512, rsa, padding);


        private static ConcurrentDictionary<HashAlgorithmName, HashAlgorithm> _algorithms
            = new ConcurrentDictionary<HashAlgorithmName, HashAlgorithm>();

        private static byte[] Hash(this byte[] buffer, HashAlgorithmName algorithmName, RSA rsa = null, RSASignaturePadding padding = null)
        {
            var algorithm = _algorithms.GetOrAdd(algorithmName, name => HashAlgorithm.Create(name.Name));
            var hash = algorithm.ComputeHash(buffer);

            if (rsa.IsNotNull())
                hash = rsa.SignHash(hash, algorithmName, padding ?? RSASignaturePadding.Pkcs1);

            return hash;
        }

        #endregion


        #region HMAC Algorithm

        /// <summary>
        /// 计算 HMACMD5。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回字符串。</returns>
        public static string HmacMd5Base64String(this string str, byte[] key)
            => str.FromEncodingString().HmacMd5(key).AsBase64String();

        /// <summary>
        /// 计算 HMACSHA1。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回字符串。</returns>
        public static string HmacSha1Base64String(this string str, byte[] key)
            => str.FromEncodingString().HmacSha1(key).AsBase64String();

        /// <summary>
        /// 计算 HMACSHA256。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回字符串。</returns>
        public static string HmacSha256Base64String(this string str, byte[] key)
            => str.FromEncodingString().HmacSha256(key).AsBase64String();

        /// <summary>
        /// 计算 HMACSHA384。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回字符串。</returns>
        public static string HmacSha384Base64String(this string str, byte[] key)
            => str.FromEncodingString().HmacSha384(key).AsBase64String();

        /// <summary>
        /// 计算 HMACSHA512。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回字符串。</returns>
        public static string HmacSha512Base64String(this string str, byte[] key)
            => str.FromEncodingString().HmacSha512(key).AsBase64String();


        /// <summary>
        /// 计算 HMACMD5。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] HmacMd5(this byte[] buffer, byte[] key)
            => new HMACMD5(key).ComputeHash(buffer);

        /// <summary>
        /// 计算 HMACSHA1。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] HmacSha1(this byte[] buffer, byte[] key)
            => new HMACSHA1(key).ComputeHash(buffer);

        /// <summary>
        /// 计算 HMACSHA256。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] HmacSha256(this byte[] buffer, byte[] key)
            => new HMACSHA256(key).ComputeHash(buffer);

        /// <summary>
        /// 计算 HMACSHA384。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] HmacSha384(this byte[] buffer, byte[] key)
            => new HMACSHA384(key).ComputeHash(buffer);

        /// <summary>
        /// 计算 HMACSHA512。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] HmacSha512(this byte[] buffer, byte[] key)
            => new HMACSHA512(key).ComputeHash(buffer);

        #endregion


        #region Symmetric Algorithm

        /// <summary>
        /// 转换为 AES。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回字符串。</returns>
        public static string AsAesBase64String(this string str, byte[] key)
            => str.FromEncodingString().AsAes(key).AsBase64String();

        /// <summary>
        /// 还原 AES。
        /// </summary>
        /// <param name="base64String">给定的 BASE64 字符串。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回字符串。</returns>
        public static string FromAesBase64String(this string base64String, byte[] key)
            => base64String.FromBase64String().FromAes(key).AsEncodingString();


        /// <summary>
        /// 转换为 DES。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回字符串。</returns>
        public static string AsDesBase64String(this string str, byte[] key)
            => str.FromEncodingString().AsDes(key).AsBase64String();

        /// <summary>
        /// 还原 DES。
        /// </summary>
        /// <param name="base64String">给定的 BASE64 字符串。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回字符串。</returns>
        public static string FromDesBase64String(this string base64String, byte[] key)
            => base64String.FromBase64String().FromDes(key).AsEncodingString();


        /// <summary>
        /// 转换为 TripleDES。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回字符串。</returns>
        public static string AsTripleDesBase64String(this string str, byte[] key)
            => str.FromEncodingString().AsTripleDes(key).AsBase64String();

        /// <summary>
        /// 还原 TripleDES。
        /// </summary>
        /// <param name="base64String">给定的 BASE64 字符串。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回字符串。</returns>
        public static string FromTripleDesBase64String(this string base64String, byte[] key)
            => base64String.FromBase64String().FromTripleDes(key).AsEncodingString();


        /// <summary>
        /// 转换为 AES。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] AsAes(this byte[] buffer, byte[] key)
            => buffer.AsSymmetric(Aes.Create(), key);

        /// <summary>
        /// 还原 AES。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] FromAes(this byte[] buffer, byte[] key)
            => buffer.FromSymmetric(Aes.Create(), key);


        /// <summary>
        /// 转换为 DES。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] AsDes(this byte[] buffer, byte[] key)
            => buffer.AsSymmetric(DES.Create(), key);

        /// <summary>
        /// 还原 DES。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] FromDes(this byte[] buffer, byte[] key)
            => buffer.FromSymmetric(DES.Create(), key);


        /// <summary>
        /// 转换为 TripleDES。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] AsTripleDes(this byte[] buffer, byte[] key)
            => buffer.AsSymmetric(TripleDES.Create(), key);

        /// <summary>
        /// 还原 TripleDES。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="key">给定的密钥。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] FromTripleDes(this byte[] buffer, byte[] key)
            => buffer.FromSymmetric(TripleDES.Create(), key);


        private static byte[] AsSymmetric(this byte[] buffer, SymmetricAlgorithm algorithm, byte[] key)
        {
            algorithm.Key = key;
            algorithm.Mode = CipherMode.ECB;
            algorithm.Padding = PaddingMode.PKCS7;

            var encryptor = algorithm.CreateEncryptor();
            return encryptor.TransformFinalBlock(buffer, 0, buffer.Length);
        }

        private static byte[] FromSymmetric(this byte[] buffer, SymmetricAlgorithm algorithm, byte[] key)
        {
            algorithm.Key = key;
            algorithm.Mode = CipherMode.ECB;
            algorithm.Padding = PaddingMode.PKCS7;

            var encryptor = algorithm.CreateDecryptor();
            return encryptor.TransformFinalBlock(buffer, 0, buffer.Length);
        }

        #endregion


        #region Asymmetric Algorithm : RSA

        /// <summary>
        /// 转换为 RSA。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="parameters">给定的 <see cref="RSAParameters"/>。</param>
        /// <param name="padding">给定的 <see cref="RSAEncryptionPadding"/>（可选；默认使用 <see cref="RSAEncryptionPadding.Pkcs1"/>）。</param>
        /// <returns>返回字符串。</returns>
        public static string AsRsaBase64String(this string str, RSAParameters parameters, RSAEncryptionPadding padding = null)
            => str.FromEncodingString().AsRsa(parameters, padding).AsBase64String();

        /// <summary>
        /// 还原 RSA。
        /// </summary>
        /// <param name="base64String">给定的 BASE64 字符串。</param>
        /// <param name="parameters">给定的 <see cref="RSAParameters"/>。</param>
        /// <param name="padding">给定的 <see cref="RSAEncryptionPadding"/>（可选；默认使用 <see cref="RSAEncryptionPadding.Pkcs1"/>）。</param>
        /// <returns>返回字符串。</returns>
        public static string FromRsaBase64String(this string base64String, RSAParameters parameters, RSAEncryptionPadding padding = null)
            => base64String.FromBase64String().FromRsa(parameters, padding).AsEncodingString();


        /// <summary>
        /// 转换为 RSA。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="parameters">给定的 <see cref="RSAParameters"/>。</param>
        /// <param name="padding">给定的 <see cref="RSAEncryptionPadding"/>（可选；默认使用 <see cref="RSAEncryptionPadding.Pkcs1"/>）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] AsRsa(this byte[] buffer, RSAParameters parameters, RSAEncryptionPadding padding = null)
        {
            var rsa = RSA.Create();
            rsa.ImportParameters(parameters);

            return rsa.Encrypt(buffer, padding ?? RSAEncryptionPadding.Pkcs1);
        }

        /// <summary>
        /// 还原 RSA。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="parameters">给定的 <see cref="RSAParameters"/>。</param>
        /// <param name="padding">给定的 <see cref="RSAEncryptionPadding"/>（可选；默认使用 <see cref="RSAEncryptionPadding.Pkcs1"/>）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] FromRsa(this byte[] buffer, RSAParameters parameters, RSAEncryptionPadding padding = null)
        {
            var rsa = RSA.Create();
            rsa.ImportParameters(parameters);

            return rsa.Decrypt(buffer, padding ?? RSAEncryptionPadding.Pkcs1);
        }

        #endregion

    }
}
