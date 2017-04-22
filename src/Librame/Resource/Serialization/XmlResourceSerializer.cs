#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.IO;
using System.Xml.Serialization;

namespace Librame.Resource.Serialization
{
    using Schema;

    /// <summary>
    /// XML 资源序列化器。
    /// </summary>
    public class XmlResourceSerializer : FileResourceSerializer, IXmlResourceSerializer
    {
        /// <summary>
        /// 构造一个 <see cref="XmlResourceSerializer"/> 实例。
        /// </summary>
        /// <param name="settings">给定的 <see cref="Adaptation.AdapterSettings"/>。</param>
        public XmlResourceSerializer(Adaptation.AdapterSettings settings)
            : base(settings)
        {
            // 默认空间和前缀
            Namespaces = new XmlSerializerNamespaces();
            Namespaces.Add(string.Empty, string.Empty);
        }


        /// <summary>
        /// 获取文件扩展名。
        /// </summary>
        public override string FileExtension
        {
            get { return ResourceSchema.FORMAT_EXTENSION_XML; }
        }


        #region IResourceSerializer

        /// <summary>
        /// 反序列化资源结构核心。
        /// </summary>
        /// <param name="content">给定的结构内容。</param>
        /// <param name="schemaType">给定的资源结构类型。</param>
        /// <returns>返回 <see cref="ResourceSchema"/>。</returns>
        protected override ResourceSchema DeserializeSchemaCore(string content, Type schemaType)
        {
            using (var sr = new StringReader(content))
            {
                var xs = new XmlSerializer(schemaType);
                return (ResourceSchema)xs.Deserialize(sr);
            }
        }


        /// <summary>
        /// 序列化资源结构核心。
        /// </summary>
        /// <param name="schema">给定的 <see cref="ResourceSchema"/>。</param>
        /// <returns>返回字符串。</returns>
        protected override string SerializeSchemaCore(ResourceSchema schema)
        {
            byte[] buffer = null;
            using (var ms = new MemoryStream())
            {
                var xs = new XmlSerializer(schema.GetType());
                // 序列化
                xs.Serialize(ms, schema, Namespaces);

                buffer = ms.ToArray();
            }

            if (ReferenceEquals(buffer, null))
                return string.Empty;

            return Settings.Encoding.GetString(buffer, 0, buffer.Length);
        }

        #endregion


        #region IXmlResourceSerializer

        /// <summary>
        /// XML 命名空间和前缀。
        /// </summary>
        public XmlSerializerNamespaces Namespaces { get; set; }

        #endregion

    }
}
