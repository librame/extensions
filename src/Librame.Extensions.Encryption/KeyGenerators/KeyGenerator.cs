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

namespace Librame.Extensions.Encryption.KeyGenerators
{
    using Builders;
    using Core.Services;

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class KeyGenerator : AbstractExtensionBuilderService<EncryptionBuilderOptions>, IKeyGenerator
    {
        private readonly KeyDescriptor _defaultDescriptor;


        public KeyGenerator(IOptions<EncryptionBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(options, loggerFactory)
        {
            _defaultDescriptor = new KeyDescriptor(Options.Key);
        }


        public byte[] GenerateKey(int length, KeyDescriptor descriptor = null)
        {
            if (descriptor.IsNull())
                descriptor = _defaultDescriptor;

            var readOnlyMemory = descriptor.ToReadOnlyMemory();
            Logger.LogDebug($"Use Key: {descriptor}");

            return GenerateKey(readOnlyMemory.ToArray(), length);
        }


        private byte[] GenerateKey(byte[] initialKey, int length)
        {
            var resultKey = new byte[length];
            
            // 计算最大公约数
            var gcd = initialKey.Length.ComputeGCD(length);

            if (Options.GenerateRandomKey)
            {
                // 得到最大索引长度
                var maxIndexLength = (gcd <= initialKey.Length) ? initialKey.Length : gcd;

                var rnd = new Random();
                for (var i = 0; i < length; i++)
                {
                    resultKey[i] = initialKey[rnd.Next(maxIndexLength)];
                }
            }
            else
            {
                for (var i = 0; i < length; i++)
                {
                    if (i >= initialKey.Length)
                    {
                        var multiples = (i + 1) / initialKey.Length;
                        resultKey[i] = initialKey[i + 1 - multiples * initialKey.Length];
                    }
                    else
                    {
                        resultKey[i] = initialKey[i];
                    }
                }
            }
            Logger.LogDebug($"Generate key length: {length}");

            return resultKey;
        }

    }
}
