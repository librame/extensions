#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Net;

namespace Librame.Extensions.Network.Options
{
    /// <summary>
    /// SMTP 选项。
    /// </summary>
    public class SmtpOptions
    {
        /// <summary>
        /// 服务器。
        /// </summary>
        public string Server { get; set; }
            = "smtp.contoso.com";

        /// <summary>
        /// 端口。
        /// </summary>
        public int Port { get; set; }
            = 587;

        /// <summary>
        /// 网络凭据。
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public NetworkCredential Credential { get; set; }
            = new NetworkCredential("LoginName", "LoginPass");
    }
}
