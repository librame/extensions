#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Xml.Serialization;

namespace Librame.Resource.Serialization
{
    /// <summary>
    /// XML 资源序列化器接口。
    /// </summary>
    public interface IXmlResourceSerializer : IResourceSerializer
    {
        /// <summary>
        /// XML 命名空间和前缀。
        /// </summary>
        XmlSerializerNamespaces Namespaces { get; set; }
    }
}
