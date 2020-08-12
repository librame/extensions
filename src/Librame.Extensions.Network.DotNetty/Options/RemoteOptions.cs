#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;

namespace Librame.Extensions.Network.DotNetty.Options
{
    using Encryption.Builders;

    /// <summary>
    /// 远端选项。
    /// </summary>
    public class RemoteOptions
    {
        /// <summary>
        /// 使用 Libuv（默认使用）。
        /// </summary>
        public bool UseLibuv { get; set; }

        /// <summary>
        /// 退出命名。
        /// </summary>
        public string ExitCommand { get; set; }
            = "exit";

        /// <summary>
        /// 签名证书键名（默认使用全局键名）。
        /// </summary>
        public string SigningCredentialsKey { get; set; }
            = EncryptionBuilderOptions.GlobalSigningCredentialsKey;

        /// <summary>
        /// 是 SSL（默认不是）。
        /// </summary>
        public bool IsSsl { get; set; }

        /// <summary>
        /// 主机。
        /// </summary>
        public string Host { get; set; }
            = "127.0.0.1";

        /// <summary>
        /// 端口。
        /// </summary>
        public int Port { get; set; }
            = 8070;

        /// <summary>
        /// 静默时间间隔（默认 100 毫秒，即 0.1 秒）。
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public TimeSpan QuietPeriod { get; set; }
            = TimeSpan.FromMilliseconds(100);

        /// <summary>
        /// 超时时间间隔（默认 1 秒）。
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public TimeSpan TimeOut { get; set; }
            = TimeSpan.FromSeconds(1);
    }
}
