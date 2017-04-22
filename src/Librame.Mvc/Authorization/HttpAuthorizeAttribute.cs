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
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Librame.Authorization
{
    /// <summary>
    /// HTTP 认证属性。
    /// </summary>
    public class HttpAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// 构造一个 <see cref="HttpAuthorizeAttribute"/> 实例。
        /// </summary>
        public HttpAuthorizeAttribute()
            : base()
        {
        }


        /// <summary>
        /// 当前 HTTP 上下文。
        /// </summary>
        protected HttpContextBase HttpContext = null;
        /// <summary>
        /// 当前用户。
        /// </summary>
        protected IPrincipal CurrentUser = null;

        /// <summary>
        /// 开始认证。
        /// </summary>
        /// <param name="actionContext">给定的 HTTP 动作上下文。</param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (!IsAuthorized(actionContext))
            {
                // 使用策略开始认证
                HttpContext?.Session.OnAuthentication();
            }
        }


        /// <summary>
        /// 当前动作是否已认证。
        /// </summary>
        /// <param name="actionContext">给定的 HTTP 动作上下文。</param>
        /// <returns>返回布尔值。</returns>
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            HttpContext = ResolveHttpContext(actionContext);

            if (ReferenceEquals(HttpContext, null))
                return false;

            // 使用策略进行认证
            return HttpContext.Session.IsAuthenticated(out CurrentUser);
        }
        
        /// <summary>
        /// 解析 HTTP 上下文信息。
        /// </summary>
        /// <param name="actionContext">给定的 HTTP 动作上下文。</param>
        /// <returns>返回 HTTP 上下文信息。</returns>
        protected virtual HttpContextBase ResolveHttpContext(HttpActionContext actionContext)
        {
            return (HttpContextBase)actionContext.Request.Properties["MS_HttpContext"];
        }

    }
}
