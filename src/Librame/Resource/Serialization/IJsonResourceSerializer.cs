#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Newtonsoft.Json;

namespace Librame.Resource.Serialization
{
    /// <summary>
    /// JSON 资源序列化器接口。
    /// </summary>
    public interface IJsonResourceSerializer : IResourceSerializer
    {
        /// <summary>
        /// JSON 格式化枚举。
        /// </summary>
        Formatting Formatting { get; set; }

        /// <summary>
        /// JSON 序列化自定义设置。
        /// </summary>
        JsonSerializerSettings SerializerSettings { get; set; }

        /// <summary>
        /// JSON 转换器数组。
        /// </summary>
        JsonConverter[] Converters { get; set; }
    }
}
