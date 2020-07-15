#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections.Generic;

namespace Librame.Extensions.Core
{
    using Combiners;
    using Converters;

    /// <summary>
    /// 核心偏好设置接口。
    /// </summary>
    public interface ICorePreferenceSetting : IPreferenceSetting
    {
        /// <summary>
        /// 配置文件夹（用于存储功能配置的文件夹）。
        /// </summary>
        string ConfigsFolder { get; }

        /// <summary>
        /// 报告文件夹（用于存储动态生成信息的文件夹）。
        /// </summary>
        string ReportsFolder { get; }

        /// <summary>
        /// 资源文件夹（用于存储静态资源的文件夹）。
        /// </summary>
        string ResourcesFolder { get; }


        /// <summary>
        /// 标识符文件夹。
        /// </summary>
        string IdentifiersFolder { get; }


        /// <summary>
        /// 编译程序集文件扩展名（不推荐使用”.dll“扩展名 ）。
        /// </summary>
        string CompileAssemblyFileExtension { get; }

        /// <summary>
        /// 密钥分隔符。
        /// </summary>
        char KeySeparator { get; }

        /// <summary>
        /// 默认创建时间。
        /// </summary>
        IAlgorithmConverter SecurityIdentifierConverter { get; }

        /// <summary>
        /// 安全标识符钥匙信息集合数。
        /// </summary>
        int SecurityIdentifierKeyInfosCount { get; }

        /// <summary>
        /// 安全标识符钥匙环文件路径。
        /// </summary>
        FilePathCombiner SecurityIdentifierKeyRingFilePath { get; }

        /// <summary>
        /// 系统程序集前缀。
        /// </summary>
        IReadOnlyList<string> SystemAssemblyPrefixes { get; }
    }
}
