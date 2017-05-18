#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame;
using Librame.Authorization;
using System.Web.Caching;
using System.Web.Security;

namespace System.Web.Mvc
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
        /// 当前认证上下文包含的 HTTP 会话状态（需调用 <see cref="OnAuthorization(AuthorizationContext)"/> 开始认证方法）。
        /// </summary>
        protected HttpSessionStateBase Session { get; private set; }

        /// <summary>
        /// 当前认证上下文包含的 Web 缓存（需调用 <see cref="OnAuthorization(AuthorizationContext)"/> 开始认证方法）。
        /// </summary>
        protected Cache Cache { get; private set; }

        /// <summary>
        /// 当前用户。
        /// </summary>
        protected AccountPrincipal CurrentUser = null;


        /// <summary>
        /// 开始认证。
        /// </summary>
        /// <param name="filterContext">给定的认证上下文。</param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!AuthorizeCore(filterContext.HttpContext))
            {
                var authorize = Archit.Adapters.Authorization;

                // 如果不启用认证
                if (!authorize.AuthSettings.EnableAuthorize)
                    return;

                // 失败
                AuthorizeFailed(filterContext, authorize);
            }
            else
            {
                // 成功
                AuthorizeSuccess(filterContext);
            }
        }

        /// <summary>
        /// 认证成功。
        /// </summary>
        /// <param name="filterContext">给定的认证上下文。</param>
        protected virtual void AuthorizeSuccess(AuthorizationContext filterContext)
        {
        }

        /// <summary>
        /// 认证失败。
        /// </summary>
        /// <param name="filterContext">给定的认证上下文。</param>
        /// <param name="authorize">给定的认证适配器接口。</param>
        protected virtual void AuthorizeFailed(AuthorizationContext filterContext, IAuthorizeAdapter authorize)
        {
            // 解析登陆链接
            string loginUrl = Session.ResolveLoginUrl();

            // 转向登陆
            filterContext.HttpContext.Response.Redirect(loginUrl);
            return;
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

            Cache = httpContext.Cache;
            Session = httpContext.Session;

            // 使用策略进行认证
            return Session.IsAuthenticated(out CurrentUser);
        }

    }
}
