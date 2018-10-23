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
using Newtonsoft.Json;
using System;
using System.IO;

namespace Librame.Extensions.Encryption.RsaKeySerializers
{
    using Services;

    /// <summary>
    /// JSON 文件 RSA 密钥序列化器。
    /// </summary>
    public class JsonFileRsaKeySerializer : AbstractService<JsonFileRsaKeySerializer>, IRsaKeySerializer
    {
        /// <summary>
        /// 构造一个 <see cref="JsonFileRsaKeySerializer"/> 实例。
        /// </summary>
        /// <param name="logger">给定的 <see cref="ILogger{JsonFileRsaKeySerializer}"/>。</param>
        public JsonFileRsaKeySerializer(ILogger<JsonFileRsaKeySerializer> logger)
            : base(logger)
        {
            FileExtension = ".json";
        }


        /// <summary>
        /// 文件扩展名。
        /// </summary>
        public virtual string FileExtension { get; }


        /// <summary>
        /// 反序列化。
        /// </summary>
        /// <param name="fileName">给定要读取的文件名。</param>
        /// <returns>返回 <see cref="RsaKeyParameters"/>。</returns>
        public virtual RsaKeyParameters Deserialize(string fileName)
        {
            Logger.LogInformation($"Load rsa key json file: {fileName}");

            try
            {
                var content = File.ReadAllText(fileName);
                Logger.LogDebug($"Load rsa key: {content}");

                return JsonConvert.DeserializeObject<RsaKeyParameters>(content, new JsonSerializerSettings
                {
                    ContractResolver = new RsaKeyContractResolver()
                });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.AsInnerMessage());
                throw ex;
            }
        }


        /// <summary>
        /// 序列化密钥参数。
        /// </summary>
        /// <param name="keyParameters">给定的 <see cref="RsaKeyParameters"/>。</param>
        /// <param name="fileName">给定要保存的文件名。</param>
        /// <returns>返回序列化字符串。</returns>
        public virtual string Serialize(RsaKeyParameters keyParameters, string fileName)
        {
            Logger.LogInformation($"Save rsa key json file: KeyId={keyParameters.KeyId}, FileName={fileName}");

            try
            {
                var content = JsonConvert.SerializeObject(keyParameters, new JsonSerializerSettings
                {
                    ContractResolver = new RsaKeyContractResolver()
                });
                Logger.LogDebug($"Save rsa key: {content}");

                File.WriteAllText(fileName, content);

                return content;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.AsInnerMessage());
                throw ex;
            }
        }

    }
}
