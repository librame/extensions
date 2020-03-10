#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

namespace Librame.Extensions.Encryption.Services
{
    using Core.Services;
    using Encryption.Builders;
    using Encryption.KeyGenerators;

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class SymmetricService : AbstractExtensionBuilderService<EncryptionBuilderOptions>, ISymmetricService
    {
        private readonly Lazy<Aes> _aes;
        private readonly Lazy<DES> _des;
        private readonly Lazy<TripleDES> _3des;


        [SuppressMessage("Microsoft.Cryptography", "CA5351:DoNotUseBrokenCryptographicAlgorithms")]
        [SuppressMessage("Microsoft.Cryptography", "CA5350:DoNotUseWeakCryptographicAlgorithms")]
        public SymmetricService(IKeyGenerator keyGenerator)
            : base(keyGenerator.CastTo<IKeyGenerator, AbstractExtensionBuilderService<EncryptionBuilderOptions>>(nameof(keyGenerator)))
        {
            KeyGenerator = keyGenerator;

            _aes = new Lazy<Aes>(() =>
            {
                var algo = Aes.Create();
                algo.Key = KeyGenerator.GetAesKey();
                InitializeAlgorithm(algo);
                return algo;
            });

            _des = new Lazy<DES>(() =>
            {
                var algo = DES.Create();
                algo.Key = KeyGenerator.GetDesKey();
                InitializeAlgorithm(algo);
                return algo;
            });

            _3des = new Lazy<TripleDES>(() =>
            {
                var algo = TripleDES.Create();
                algo.Key = KeyGenerator.GetTripleDesKey();
                InitializeAlgorithm(algo);
                return algo;
            });
        }

        private void InitializeAlgorithm(SymmetricAlgorithm algorithm)
        {
            algorithm.Mode = CipherMode.ECB;
            algorithm.Padding = PaddingMode.PKCS7;
        }


        public IKeyGenerator KeyGenerator { get; }


        private byte[] Encrypt(SymmetricAlgorithm algorithm, byte[] buffer)
        {
            var encryptor = algorithm.CreateEncryptor();

            buffer = encryptor.TransformFinalBlock(buffer, 0, buffer.Length);
            Logger.LogDebug($"Encrypt final block: {nameof(CipherMode)}={algorithm.Mode.ToString()}, {nameof(PaddingMode)}={algorithm.Padding.ToString()}");

            return buffer;
        }

        private byte[] Decrypt(SymmetricAlgorithm algorithm, byte[] buffer)
        {
            var encryptor = algorithm.CreateDecryptor();

            buffer = encryptor.TransformFinalBlock(buffer, 0, buffer.Length);
            Logger.LogDebug($"Decrypt final block: {nameof(CipherMode)}={algorithm.Mode.ToString()}, {nameof(PaddingMode)}={algorithm.Padding.ToString()}");

            return buffer;
        }


        #region AES

        public byte[] EncryptAes(byte[] buffer, KeyDescriptor descriptor = null)
        {
            if (descriptor.IsNotNull())
            {
                var key = KeyGenerator.GetAesKey(descriptor);
                _aes.Value.Key = key;
            }

            return Encrypt(_aes.Value, buffer);
        }

        public byte[] DecryptAes(byte[] buffer, KeyDescriptor descriptor = null)
        {
            if (descriptor.IsNotNull())
            {
                var key = KeyGenerator.GetAesKey(descriptor);
                _aes.Value.Key = key;
            }

            return Decrypt(_aes.Value, buffer);
        }

        #endregion


        #region DES

        public byte[] EncryptDes(byte[] buffer, KeyDescriptor descriptor = null)
        {
            if (descriptor.IsNotNull())
            {
                var key = KeyGenerator.GetDesKey(descriptor);
                _des.Value.Key = key;
            }

            return Encrypt(_des.Value, buffer);
        }

        public byte[] DecryptDes(byte[] buffer, KeyDescriptor descriptor = null)
        {
            if (descriptor.IsNotNull())
            {
                var key = KeyGenerator.GetDesKey(descriptor);
                _des.Value.Key = key;
            }

            return Decrypt(_des.Value, buffer);
        }

        #endregion


        #region TripleDES

        public byte[] EncryptTripleDes(byte[] buffer, KeyDescriptor descriptor = null)
        {
            if (descriptor.IsNotNull())
            {
                var key = KeyGenerator.GetTripleDesKey(descriptor);
                _3des.Value.Key = key;
            }

            return Encrypt(_3des.Value, buffer);
        }

        public byte[] DecryptTripleDes(byte[] buffer, KeyDescriptor descriptor = null)
        {
            if (descriptor.IsNotNull())
            {
                var key = KeyGenerator.GetTripleDesKey(descriptor);
                _3des.Value.Key = key;
            }

            return Decrypt(_3des.Value, buffer);
        }

        #endregion

    }
}
