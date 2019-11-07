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
    /// 环境信息。
    /// </summary>
    public class EnvironmentInfo : IEnvironmentInfo
    {
        /// <summary>
        /// 构造一个 <see cref="EnvironmentInfo"/>。
        /// </summary>
        public EnvironmentInfo()
        {
            #if !NET48
            var environment = Microsoft.Extensions.PlatformAbstractions.PlatformServices.Default.Application;
            ApplicationBasePath = environment.ApplicationBasePath;
            ApplicationName = environment.ApplicationName;
            ApplicationVersion = environment.ApplicationVersion;
            RuntimeFramework = environment.RuntimeFramework;
            #else
            var appDomain = AppDomain.CurrentDomain;
            var appInfo = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileVersionInfo;
            ApplicationBasePath = appDomain.BaseDirectory;
            ApplicationName = appInfo.FileDescription.SplitPair('.').Key;
            ApplicationVersion = new Version(appInfo.FileMajorPart, appInfo.FileMinorPart, appInfo.FileBuildPart, appInfo.FilePrivatePart).ToString();
            RuntimeFramework = new FrameworkName(appDomain.SetupInformation.TargetFrameworkName);
            #endif
        }


        /// <summary>
        /// 应用基础路径。
        /// </summary>
        public string ApplicationBasePath { get; }

        /// <summary>
        /// 应用名称。
        /// </summary>
        public string ApplicationName { get; }

        /// <summary>
        /// 应用版本。
        /// </summary>
        public string ApplicationVersion { get; }

        /// <summary>
        /// .NET Framework 的版本的名称。
        /// </summary>
        public FrameworkName RuntimeFramework { get; }


        /// <summary>
        /// 获取此本地计算机的 NetBIOS 名称。
        /// </summary>
        public string MachineName
            => Environment.MachineName;

        /// <summary>
        /// 获取当前计算机上的处理器数。
        /// </summary>
        public int ProcessorCount
            => Environment.ProcessorCount;

        /// <summary>
        /// 获取系统启动后经过的毫秒数。
        /// </summary>
        public int TickCount
            => Environment.TickCount;

        /// <summary>
        /// 是 64 位操作系统。
        /// </summary>
        public bool Is64BitOperatingSystem
            => Environment.Is64BitOperatingSystem;

        /// <summary>
        /// 是 64 位进程。
        /// </summary>
        public bool Is64BitProcess
            => Environment.Is64BitProcess;

        /// <summary>
        /// 获取系统启动后经过的时间间隔。
        /// </summary>
        public TimeSpan TickTimeSpan
            => new TimeSpan(TickCount * 10000);

        /// <summary>
        /// 获取文化信息。
        /// </summary>
        public CultureInfo Culture
            => CultureInfo.CurrentCulture;

        /// <summary>
        /// 获取 UI 文化信息。
        /// </summary>
        public CultureInfo UICulture
            => CultureInfo.CurrentUICulture;
    }
}
