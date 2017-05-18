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
using System.Text;

namespace Librame.Utility
{
    /// <summary>
    /// <see cref="Path"/> 实用工具。
    /// </summary>
    public static class PathUtility
    {

        #region Directory

        private static readonly bool _AtWebEnvironment = false;

        private static readonly string _baseDirectory = null;
        private static readonly string _binDirectory = null;
        private static readonly string _configsDirectory = null;
        
        static PathUtility()
        {
            var domain = AppDomain.CurrentDomain;

            // 是否运行于 Web 环境
            _AtWebEnvironment = domain.SetupInformation.ConfigurationFile.EndsWith("web.config",
                StringComparison.CurrentCultureIgnoreCase);

            // 基础目录
            if (ReferenceEquals(_baseDirectory, null))
            {
                _baseDirectory = domain.BaseDirectory;
            }
            
            // Bin 目录
            if (ReferenceEquals(_binDirectory, null))
            {
                // 非 Web 环境为空
                _binDirectory = domain.RelativeSearchPath ?? _baseDirectory;
            }

            // Configs 目录
            if (ReferenceEquals(_configsDirectory, null))
            {
                // Base 目录下（纳入代码管理，Bin 目录不进行代码管理）
                _configsDirectory = _baseDirectory.AppendDirectoryName("_configs");
            }
        }


        /// <summary>
        /// 是否位于 Web 环境。
        /// </summary>
        public static bool IsWebEnvironment
        {
            get { return _AtWebEnvironment; }
        }


        /// <summary>
        /// 获取基础目录。
        /// </summary>
        public static string BaseDirectory
        {
            get { return _baseDirectory; }
        }

        /// <summary>
        /// 获取 Bin 目录（在 Web 环境中为 Web\Bin 目录）。
        /// </summary>
        public static string BinDirectory
        {
            get { return _binDirectory; }
        }

        /// <summary>
        /// 获取 Configs 目录。
        /// </summary>
        public static string ConfigsDirectory
        {
            get { return _configsDirectory; }
        }

        #endregion


        /// <summary>
        /// 物理路径分隔符。
        /// </summary>
        public static readonly string PhysicalPathSeparator = Path.DirectorySeparatorChar.ToString();

        /// <summary>
        /// 虚拟路径分隔符。
        /// </summary>
        public static readonly string VirtualPathSeparator = Path.AltDirectorySeparatorChar.ToString();

        /// <summary>
        /// 虚拟路径方案集。
        /// </summary>
        public static string[] VirtualPathSchemes
        {
            get { return new string[] { Uri.UriSchemeFtp, Uri.UriSchemeGopher, Uri.UriSchemeHttp }; }
        }


        /// <summary>
        /// 合并路径。
        /// </summary>
        /// <remarks>
        /// 自定义高级路径合并，修复在特殊环境下 <see cref="Path.Combine(string, string)"/> 不能正常合并的情况。
        /// </remarks>
        /// <param name="paths">给定的子路径集合。</param>
        /// <returns>返回路径。</returns>
        public static string CombinePath(params string[] paths)
        {
            if (paths.Length == 0)
                throw new ArgumentException("please input path.");

            var sb = new StringBuilder();

            string firstPath = paths[0];

            // 默认为物理路径分隔符
            string separator = PhysicalPathSeparator;

            // 是否为虚拟路径方案
            foreach (var scheme in VirtualPathSchemes)
            {
                if (firstPath.StartsWith(scheme, StringComparison.OrdinalIgnoreCase))
                {
                    // 更新为虚拟路径分隔符
                    separator = VirtualPathSeparator;
                    break;
                }
            }

            if (!firstPath.EndsWith(separator))
            {
                // 初始路径附加分隔符
                firstPath = firstPath + separator;
            }

            sb.Append(firstPath);

            for (int i = 1; i < paths.Length; i++)
            {
                string nextPath = paths[i];

                if (nextPath.StartsWith(VirtualPathSeparator) || nextPath.StartsWith(PhysicalPathSeparator))
                {
                    // 支持相对路径
                    nextPath = nextPath.Substring(1);
                }

                // not the last one
                if (i != paths.Length - 1)
                {
                    if (nextPath.EndsWith(VirtualPathSeparator) || nextPath.EndsWith(PhysicalPathSeparator))
                    {
                        // 规范化末尾分隔符
                        nextPath = nextPath.Substring(0, nextPath.Length - 1) + separator;
                    }
                    else
                    {
                        // 附加末尾分隔符
                        nextPath = nextPath + separator;
                    }
                }

                sb.Append(nextPath);
            }

            return sb.ToString();
        }


        /// <summary>
        /// 将指定相对路径附加到当前基础路径。
        /// </summary>
        /// <remarks>
        /// 基于 <see cref="Path.Combine(string, string)"/> 方法。
        /// </remarks>
        /// <param name="basePath">给定的基础路径。</param>
        /// <param name="relativePath">给定的相对路径。</param>
        /// <returns>返回路径。</returns>
        public static string AppendPath(this string basePath, string relativePath)
        {
            return Path.Combine(basePath, relativePath);
        }


        /// <summary>
        /// 检测当前路径是否为相对路径。
        /// </summary>
        /// <param name="path">给定的路径。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsRelativePath(this string path)
        {
            // 是否不包含根路径
            return (!Path.IsPathRooted(path));
        }


        /// <summary>
        /// 取得路径扩展名。
        /// </summary>
        /// <param name="path">给定的路径。</param>
        /// <returns>返回字符串。</returns>
        public static string PathExtension(this string path)
        {
            // 如.txt
            return Path.GetExtension(path);
        }


        /// <summary>
        /// 创建指定路径中包含的目录信息。
        /// </summary>
        /// <param name="path">给定的路径。</param>
        /// <returns>返回完整目录字符串。</returns>
        public static string CreateDirectoryName(this string path)
        {
            var di = CreateDirectoryInfo(path);

            return di.FullName;
        }
        /// <summary>
        /// 创建指定路径中包含的目录信息。
        /// </summary>
        /// <param name="path">给定的路径。</param>
        /// <returns>返回目录信息。</returns>
        public static DirectoryInfo CreateDirectoryInfo(this string path)
        {
            try
            {
                string dirctory = Path.GetDirectoryName(path);

                return Directory.CreateDirectory(dirctory);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 附加并创建目录。
        /// </summary>
        /// <param name="baseDirectory">给定的基础目录。</param>
        /// <param name="relativeDirectory">给定的相对目录。</param>
        /// <returns>返回完整目录字符串。</returns>
        public static string AppendDirectoryName(this string baseDirectory, string relativeDirectory)
        {
            var di = AppendDirectoryInfo(baseDirectory, relativeDirectory);

            return di.FullName;
        }
        /// <summary>
        /// 附加并创建目录。
        /// </summary>
        /// <param name="baseDirectory">给定的基础目录。</param>
        /// <param name="relativeDirectory">给定的相对目录。</param>
        /// <returns>返回目录信息。</returns>
        public static DirectoryInfo AppendDirectoryInfo(this string baseDirectory, string relativeDirectory)
        {
            try
            {
                var directory = baseDirectory.AppendPath(relativeDirectory);

                return Directory.CreateDirectory(directory);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
