#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;

namespace Librame.Extensions.Encryption
{
    using Core;

    /// <summary>
    /// 加密缓冲区接口。
    /// </summary>
    /// <typeparam name="TConverter">指定的转换器类型。</typeparam>
    /// <typeparam name="TSource">指定的来源类型。</typeparam>
    public interface IEncryptionBuffer<TConverter, TSource> : IByteMemoryBuffer
        where TConverter : IByteMemoryBufferToConverter<TSource>
    {
        /// <summary>
        /// 转换器。
        /// </summary>
        TConverter Converter { get; }


        /// <summary>
        /// 服务提供程序。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// ServiceProvider is null. Make sure of used <see cref="ApplyServiceProvider(IServiceProvider)"/>.
        /// </exception>
        /// <value>
        /// 返回 <see cref="IServiceProvider"/> 或抛出空异常。
        /// </value>
        IServiceProvider ServiceProvider { get; }


        /// <summary>
        /// 应用服务提供程序。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// serviceProvider is null.
        /// </exception>
        /// <param name="serviceProvider">给定的 <see cref="IServiceProvider"/>。</param>
        /// <returns>返回 <see cref="IEncryptionBuffer{TConverter, TSource}"/>。</returns>
        IEncryptionBuffer<TConverter, TSource> ApplyServiceProvider(IServiceProvider serviceProvider);


        /// <summary>
        /// 创建副本。
        /// </summary>
        /// <returns>返回 <see cref="IEncryptionBuffer{TConverter, TSource}"/>。</returns>
        IEncryptionBuffer<TConverter, TSource> Copy();
    }
}
