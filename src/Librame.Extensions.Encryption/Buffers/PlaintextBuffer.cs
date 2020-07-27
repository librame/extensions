#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Text;

namespace Librame.Extensions.Encryption.Buffers
{
    internal class PlaintextBuffer : AlgorithmBuffer, IPlaintextBuffer
    {
        public PlaintextBuffer(IServiceProvider serviceProvider, string source)
            : base(serviceProvider, CreateBuffer(source, out Encoding encoding))
        {
            Source = source;
            Encoding = encoding;
        }


        public string Source { get; }

        public Encoding Encoding { get; }


        private static byte[] CreateBuffer(string source, out Encoding encoding)
        {
            encoding = ExtensionSettings.Preference.DefaultEncoding;
            return source.FromEncodingString(encoding);
        }
    }
}
