#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Librame.Extensions.Core.Tokens
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class SecurityTokenProtector : ISecurityTokenProtector
    {
        public SecurityTokenProtector(ISecurityTokenKeyRing keyRing)
        {
            KeyRing = keyRing.NotNull(nameof(keyRing));
        }


        public ISecurityTokenKeyRing KeyRing { get; }


        public string Protect(string index, string data)
        {
            SecurityToken.TryGetToken(KeyRing[index], out var identifier);
            (byte[] key, byte[] vector) = GenerateAesParameters(identifier.ToReadOnlyMemory().ToArray());

            var buffer = data.FromEncodingString();
            buffer = buffer.AsAes(key, vector);

            return buffer.AsBase64String();
        }

        public string Unprotect(string index, string data)
        {
            SecurityToken.TryGetToken(KeyRing[index], out var identifier);
            (byte[] key, byte[] vector) = GenerateAesParameters(identifier.ToReadOnlyMemory().ToArray());

            var buffer = data.FromBase64String();
            buffer = buffer.FromAes(key, vector);

            return buffer.AsEncodingString();
        }


        private static (byte[] key, byte[] vector) GenerateAesParameters(byte[] initialBytes)
        {
            // 32
            var key = initialBytes.Concat(initialBytes.Reverse()).ToArray();
            // 16
            var vector = initialBytes.Reverse().ToArray();

            return (key, vector);
        }

    }
}
