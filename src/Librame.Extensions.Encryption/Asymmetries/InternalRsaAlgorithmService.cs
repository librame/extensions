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
using System.Security.Cryptography;

namespace Librame.Extensions.Encryption
{
    using Buffers;
    using Services;

    /// <summary>
    /// 内部 RSA 非对称算法服务。
    /// </summary>
    internal class InternalRsaAlgorithmService : AbstractService<InternalRsaAlgorithmService>, IRsaAlgorithmService
    {
        /// <summary>
        /// 构造一个 <see cref="InternalRsaAlgorithmService"/> 实例。
        /// </summary>
        /// <param name="keyGenerator">给定的 <see cref="IRsaKeyGenerator"/>。</param>
        /// <param name="logger">给定的 <see cref="ILogger{InternalRsaAlgorithmService}"/>。</param>
        public InternalRsaAlgorithmService(IRsaKeyGenerator keyGenerator, ILogger<InternalRsaAlgorithmService> logger)
            : base(logger)
        {
            KeyGenerator = keyGenerator.NotDefault(nameof(keyGenerator));
        }

        
        /// <summary>
        /// 密钥生成器。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IRsaKeyGenerator"/>。
        /// </value>
        public IRsaKeyGenerator KeyGenerator { get; }


        /// <summary>
        /// 签名散列算法名称（默认为 <see cref="HashAlgorithmName.SHA256"/>）。
        /// </summary>
        public HashAlgorithmName SignHashAlgorithm { get; set; } = HashAlgorithmName.SHA256;
        
        /// <summary>
        /// 签名填充（默认为 <see cref="RSASignaturePadding.Pkcs1"/>）。
        /// </summary>
        public RSASignaturePadding SignaturePadding { get; set; } = RSASignaturePadding.Pkcs1;

        /// <summary>
        /// 加密填充（默认为 <see cref="RSAEncryptionPadding.Pkcs1"/>）。
        /// </summary>
        public RSAEncryptionPadding EncryptionPadding { get; set; } = RSAEncryptionPadding.Pkcs1;


        /// <summary>
        /// 创建 RSA。
        /// </summary>
        /// <param name="parameters">给定的 <see cref="RSAParameters"/>（可选；默认使用 <see cref="KeyGenerator"/>）。</param>
        /// <returns>返回 <see cref="RSA"/>。</returns>
        protected virtual RSA CreateRsa(RSAParameters? parameters = null)
        {
            if (!parameters.HasValue)
            {
                parameters = KeyGenerator.GenerateKeyParameters().Parameters;
                Logger.LogDebug($"Use default rsa parameters: {nameof(KeyGenerator)}.{nameof(KeyGenerator.GenerateKeyParameters)}");
            }

            var rsa = RSA.Create();
            rsa.ImportParameters(parameters.Value);

            return rsa;
        }

        /// <summary>
        /// 创建 RSA。
        /// </summary>
        /// <param name="padding">给定的 <see cref="RSAEncryptionPadding"/>（可选；默认使用 <see cref="EncryptionPadding"/>）。</param>
        /// <param name="parameters">给定的 <see cref="RSAParameters"/>（可选；默认使用 <see cref="KeyGenerator"/>）。</param>
        /// <returns>返回 <see cref="RSA"/>。</returns>
        protected virtual RSA CreateRsa(ref RSAEncryptionPadding padding, RSAParameters? parameters = null)
        {
            if (padding.IsDefault())
            {
                padding = EncryptionPadding;

                var detail = padding.Mode == RSAEncryptionPaddingMode.Oaep ? $".{padding.OaepHashAlgorithm.Name}" : string.Empty;
                Logger.LogDebug($"Use default rsa encryption padding: {padding.Mode.ToString()}{detail}");
            }

            return CreateRsa(parameters);
        }

