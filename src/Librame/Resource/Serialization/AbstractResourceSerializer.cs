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

namespace Librame.Resource.Serialization
{
    using Schema;
    using Utility;

    /// <summary>
    /// 抽象资源序列化器。
    /// </summary>
    public abstract class AbstractResourceSerializer : IResourceSerializer
    {
        /// <summary>
        /// 获取或设置 <see cref="Adaptation.AdapterSettings"/>。
        /// </summary>
        public Adaptation.AdapterSettings Settings { get; }

        /// <summary>
        /// 构造一个 <see cref="AbstractResourceSerializer"/> 实例。
        /// </summary>
        /// <param name="settings">给定的 <see cref="Adaptation.AdapterSettings"/>。</param>
        public AbstractResourceSerializer(Adaptation.AdapterSettings settings)
        {
            Settings = settings.NotNull(nameof(settings));
        }


        /// <summary>
        /// 反序列化资源结构。
        /// </summary>
        /// <param name="content">给定的结构内容。</param>
        /// <param name="schemaType">给定的资源结构类型。</param>
        /// <returns>返回 <see cref="ResourceSchema"/>。</returns>
        public virtual ResourceSchema DeserializeSchema(string content, Type schemaType)
        {
            return DeserializeSchemaCore(content, schemaType);
        }
        /// <summary>
        /// 反序列化资源结构核心。
        /// </summary>
        /// <param name="content">给定的结构内容。</param>
        /// <param name="schemaType">给定的资源结构类型。</param>
        /// <returns>返回 <see cref="ResourceSchema"/>。</returns>
        protected abstract ResourceSchema DeserializeSchemaCore(string content, Type schemaType);


        /// <summary>
        /// 序列化资源结构。
        /// </summary>
        /// <param name="schema">给定的 <see cref="ResourceSchema"/>。</param>
        /// <returns>返回字符串。</returns>
        public virtual string SerializeSchema(ResourceSchema schema)
        {
            schema.NotNull(nameof(schema));

            return SerializeSchemaCore(schema);
        }
        /// <summary>
        /// 序列化资源结构核心。
        /// </summary>
        /// <param name="schema">给定的 <see cref="ResourceSchema"/>。</param>
        /// <returns>返回字符串。</returns>
        protected abstract string SerializeSchemaCore(ResourceSchema schema);

    }
}
