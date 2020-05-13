#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Network.DotNetty.Options
{
    /// <summary>
    /// 析因客户端选项。
    /// </summary>
    public class FactorialClientOptions : ClientOptions
    {
        /// <summary>
        /// 数量。
        /// </summary>
        public int Count { get; set; }
            = 100;
    }
}
