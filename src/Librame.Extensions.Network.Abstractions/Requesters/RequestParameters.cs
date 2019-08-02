#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections.Generic;

namespace Librame.Extensions.Network
{
    /// <summary>
    /// 请求参数集。
    /// </summary>
    public struct RequestParameters
    {
        /// <summary>
        /// 接受的内容类型。
        /// </summary>
        public string Accept { get; set; }

        /// <summary>
        /// 内容类型。
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// 头部信息。
        /// </summary>
        public Dictionary<string, string> Headers { get; set; }
    }
}
