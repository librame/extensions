#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions;
using Librame.Extensions.Encryption.Resources;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.IdentityModel.Tokens
{
    /// <summary>
    /// 抽象签名证书静态扩展。
    /// </summary>
    public static class AbstractionSigningCredentialsExtensions
    {
        /// <summary>
        /// 解析 RSA。
        /// </summary>
        /// <param name="credentials">给定的 <see cref="SigningCredentials"/>。</param>
        /// <param name="throwIfError">是否抛出异常（可选；默认抛出异常）。</param>
        /// <returns>返回 <see cref="RSA"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static RSA ResolveRsa(this SigningCredentials credentials, bool throwIfError = true)
        {
            credentials.NotNull(nameof(credentials));

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
                throw new NotSupportedException(InternalResource.NotSupportedExceptionSecurityKey);

            return null;
        }


        /// <summary>
        /// 解析证书。
        /// </summary>
        /// <param name="credentials">给定的 <see cref="SigningCredentials"/>。</param>
        /// <param name="throwIfError">是否抛出异常（可选；默认抛出异常）。</param>
        /// <returns>返回 <see cref="X509Certificate2"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static X509Certificate2 ResolveCertificate(this SigningCredentials credentials, bool throwIfError = true)
        {
            credentials.NotNull(nameof(credentials));
            
            if (credentials.Key is X509SecurityKey x509Key)
                return x509Key.Certificate;

            if (throwIfError)
                throw new NotSupportedException(InternalResource.NotSupportedExceptionSecurityKey);

            return null;
        }

    }
}
