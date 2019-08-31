﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace Librame.Extensions.Encryption
{
    using Core;

    class KeyGenerator : ExtensionBuilderServiceBase<EncryptionBuilderOptions>, IKeyGenerator
    {
        private readonly UniqueIdentifier _optionsIdentifier;


        public KeyGenerator(IOptions<EncryptionBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(options, loggerFactory)
        {
            _optionsIdentifier = new UniqueIdentifier(Options.Identifier, Options.IdentifierConverter);
        }


        public IByteBuffer GenerateKey(int length, UniqueIdentifier? identifier = null)
        {
            ReadOnlyMemory<byte> memory;

            if (identifier.HasValue)
            {
                memory = identifier.Value.Memory;
                Logger.LogDebug($"Use set identifier: {identifier.Value}");
            }
            else
            {
                memory = _optionsIdentifier.Memory;
                Logger.LogDebug($"Use options identifier: {_optionsIdentifier}");
            }

            return GenerateKey(memory.ToArray(), length);
        }


        private IByteBuffer GenerateKey(byte[] bytes, int length)
        {
            var result = new byte[length];

            // 计算最大公约数
            var gcf = bytes.Length.ComputeGCD(length);

            if (Options.KeyGenerator.IsRandomKey)
            {
                // 得到最大索引长度
                var maxIndexLength = (gcf <= bytes.Length) ? bytes.Length : gcf;

                var rnd = new Random();
                for (var i = 0; i < length; i++)
                {
                    result[i] = bytes[rnd.Next(maxIndexLength)];
                }
            }
            else
            {
                for (var i = 0; i < length; i++)
                {
                    if (i >= bytes.Length)
                    {
                        var multiples = (i + 1) / bytes.Length;
                        result[i] = bytes[i + 1 - multiples * bytes.Length];
                    }
                    else
                    {
                        result[i] = bytes[i];
                    }
                }
            }
            Logger.LogDebug($"Generate key length: {length}");

            return result.AsByteBuffer();
        }

    }
}
