#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.Globalization;
using System.Runtime.Versioning;

namespace Librame.Extensions.Storage
{
    /// <summary>
    /// 平台描述符。
    /// </summary>
    public class PlatformDescriptor
    {
        /// <summary>
        /// 构造一个 <see cref="PlatformDescriptor"/> 实例。
        /// </summary>
        /// <param name="application">给定的 <see cref="ApplicationEnvironment"/>（可选）。</param>
        public PlatformDescriptor(ApplicationEnvironment application = null)
        {
            Application = application ?? PlatformServices.Default.Application;
        }


        /// <summary>
        /// 应用环境。
        /// </summary>
        /// <value>
        /// 返回 <see cref="ApplicationEnvironment"/>。
        /// </value>
        public ApplicationEnvironment Application { get; }


        /// <summary>
        /// 应用基础路径。
        /// </summary>
        public string ApplicationBasePath => Application.ApplicationBasePath;

        /// <summary>
        /// 应用名称。
        /// </summary>
        public string ApplicationName => Application.ApplicationName;

        /// <summary>
        /// 应用版本。
        /// </summary>
        public string ApplicationVersion => Application.ApplicationVersion;

        /// <summary>
        /// .NET Framework 的版本的名称。
        /// </summary>
        public FrameworkName RuntimeFramework => Application.RuntimeFramework;


        /// <summary>
        /// 获取此本地计算机的 NetBIOS 名称。
        /// </summary>
        public string MachineName => Environment.MachineName;

        /// <summary>
        /// 获取当前计算机上的处理器数。
        /// </summary>
        public int ProcessorCount => Environment.ProcessorCount;

        /// <summary>
        /// 获取系统启动后经过的毫秒数。
        /// </summary>
        public int TickCount => Environment.TickCount;

        /// <summary>
        /// 获取系统启动后经过的时间间隔。
        /// </summary>
        public TimeSpan TickTimeSpan => new TimeSpan(TickCount * 10000);
        
        /// <summary>
        /// 获取区域性信息。
        /// </summary>
        public CultureInfo Culture => CultureInfo.CurrentCulture;
    }
}
