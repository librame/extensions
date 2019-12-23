#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography.X509Certificates;

namespace Librame.Extensions.Encryption.Builders
{
    using Resources;
    using Services;

    /// <summary>
    /// 签名证书加密构建器静态扩展。
    /// </summary>
    public static class SigningCredentialsEncryptionBuilderExtensions
    {
        /// <summary>
        /// 添加签名证书集合。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IEncryptionBuilder"/>。</param>
        /// <param name="credentials">给定的签名证书集合。</param>
        /// <returns>返回 <see cref="IEncryptionBuilder"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "builder")]
        public static IEncryptionBuilder AddSigningCredentials(this IEncryptionBuilder builder,
            params KeyValuePair<string, SigningCredentials>[] credentials)
        {
            builder.NotNull(nameof(builder));

            foreach (var cred in credentials)
            {
                if (!(cred.Value.Key is AsymmetricSecurityKey
                    || cred.Value.Key is JsonWebKey && ((JsonWebKey)cred.Value.Key).HasPrivateKey))
                    throw new InvalidOperationException(InternalResource.InvalidOperationExceptionInvalidSigningKey);
            }

            builder.Services.AddSingleton<ISigningCredentialsService>(provider =>
            {
                var options = provider.GetRequiredService<IOptions<EncryptionBuilderOptions>>();
                var loggerFactory = provider.GetRequiredService<ILoggerFactory>();

                return new SigningCredentialsService(credentials, options, loggerFactory);
            });

            return builder;
        }

        /// <summary>
        /// 添加全局签名证书。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IEncryptionBuilder"/>。</param>
        /// <param name="credentials">给定的 <see cref="SigningCredentials"/>。</param>
        /// <returns>返回 <see cref="IEncryptionBuilder"/>。</returns>
        public static IEncryptionBuilder AddGlobalSigningCredentials(this IEncryptionBuilder builder, SigningCredentials credentials)
            => builder.AddSigningCredentials(new KeyValuePair<string, SigningCredentials>(EncryptionBuilderOptions.GlobalSigningCredentialsKey, credentials));

        /// <summary>
        /// 添加全局签名证书。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IEncryptionBuilder"/>。</param>
        /// <param name="certificate">给定的 <see cref="X509Certificate2"/>。</param>
        /// <returns>返回 <see cref="IEncryptionBuilder"/>。</returns>
        public static IEncryptionBuilder AddGlobalSigningCredentials(this IEncryptionBuilder builder, X509Certificate2 certificate)
        {
            if (!certificate.NotNull(nameof(certificate)).HasPrivateKey)
                throw new InvalidOperationException(InternalResource.InvalidOperationExceptionNotHavePrivateKeyFormat.Format(nameof(X509Certificate2)));

            var credentials = new SigningCredentials(new X509SecurityKey(certificate), SecurityAlgorithms.RsaSha256);
            return builder.AddGlobalSigningCredentials(credentials);
        }

        /// <summary>
        /// 添加全局签名证书。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IEncryptionBuilder"/>。</param>
        /// <param name="rsaKey">给定的 <see cref="RsaSecurityKey"/>。</param>
        /// <returns>返回 <see cref="IEncryptionBuilder"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "rsaKey")]
        public static IEncryptionBuilder AddGlobalSigningCredentials(this IEncryptionBuilder builder, RsaSecurityKey rsaKey)
        {
            rsaKey.NotNull(nameof(rsaKey));

            if (rsaKey.PrivateKeyStatus == PrivateKeyStatus.DoesNotExist)
                throw new InvalidOperationException(InternalResource.InvalidOperationExceptionNotHavePrivateKeyFormat.Format(nameof(RsaSecurityKey)));

            var credential = new SigningCredentials(rsaKey, SecurityAlgorithms.RsaSha256);
            return builder.AddGlobalSigningCredentials(credential);
        }

        /// <summary>
        /// 添加开发者全局签名证书（默认原生兼容 IdentityServer4 生成的临时密钥文件）。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IEncryptionBuilder"/>。</param>
        /// <param name="persistKey">是否持久化密钥（可选；默认持久化）。</param>
        /// <param name="fileName">给定的文件名（可选；默认兼容 IdentityServer4 生成的临时密钥文件）。</param>
        /// <returns>返回 <see cref="IEncryptionBuilder"/>。</returns>
        public static IEncryptionBuilder AddDeveloperGlobalSigningCredentials(this IEncryptionBuilder builder,
            bool persistKey = true, string fileName = null)
        {
            var rsaKey = RsaSecurityKeyLoader.LoadRsaSecurityKey(fileName, persistKey);
            return builder.AddGlobalSigningCredentials(rsaKey);
        }

    }
}
