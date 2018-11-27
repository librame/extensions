#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Librame.Builders
{
    using Extensions;
    using Extensions.Encryption;

    /// <summary>
    /// 加密构建器静态扩展。
    /// </summary>
    public static class EncryptionBuilderExtensions
    {

        /// <summary>
        /// 添加加密扩展。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IBuilder"/>。</param>
        /// <param name="configureOptions">给定的 <see cref="Action{EncryptionBuilderOptions}"/>（可选）。</param>
        /// <param name="configuration">给定的 <see cref="IConfiguration"/>（可选）。</param>
        /// <returns>返回 <see cref="IEncryptionBuilder"/>。</returns>
        public static IEncryptionBuilder AddEncryption(this IBuilder builder,
            Action<EncryptionBuilderOptions> configureOptions = null, IConfiguration configuration = null)
        {
            return builder.AddBuilder(configureOptions, configuration, _builder =>
            {
                return _builder.AsEncryptionBuilder()
                    .AddKeyGenerators()
                    .AddRsaAlgorithms()
                    .AddHashAlgorithms()
                    .AddSymmetricAlgorithms()
                    .AddConverters()
                    .AddBuffers();
            });
        }


        /// <summary>
        /// 转换为加密构建器。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IBuilder"/>。</param>
        /// <returns>返回 <see cref="IEncryptionBuilder"/>。</returns>
        public static IEncryptionBuilder AsEncryptionBuilder(this IBuilder builder)
        {
            return new InternalEncryptionBuilder(builder);
        }

        /// <summary>
        /// 添加密钥生成器。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IEncryptionBuilder"/>。</param>
        /// <returns>返回 <see cref="IEncryptionBuilder"/>。</returns>
        public static IEncryptionBuilder AddKeyGenerators(this IEncryptionBuilder builder)
        {
            builder.Services.AddSingleton<IKeyGenerator, InternalKeyGenerator>();

            return builder;
        }

        /// <summary>
        /// 添加散列算法。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IEncryptionBuilder"/>。</param>
        /// <returns>返回 <see cref="IEncryptionBuilder"/>。</returns>
        public static IEncryptionBuilder AddHashAlgorithms(this IEncryptionBuilder builder)
        {
            builder.Services.AddSingleton<IHashAlgorithmService, InternalHashAlgorithmService>();
            builder.Services.AddSingleton<IKeyedHashAlgorithmService, InternalKeyedHashAlgorithmService>();

            return builder;
        }

        /// <summary>
        /// 添加 RSA 非对称算法。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IEncryptionBuilder"/>。</param>
        /// <returns>返回 <see cref="IEncryptionBuilder"/>。</returns>
        public static IEncryptionBuilder AddRsaAlgorithms(this IEncryptionBuilder builder)
        {
            builder.Services.AddSingleton<IRsaAlgorithmService, InternalRsaAlgorithmService>();

            return builder;
        }

        /// <summary>
        /// 添加对称算法。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IEncryptionBuilder"/>。</param>
        /// <returns>返回 <see cref="IEncryptionBuilder"/>。</returns>
        public static IEncryptionBuilder AddSymmetricAlgorithms(this IEncryptionBuilder builder)
        {
            builder.Services.AddSingleton<ISymmetricAlgorithmService, InternalSymmetricAlgorithmService>();

            return builder;
        }

        /// <summary>
        /// 添加转换器。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IEncryptionBuilder"/>。</param>
        /// <returns>返回 <see cref="IEncryptionBuilder"/>。</returns>
        public static IEncryptionBuilder AddConverters(this IEncryptionBuilder builder)
        {
            builder.Services.AddSingleton<ICiphertextAlgorithmConverter, CiphertextAlgorithmConverter>();
            builder.Services.AddSingleton<IPlaintextAlgorithmConverter, PlaintextAlgorithmConverter>();

            return builder;
        }

        /// <summary>
        /// 添加缓冲区。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IEncryptionBuilder"/>。</param>
        /// <returns>返回 <see cref="IEncryptionBuilder"/>。</returns>
        public static IEncryptionBuilder AddBuffers(this IEncryptionBuilder builder)
        {
            builder.Services.AddTransient(typeof(IEncryptionBuffer<,>), typeof(InternalEncryptionBuffer<,>));

            return builder;
        }


        #region SigningCredentials

        /// <summary>
        /// 添加签名证书集合。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IEncryptionBuilder"/>。</param>
        /// <param name="credentials">给定的签名证书集合。</param>
        /// <returns>返回 <see cref="IEncryptionBuilder"/>。</returns>
        public static IEncryptionBuilder AddSigningCredentials(this IEncryptionBuilder builder, params KeyValuePair<string, SigningCredentials>[] credentials)
        {
            foreach (var cred in credentials)
            {
                if (!(cred.Value.Key is AsymmetricSecurityKey
                    || cred.Value.Key is JsonWebKey && ((JsonWebKey)cred.Value.Key).HasPrivateKey))
                    //&& !credential.Key.IsSupportedAlgorithm(SecurityAlgorithms.RsaSha256Signature))
                    throw new InvalidOperationException("Signing key is not asymmetric");
            }

            builder.Services.AddSingleton<ISigningCredentialsProvider>(new InternalSigningCredentialsProvider(credentials));

            return builder;
        }

        /// <summary>
        /// 添加全局签名证书。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IEncryptionBuilder"/>。</param>
        /// <param name="credentials">给定的 <see cref="SigningCredentials"/>。</param>
        /// <returns>返回 <see cref="IEncryptionBuilder"/>。</returns>
        public static IEncryptionBuilder AddGlobalSigningCredentials(this IEncryptionBuilder builder, SigningCredentials credentials)
        {
            return builder.AddSigningCredentials(new KeyValuePair<string, SigningCredentials>(EncryptionBuilderOptions.GLOBAL_KEY, credentials));
        }

        /// <summary>
        /// 添加全局签名证书。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IEncryptionBuilder"/>。</param>
        /// <param name="certificate">给定的 <see cref="X509Certificate2"/>。</param>
        /// <returns>返回 <see cref="IEncryptionBuilder"/>。</returns>
        public static IEncryptionBuilder AddGlobalSigningCredentials(this IEncryptionBuilder builder, X509Certificate2 certificate)
        {
            if (!certificate.NotDefault(nameof(certificate)).HasPrivateKey)
                throw new InvalidOperationException("X509 certificate does not have a private key.");

            var credentials = new SigningCredentials(new X509SecurityKey(certificate), SecurityAlgorithms.RsaSha256);
            return builder.AddGlobalSigningCredentials(credentials);
        }

        /// <summary>
        /// 添加全局签名证书。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IEncryptionBuilder"/>。</param>
        /// <param name="rsaKey">给定的 <see cref="RsaSecurityKey"/>。</param>
        /// <returns>返回 <see cref="IEncryptionBuilder"/>。</returns>
        public static IEncryptionBuilder AddGlobalSigningCredentials(this IEncryptionBuilder builder, RsaSecurityKey rsaKey)
        {
            if (rsaKey.PrivateKeyStatus == PrivateKeyStatus.DoesNotExist)
                throw new InvalidOperationException("RSA key does not have a private key.");

            var credential = new SigningCredentials(rsaKey, SecurityAlgorithms.RsaSha256);
            return builder.AddGlobalSigningCredentials(credential);
        }

        /// <summary>
        /// 添加开发者全局签名证书。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IEncryptionBuilder"/>。</param>
        /// <param name="persistKey">是否持久化密钥（可选；默认持久化）。</param>
        /// <param name="fileName">给定的文件名（可选；默认兼容 IdentityServer4 生成的临时密钥文件）。</param>
        /// <returns>返回 <see cref="IEncryptionBuilder"/>。</returns>
        public static IEncryptionBuilder AddDeveloperGlobalSigningCredentials(this IEncryptionBuilder builder, bool persistKey = true, string fileName = null)
        {
            var rsaKey = RsaSecurityKeyHelper.LoadRsaSecurityKey(fileName, persistKey);
            return builder.AddGlobalSigningCredentials(rsaKey);
        }

        #endregion

    }
}
