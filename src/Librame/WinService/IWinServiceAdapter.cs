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
using Topshelf;
using Topshelf.HostConfigurators;

namespace Librame.WinService
{
    /// <summary>
    /// Windows 服务适配器接口。
    /// </summary>
    public interface IWinServiceAdapter : Adaptation.IAdapter
    {
        /// <summary>
        /// 新增服务。
        /// </summary>
        /// <typeparam name="T">指定的服务控件类型。</typeparam>
        /// <param name="serviceName">给定的服务名称。</param>
        /// <param name="displayName">给定的显示名称。</param>
        /// <param name="description">给定的服务说明。</param>
        /// <param name="isAutomatic">是否自动启动。</param>
        /// <param name="action">自定义操作方法。</param>
        void AddService<T>(string serviceName, string displayName, string description,
            bool isAutomatic = false, Action<HostConfigurator> action = null) where T : class, ServiceControl, new();
    }
}
