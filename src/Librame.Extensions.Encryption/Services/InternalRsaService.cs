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
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace Librame.Extensions.Encryption
{
    using Core;

    /// <summary>
    /// 内部 RSA 服务。
    /// </summary>
    internal class InternalRsaService : AbstractEncryptionService, IRsaService
    {
        private RSA _rsa = null;


        /// <summary>
        /// 构造一个 <see cref="InternalRsaService"/> 实例。
        /// </summary>
        /// <param name="signingCredentials">给定的 <see cref="ISigningCredentialsService"/>。</param>
        /// <param name="options">给定的 <see cref="IOptions{EncryptionBuilderOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public InternalRsaService(ISigningCredentialsService signingCredentials,
            IOptions<EncryptionBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(options, loggerFactory)
        {
            SigningCredentials = signingCredentials.NotNull(nameof(signingCredentials));

            InitializeRsa();
        }
        
        private void InitializeRsa()
        {
            if (_rsa.IsNull())
            {
                var credentials = SigningCredentials.GetSigningCredentials(Options.SigningCredentialsKey);
                _rsa = credentials.ResolveRsa();
            }
        }


        /// <summary>
        /// 签名证书。
        /// </summary>
        /// <value>
        /// 返回 <see cref="ISigningCredentialsService"/>。
        /// </value>
        public ISigningCredentialsService SigningCredentials { get; }


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
        /// <param name="buffer">给定要签名的 <see cref="IByteBuffer"/>。</param>
        /// <returns>返回签名后的 <see cref="IByteBuffer"/>。</returns>
        public IByteBuffer SignData(IByteBuffer buffer)
        {
            return buffer.Change(memory => _rsa.SignData(memory.ToArray(), SignHashAlgorithm, SignaturePadding));
        }

        /// <summary>
        /// 签名散列。
        /// </summary>
        /// <param name="buffer">给定要签名的 <see cref="IByteBuffer"/>。</param>
        /// <returns>返回签名后的 <see cref="IByteBuffer"/>。</returns>
        public IByteBuffer SignHash(IByteBuffer buffer)
        {
            return buffer.Change(memory => _rsa.SignHash(memory.ToArray(), SignHashAlgorithm, SignaturePadding));
        }


        /// <summary>
        /// 验证数据。
        /// </summary>
        /// <param name="buffer">给定要签名的 <see cref="IByteBuffer"/>。</param>
        /// <param name="signedBuffer">给定已签名的 <see cref="IReadOnlyBuffer{Byte}"/>。</param>
        /// <returns>返回签名后的 <see cref="IByteBuffer"/>。</returns>
        public bool VerifyData(IByteBuffer buffer, IReadOnlyBuffer<byte> signedBuffer)
        {
            return _rsa.VerifyData(buffer.Memory.ToArray(), signedBuffer.Memory.ToArray(), SignHashAlgorithm, SignaturePadding);
        }

        /// <summary>
        /// 验证散列。
        /// </summary>
        /// <param name="buffer">给定要签名的 <see cref="IByteBuffer"/>。</param>
        /// <param name="signedBuffer">给定已签名的 <see cref="IReadOnlyBuffer{Byte}"/>。</param>
        /// <returns>返回签名后的 <see cref="IByteBuffer"/>。</returns>
        public bool VerifyHash(IByteBuffer buffer, IReadOnlyBuffer<byte> signedBuffer)
        {
            return _rsa.VerifyHash(buffer.Memory.ToArray(), signedBuffer.Memory.ToArray(), SignHashAlgorithm, SignaturePadding);
        }


        /// <summary>
        /// 加密 <see cref="IByteBuffer"/>。
        /// </summary>
        /// <param name="buffer">给定待加密的 <see cref="IByteBuffer"/>。</param>
        /// <returns>返回 <see cref="IByteBuffer"/>。</returns>
        public IByteBuffer Encrypt(IByteBuffer buffer)
        {
            return buffer.Change(memory => _rsa.Encrypt(memory.ToArray(), EncryptionPadding));
        }


        /// <summary>
        /// 解密 <see cref="IByteBuffer"/>。
        /// </summary>
        /// <param name="buffer">给定的加密 <see cref="IByteBuffer"/>。</param>
        /// <returns>返回 <see cref="IByteBuffer"/>。</returns>
        public IByteBuffer Decrypt(IByteBuffer buffer)
        {
            return buffer.Change(memory => _rsa.Decrypt(memory.ToArray(), EncryptionPadding));
        }

    }
}
