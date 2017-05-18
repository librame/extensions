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
using System.Runtime.InteropServices;

namespace Librame.Utility
{
    /// <summary>
    /// <see cref="Environment"/> 信息。
    /// </summary>
    public class EnvironmentInfo
    {
        /// <summary>
        /// 获取此本地计算机的 NetBIOS 名称。
        /// </summary>
        public string MachineName
        {
            get { return Environment.MachineName; }
        }

        /// <summary>
        /// 获取当前计算机上的处理器数。
        /// </summary>
        public int ProcessorCount
        {
            get { return Environment.ProcessorCount; }
        }

        /// <summary>
        /// 获取映射到进程上下文的物理内存量。
        /// </summary>
        public long WorkingSet
        {
            get { return Environment.WorkingSet; }
        }

        /// <summary>
        /// 获取包含当前平台标识符和版本号的 System.OperatingSystem 对象。
        /// </summary>
        public OperatingSystem OSVersion
        {
            get { return Environment.OSVersion; }
        }

        /// <summary>
        /// 获取一个 <see cref="Version"/> 对象，该对象描述公共语言运行时的主版本、次版本、内部版本和修订号。
        /// </summary>
        public Version Version
        {
            get { return Environment.Version; }
        }

        /// <summary>
        /// 获取系统启动后经过的毫秒数。
        /// </summary>
        public int TickCount
        {
            get { return Environment.TickCount; }
        }

        /// <summary>
        /// 获取操作系统的页面文件的内存量。
        /// </summary>
        public int SystemPageSize
        {
            get { return Environment.SystemPageSize; }
        }

        /// <summary>
        /// 获取系统目录的完全限定路径。
        /// </summary>
        public string SystemDirectory
        {
            get { return Environment.SystemDirectory; }
        }

        /// <summary>
        /// 获取当前工作目录的完全限定路径。
        /// </summary>
        public string CurrentDirectory
        {
            get { return Environment.CurrentDirectory; }
        }

        /// <summary>
        /// 获取安装的区域信息。
        /// </summary>
        public CultureInfo Culture
        {
            get { return CultureInfo.InstalledUICulture; }
        }

        /// <summary>
        /// 获取当前已登录到 Windows 操作系统的人员的用户名。
        /// </summary>
        public string UserName
        {
            get { return Environment.UserName; }
        }

        /// <summary>
        /// 获取与当前用户关联的网络域名。
        /// </summary>
        public string UserDomainName
        {
            get { return Environment.UserDomainName; }
        }

        /// <summary>
        /// 获取一个值，用以指示当前进程是否在用户交互模式中运行。
        /// </summary>
        public bool UserInteractive
        {
            get { return Environment.UserInteractive; }
        }
        
        /// <summary>
        /// 获取该进程的命令行。
        /// </summary>
        public string CommandLine
        {
            get { return Environment.CommandLine; }
        }

        /// <summary>
        /// 确定当前操作系统是否为 64 位操作系统。
        /// </summary>
        public bool Is64BitOperatingSystem
        {
            get { return Environment.Is64BitOperatingSystem; }
        }

        /// <summary>
        /// 确定当前进程是否为 64 位进程。
        /// </summary>
        public bool Is64BitProcess
        {
            get { return Environment.Is64BitProcess; }
        }

    }
}
