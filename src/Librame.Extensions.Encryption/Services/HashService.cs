#region License

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
    using Core.Services;
    using Encryption.Builders;

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class HashService : AbstractExtensionBuilderService<EncryptionBuilderOptions>, IHashService
    {
        public HashService(IRsaService rsa)
            : base(rsa.CastTo<IRsaService, AbstractExtensionBuilderService<EncryptionBuilderOptions>>(nameof(rsa)))
        {
            Rsa = rsa;
        }


        public IRsaService Rsa { get; }


        public byte[] Md5(byte[] buffer, bool isSigned = false)
            => buffer.Md5(isSigned, Rsa.Source, Rsa.SignaturePadding);

        public byte[] Sha1(byte[] buffer, bool isSigned = false)
            => buffer.Sha1(isSigned, Rsa.Source, Rsa.SignaturePadding);

        public byte[] Sha256(byte[] buffer, bool isSigned = false)
            => buffer.Sha256(isSigned, Rsa.Source, Rsa.SignaturePadding);

        public byte[] Sha384(byte[] buffer, bool isSigned = false)
            => buffer.Sha384(isSigned, Rsa.Source, Rsa.SignaturePadding);

        public byte[] Sha512(byte[] buffer, bool isSigned = false)
            => buffer.Sha512(isSigned, Rsa.Source, Rsa.SignaturePadding);
    }
}
