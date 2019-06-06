#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// Librame 选项。
    /// </summary>
    public class LibrameOptions
    {
        /// <summary>
        /// 配置本地化。
        /// </summary>
        public Action<LocalizationOptions> ConfigureLocalization { get; set; }
            = options =>
            {
                // 初始限定资源目录名
                options.ResourcesPath = "Resources";
            };

        /// <summary>
        /// 配置日志。
        /// </summary>
        public Action<ILoggingBuilder> ConfigureLogging { get; set; }
            = _ => { };

        /// <summary>
        /// 选项名称。
        /// </summary>
        public string OptionsName { get; set; }

        /// <summary>
        /// 使用自动注册服务集合（默认不启用）。
        /// </summary>
        public bool UseAutoRegistrationServices { get; set; }
            = false;
    }
}
