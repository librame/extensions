#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IO;
using System.Security.Cryptography;

namespace Librame.Extensions.Encryption
{
    using Core;

    /// <summary>
    /// RSA 安全密钥加载器。
    /// </summary>
    public class RsaSecurityKeyLoader
    {
        /// <summary>
        /// 加载 RSA 安全密钥。
        /// </summary>
        /// <param name="fileName">给定的文件名。</param>
        /// <param name="persistKey">是否持久化密钥。</param>
        /// <returns>返回 <see cref="RsaSecurityKey"/>。</returns>
        public static RsaSecurityKey LoadRsaSecurityKey(string fileName, bool persistKey)
        {
            if (fileName.IsEmpty()) // 默认与 IdentityServer4 同时路径及文件名
                fileName = Directory.GetCurrentDirectory().CombinePath("tempkey.rsa");

            RsaSecurityKey key;

            if (File.Exists(fileName))
            {
                var keyFile = File.ReadAllText(fileName);
                var tempKey = JsonConvert.DeserializeObject<TemporaryRsaKey>(keyFile, new JsonSerializerSettings
                {
                    ContractResolver = new RsaKeyContractResolver()
                });

                key = CreateRsaSecurityKey(tempKey.Parameters, tempKey.KeyId);
            }
            else
            {
                key = CreateRsaSecurityKey();

                RSAParameters parameters;

                if (key.Rsa.IsNotNull())
                    parameters = key.Rsa.ExportParameters(includePrivateParameters: true);
                else
                    parameters = key.Parameters;

                var tempKey = new TemporaryRsaKey
                {
                    Parameters = parameters,
                    KeyId = key.KeyId
                };

                if (persistKey)
                {
                    File.WriteAllText(fileName, JsonConvert.SerializeObject(tempKey, new JsonSerializerSettings
                    {
                        ContractResolver = new RsaKeyContractResolver()
                    }));
                }
            }

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

        private static RsaSecurityKey CreateRsaSecurityKey()
        {
            var rsa = RSA.Create();
            RsaSecurityKey key;

            if (rsa is RSACryptoServiceProvider)
            {
                rsa.Dispose();
                var cng = new RSACng(2048);

                var parameters = cng.ExportParameters(includePrivateParameters: true);
                key = new RsaSecurityKey(parameters);
            }
            else
            {
                rsa.KeySize = 2048;
                key = new RsaSecurityKey(rsa);
            }

            key.KeyId = RandomNumberAlgorithmIdentifier.New(16);
            return key;
        }


        private class TemporaryRsaKey
        {
            public string KeyId { get; set; }

            public RSAParameters Parameters { get; set; }
        }

    }
}
