#region License

/* **************************************************************************************
 * Copyright (c) zwbwl All rights reserved.
 * 
 * http://51zwb.com
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Utility;

namespace System.Web.Routing
{
    /// <summary>
    /// <see cref="RouteData"/> 静态扩展。
    /// </summary>
    public static class RouteDataExtensions
    {
        /// <summary>
        /// 解析路由信息。
        /// </summary>
        /// <param name="routeData">给定的 <see cref="RouteData"/>。</param>
        /// <returns>返回路由描述符。</returns>
        public static RouteDescriptor ResolveRoute(this RouteData routeData)
        {
            if (ReferenceEquals(routeData, null))
                return null;
            
            var descriptor = new RouteDescriptor();

            descriptor.ActionName = routeData.Values["action"]?.ToString();
            descriptor.ControllerName = routeData.Values["controller"]?.ToString();
            descriptor.AreaName = routeData.DataTokens["area"]?.ToString();

            return descriptor;
        }


        /// <summary>
        /// 重定向到路由。
        /// </summary>
        /// <param name="route">给定的路由描述符。</param>
        /// <returns>返回重定向到路由结果。</returns>
        public static Mvc.RedirectToRouteResult RedirectToRoute(this RouteDescriptor route)
        {
            route.GuardNull(nameof(route));

            var routeValues = new RouteValueDictionary(new
            {
                action = route.ActionName,
                controller = route.ControllerName,
                area = route.AreaName
            });

            return new Mvc.RedirectToRouteResult(routeValues);
        }

        ///// <summary>
        ///// 重定向到路由。
        ///// </summary>
        ///// <param name="route">给定的路由描述符。</param>
        ///// <returns>返回重定向到路由结果。</returns>
        //public static Http.Results.RedirectToRouteResult RedirectToRoute(this RouteDescriptor route)
        //{
        //    route.GuardNull(nameof(route));

        //    var routeValues = new RouteValueDictionary(new
        //    {
        //        action = route.ActionName,
        //        controller = route.ControllerName,
        //        area = route.AreaName
        //    });

        //    return new Http.Results.RedirectToRouteResult(routeValues);
        //}

    }
}
