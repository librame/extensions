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

namespace Librame.Extensions.Encryption.Services
{
    using Core.Services;
    using Core.Tokens;
    using Encryption.Builders;
    using Encryption.Generators;

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class SymmetricService : AbstractExtensionBuilderService<EncryptionBuilderOptions>, ISymmetricService
    {
        [SuppressMessage("Microsoft.Cryptography", "CA5351:DoNotUseBrokenCryptographicAlgorithms")]
        [SuppressMessage("Microsoft.Cryptography", "CA5350:DoNotUseWeakCryptographicAlgorithms")]
        public SymmetricService(IKeyGenerator keyGenerator, IVectorGenerator vectorGenerator)
            : base(keyGenerator.CastTo<IKeyGenerator, AbstractExtensionBuilderService<EncryptionBuilderOptions>>(nameof(keyGenerator)))
        {
            KeyGenerator = keyGenerator;
            VectorGenerator = vectorGenerator;
        }


        public IKeyGenerator KeyGenerator { get; }

        public IVectorGenerator VectorGenerator { get; }


        #region AES

        public byte[] EncryptAes(byte[] buffer, out byte[] key, out byte[] vector,
            SecurityToken token = null)
        {
            key = KeyGenerator.GetAesKey(token);
            vector = VectorGenerator.GetAesVector(key, token);

            return buffer.AsAes(key, vector);
        }

        public byte[] DecryptAes(byte[] buffer, out byte[] key, out byte[] vector,
            SecurityToken token = null)
        {
            key = KeyGenerator.GetAesKey(token);
            vector = VectorGenerator.GetAesVector(key, token);

            return buffer.FromAes(key, vector);
        }

        #endregion


        #region DES

        public byte[] EncryptDes(byte[] buffer, out byte[] key, out byte[] vector,
            SecurityToken token = null)
        {
            key = KeyGenerator.GetDesKey(token);
            vector = VectorGenerator.GetDesVector(key, token);

            return buffer.AsDes(key, vector);
        }

        public byte[] DecryptDes(byte[] buffer, out byte[] key, out byte[] vector,
            SecurityToken token = null)
        {
            key = KeyGenerator.GetDesKey(token);
            vector = VectorGenerator.GetDesVector(key, token);

            return buffer.FromDes(key, vector);
        }

        #endregion


        #region TripleDES

        public byte[] EncryptTripleDes(byte[] buffer, out byte[] key, out byte[] vector,
            SecurityToken token = null)
        {
            key = KeyGenerator.GetTripleDesKey(token);
            vector = VectorGenerator.GetTripleDesVector(key, token);

            return buffer.AsTripleDes(key, vector);
        }

        public byte[] DecryptTripleDes(byte[] buffer, out byte[] key, out byte[] vector,
            SecurityToken token = null)
        {
            key = KeyGenerator.GetTripleDesKey(token);
            vector = VectorGenerator.GetTripleDesVector(key, token);

            return buffer.FromTripleDes(key, vector);
        }

        #endregion

    }
}
