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
using System.Reflection;

namespace Librame.Utility
{
    /// <summary>
    /// <see cref="Assembly"/> 实用工具。
    /// </summary>
    public class AssemblyUtility
    {
        private readonly static Assembly _currentAssembly = null;

        static AssemblyUtility()
        {
            if (ReferenceEquals(_currentAssembly, null))
                _currentAssembly = Assembly.GetCallingAssembly();
        }


        /// <summary>
        /// 导出嵌入的资源配置文件。
        /// </summary>
        /// <param name="outputFilePath">给定的输出文件路径。</param>
        /// <param name="manifestResourceName">给定的清单资源名。</param>
        /// <param name="assembly">给定的程序集（可选；默认为当前程序集）。</param>
        public static void ExportManifestResourceFile(string outputFilePath, string manifestResourceName,
            Assembly assembly = null)
        {
            // 默认程序集为当前程序集
            if (ReferenceEquals(assembly, null))
                assembly = _currentAssembly;

            try
            {
                // 如果文件已存在，则不导出
                if (File.Exists(outputFilePath))
                    return;

                // 读取嵌入的资源流
                using (var s = assembly.GetManifestResourceStream(manifestResourceName))
                {
                    using (var sr = new StreamReader(s))
                    {
                        var lenth = sr.BaseStream.Length;

                        int i = 0;
                        var buffer = new char[256];

                        // 输出到文件流
                        using (var fs = new FileStream(outputFilePath, FileMode.Create))
                        {
                            using (var sw = new StreamWriter(fs))
                            {
                                while (!sr.EndOfStream)
                                {
                                    i = sr.Read(buffer, 0, 256);
                                    sw.Write(buffer, 0, i);
                                }

                                sw.Flush();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }


    /// <summary>
    /// <see cref="AssemblyUtility"/> 静态扩展。
    /// </summary>
    public static class AssemblyUtilityExtensions
    {
        /// <summary>
        /// 导出嵌入的资源配置文件。
        /// </summary>
        /// <param name="assembly">给定的程序集。</param>
        /// <param name="outputFilePath">给定的输出文件路径。</param>
        /// <param name="manifestResourceName">给定的清单资源名。</param>
        public static void ExportManifestResourceFile(this Assembly assembly,
            string outputFilePath, string manifestResourceName)
        {
            AssemblyUtility.ExportManifestResourceFile(outputFilePath, manifestResourceName, assembly);
        }

    }

}
