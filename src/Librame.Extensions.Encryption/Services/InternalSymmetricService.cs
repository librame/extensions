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
using System.Security.Cryptography;

namespace Librame.Extensions.Encryption
{
    using Core;

    /// <summary>
    /// 内部对称算法服务。
    /// </summary>
    internal class InternalSymmetricService : AbstractEncryptionService<InternalSymmetricService>, ISymmetricService
    {
        /// <summary>
        /// 构造一个 <see cref="InternalSymmetricService"/> 实例。
        /// </summary>
        /// <param name="keyGenerator">给定的 <see cref="IKeyGenerator"/>。</param>
        /// <param name="options">给定的 <see cref="IOptions{EncryptionBuilderOptions}"/>。</param>
        /// <param name="logger">给定的 <see cref="ILogger{InternalSymmetricAlgorithmService}"/>。</param>
        public InternalSymmetricService(IKeyGenerator keyGenerator,
            IOptions<EncryptionBuilderOptions> options, ILogger<InternalSymmetricService> logger)
            : base(options, logger)
        {
            KeyGenerator = keyGenerator.NotNull(nameof(keyGenerator));
        }


        /// <summary>
        /// 密钥生成器。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IKeyGenerator"/>。
        /// </value>
        public IKeyGenerator KeyGenerator { get; }


        /// <summary>
        /// 加密字节数组。
        /// </summary>
        /// <param name="algorithm">给定的 <see cref="SymmetricAlgorithm"/>。</param>
        /// <param name="buffer">给定的字节数组。</param>
        /// <returns>返回字节数组。</returns>
        private IByteBuffer Encrypt(SymmetricAlgorithm algorithm, IByteBuffer buffer)
        {
            algorithm.Mode = CipherMode.ECB;
            algorithm.Padding = PaddingMode.PKCS7;

            var encryptor = algorithm.CreateEncryptor();

            return buffer.Change(memory =>
            {
                var bytes = memory.ToArray();
                bytes = encryptor.TransformFinalBlock(bytes, 0, bytes.Length);
                Logger.LogDebug($"Encrypt final block: {nameof(CipherMode)}={algorithm.Mode.ToString()}, {nameof(PaddingMode)}={algorithm.Padding.ToString()}");

                return bytes;
            });
        }

        /// <summary>
        /// 解密字节数组。
        /// </summary>
        /// <param name="algorithm">给定的 <see cref="SymmetricAlgorithm"/>。</param>
        /// <param name="buffer">给定的字节数组。</param>
        /// <returns>返回字节数组。</returns>
        private IByteBuffer Decrypt(SymmetricAlgorithm algorithm, IByteBuffer buffer)
        {
            algorithm.Mode = CipherMode.ECB;
            algorithm.Padding = PaddingMode.PKCS7;

            var encryptor = algorithm.CreateDecryptor();

            return buffer.Change(memory =>
            {
                var bytes = memory.ToArray();
                bytes = encryptor.TransformFinalBlock(bytes, 0, bytes.Length);
                Logger.LogDebug($"Decrypt final block: {nameof(CipherMode)}={algorithm.Mode.ToString()}, {nameof(PaddingMode)}={algorithm.Padding.ToString()}");

                return bytes;
            });
        }


        #region AES

        /// <summary>
        /// 转换为 AES。
        /// </summary>
        /// <param name="buffer">给定待加密的字节数组。</param>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回字节数组。</returns>
        public IByteBuffer ToAes(IByteBuffer buffer, string identifier = null)
        {
            var algorithm = CreateAes(identifier);

            return Encrypt(algorithm, buffer);
        }

        /// <summary>
        /// 还原 AES。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回字节数组。</returns>
        public IByteBuffer FromAes(IByteBuffer buffer, string identifier = null)
        {
            var algorithm = CreateAes(identifier);

            return Decrypt(algorithm, buffer);
        }

        /// <summary>
        /// 创建 AES。
        /// </summary>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回 <see cref="Aes"/>。</returns>
        private Aes CreateAes(string identifier = null)
        {
            var algorithm = Aes.Create();
            algorithm.Key = KeyGenerator.GetKey256(identifier).Memory.ToArray();
            Logger.LogDebug($"Use AES algorithm");

            return algorithm;
        }

        #endregion


        #region DES

        /// <summary>
        /// 转换为 DES。
        /// </summary>
        /// <param name="buffer">给定待加密的字节数组。</param>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回字节数组。</returns>
        public IByteBuffer ToDes(IByteBuffer buffer, string identifier = null)
        {
            var algorithm = CreateDes(identifier);

            return Encrypt(algorithm, buffer);
        }

        /// <summary>
        /// 还原 DES。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回字节数组。</returns>
        public IByteBuffer FromDes(IByteBuffer buffer, string identifier = null)
        {
            var algorithm = CreateDes(identifier);

            return Decrypt(algorithm, buffer);
        }

        /// <summary>
        /// 创建 DES。
        /// </summary>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回 <see cref="DES"/>。</returns>
        private DES CreateDes(string identifier = null)
        {
            var algorithm = DES.Create();
            algorithm.Key = KeyGenerator.GetKey64(identifier).Memory.ToArray();
            Logger.LogDebug($"Use DES algorithm");

            return algorithm;
        }

        #endregion


        #region TripleDES

        /// <summary>
        /// 转换为 TripleDES。
        /// </summary>
        /// <param name="buffer">给定待加密的字节数组。</param>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回字节数组。</returns>
        public IByteBuffer ToTripleDes(IByteBuffer buffer, string identifier = null)
        {
            var algorithm = CreateTripleDes(identifier);

            return Encrypt(algorithm, buffer);
        }

        /// <summary>
        /// 还原 TripleDES。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回字节数组。</returns>
        public IByteBuffer FromTripleDes(IByteBuffer buffer, string identifier = null)
        {
            var algorithm = CreateTripleDes(identifier);

            return Decrypt(algorithm, buffer);
        }

        /// <summary>
        /// 创建 TripleDES。
        /// </summary>
        /// <param name="identifier">给定的标识符（可选；默认使用选项配置。详情可参考 <see cref="AlgorithmIdentifier"/>）。</param>
        /// <returns>返回 <see cref="TripleDES"/>。</returns>
        private TripleDES CreateTripleDes(string identifier = null)
        {
            var algorithm = TripleDES.Create();
            algorithm.Key = KeyGenerator.GetKey192(identifier).Memory.ToArray();
            Logger.LogDebug($"Use TripleDES algorithm");

            return algorithm;
        }

        #endregion

    }
}
