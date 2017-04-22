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

    /// <summary>
    /// 资源序列化器接口。
    /// </summary>
    public interface IResourceSerializer
    {
        /// <summary>
        /// 获取或设置 <see cref="Adaptation.AdapterSettings"/>。
        /// </summary>
        Adaptation.AdapterSettings Settings { get; }


        /// <summary>
        /// 反序列化资源结构。
        /// </summary>
        /// <param name="content">给定的结构内容。</param>
        /// <param name="schemaType">给定的资源结构类型。</param>
        /// <returns>返回 <see cref="ResourceSchema"/>。</returns>
        ResourceSchema DeserializeSchema(string content, Type schemaType);

        /// <summary>
        /// 序列化资源结构。
        /// </summary>
        /// <param name="schema">给定的 <see cref="ResourceSchema"/>。</param>
        /// <returns>返回字符串。</returns>
        string SerializeSchema(ResourceSchema schema);
    }
}
