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
using Newtonsoft.Json;

namespace Librame.Resource.Serialization
{
    using Schema;
    using Utility;

    /// <summary>
    /// JSON 资源序列化器。
    /// </summary>
    public class JsonResourceSerializer : FileResourceSerializer, IJsonResourceSerializer
    {
        /// <summary>
        /// 构造一个 <see cref="JsonResourceSerializer"/> 实例。
        /// </summary>
        /// <param name="settings">给定的 <see cref="Adaptation.AdapterSettings"/>。</param>
        public JsonResourceSerializer(Adaptation.AdapterSettings settings)
            : base(settings)
        {
            // 默认缩进
            Formatting = Formatting.Indented;
        }


        /// <summary>
        /// 获取文件扩展名。
        /// </summary>
        public override string FileExtension
        {
            get { return ResourceSchema.FORMAT_EXTENSION_JSON; }
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
            return (ResourceSchema)content.FromJson(schemaType, true, SerializerSettings, Converters);
        }


        /// <summary>
        /// 序列化资源结构核心。
        /// </summary>
        /// <param name="schema">给定的 <see cref="ResourceSchema"/>。</param>
        /// <returns>返回字符串。</returns>
        protected override string SerializeSchemaCore(ResourceSchema schema)
        {
            return schema.AsJson(true, Formatting, SerializerSettings, Converters);
        }

        #endregion


        #region IJsonResourceSerializer

        /// <summary>
        /// JSON 格式化枚举。
        /// </summary>
        public Formatting Formatting { get; set; }

        /// <summary>
        /// 获取或设置 JSON 序列化自定义设置。
        /// </summary>
        public JsonSerializerSettings SerializerSettings { get; set; }

        /// <summary>
        /// 获取或设置 JSON 转换器数组。
        /// </summary>
        public JsonConverter[] Converters { get; set; }

        #endregion

    }
}
