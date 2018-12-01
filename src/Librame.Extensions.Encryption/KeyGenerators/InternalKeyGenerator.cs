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
    using Buffers;
    using Builders;
    using Services;

    /// <summary>
    /// 内部密钥生成器。
    /// </summary>
    internal class InternalKeyGenerator : AbstractService<InternalKeyGenerator, EncryptionBuilderOptions>, IKeyGenerator
    {
        private readonly AlgorithmIdentifier _optionIdentifier;


        /// <summary>
        /// 构造一个 <see cref="InternalKeyGenerator"/> 实例。
        /// </summary>
        /// <param name="builderOptions">给定的 <see cref="IOptions{EncryptionBuilderOptions}"/>。</param>
        /// <param name="logger">给定的 <see cref="ILogger{InternalKeyGenerator}"/>。</param>
        public InternalKeyGenerator(IOptions<EncryptionBuilderOptions> builderOptions, ILogger<InternalKeyGenerator> logger)
            : base(builderOptions, logger)
        {
            _optionIdentifier = AlgorithmIdentifier.Parse(BuilderOptions.Identifier);
        }


        /// <summary>
        /// 生成密钥。
        /// </summary>
        /// <param name="length">给定要生成的密钥长度。</param>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回 <see cref="IByteBuffer"/>。</returns>
        public IByteBuffer GenerateKey(int length, string identifier = null)
        {
            Memory<byte> memory;

            if (identifier.IsNotEmpty())
            {
                memory = AlgorithmIdentifier.Parse(identifier).Memory;
                Logger.LogDebug($"Use set identifier: {identifier}");
            }
            else
            {
                memory = _optionIdentifier.Memory;
                Logger.LogDebug($"Use options identifier: {BuilderOptions.Identifier}");
            }

            return GenerateKey(memory.ToArray(), length);
        }


        /// <summary>
        /// 生成密钥。
        /// </summary>
        /// <param name="bytes">给定的基础 <see cref="IByteBuffer"/>。</param>
        /// <param name="length">给定要生成的 <see cref="IByteBuffer"/>长度。</param>
        /// <returns>返回 <see cref="IByteBuffer"/>。</returns>
        private IByteBuffer GenerateKey(byte[] bytes, int length)
        {
            var result = new byte[length];

            // 计算最大公约数
            var gcf = bytes.Length.ComputeGCD(length);

            if (BuilderOptions.KeyGenerator.IsRandomKey)
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
