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
using System.Diagnostics;

namespace Librame.Extensions
{
    /// <summary>
    /// 进程静态扩展。
    /// </summary>
    public static class ProcessExtensions
    {
        /// <summary>
        /// 启动文件在资源管理器中定位。
        /// </summary>
        /// <param name="fileName">给定的文件名。</param>
        /// <param name="startInfoAction">给定的自定义进程启动信息动作方法（可选）。</param>
        /// <returns>返回 <see cref="Process"/>。</returns>
        public static Process StartLocateInExplorer(this string fileName,
            Action<ProcessStartInfo> startInfoAction = null)
            => "explorer.exe".StartProcess("/e,/select," + fileName, startInfoAction);


        /// <summary>
        /// 启动进程。
        /// </summary>
        /// <param name="exeFileName">给定的可执行文件名。</param>
        /// <param name="arguments">给定的参数字符串（可选）。</param>
        /// <param name="startInfoAction">给定的自定义进程启动信息动作方法（可选）。</param>
        /// <returns>返回 <see cref="Process"/>。</returns>
        public static Process StartProcess(this string exeFileName,
            string arguments = null, Action<ProcessStartInfo> startInfoAction = null)
        {
            var startInfo = new ProcessStartInfo(exeFileName)
            {
                Arguments = arguments ?? string.Empty
            };
            startInfoAction?.Invoke(startInfo);
            
            return Process.Start(startInfo);
        }

    }
}
