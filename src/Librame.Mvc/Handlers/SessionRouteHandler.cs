#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion
 
using System.Web.Routing;
using System.Web.SessionState;

namespace System.Web.Http.WebHost
{
    /// <summary>
    /// 会话路由处理程序。
    /// </summary>
    public class SessionRouteHandler : HttpControllerHandler, IRequiresSessionState
    {
        /// <summary>
        /// 构造一个会话路由处理程序实例。
        /// </summary>
        /// <param name="routeData">给定的路由数据。</param>
        public SessionRouteHandler(RouteData routeData)
            : base(routeData)
        {
        }

    }
}
