﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using DotNetty.Common;

namespace Librame.Extensions.Network.DotNetty.Options
{
    /// <summary>
    /// HTTP 服务端选项。
    /// </summary>
    public class HttpServerOptions : ServerOptions
    {
        /// <summary>
        /// 资源探测器检测级别。
        /// </summary>
        public ResourceLeakDetector.DetectionLevel LeakDetector { get; set; }
            = ResourceLeakDetector.DetectionLevel.Disabled;
    }
}
