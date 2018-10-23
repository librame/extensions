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
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Librame.Extensions.Encryption.RsaKeySerializers
{
    using Services;

    /// <summary>
    /// XML 文件 RSA 密钥序列化器。
    /// </summary>
    public class XmlFileRsaKeySerializer : AbstractService<XmlFileRsaKeySerializer>, IRsaKeySerializer
    {
        /// <summary>
        /// 构造一个 <see cref="XmlFileRsaKeySerializer"/> 实例。
        /// </summary>
        /// <param name="logger">给定的 <see cref="ILogger{XmlFileRsaKeySerializer}"/>。</param>
        public XmlFileRsaKeySerializer(ILogger<XmlFileRsaKeySerializer> logger)
            : base(logger)
        {
            FileExtension = ".xml";
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
            Logger.LogInformation($"Load rsa key xml file: {fileName}");

            try
            {
                var doc = new XmlDocument();
                doc.Load(fileName);
                Logger.LogDebug($"Load rsa key: {doc.DocumentElement.OuterXml}");

                var parameters = new RSAParameters();
                parameters.Modulus = doc.DocumentElement.SelectSingleNode("/RSAKeyValue/Modulus").InnerText.FromBase64String();
                parameters.Exponent = doc.DocumentElement.SelectSingleNode("/RSAKeyValue/Exponent").InnerText.FromBase64String();

                if (doc.DocumentElement.ChildNodes.Count > 2)
                {
                    // Private Key
                    parameters.P = doc.DocumentElement.SelectSingleNode("/RSAKeyValue/P").InnerText.FromBase64String();
                    parameters.Q = doc.DocumentElement.SelectSingleNode("/RSAKeyValue/Q").InnerText.FromBase64String();
                    parameters.DP = doc.DocumentElement.SelectSingleNode("/RSAKeyValue/DP").InnerText.FromBase64String();
                    parameters.DQ = doc.DocumentElement.SelectSingleNode("/RSAKeyValue/DQ").InnerText.FromBase64String();
                    parameters.InverseQ = doc.DocumentElement.SelectSingleNode("/RSAKeyValue/InverseQ").InnerText.FromBase64String();
                    parameters.D = doc.DocumentElement.SelectSingleNode("/RSAKeyValue/D").InnerText.FromBase64String();
                }

                var keyId = doc.DocumentElement.SelectSingleNode("/RSAKeyValue/KeyId").InnerText;
                return new RsaKeyParameters
                {
                    KeyId = keyId.AsValueOrDefault(() => UniqueIdentifier.NewByRng(16).ToString()),
                    Parameters = parameters
                };
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
            var sb = new StringBuilder();

            sb.Append("<RSAKeyValue>");

            sb.AppendFormat("<KeyId>{0}</KeyId>", keyParameters.KeyId);

            sb.AppendFormat("<Modulus>{0}</Modulus>", keyParameters.Parameters.Modulus.AsBase64String());
            sb.AppendFormat("<Exponent>{0}</Exponent>", keyParameters.Parameters.Exponent.AsBase64String());

            if (keyParameters.Parameters.P.IsNotEmpty())
            {
                // Private Key
                sb.AppendFormat("<P>{0}</P>", keyParameters.Parameters.P.AsBase64String());
                sb.AppendFormat("<Q>{0}</Q>", keyParameters.Parameters.Q.AsBase64String());
                sb.AppendFormat("<DP>{0}</DP>", keyParameters.Parameters.DP.AsBase64String());
                sb.AppendFormat("<DQ>{0}</DQ>", keyParameters.Parameters.DQ.AsBase64String());
                sb.AppendFormat("<InverseQ>{0}</InverseQ>", keyParameters.Parameters.InverseQ.AsBase64String());
                sb.AppendFormat("<D>{0}</D>", keyParameters.Parameters.D.AsBase64String());
            }

            sb.Append("</RSAKeyValue>");
            
            try
            {

                var content = sb.ToString();
                Logger.LogDebug($"Save rsa key: {content}");

                var doc = new XmlDocument();
                doc.LoadXml(content);
                doc.Save(fileName);
                Logger.LogInformation($"Save rsa key xml file: KeyId={keyParameters.KeyId}, FileName={fileName}");

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
