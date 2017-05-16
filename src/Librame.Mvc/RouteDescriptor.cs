#region License

/* **************************************************************************************
 * Copyright (c) zwbwl All rights reserved.
 * 
 * http://51zwb.com
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Runtime.InteropServices;

namespace System.Web.Routing
{
    /// <summary>
    /// 路由描述符。
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class RouteDescriptor
    {
        /// <summary>
        /// 动作名。
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// 控制器名。
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// 区域名。
        /// </summary>
        public string AreaName { get; set; }
    }
}
