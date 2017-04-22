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

namespace Librame.Resource
{
    /// <summary>
    /// 资源监视器接口。
    /// </summary>
    public interface IResourceWatcher
    {
        /// <summary>
        /// 获取或设置 <see cref="Adaptation.AdapterSettings"/>。
        /// </summary>
        Adaptation.AdapterSettings Settings { get; }


        /// <summary>
        /// 是否正在监视。
        /// </summary>
        bool IsWatching { get; }

        /// <summary>
        /// 最后一次刷新时间。
        /// </summary>
        DateTime LastRefreshedTime { get; }

        /// <summary>
        /// 获取或设置刷新后的动作。
        /// </summary>
        Action<IResourceProvider> RefreshedAction { get; set; }


        /// <summary>
        /// 开始监视。
        /// </summary>
        /// <param name="provider">给定的 <see cref="IResourceProvider"/>。</param>
        void Watching(IResourceProvider provider);

        /// <summary>
        /// 刷新资源。
        /// </summary>
        /// <param name="provider">给定的 <see cref="IResourceProvider"/>。</param>
        void Refresh(IResourceProvider provider);
    }
}
