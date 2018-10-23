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
using System;

namespace Librame.Builders
{
    using Extensions;
    using Extensions.Encryption;
    using Extensions.Encryption.RsaKeySerializers;

    /// <summary>
    /// 加密构建器静态扩展。
    /// </summary>
    public static class EncryptionBuilderExtensions
    {

        /// <summary>
        /// 添加加密。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IBuilder"/>。</param>
        /// <param name="configuration">给定的 <see cref="IConfiguration"/>。</param>
        /// <param name="configureOptions">给定的 <see cref="Action{ICryptographyBuilderOptions}"/>。</param>
        /// <returns>返回 <see cref="IEncryptionBuilder"/>。</returns>
        public static IEncryptionBuilder AddEncryption(this IBuilder builder,
            IConfiguration configuration = null, Action<IEncryptionBuilderOptions> configureOptions = null)
        {
            return builder.AddEncryption<DefaultEncryptionBuilderOptions>(configuration, configureOptions);
        }
        /// <summary>
        /// 添加加密。
        /// </summary>
        /// <typeparam name="TBuilderOptions">指定的构建器选项类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IBuilder"/>。</param>
        /// <param name="configuration">给定的 <see cref="IConfiguration"/>。</param>
        /// <param name="configureOptions">给定的 <see cref="Action{TBuilderOptions}"/>。</param>
        /// <returns>返回 <see cref="IEncryptionBuilder"/>。</returns>
        public static IEncryptionBuilder AddEncryption<TBuilderOptions>(this IBuilder builder,
            IConfiguration configuration = null, Action<TBuilderOptions> configureOptions = null)
            where TBuilderOptions : class, IEncryptionBuilderOptions
        {
            if (configuration.IsNotDefault())
                builder.Services.Configure<TBuilderOptions>(configuration);

            if (configureOptions.IsNotDefault())
                builder.Services.Configure(configureOptions);

            var encryptionBuilder = builder.AsEncryptionBuilder();

            encryptionBuilder.AddKeyGenerator()
                .AddRsaAlgorithm()
                .AddHashAlgorithm()
                .AddSymmetricAlgorithm()
                .AddConverter()
                .AddBuffer();

            return encryptionBuilder;
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
        public static IEncryptionBuilder AddKeyGenerator(this IEncryptionBuilder builder)
        {
            builder.Services.AddSingleton<IRsaKeySerializer, JsonFileRsaKeySerializer>();
            builder.Services.AddSingleton<IRsaKeySerializer, PemFileRsaKeySerializer>();
            builder.Services.AddSingleton<IRsaKeySerializer, XmlFileRsaKeySerializer>();

            builder.Services.AddSingleton<IKeyGenerator, InternalKeyGenerator>();
            builder.Services.AddSingleton<IRsaKeyGenerator, InternalRsaKeyGenerator>();

            return builder;
        }

        /// <summary>
        /// 添加散列算法。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IEncryptionBuilder"/>。</param>
        /// <returns>返回 <see cref="IEncryptionBuilder"/>。</returns>
        public static IEncryptionBuilder AddHashAlgorithm(this IEncryptionBuilder builder)
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
        public static IEncryptionBuilder AddRsaAlgorithm(this IEncryptionBuilder builder)
        {
            builder.Services.AddSingleton<IRsaAlgorithmService, InternalRsaAlgorithmService>();

            return builder;
        }

        /// <summary>
        /// 添加对称算法。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IEncryptionBuilder"/>。</param>
        /// <returns>返回 <see cref="IEncryptionBuilder"/>。</returns>
        public static IEncryptionBuilder AddSymmetricAlgorithm(this IEncryptionBuilder builder)
        {
            builder.Services.AddSingleton<ISymmetricAlgorithmService, InternalSymmetricAlgorithmService>();

            return builder;
        }

        /// <summary>
        /// 添加转换器。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IEncryptionBuilder"/>。</param>
        /// <returns>返回 <see cref="IEncryptionBuilder"/>。</returns>
        public static IEncryptionBuilder AddConverter(this IEncryptionBuilder builder)
        {
            builder.Services.AddSingleton<ICiphertextAlgorithmConverter, DefaultCiphertextAlgorithmConverter>();
            builder.Services.AddSingleton<IPlaintextAlgorithmConverter, DefaultPlaintextAlgorithmConverter>();

            return builder;
        }

        /// <summary>
        /// 添加缓冲区。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IEncryptionBuilder"/>。</param>
        /// <returns>返回 <see cref="IEncryptionBuilder"/>。</returns>
        public static IEncryptionBuilder AddBuffer(this IEncryptionBuilder builder)
        {
            builder.Services.AddSingleton(typeof(IEncryptionBuffer<,>), typeof(InternalEncryptionBuffer<,>));

            return builder;
        }

    }
}
