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

    class EncryptionBuffer<TConverter, TSource> : ByteBuffer, IEncryptionBuffer<TConverter, TSource>
        where TConverter : IAlgorithmConverter<TSource>
    {
        public EncryptionBuffer(TConverter converter, TSource source)
            : this(converter, source, converter.To(source))
        {
        }

        internal EncryptionBuffer(TConverter converter, TSource source, IByteBuffer buffer)
            : base(buffer.Memory)
        {
            Converter = converter;
            Source = source;
        }


        public TConverter Converter { get; }

        public TSource Source { get; }


        private IServiceProvider _serviceProvider;

        public IServiceProvider ServiceProvider
            => _serviceProvider.NotNull(nameof(_serviceProvider));


        public IEncryptionBuffer<TConverter, TSource> ApplyServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            return this;
        }


        public new IEncryptionBuffer<TConverter, TSource> Copy()
        {
            var buffer = new EncryptionBuffer<TConverter, TSource>(Converter, Source, this);

            if (_serviceProvider.IsNotNull())
                buffer.ApplyServiceProvider(_serviceProvider);

            return buffer;
        }

    }
}
