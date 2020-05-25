#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

namespace Librame.Extensions.Encryption
{
    using Core.Combiners;
    using Core.Utilities;

    /// <summary>
    /// RSA 安全密钥加载器。
    /// </summary>
    public static class RsaSecurityKeyHelper
    {
        /// <summary>
        /// 加载 RSA 安全密钥。
        /// </summary>
        /// <param name="fileName">给定的文件名。</param>
        /// <param name="persistKey">是否持久化密钥。</param>
        /// <returns>返回 <see cref="RsaSecurityKey"/>。</returns>
        public static RsaSecurityKey Load(string fileName, bool persistKey)
        {
            if (fileName.IsEmpty()) // 默认兼容 IdentityServer4 生成的临时密钥文件
                fileName = "tempkey.rsa".CombineCurrentDirectory();

            return ExtensionSettings.Preference.RunLockerResult(() =>
            {
                RsaSecurityKey securityKey;

                var filePath = fileName.AsFilePathCombiner();
                if (filePath.Exists())
                {
                    var tempKey = filePath.ReadJson<TemporaryRsaKey>(settings: new JsonSerializerSettings
                    {
                        ContractResolver = new RsaKeyContractResolver()
                    });

                    securityKey = CreateRsaSecurityKey(tempKey.Parameters, tempKey.KeyId);
                }
                else
                {
                    securityKey = Create();

                    RSAParameters parameters;

                    if (securityKey.Rsa.IsNotNull())
                        parameters = securityKey.Rsa.ExportParameters(includePrivateParameters: true);
                    else
                        parameters = securityKey.Parameters;

                    var tempKey = new TemporaryRsaKey
                    {
                        Parameters = parameters,
                        KeyId = securityKey.KeyId
                    };

                    if (persistKey)
                    {
                        filePath.WriteJson(tempKey, settings: new JsonSerializerSettings
                        {
                            ContractResolver = new RsaKeyContractResolver()
                        });
                    }
                }

                return securityKey;
            });
        }


        [SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope")]
        private static RsaSecurityKey Create()
        {
            RsaSecurityKey key;

            var rsa = RSA.Create();
            if (rsa is RSACryptoServiceProvider)
            {
                rsa.Dispose();

                using (var cng = new RSACng(2048))
                {
                    var parameters = cng.ExportParameters(includePrivateParameters: true);
                    key = new RsaSecurityKey(parameters);
                }
            }
            else
            {
                rsa.KeySize = 2048;
                key = new RsaSecurityKey(rsa);
            }

            var buffer = RandomUtility.GenerateByteArray(16);
            key.KeyId = buffer.AsBase64String();

            return key;
        }

        private static RsaSecurityKey CreateRsaSecurityKey(RSAParameters parameters, string id)
        {
            var key = new RsaSecurityKey(parameters)
            {
                KeyId = id
            };

            return key;
        }


        private class TemporaryRsaKey
        {
            public string KeyId { get; set; }

            public RSAParameters Parameters { get; set; }
        }

    }
}
