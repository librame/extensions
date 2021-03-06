﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Storage.Options
{
    /// <summary>
    /// 文件传输选项。
    /// </summary>
    public class FileTransferOptions
    {
        /// <summary>
        /// 超时（毫秒）。
        /// </summary>
        public int Timeout { get; set; }
            = 10000;

        /// <summary>
        /// 浏览器代理。
        /// </summary>
        public string UserAgent { get; set; }
            = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/75.0.3770.100 Safari/537.36";
    }
}
