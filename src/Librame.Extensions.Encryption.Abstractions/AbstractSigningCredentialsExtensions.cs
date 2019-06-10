#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions;
using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// 抽象签名证书静态扩展。
    /// </summary>
    public static class AbstractSigningCredentialsExtensions
    {

        /// <summary>
        /// 解析 RSA。
        /// </summary>
        /// <param name="credentials">给定的 <see cref="SigningCredentials"/>。</param>
        /// <param name="throwIfError">是否抛出异常（可选；默认抛出异常）。</param>
        /// <returns>返回 <see cref="RSA"/>。</returns>
        public static RSA ResolveRsa(this SigningCredentials credentials, bool throwIfError = true)
        {
            if (credentials.IsNull() && throwIfError)
                throw new ArgumentException($"Have you registered the IEncryptionBuilder.AddSigningCredentials()");

            if (credentials.Key is X509SecurityKey x509Key)
                return x509Key.PrivateKey as RSA;

            if (credentials.Key is RsaSecurityKey rsaKey)
            {
                if (rsaKey.Rsa.IsNull())
                {
                    var rsa = RSA.Create();
                    rsa.ImportParameters(rsaKey.Parameters);

                    return rsa;
                }

                return rsaKey.Rsa;
            }

            if (throwIfError)
                throw new ArgumentException("Creation of RSA is not supported");

            return null;
        }


        /// <summary>
        /// 解析证书。
        /// </summary>
        /// <param name="credentials">给定的 <see cref="SigningCredentials"/>。</param>
        /// <param name="throwIfError">是否抛出异常（可选；默认抛出异常）。</param>
        /// <returns>返回 <see cref="X509Certificate2"/>。</returns>
        public static X509Certificate2 ResolveCertificate(this SigningCredentials credentials, bool throwIfError = true)
        {
            if (credentials.IsNull() && throwIfError)
                throw new ArgumentException($"Have you registered the IEncryptionBuilder.AddSigningCredentials()");
            
            if (credentials.Key is X509SecurityKey x509Key)
                return x509Key.Certificate;

            if (throwIfError)
                throw new ArgumentException("Creation of RSA is not supported");

            return null;
        }

    }
}
