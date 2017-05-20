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

namespace System.Web.Http.WebHost
{
    /// <summary>
    /// 会话控制器路由处理程序。
    /// </summary>
    public class SessionControllerRouteHandler : HttpControllerRouteHandler
    {
        /// <summary>
        /// 获取 HTTP 处理程序。
        /// </summary>
        /// <param name="requestContext">给定的请求上下文。</param>
        /// <returns>返回 HTTP 处理程序接口。</returns>
        protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new SessionRouteHandler(requestContext.RouteData);
        }

    }
}
