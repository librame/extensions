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
using Microsoft.Extensions.Options;
using System;
using System.Text;

namespace Librame.Extensions.Encryption.Buffers
{
    using Core.Builders;

    internal class PlaintextBuffer : AlgorithmBuffer, IPlaintextBuffer
    {
        public PlaintextBuffer(IServiceProvider serviceProvider, string source)
            : base(serviceProvider, CreateBuffer(serviceProvider, source, out Encoding encoding))
        {
            Source = source;
            Encoding = encoding;
        }


        public string Source { get; }

        public Encoding Encoding { get; }


        private static byte[] CreateBuffer(IServiceProvider services, string source, out Encoding encoding)
        {
            var options = services.GetRequiredService<IOptions<CoreBuilderOptions>>().Value;
            encoding = options.Encoding;

            return source.FromEncodingString(encoding);
        }
    }
}
