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
using System.Linq;

namespace Librame.Extensions.Encryption.Buffers
{
    internal class AlgorithmBuffer : IAlgorithmBuffer
    {
        private byte[] _buffer;


        public AlgorithmBuffer(IServiceProvider serviceProvider, byte[] buffer)
        {
            _buffer = buffer.NotEmpty(nameof(buffer));

            ServiceProvider = serviceProvider.NotNull(nameof(serviceProvider));
        }


        public IServiceProvider ServiceProvider { get; }

        public ReadOnlyMemory<byte> CurrentBuffer
            => _buffer;


        public IAlgorithmBuffer ChangeBuffer(Func<byte[], byte[]> newBufferFactory)
            => ChangeBuffer(newBufferFactory?.Invoke(_buffer));

        public IAlgorithmBuffer ChangeBuffer(byte[] newBuffer)
        {
            // 允许清空但不能为空
            _buffer = newBuffer.NotNull(nameof(newBuffer));
            return this;
        }


        private bool Equals(AlgorithmBuffer other)
            => _buffer.SequenceEqual(other?._buffer);

        public bool Equals(IAlgorithmBuffer other)
        {
            if (other is AlgorithmBuffer buffer)
                return Equals(buffer);

            return _buffer.SequenceEqual(other.CurrentBuffer.ToArray());
        }

        public override bool Equals(object obj)
            => obj is AlgorithmBuffer other ? Equals(other) : false;


        public override int GetHashCode()
            => _buffer.GetHashCode();
    }
}
