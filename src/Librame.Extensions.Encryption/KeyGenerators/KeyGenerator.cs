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
    using Core.Identifiers;
    using Core.Services;

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class KeyGenerator : AbstractExtensionBuilderService<EncryptionBuilderOptions>, IKeyGenerator
    {
        private readonly IAlgorithmIdentifier _defaultIdentifier;


        public KeyGenerator(IOptions<EncryptionBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(options, loggerFactory)
        {
            _defaultIdentifier = new UniqueAlgorithmIdentifier(Options.Identifier);
        }


        public byte[] GenerateKey(int length, IAlgorithmIdentifier identifier = null)
        {
            var readOnlyMemory = (identifier ?? _defaultIdentifier).ReadOnlyMemory;
            Logger.LogDebug($"Use identifier: {readOnlyMemory.Value}");

            return GenerateKey(readOnlyMemory.Source.ToArray(), length);
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
