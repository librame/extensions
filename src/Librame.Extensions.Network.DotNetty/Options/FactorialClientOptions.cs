#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
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
