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
using Librame.Utility;
using System.Net;
using System.Net.Http;
using System.Web.Caching;
using System.Web.Http.Controllers;

namespace System.Web.Http
{
    /// <summary>
    /// API 认证属性。
    /// </summary>
    public class ApiAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// 构造一个 <see cref="ApiAuthorizeAttribute"/> 实例。
        /// </summary>
        public ApiAuthorizeAttribute()
            : base()
        {
        }

        /// <summary>
        /// 当前认证上下文包含的 HTTP 会话状态（需调用 <see cref="OnAuthorization(HttpActionContext)"/> 开始认证方法）。
        /// </summary>
        protected HttpSessionStateBase Session { get; private set; }

        /// <summary>
        /// 当前认证上下文包含的 Web 缓存（需调用 <see cref="OnAuthorization(HttpActionContext)"/> 开始认证方法）。
        /// </summary>
        protected Cache Cache { get; private set; }

        /// <summary>
        /// 当前 HTTP 上下文（需调用 <see cref="OnAuthorization(HttpActionContext)"/> 开始认证方法）。
        /// </summary>
        protected HttpContextBase HttpContext { get; private set; }

        /// <summary>
        /// 当前用户。
        /// </summary>
        protected AccountPrincipal CurrentUser = null;

        /// <summary>
        /// 开始认证。
        /// </summary>
        /// <param name="actionContext">给定的 HTTP 动作上下文。</param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (!IsAuthorized(actionContext))
            {
                var authorize = Archit.Adapters.Authorization;

                // 如果不启用认证
                if (!authorize.AuthSettings.EnableAuthorize)
                    return;

                // 失败
                AuthorizeFailed(actionContext, authorize);
            }
            else
            {
                // 成功
                AuthorizeSuccess(actionContext);
            }
        }

        /// <summary>
        /// 认证成功。
        /// </summary>
        /// <param name="actionContext">给定的 HTTP 动作上下文。</param>
        protected virtual void AuthorizeSuccess(HttpActionContext actionContext)
        {
        }

        /// <summary>
        /// 认证失败。
        /// </summary>
        /// <param name="actionContext">给定的 HTTP 动作上下文。</param>
        /// <param name="authorize">给定的认证适配器接口。</param>
        protected virtual void AuthorizeFailed(HttpActionContext actionContext, IAuthorizeAdapter authorize)
        {
            // 解析登陆链接
            string loginUrl = Session.ResolveLoginUrl();

            var obj = new
            {
                LoginUrl = loginUrl,
                Message = "当前请求未获得授权！",
                StatusCode = HttpStatusCode.Unauthorized
            };

            actionContext.Response = new HttpResponseMessage()
            {
                StatusCode = obj.StatusCode,
                Content = new StringContent(obj.AsJson())
            };
        }

        /// <summary>
        /// 当前动作是否已认证。
        /// </summary>
        /// <param name="actionContext">给定的 HTTP 动作上下文。</param>
        /// <returns>返回布尔值。</returns>
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            HttpContext = ResolveHttpContext(actionContext);

            if (HttpContext == null)
                return false;

            Cache = HttpContext.Cache;
            Session = HttpContext.Session;

            // 使用策略进行认证
            return Session.IsAuthenticated(out CurrentUser);
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
