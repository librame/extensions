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
using System.Runtime.InteropServices;

namespace Librame.Resource.Schema
{
    /// <summary>
    /// 资源结构。
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class ResourceSchema
    {
        /// <summary>
        /// JSON 格式扩展名。
        /// </summary>
        public const string FORMAT_EXTENSION_JSON = ".json";

        /// <summary>
        /// XML 格式扩展名。
        /// </summary>
        public const string FORMAT_EXTENSION_XML = ".xml";


        /// <summary>
        /// 构造一个 <see cref="ResourceSchema"/> 实例。
        /// </summary>
        public ResourceSchema()
            : this("Resource Schemas Definition")
        {
        }
        /// <summary>
        /// 构造一个 <see cref="ResourceSchema"/> 实例。
        /// </summary>
        /// <param name="title">给定的标题。</param>
        protected ResourceSchema(string title)
        {
            Initialize(title);
        }
        
        /// <summary>
        /// 初始化资源结构。
        /// </summary>
        /// <param name="title">给定的标题。</param>
        protected virtual void Initialize(string title)
        {
            Format = new FormatSchemaSection()
            {
                Title = title,
                Extension = FORMAT_EXTENSION_JSON,
                Version = "1.0"
            };
        }


        /// <summary>
        /// 结构格式部分。
        /// </summary>
        public FormatSchemaSection Format { get; set; }
    }
}
