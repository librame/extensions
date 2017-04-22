#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Resource
{
    using Schema;
    using Serialization;

    /// <summary>
    /// 资源管道接口。
    /// </summary>
    public interface IResourceProvider
    {
        /// <summary>
        /// 获取或设置 <see cref="Adaptation.AdapterSettings"/>。
        /// </summary>
        Adaptation.AdapterSettings Settings { get; }

        /// <summary>
        /// 获取或设置 <see cref="IResourceSerializer"/>。
        /// </summary>
        IResourceSerializer Serializer { get; }

        /// <summary>
        /// 获取或设置 <see cref="IResourceWatcher"/>。
        /// </summary>
        IResourceWatcher Watcher { get; }


        /// <summary>
        /// 获取 <see cref="ResourceSourceInfo"/>。
        /// </summary>
        ResourceSourceInfo SourceInfo { get; }

        /// <summary>
        /// 获取已存在的 <see cref="ResourceSchema"/>。
        /// </summary>
        ResourceSchema ExistingSchema { get; }
        

        /// <summary>
        /// 加载资源结构。
        /// </summary>
        void Load();


        /// <summary>
        /// 保存资源结构。
        /// </summary>
        void Save();

        /// <summary>
        /// 资源结构另存为。
        /// </summary>
        /// <param name="saveAsFileName">给定要另存为的文件名。</param>
        void SaveAs(string saveAsFileName);
    }
}
