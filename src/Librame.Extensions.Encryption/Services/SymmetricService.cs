﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Encryption.Services
{
    using Core.Identifiers;
    using Core.Services;
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

        public byte[] EncryptAes(byte[] buffer, SecurityIdentifier identifier = null)
        {
            var key = KeyGenerator.GetAesKey(identifier);
            var vector = VectorGenerator.GetAesVector(key, identifier);

            return buffer.AsAes(key, vector);
        }

        public byte[] DecryptAes(byte[] buffer, SecurityIdentifier identifier = null)
        {
            var key = KeyGenerator.GetAesKey(identifier);
            var vector = VectorGenerator.GetAesVector(key, identifier);

            return buffer.FromAes(key, vector);
        }

        #endregion


        #region DES

        public byte[] EncryptDes(byte[] buffer, SecurityIdentifier identifier = null)
        {
            var key = KeyGenerator.GetDesKey(identifier);
            var vector = VectorGenerator.GetDesVector(key, identifier);

            return buffer.AsDes(key, vector);
        }

        public byte[] DecryptDes(byte[] buffer, SecurityIdentifier identifier = null)
        {
            var key = KeyGenerator.GetDesKey(identifier);
            var vector = VectorGenerator.GetDesVector(key, identifier);

            return buffer.FromDes(key, vector);
        }

        #endregion


        #region TripleDES

        public byte[] EncryptTripleDes(byte[] buffer, SecurityIdentifier identifier = null)
        {
            var key = KeyGenerator.GetTripleDesKey(identifier);
            var vector = VectorGenerator.GetTripleDesVector(key, identifier);

            return buffer.AsTripleDes(key, vector);
        }

        public byte[] DecryptTripleDes(byte[] buffer, SecurityIdentifier identifier = null)
        {
            var key = KeyGenerator.GetTripleDesKey(identifier);
            var vector = VectorGenerator.GetTripleDesVector(key, identifier);

            return buffer.FromTripleDes(key, vector);
        }

        #endregion

    }
}
