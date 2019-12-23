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
using System;
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Encryption.Buffers
{
    using Services;

    /// <summary>
    /// 抽象缓冲区静态扩展。
    /// </summary>
    public static class AbstractionBufferExtensions
    {
        /// <summary>
        /// 使用散列服务。
        /// </summary>
        /// <typeparam name="TAlgorithmBuffer">指定的算法缓冲区类型。</typeparam>
        /// <param name="algorithmBuffer">给定的 <typeparamref name="TAlgorithmBuffer"/>。</param>
        /// <param name="newBufferFactory">给定的新缓冲区工厂方法。</param>
        /// <returns>返回 <typeparamref name="TAlgorithmBuffer"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static TAlgorithmBuffer UseHash<TAlgorithmBuffer>(this TAlgorithmBuffer algorithmBuffer,
            Func<IHashService, byte[], byte[]> newBufferFactory)
            where TAlgorithmBuffer : IAlgorithmBuffer
        {
            algorithmBuffer.NotNull(nameof(algorithmBuffer));
            newBufferFactory.NotNull(nameof(newBufferFactory));

            algorithmBuffer.ChangeBuffer(buffer =>
            {
                var hash = algorithmBuffer.ServiceProvider.GetRequiredService<IHashService>();
                return newBufferFactory.Invoke(hash, buffer);
            });

            return algorithmBuffer;
        }

        /// <summary>
        /// 使用键控散列服务。
        /// </summary>
        /// <typeparam name="TAlgorithmBuffer">指定的算法缓冲区类型。</typeparam>
        /// <param name="algorithmBuffer">给定的 <typeparamref name="TAlgorithmBuffer"/>。</param>
        /// <param name="newBufferFactory">给定的新缓冲区工厂方法。</param>
        /// <returns>返回 <typeparamref name="TAlgorithmBuffer"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static TAlgorithmBuffer UseKeyedHash<TAlgorithmBuffer>(this TAlgorithmBuffer algorithmBuffer,
            Func<IKeyedHashService, byte[], byte[]> newBufferFactory)
            where TAlgorithmBuffer : IAlgorithmBuffer
        {
            algorithmBuffer.NotNull(nameof(algorithmBuffer));
            newBufferFactory.NotNull(nameof(newBufferFactory));

            algorithmBuffer.ChangeBuffer(buffer =>
            {
                var keyedHash = algorithmBuffer.ServiceProvider.GetRequiredService<IKeyedHashService>();
                return newBufferFactory.Invoke(keyedHash, buffer);
            });

            return algorithmBuffer;
        }

        /// <summary>
        /// 使用对称算法服务。
        /// </summary>
        /// <typeparam name="TAlgorithmBuffer">指定的算法缓冲区类型。</typeparam>
        /// <param name="algorithmBuffer">给定的 <typeparamref name="TAlgorithmBuffer"/>。</param>
        /// <param name="newBufferFactory">给定的新缓冲区工厂方法。</param>
        /// <returns>返回 <typeparamref name="TAlgorithmBuffer"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static TAlgorithmBuffer UseSymmetric<TAlgorithmBuffer>(this TAlgorithmBuffer algorithmBuffer,
            Func<ISymmetricService, byte[], byte[]> newBufferFactory)
            where TAlgorithmBuffer : IAlgorithmBuffer
        {
            algorithmBuffer.NotNull(nameof(algorithmBuffer));
            newBufferFactory.NotNull(nameof(newBufferFactory));

            algorithmBuffer.ChangeBuffer(buffer =>
            {
                var symmetric = algorithmBuffer.ServiceProvider.GetRequiredService<ISymmetricService>();
                return newBufferFactory.Invoke(symmetric, buffer);
            });

            return algorithmBuffer;
        }

        /// <summary>
        /// 使用 RSA 非对称算法服务。
        /// </summary>
        /// <typeparam name="TAlgorithmBuffer">指定的算法缓冲区类型。</typeparam>
        /// <param name="algorithmBuffer">给定的 <typeparamref name="TAlgorithmBuffer"/>。</param>
        /// <param name="newBufferFactory">给定的新缓冲区工厂方法。</param>
        /// <returns>返回 <typeparamref name="TAlgorithmBuffer"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static TAlgorithmBuffer UseRsa<TAlgorithmBuffer>(this TAlgorithmBuffer algorithmBuffer,
            Func<IRsaService, byte[], byte[]> newBufferFactory)
            where TAlgorithmBuffer : IAlgorithmBuffer
        {
            algorithmBuffer.NotNull(nameof(algorithmBuffer));
            newBufferFactory.NotNull(nameof(newBufferFactory));

            algorithmBuffer.ChangeBuffer(buffer =>
            {
                var symmetric = algorithmBuffer.ServiceProvider.GetRequiredService<IRsaService>();
                return newBufferFactory.Invoke(symmetric, buffer);
            });

            return algorithmBuffer;
        }
    }
}
