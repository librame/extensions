﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
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
    /// 核心偏好设置。
    /// </summary>
    public class CorePreferenceSetting : AbstractPreferenceSetting, ICorePreferenceSetting
    {
        /// <summary>
        /// 配置文件夹（用于存储功能配置的文件夹）。
        /// </summary>
        public virtual string ConfigsFolder
            => "_configs";

        /// <summary>
        /// 报告文件夹（用于存储动态生成信息的文件夹）。
        /// </summary>
        public virtual string ReportsFolder
            => "_reports";

        /// <summary>
        /// 资源文件夹（用于存储静态资源的文件夹）。
        /// </summary>
        public virtual string ResourcesFolder
            => "_resources";


        /// <summary>
        /// 标识符文件夹。
        /// </summary>
        public virtual string IdentifiersFolder
            => "core_identifiers";


        /// <summary>
        /// 编译程序集文件扩展名（禁止使用”.dll“扩展名 ）。
        /// </summary>
        public virtual string CompileAssemblyFileExtension
            => ".dat";

        /// <summary>
        /// 密钥分隔符（英文冒号）。
        /// </summary>
        public virtual char KeySeparator
            => ':';

        /// <summary>
        /// 安全标识符转换器（<see cref="HexStringConverter"/>）。
        /// </summary>
        public virtual IAlgorithmConverter SecurityIdentifierConverter
            => ConverterManager.GetAlgorithm<HexStringConverter>();

        /// <summary>
        /// 安全标识符钥匙信息集合数（默认生成 20 条记录）。
        /// </summary>
        public virtual int SecurityIdentifierKeyInfosCount
            => 20;

        /// <summary>
        /// 安全标识符钥匙环文件路径。
        /// </summary>
        public virtual FilePathCombiner SecurityIdentifierKeyRingFilePath
            => new FilePathCombiner("security_identifier.keyring");

        /// <summary>
        /// 系统程序集前缀。
        /// </summary>
        public virtual IReadOnlyList<string> SystemAssemblyPrefixes
            => new List<string>
            {
                "anonymously",
                "microsoft",
                "mscorlib",
                "newtonsoft",
                "netstandard",
                "nuget",
                "proxybuilder",
                "runtime",
                "sos",
                "system",
                "testhost",
                "window",
                "xunit"
            };
    }
}
