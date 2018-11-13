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
    using Buffers;
    using Services;

    /// <summary>
    /// 内部 RSA 非对称算法服务。
    /// </summary>
    internal class InternalRsaAlgorithmService : AbstractService<InternalRsaAlgorithmService>, IRsaAlgorithmService
    {
        private RSA _rsa = null;


        /// <summary>
        /// 构造一个 <see cref="InternalRsaAlgorithmService"/> 实例。
        /// </summary>
        /// <param name="provider">给定的 <see cref="ISigningCredentialsProvider"/>。</param>
        /// <param name="options">给定的 <see cref="IOptions{DefaultEncryptionBuilderOptions}"/>。</param>
        /// <param name="logger">给定的 <see cref="ILogger{InternalRsaAlgorithmService}"/>。</param>
        public InternalRsaAlgorithmService(ISigningCredentialsProvider provider, IOptions<EncryptionBuilderOptions> options,
            ILogger<InternalRsaAlgorithmService> logger)
            : base(logger)
        {
            Provider = provider.NotDefault(nameof(provider));
            Options = options.NotDefault(nameof(options)).Value;

            InitializeRsa();
        }
        
        private void InitializeRsa()
        {
            if (_rsa.IsDefault())
            {
                var credentials = Provider.GetSigningCredentials(Options.SigningCredentialsKey);
                _rsa = credentials.ResolveRsa();
            }
        }


        /// <summary>
        /// 签名证书提供程序。
        /// </summary>
        /// <value>
        /// 返回 <see cref="ISigningCredentialsProvider"/>。
        /// </value>
        public ISigningCredentialsProvider Provider { get; }

        /// <summary>
        /// 加密构建器选项。
        /// </summary>
        /// <value>
        /// 返回 <see cref="EncryptionBuilderOptions"/>。
        /// </value>
        public EncryptionBuilderOptions Options { get; }


        /// <summary>
        /// 签名散列算法。
        /// </summary>
        public HashAlgorithmName SignHashAlgorithm { get; set; } = HashAlgorithmName.SHA256;

        /// <summary>
        /// 签名填充。
        /// </summary>
        public RSASignaturePadding SignaturePadding { get; set; } = RSASignaturePadding.Pkcs1;

        /// <summary>
        /// 加密填充。
        /// </summary>
        public RSAEncryptionPadding EncryptionPadding { get; set; } = RSAEncryptionPadding.Pkcs1;


        /// <summary>
        /// 签名数据。
        /// </summary>
        /// <param name="buffer">给定要签名的 <see cref="IBuffer{Byte}"/>。</param>
        /// <returns>返回签名后的 <see cref="IBuffer{Byte}"/>。</returns>
        public virtual IBuffer<byte> SignData(IBuffer<byte> buffer)
        {
            return buffer.ChangeMemory(memory => _rsa.SignData(memory.ToArray(), SignHashAlgorithm, SignaturePadding));
        }

        /// <summary>
        /// 签名散列。
        /// </summary>
        /// <param name="buffer">给定要签名的 <see cref="IBuffer{Byte}"/>。</param>
        /// <returns>返回签名后的 <see cref="IBuffer{Byte}"/>。</returns>
        public virtual IBuffer<byte> SignHash(IBuffer<byte> buffer)
        {
            return buffer.ChangeMemory(memory => _rsa.SignHash(memory.ToArray(), SignHashAlgorithm, SignaturePadding));
        }


        /// <summary>
        /// 验证数据。
        /// </summary>
        /// <param name="buffer">给定要签名的 <see cref="IBuffer{Byte}"/>。</param>
        /// <param name="signedBuffer">给定已签名的 <see cref="IReadOnlyBuffer{Byte}"/>。</param>
        /// <returns>返回签名后的 <see cref="IBuffer{Byte}"/>。</returns>
        public virtual bool VerifyData(IBuffer<byte> buffer, IReadOnlyBuffer<byte> signedBuffer)
        {
            return _rsa.VerifyData(buffer.Memory.ToArray(), signedBuffer.Memory.ToArray(), SignHashAlgorithm, SignaturePadding);
        }

        /// <summary>
        /// 验证散列。
        /// </summary>
        /// <param name="buffer">给定要签名的 <see cref="IBuffer{Byte}"/>。</param>
        /// <param name="signedBuffer">给定已签名的 <see cref="IReadOnlyBuffer{Byte}"/>。</param>
        /// <returns>返回签名后的 <see cref="IBuffer{Byte}"/>。</returns>
        public virtual bool VerifyHash(IBuffer<byte> buffer, IReadOnlyBuffer<byte> signedBuffer)
        {
            return _rsa.VerifyHash(buffer.Memory.ToArray(), signedBuffer.Memory.ToArray(), SignHashAlgorithm, SignaturePadding);
        }


        /// <summary>
        /// 加密 <see cref="IBuffer{Byte}"/>。
        /// </summary>
        /// <param name="buffer">给定待加密的 <see cref="IBuffer{Byte}"/>。</param>
        /// <returns>返回 <see cref="IBuffer{Byte}"/>。</returns>
        public virtual IBuffer<byte> Encrypt(IBuffer<byte> buffer)
        {
            return buffer.ChangeMemory(memory => _rsa.Encrypt(memory.ToArray(), EncryptionPadding));
        }


        /// <summary>
        /// 解密 <see cref="IBuffer{Byte}"/>。
        /// </summary>
        /// <param name="buffer">给定的加密 <see cref="IBuffer{Byte}"/>。</param>
        /// <returns>返回 <see cref="IBuffer{Byte}"/>。</returns>
        public virtual IBuffer<byte> Decrypt(IBuffer<byte> buffer)
        {
            return buffer.ChangeMemory(memory => _rsa.Decrypt(memory.ToArray(), EncryptionPadding));
        }

    }
}
