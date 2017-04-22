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

namespace Librame.Resource.Serialization
{
    using Schema;
    using Utility;

    /// <summary>
    /// 文件资源序列化器抽象基类。
    /// </summary>
    public abstract class FileResourceSerializer : AbstractResourceSerializer
    {
        private const string FILE_EXTENSION_SEPARATOR = ".";


        /// <summary>
        /// 构造一个 <see cref="FileResourceSerializer"/> 实例。
        /// </summary>
        /// <param name="settings">给定的 <see cref="Adaptation.AdapterSettings"/>。</param>
        public FileResourceSerializer(Adaptation.AdapterSettings settings)
            : base(settings)
        {
        }


        /// <summary>
        /// 获取文件扩展名。
        /// </summary>
        public abstract string FileExtension { get; }


        /// <summary>
        /// 修改文件扩展名。
        /// </summary>
        /// <param name="fileName">给定的文件名。</param>
        /// <returns>返回修改后的文件名。</returns>
        public virtual string ChangeFileExtension(string fileName)
        {
            fileName.GuardNullOrEmpty(nameof(fileName));

            string currentExtension = FileExtension;

            // 如果当前扩展名为空，则返回
            if (string.IsNullOrEmpty(currentExtension))
                return fileName;

            // 尝试修正扩展名以英文句号开始
            if (!currentExtension.StartsWith(FILE_EXTENSION_SEPARATOR))
                currentExtension = FILE_EXTENSION_SEPARATOR + currentExtension;

            // 获取指定文件名中的扩展名
            string oldExtension = Path.GetExtension(fileName);

            // 如果没有扩展名，则直接附加当前扩展名
            if (string.IsNullOrEmpty(oldExtension))
                return (fileName + currentExtension);

            // 如果扩展名无变化，则返回
            if (oldExtension == currentExtension)
                return fileName;

            // 修改扩展名
            var newFileName = fileName.TrimEnd(oldExtension, false);
            return (newFileName + currentExtension);
        }

        /// <summary>
        /// 修改格式部分。
        /// </summary>
        /// <param name="schema">给定的 <see cref="ResourceSchema"/>。</param>
        /// <returns>返回 <see cref="ResourceSchema"/>。</returns>
        protected virtual ResourceSchema ChangeSchemaFormat(ResourceSchema schema)
        {
            if (!ReferenceEquals(schema, null) && schema.Format.Extension != FileExtension)
                schema.Format.Extension = FileExtension;

            return schema;
        }


        /// <summary>
        /// 反序列化资源结构。
        /// </summary>
        /// <param name="content">给定的结构内容。</param>
        /// <param name="schemaType">给定的资源结构类型。</param>
        /// <returns>返回 <see cref="ResourceSchema"/>。</returns>
        public override ResourceSchema DeserializeSchema(string content, Type schemaType)
        {
            var schema = DeserializeSchemaCore(content, schemaType);

            return ChangeSchemaFormat(schema);
        }

        /// <summary>
        /// 序列化资源结构。
        /// </summary>
        /// <param name="schema">给定的 <see cref="ResourceSchema"/>。</param>
        /// <returns>返回字符串。</returns>
        public override string SerializeSchema(ResourceSchema schema)
        {
            schema.GuardNull(nameof(schema));

            schema = ChangeSchemaFormat(schema);

            return SerializeSchemaCore(schema);
        }

    }
}
