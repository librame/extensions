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

    /// <summary>
    /// 核心偏好设置。
    /// </summary>
    public class CorePreferenceSetting : AbstractPreferenceSetting, ICorePreferenceSetting
    {
        /// <summary>
        /// 配置文件夹（用于存储功能配置的文件夹）。
        /// </summary>
        public virtual string ConfigsFolder { get; }
            = "_configs";

        /// <summary>
        /// 报告文件夹（用于存储动态生成信息的文件夹）。
        /// </summary>
        public virtual string ReportsFolder { get; }
            = "_reports";

        /// <summary>
        /// 资源文件夹（用于存储静态资源的文件夹）。
        /// </summary>
        public virtual string ResourcesFolder { get; }
            = "_resources";


        /// <summary>
        /// 令牌文件夹。
        /// </summary>
        public virtual string TokensFolder { get; }
            = "core_tokens";


        /// <summary>
        /// 编译程序集文件扩展名（不推荐使用”.dll“扩展名 ）。
        /// </summary>
        public virtual string CompileAssemblyFileExtension { get; }
            = ".dat";

        /// <summary>
        /// 密钥分隔符（英文冒号）。
        /// </summary>
        public virtual char KeySeparator { get; }
            = ':';

        /// <summary>
        /// 安全令牌数（默认生成 20 条记录）。
        /// </summary>
        public virtual int SecurityTokensCount { get; }
            = 20;

        /// <summary>
        /// 安全令牌钥匙环文件路径。
        /// </summary>
        public virtual FilePathCombiner SecurityTokenKeyRingFilePath { get; }
            = new FilePathCombiner("security_tokens.keyring");


        /// <summary>
        /// 包含第三方服务的程序集前缀列表。
        /// </summary>
        public virtual List<string> ThirdPartyAssemblyPrefixes { get; }
            = new List<string>
            {
                $"{nameof(Librame)}."
            };
    }
}
