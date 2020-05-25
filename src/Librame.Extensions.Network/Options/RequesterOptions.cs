#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Polly;
using System;

namespace Librame.Extensions.Network.Options
{
    /// <summary>
    /// 请求程序选项。
    /// </summary>
    public class RequesterOptions
    {
        /// <summary>
        /// 连接次数（默认 10 次）。
        /// </summary>
        public int ConnectionLimit { get; set; }
            = 10;

        /// <summary>
        /// 重试次数（默认 3 次）。
        /// </summary>
        public int RetryCount { get; set; }
            = 3;

        /// <summary>
        /// 重试暂停间隔的工厂方法（默认等待 1 秒后重试）。
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public Func<int, Context, TimeSpan> SleepDurationFactory { get; set; }
            = (retryCount, context) => TimeSpan.FromSeconds(1);

        /// <summary>
        /// 超时（默认 7 秒）。
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public TimeSpan Timeout { get; set; }
            = TimeSpan.FromSeconds(7);

        /// <summary>
        /// 浏览器代理。
        /// </summary>
        public string UserAgent { get; set; }
            = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/75.0.3770.100 Safari/537.36";
    }
}
