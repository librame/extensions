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
using System.Security.Cryptography;

namespace Librame.Extensions.Encryption
{
    using Core;

    class SymmetricService : ExtensionBuilderServiceBase<EncryptionBuilderOptions>, ISymmetricService
    {
        public SymmetricService(IKeyGenerator keyGenerator)
            : base(keyGenerator.CastTo<IKeyGenerator, ExtensionBuilderServiceBase<EncryptionBuilderOptions>>(nameof(keyGenerator)))
        {
            KeyGenerator = keyGenerator;
        }


        public IKeyGenerator KeyGenerator { get; }


        private IByteBuffer Encrypt(SymmetricAlgorithm algorithm, IByteBuffer buffer)
        {
            algorithm.Mode = CipherMode.ECB;
            algorithm.Padding = PaddingMode.PKCS7;

            var encryptor = algorithm.CreateEncryptor();

            return buffer.Change(memory =>
            {
                var bytes = memory.ToArray();
                bytes = encryptor.TransformFinalBlock(bytes, 0, bytes.Length);
                Logger.LogDebug($"Encrypt final block: {nameof(CipherMode)}={algorithm.Mode.ToString()}, {nameof(PaddingMode)}={algorithm.Padding.ToString()}");

                return bytes;
            });
        }

        private IByteBuffer Decrypt(SymmetricAlgorithm algorithm, IByteBuffer buffer)
        {
            algorithm.Mode = CipherMode.ECB;
            algorithm.Padding = PaddingMode.PKCS7;

            var encryptor = algorithm.CreateDecryptor();

            return buffer.Change(memory =>
            {
                var bytes = memory.ToArray();
                bytes = encryptor.TransformFinalBlock(bytes, 0, bytes.Length);
                Logger.LogDebug($"Decrypt final block: {nameof(CipherMode)}={algorithm.Mode.ToString()}, {nameof(PaddingMode)}={algorithm.Padding.ToString()}");

                return bytes;
            });
        }


        #region AES

        public IByteBuffer ToAes(IByteBuffer buffer, string identifier = null)
        {
            var algorithm = CreateAes(identifier);

            return Encrypt(algorithm, buffer);
        }

        public IByteBuffer FromAes(IByteBuffer buffer, string identifier = null)
        {
            var algorithm = CreateAes(identifier);

            return Decrypt(algorithm, buffer);
        }

        private Aes CreateAes(string identifier = null)
        {
            var algorithm = Aes.Create();
            algorithm.Key = KeyGenerator.GetAesKey(identifier);
            Logger.LogDebug($"Use AES algorithm");

            return algorithm;
        }

        #endregion


        #region DES

        public IByteBuffer ToDes(IByteBuffer buffer, string identifier = null)
        {
            var algorithm = CreateDes(identifier);

            return Encrypt(algorithm, buffer);
        }

        public IByteBuffer FromDes(IByteBuffer buffer, string identifier = null)
        {
            var algorithm = CreateDes(identifier);

            return Decrypt(algorithm, buffer);
        }

        private DES CreateDes(string identifier = null)
        {
            var algorithm = DES.Create();
            algorithm.Key = KeyGenerator.GetDesKey(identifier);
            Logger.LogDebug($"Use DES algorithm");

            return algorithm;
        }

        #endregion


        #region TripleDES

        public IByteBuffer ToTripleDes(IByteBuffer buffer, string identifier = null)
        {
            var algorithm = CreateTripleDes(identifier);

            return Encrypt(algorithm, buffer);
        }

        public IByteBuffer FromTripleDes(IByteBuffer buffer, string identifier = null)
        {
            var algorithm = CreateTripleDes(identifier);

            return Decrypt(algorithm, buffer);
        }

        private TripleDES CreateTripleDes(string identifier = null)
        {
            var algorithm = TripleDES.Create();
            algorithm.Key = KeyGenerator.GetTripleDesKey();
            Logger.LogDebug($"Use TripleDES algorithm");

            return algorithm;
        }

        #endregion

    }
}