        /// <summary>
        /// 创建 RSA。
        /// </summary>
        /// <param name="hashAlgorithm">给定的 <see cref="HashAlgorithmName"/>（可选；默认使用 <see cref="SignHashAlgorithm"/>）。</param>
        /// <param name="padding">给定的 <see cref="RSASignaturePadding"/>（可选；默认使用 <see cref="SignaturePadding"/>）。</param>
        /// <param name="parameters">给定的 <see cref="RSAParameters"/>（可选；默认使用 <see cref="KeyGenerator"/>）。</param>
        /// <returns>返回 <see cref="RSA"/>。</returns>
        protected virtual RSA CreateRsa(ref HashAlgorithmName hashAlgorithm, ref RSASignaturePadding padding, RSAParameters? parameters = null)
        {
            if (hashAlgorithm.Name.IsEmpty())
            {
                hashAlgorithm = SignHashAlgorithm;
                Logger.LogDebug($"Use default hash algorithm name: {hashAlgorithm.Name}");
            }

            if (padding.IsDefault())
            {
                padding = SignaturePadding;
                Logger.LogDebug($"Use default rsa signature padding: {padding.Mode.ToString()}");
            }

            return CreateRsa(parameters);
        }


        /// <summary>
        /// 签名数据。
        /// </summary>
        /// <param name="buffer">给定要签名的<see cref="IBuffer{T}"/>。</param>
        /// <param name="hashAlgorithm">给定的 <see cref="HashAlgorithmName"/>（可选；默认为 <see cref="SignHashAlgorithm"/>）。</param>
        /// <param name="padding">给定的 <see cref="RSASignaturePadding"/>（可选；默认为 <see cref="SignaturePadding"/>）。</param>
        /// <param name="parameters">给定的 <see cref="RSAParameters"/>（可选；默认使用 <see cref="KeyGenerator"/>）。</param>
        /// <returns>返回签名后的<see cref="IBuffer{T}"/>。</returns>
        public virtual IBuffer<byte> SignData(IBuffer<byte> buffer,
            HashAlgorithmName hashAlgorithm = default, RSASignaturePadding padding = null, RSAParameters? parameters = null)
        {
            var rsa = CreateRsa(ref hashAlgorithm, ref padding, parameters);

            return buffer.ChangeMemory(memory => rsa.SignData(memory.ToArray(), hashAlgorithm, padding));
        }

        /// <summary>
        /// 签名散列。
        /// </summary>
        /// <param name="buffer">给定要签名的<see cref="IBuffer{T}"/>。</param>
        /// <param name="hashAlgorithm">给定的 <see cref="HashAlgorithmName"/>（可选；默认为 <see cref="SignHashAlgorithm"/>）。</param>
        /// <param name="padding">给定的 <see cref="RSASignaturePadding"/>（可选；默认为 <see cref="SignaturePadding"/>）。</param>
        /// <param name="parameters">给定的 <see cref="RSAParameters"/>（可选；默认使用 <see cref="KeyGenerator"/>）。</param>
        /// <returns>返回签名后的<see cref="IBuffer{T}"/>。</returns>
        public virtual IBuffer<byte> SignHash(IBuffer<byte> buffer,
            HashAlgorithmName hashAlgorithm = default, RSASignaturePadding padding = null, RSAParameters? parameters = null)
        {
            var rsa = CreateRsa(ref hashAlgorithm, ref padding, parameters);

            return buffer.ChangeMemory(memory => rsa.SignHash(memory.ToArray(), hashAlgorithm, padding));
        }


