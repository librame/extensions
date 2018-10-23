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
    using Buffers;

    /// <summary>
    /// 内部加密缓冲区。
    /// </summary>
    /// <typeparam name="TConverter">指定的转换器类型。</typeparam>
    /// <typeparam name="TSource">指定的来源类型。</typeparam>
    internal class InternalEncryptionBuffer<TConverter, TSource> : DefaultBuffer<byte>, IEncryptionBuffer<TConverter, TSource>
        where TConverter : IAlgorithmConverter<TSource>
    {
        /// <summary>
        /// 构造一个 <see cref="InternalEncryptionBuffer{TConverter, TSource}"/> 实例。
        /// </summary>
        /// <param name="converter">给定的转换器。</param>
        /// <param name="source">给定的来源实例。</param>
        public InternalEncryptionBuffer(TConverter converter, TSource source)
            : this(converter, source, converter.ToResult(source).Memory)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="InternalEncryptionBuffer{TConverter, TSource}"/> 实例。
        /// </summary>
        /// <param name="converter">给定的转换器。</param>
        /// <param name="source">给定的来源实例。</param>
        /// <param name="memory">给定的存储器。</param>
        internal InternalEncryptionBuffer(TConverter converter, TSource source, Memory<byte> memory)
        {
            Converter = converter;
            Source = source;

            ChangeMemory(memory);
        }


        /// <summary>
        /// 转换器。
        /// </summary>
        public TConverter Converter { get; }

        /// <summary>
        /// 来源实例。
        /// </summary>
        public TSource Source { get; }


        private IServiceProvider _serviceProvider;
        /// <summary>
        /// 服务提供程序。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// ServiceProvider is null. Make sure of used <see cref="ApplyServiceProvider(IServiceProvider)"/>。
        /// </exception>
        /// <value>
        /// 返回 <see cref="IServiceProvider"/> 或抛出空异常。
        /// </value>
        public IServiceProvider ServiceProvider
        {
            get { return _serviceProvider.NotDefault(nameof(_serviceProvider)); }
        }


        /// <summary>
        /// 应用服务提供程序。
        /// </summary>
        /// <param name="serviceProvider">给定的 <see cref="IServiceProvider"/>。</param>
        /// <returns>返回 <see cref="IEncryptionBuffer{TConverter, TSource}"/>。</returns>
        public IEncryptionBuffer<TConverter, TSource> ApplyServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            return this;
        }


        /// <summary>
        /// 创建副本。
        /// </summary>
        /// <returns>返回 <see cref="IEncryptionBuffer{TConverter, TSource}"/>。</returns>
        public new IEncryptionBuffer<TConverter, TSource> Copy()
        {
            return new InternalEncryptionBuffer<TConverter, TSource>(Converter, Source, Memory)
                .ApplyServiceProvider(ServiceProvider);
        }

    }
}
