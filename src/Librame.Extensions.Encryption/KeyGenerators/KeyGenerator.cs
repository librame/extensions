#region License

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
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Encryption
{
    using Core;

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class KeyGenerator : AbstractExtensionBuilderService<EncryptionBuilderOptions>, IKeyGenerator
    {
        private readonly IAlgorithmIdentifier _optionsIdentifier;


        public KeyGenerator(IOptions<EncryptionBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(options, loggerFactory)
        {
            _optionsIdentifier = Options.Identifier.FromHashString();
        }


        public IByteMemoryBuffer GenerateKey(int length, IAlgorithmIdentifier identifier = null)
        {
            ReadOnlyMemory<byte> memory;

            if (identifier.IsNotNull())
            {
                memory = identifier.Memory;
                Logger.LogDebug($"Use set identifier: {identifier}");
            }
            else
            {
                memory = _optionsIdentifier.Memory;
                Logger.LogDebug($"Use options identifier: {_optionsIdentifier}");
            }

            return GenerateKey(memory.ToArray(), length);
        }


        private IByteMemoryBuffer GenerateKey(byte[] bytes, int length)
        {
            var result = new byte[length];

            // 计算最大公约数
            var gcf = bytes.Length.ComputeGCD(length);

            if (Options.GenerateRandomKey)
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

            return new ByteMemoryBuffer(result);
        }

    }
}
