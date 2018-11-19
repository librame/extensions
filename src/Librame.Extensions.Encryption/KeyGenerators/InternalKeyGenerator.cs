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

namespace Librame.Extensions.Encryption
{
    using Buffers;
    using Services;

    /// <summary>
    /// 内部密钥生成器。
    /// </summary>
    internal class InternalKeyGenerator : AbstractService<InternalKeyGenerator>, IKeyGenerator
    {
        private readonly EncryptionBuilderOptions _options;
        private readonly AlgorithmIdentifier _optionIdentifier;


        /// <summary>
        /// 构造一个 <see cref="InternalKeyGenerator"/> 实例。
        /// </summary>
        /// <param name="optionsMonitor">给定的 <see cref="IOptions{DefaultEncryptionBuilderOptions}"/>。</param>
        /// <param name="logger">给定的 <see cref="ILogger{InternalKeyGenerator}"/>。</param>
        public InternalKeyGenerator(IOptions<EncryptionBuilderOptions> optionsMonitor, ILogger<InternalKeyGenerator> logger)
            : base(logger)
        {
            _options = optionsMonitor.NotDefault(nameof(optionsMonitor)).Value;
            _optionIdentifier = AlgorithmIdentifier.Parse(_options.Identifier);
        }


        /// <summary>
        /// 获取 64 位密钥。
        /// </summary>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        public IBuffer<byte> GetKey64(string identifier = null)
        {
            return GenerateKey(identifier, 8); // 64
        }

        /// <summary>
        /// 获取 128 位密钥。
        /// </summary>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        public IBuffer<byte> GetKey128(string identifier = null)
        {
            return GenerateKey(identifier, 16); // 128
        }

        /// <summary>
        /// 获取 192 位密钥。
        /// </summary>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        public IBuffer<byte> GetKey192(string identifier = null)
        {
            return GenerateKey(identifier, 24); // 192
        }

        /// <summary>
        /// 获取 256 位密钥。
        /// </summary>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        public IBuffer<byte> GetKey256(string identifier = null)
        {
            return GenerateKey(identifier, 32); // 256
        }

        /// <summary>
        /// 获取 384 位密钥。
        /// </summary>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        public IBuffer<byte> GetKey384(string identifier = null)
        {
            return GenerateKey(identifier, 48); // 384
        }

        /// <summary>
        /// 获取 512 位密钥。
        /// </summary>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        public IBuffer<byte> GetKey512(string identifier = null)
        {
            return GenerateKey(identifier, 64); // 512
        }

        /// <summary>
        /// 获取 1024 位密钥。
        /// </summary>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        public IBuffer<byte> GetKey1024(string identifier = null)
        {
            return GenerateKey(identifier, 128); // 1024
        }

        /// <summary>
        /// 获取 2048 位密钥。
        /// </summary>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        public IBuffer<byte> GetKey2048(string identifier = null)
        {
            return GenerateKey(identifier, 256); // 2048
        }


        /// <summary>
        /// 生成密钥。
        /// </summary>
        /// <param name="identifier">给定的标识符。</param>
        /// <param name="length">给定要生成的 <see cref="IBuffer{T}"/>长度。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        private IBuffer<byte> GenerateKey(string identifier, int length)
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
                Logger.LogDebug($"Use options identifier: {_options.Identifier}");
            }

            return GenerateKey(memory.ToArray(), length);
        }

        /// <summary>
        /// 生成密钥。
        /// </summary>
        /// <param name="bytes">给定的基础 <see cref="IBuffer{T}"/>。</param>
        /// <param name="length">给定要生成的 <see cref="IBuffer{T}"/>长度。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        private IBuffer<byte> GenerateKey(byte[] bytes, int length)
        {
            var result = new byte[length];

            // 计算最大公约数
            var gcf = bytes.Length.ComputeGCD(length);

            if (_options.KeyGenerator.IsRandomKey)
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

            return result.AsBuffer();
        }

    }
}
