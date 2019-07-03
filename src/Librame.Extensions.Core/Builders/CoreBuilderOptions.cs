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
using System.Globalization;
using System.Text;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 核心构建器选项。
    /// </summary>
    public class CoreBuilderOptions : AbstractBuilderOptions, IEncoding
    {
        private static readonly CultureInfo _zhCNCultureInfo
            = new CultureInfo("zh-CN");


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
        /// 文化信息（默认为 zh-CN）。
        /// </summary>
        public CultureInfo CultureInfo { get; set; }
            = _zhCNCultureInfo;

        /// <summary>
        /// UI 文化信息（默认为 zh-CN）。
        /// </summary>
        public CultureInfo CultureUIInfo { get; set; }
            = _zhCNCultureInfo;

        /// <summary>
        /// 字符编码（默认为 UTF8）。
        /// </summary>
        public Encoding Encoding { get; set; }
            = Encoding.UTF8;

        /// <summary>
        /// 启用自动注册服务集合（默认不启用）。
        /// </summary>
        public bool EnableAutoRegistrationServices { get; set; }
            = false;

        /// <summary>
        /// 启用扫描处理程序集合与处理器集合。
        /// </summary>
        public bool EnableScanHandlersAndProcessors { get; set; }
            = false;
    }
}