        /// <summary>
        /// 验证数据。
        /// </summary>
        /// <param name="buffer">给定要签名的<see cref="IBuffer{T}"/>。</param>
        /// <param name="signedBuffer">给定已签名的<see cref="IReadOnlyBuffer{T}"/>。</param>
        /// <param name="hashAlgorithm">给定的 <see cref="HashAlgorithmName"/>（可选；默认为 <see cref="SignHashAlgorithm"/>）。</param>
        /// <param name="padding">给定的 <see cref="RSASignaturePadding"/>（可选；默认为 <see cref="SignaturePadding"/>）。</param>
        /// <param name="parameters">给定的 <see cref="RSAParameters"/>（可选；默认使用 <see cref="KeyGenerator"/>）。</param>
        /// <returns>返回签名后的<see cref="IBuffer{T}"/>。</returns>
        public virtual bool VerifyData(IBuffer<byte> buffer, IReadOnlyBuffer<byte> signedBuffer,
            HashAlgorithmName hashAlgorithm = default, RSASignaturePadding padding = null, RSAParameters? parameters = null)
        {
            var rsa = CreateRsa(ref hashAlgorithm, ref padding, parameters);

            return rsa.VerifyData(buffer.Memory.ToArray(), signedBuffer.Memory.ToArray(), hashAlgorithm, padding);
        }

        /// <summary>
        /// 验证散列。
        /// </summary>
        /// <param name="buffer">给定要签名的<see cref="IBuffer{T}"/>。</param>
        /// <param name="signedBuffer">给定已签名的<see cref="IReadOnlyBuffer{T}"/>。</param>
        /// <param name="hashAlgorithm">给定的 <see cref="HashAlgorithmName"/>（可选；默认为 <see cref="SignHashAlgorithm"/>）。</param>
        /// <param name="padding">给定的 <see cref="RSASignaturePadding"/>（可选；默认为 <see cref="SignaturePadding"/>）。</param>
        /// <param name="parameters">给定的 <see cref="RSAParameters"/>（可选；默认使用 <see cref="KeyGenerator"/>）。</param>
        /// <returns>返回签名后的<see cref="IBuffer{T}"/>。</returns>
        public virtual bool VerifyHash(IBuffer<byte> buffer, IReadOnlyBuffer<byte> signedBuffer,
            HashAlgorithmName hashAlgorithm = default, RSASignaturePadding padding = null, RSAParameters? parameters = null)
        {
            var rsa = CreateRsa(ref hashAlgorithm, ref padding, parameters);

            return rsa.VerifyHash(buffer.Memory.ToArray(), signedBuffer.Memory.ToArray(), hashAlgorithm, padding);
        }


        /// <summary>
        /// 加密<see cref="IBuffer{T}"/>。
        /// </summary>
        /// <param name="buffer">给定待加密的<see cref="IBuffer{T}"/>。</param>
        /// <param name="padding">给定的 <see cref="RSAEncryptionPadding"/>（可选；默认为 <see cref="EncryptionPadding"/>）。</param>
        /// <param name="parameters">给定的 <see cref="RSAParameters"/>（可选；默认使用 <see cref="KeyGenerator"/>）。</param>
        /// <returns>返回<see cref="IBuffer{T}"/>。</returns>
        public virtual IBuffer<byte> Encrypt(IBuffer<byte> buffer,
            RSAEncryptionPadding padding = null, RSAParameters? parameters = null)
        {
            var rsa = CreateRsa(ref padding, parameters);

            return buffer.ChangeMemory(memory => rsa.Encrypt(memory.ToArray(), padding));
        }


        /// <summary>
        /// 解密<see cref="IBuffer{T}"/>。
        /// </summary>
        /// <param name="buffer">给定的加密<see cref="IBuffer{T}"/>。</param>
        /// <param name="padding">给定的 <see cref="RSAEncryptionPadding"/>（可选；默认为 <see cref="EncryptionPadding"/>）。</param>
        /// <param name="parameters">给定的 <see cref="RSAParameters"/>（可选；默认使用 <see cref="KeyGenerator"/>）。</param>
        /// <returns>返回<see cref="IBuffer{T}"/>。</returns>
        public virtual IBuffer<byte> Decrypt(IBuffer<byte> buffer,
            RSAEncryptionPadding padding = null, RSAParameters? parameters = null)
        {
            var rsa = CreateRsa(ref padding, parameters);

            return buffer.ChangeMemory(memory => rsa.Decrypt(memory.ToArray(), padding));
        }

    }
}
