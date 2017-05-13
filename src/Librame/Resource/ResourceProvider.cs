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

namespace Librame.Resource
{
    using Schema;
    using Serialization;
    using Utility;

    /// <summary>
    /// 资源管道。
    /// </summary>
    public class ResourceProvider : IResourceProvider
    {
        /// <summary>
        /// 获取或设置 <see cref="Adaptation.AdapterSettings"/>。
        /// </summary>
        public Adaptation.AdapterSettings Settings { get; }

        /// <summary>
        /// 获取或设置 <see cref="IResourceSerializer"/>。
        /// </summary>
        public IResourceSerializer Serializer { get; }

        /// <summary>
        /// 获取或设置 <see cref="IResourceWatcher"/>。
        /// </summary>
        public IResourceWatcher Watcher { get; }


        /// <summary>
        /// 获取 <see cref="ResourceSourceInfo"/>。
        /// </summary>
        public ResourceSourceInfo SourceInfo { get; }

        /// <summary>
        /// 获取已存在的 <see cref="ResourceSchema"/>。
        /// </summary>
        public ResourceSchema ExistingSchema { get; protected set; }


        /// <summary>
        /// 构造一个 <see cref="ResourceProvider"/> 实例。
        /// </summary>
        /// <param name="settings">给定的 <see cref="Adaptation.AdapterSettings"/>。</param>
        /// <param name="serializer">给定的 <see cref="IResourceSerializer"/>。</param>
        /// <param name="watcher">给定的 <see cref="IResourceWatcher"/>。</param>
        /// <param name="sourceInfo">给定的 <see cref="ResourceSourceInfo"/>。</param>
        /// <param name="defaultSchema">给定的默认 <see cref="ResourceSchema"/>（可选）。</param>
        public ResourceProvider(Adaptation.AdapterSettings settings, IResourceSerializer serializer, IResourceWatcher watcher,
            ResourceSourceInfo sourceInfo, ResourceSchema defaultSchema = null)
        {
            Settings = settings.NotNull(nameof(settings));
            Serializer = serializer.NotNull(nameof(serializer));
            Watcher = watcher.NotNull(nameof(watcher));
            SourceInfo = sourceInfo.NotNull(nameof(sourceInfo));
            
            // 初始化
            InitializeProvider(defaultSchema);
        }

        /// <summary>
        /// 初始化管道。
        /// </summary>
        /// <param name="defaultSchema">给定的默认 <see cref="ResourceSchema"/>。</param>
        protected virtual void InitializeProvider(ResourceSchema defaultSchema)
        {
            //// 修正更改序列化器的文件扩展名
            //SourceInfo.Path = Serializer.ChangeFileExtension(SourceInfo.Path);

            // 初始化保存仅支持本地路径资源
            if (!SourceInfo.Path.IsUrl() && !File.Exists(SourceInfo.Path))
            {
                ExistingSchema = defaultSchema.NotNull(nameof(defaultSchema));

                // 初始化保存
                Save();
            }
            else
            {
                // 加载实例
                Load();
            }

            // 启用文件变化监控
            if (!ReferenceEquals(Watcher, null))
                Watcher.Watching(this);
        }


        /// <summary>
        /// 加载资源结构。
        /// </summary>
        public virtual void Load()
        {
            string content = string.Empty;
            
            // 从指定路径加载资源内容
            string path = SourceInfo.Path;
            if (SourceInfo.Path.IsUrl())
                content = UriUtility.ReadContent(path);
            else
                content = FileUtility.ReadContent(path, Settings.Encoding);

            // 创建指定结构类型
            var schemaType = Type.GetType(SourceInfo.SchemaTypeString);

            // 反序列化资源结构
            ExistingSchema = Serializer.DeserializeSchema(content, schemaType);
        }


        /// <summary>
        /// 保存资源结构。
        /// </summary>
        public virtual void Save()
        {
            string path = SourceInfo.Path;
            if (path.IsUrl())
                path = SourceInfo.SaveAsFilename;

            path.NotNullOrEmpty(nameof(path));

            // 保存到本地
            SaveAs(path);
        }

        /// <summary>
        /// 资源结构另存为。
        /// </summary>
        /// <param name="saveAsFileName">给定要另存为的文件名。</param>
        public virtual void SaveAs(string saveAsFileName)
        {
            string content = Serializer.SerializeSchema(ExistingSchema);

            // 保存到文件
            FileUtility.WriteContent(content, saveAsFileName, Settings.Encoding);
        }

    }
}
