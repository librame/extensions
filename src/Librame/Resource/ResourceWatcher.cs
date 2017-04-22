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
    using Utility;

    /// <summary>
    /// 资源监视器。
    /// </summary>
    public class ResourceWatcher : IResourceWatcher
    {
        /// <summary>
        /// 获取或设置 <see cref="Adaptation.AdapterSettings"/>。
        /// </summary>
        public Adaptation.AdapterSettings Settings { get; }

        /// <summary>
        /// 构造一个 <see cref="ResourceWatcher"/> 实例。
        /// </summary>
        /// <param name="settings">给定的 <see cref="Adaptation.AdapterSettings"/>。</param>
        public ResourceWatcher(Adaptation.AdapterSettings settings)
        {
            Settings = settings.NotNull(nameof(settings));

            IsWatching = false;
            LastRefreshedTime = DateTime.Now;
        }


        /// <summary>
        /// 是否正在监视。
        /// </summary>
        public bool IsWatching { get; protected set; }

        /// <summary>
        /// 最后一次刷新时间。
        /// </summary>
        public DateTime LastRefreshedTime { get; protected set; }

        /// <summary>
        /// 获取或设置刷新后的动作。
        /// </summary>
        public Action<IResourceProvider> RefreshedAction { get; set; }


        /// <summary>
        /// 创建监视器。
        /// </summary>
        /// <param name="file">给定的文件信息。</param>
        /// <returns>返回 <see cref="FileSystemWatcher"/>。</returns>
        protected virtual FileSystemWatcher CreateWatcher(FileInfo file)
        {
            // 设定单文件监视器
            var watcher = new FileSystemWatcher(file.DirectoryName, file.Name);
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Changed += new FileSystemEventHandler(Watcher_Changed);
            watcher.Deleted += new FileSystemEventHandler(Watcher_Deleted);

            return watcher;
        }


        private IResourceProvider _provider = null;

        /// <summary>
        /// 开始监视（已集成缓存功能）。
        /// </summary>
        /// <param name="provider">给定的 <see cref="IResourceProvider"/>。</param>
        public virtual void Watching(IResourceProvider provider)
        {
            provider.GuardNull(nameof(provider));

            // 如果禁用监视或资源路径为 URL，则取消监视
            if (!provider.SourceInfo.EnableWatching || provider.SourceInfo.Path.IsHttpOrHttpsUrl())
                return;

            var watcher = CreateWatcher(new FileInfo(provider.SourceInfo.Path));
            watcher.EnableRaisingEvents = true;

            // 绑定监视状态
            IsWatching = watcher.EnableRaisingEvents;

            _provider = provider;
        }
        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (!ReferenceEquals(_provider, null))
            {
                // 刷新管道结构实例
                Refresh(_provider);

                // 刷新后的动作
                if (!ReferenceEquals(RefreshedAction, null))
                    RefreshedAction(_provider);
            }
        }
        private void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            DisposeWatcher(sender as FileSystemWatcher);
        }

        private void DisposeWatcher(FileSystemWatcher watcher)
        {
            if (!ReferenceEquals(watcher, null))
            {
                if (watcher.EnableRaisingEvents)
                    watcher.EnableRaisingEvents = false;

                watcher.Dispose();

                // 释放监视状态
                IsWatching = false;
            }
        }


        /// <summary>
        /// 刷新结构。
        /// </summary>
        /// <param name="provider">给定的 <see cref="IResourceProvider"/>。</param>
        public void Refresh(IResourceProvider provider)
        {
            provider.GuardNull(nameof(provider));

            // 加载资源
            provider.Load();

            // 更新时间
            LastRefreshedTime = DateTime.Now;
        }

    }
}
