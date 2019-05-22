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
using System.Globalization;
using System.Runtime.Versioning;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 环境信息接口。
    /// </summary>
    public interface IEnvironmentInfo
    {
        /// <summary>
        /// 应用基础路径。
        /// </summary>
        string ApplicationBasePath { get; }

        /// <summary>
        /// 应用名称。
        /// </summary>
        string ApplicationName { get; }

        /// <summary>
        /// 应用版本。
        /// </summary>
        string ApplicationVersion { get; }

        /// <summary>
        /// .NET Framework 的版本的名称。
        /// </summary>
        FrameworkName RuntimeFramework { get; }


        /// <summary>
        /// 获取此本地计算机的 NetBIOS 名称。
        /// </summary>
        string MachineName { get; }

        /// <summary>
        /// 获取当前计算机上的处理器数。
        /// </summary>
        int ProcessorCount { get; }

        /// <summary>
        /// 是 64 位操作系统。
        /// </summary>
        bool Is64BitOperatingSystem { get; }

        /// <summary>
        /// 是 64 位进程。
        /// </summary>
        bool Is64BitProcess { get; }

        /// <summary>
        /// 获取系统启动后经过的毫秒数。
        /// </summary>
        int TickCount { get; }

        /// <summary>
        /// 获取系统启动后经过的时间间隔。
        /// </summary>
        TimeSpan TickTimeSpan { get; }

        /// <summary>
        /// 获取区域性信息。
        /// </summary>
        CultureInfo Culture { get; }
    }
}
