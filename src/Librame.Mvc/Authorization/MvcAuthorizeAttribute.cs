#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace Librame.Authorization
{
    /// <summary>
    /// MVC 认证属性。
    /// </summary>
    public class MvcAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// 构造一个 <see cref="MvcAuthorizeAttribute"/> 实例。
        /// </summary>
        public MvcAuthorizeAttribute()
            : base()
        {
        }


        /// <summary>
        /// 当前用户。
        /// </summary>
        protected IPrincipal CurrentUser = null;

        /// <summary>
        /// 开始认证。
        /// </summary>
        /// <param name="filterContext">给定的认证上下文。</param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!AuthorizeCore(filterContext.HttpContext))
            {
                // 使用策略开始认证
                filterContext.HttpContext?.Session.OnAuthentication();
            }
        }


        /// <summary>
        /// 认证核心。
        /// </summary>
        /// <param name="httpContext">给定的 <see cref="HttpContextBase"/>。</param>
        /// <returns>返回布尔值。</returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
                return false;

            // 使用策略进行认证
            return httpContext.Session.IsAuthenticated(out CurrentUser);
        }

    }
}
