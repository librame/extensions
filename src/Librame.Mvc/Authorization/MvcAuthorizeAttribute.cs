#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Common.Logging;
using Librame;
using Librame.Authorization;

namespace System.Web.Mvc
{
    /// <summary>
    /// MVC 认证属性。
    /// </summary>
    public class MvcAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly ILog _logger = null;

        /// <summary>
        /// 构造一个 <see cref="MvcAuthorizeAttribute"/> 实例。
        /// </summary>
        public MvcAuthorizeAttribute()
            : base()
        {
            if (_logger == null)
                _logger = Archit.Log.GetLogger<MvcAuthorizeAttribute>();
        }
        
        
        /// <summary>
        /// 当前日志接口。
        /// </summary>
        public ILog Logger => _logger;

        /// <summary>
        /// 当前认证适配器接口。
        /// </summary>
        public IAuthorizeAdapter AuthorizeAdapter => LibrameArchitecture.Adapters.Authorization;

        /// <summary>
        /// 当前 HTTP 上下文。
        /// </summary>
        public HttpContextBase HttpContext { get; protected set; }

        /// <summary>
        /// 当前票根。
        /// </summary>
        public AuthenticateTicket Ticket { get; protected set; }


        /// <summary>
        /// 解析 HTTP 上下文信息。
        /// </summary>
        /// <param name="actionContext">给定的 HTTP 动作上下文。</param>
        /// <returns>返回 HTTP 上下文信息。</returns>
        protected virtual HttpContextBase ResolveHttpContext(AuthorizationContext actionContext)
        {
            return actionContext.HttpContext;
        }


        /// <summary>
        /// 开始认证。
        /// </summary>
        /// <param name="filterContext">给定的认证上下文。</param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            HttpContext = ResolveHttpContext(filterContext);

            if (AuthorizeCore(HttpContext))
            {
                base.AuthorizeCore(HttpContext);
            }
            else
            {
                HandleUnauthorizedRequest(filterContext);
            }
        }

        /// <summary>
        /// 处理未授权请求。
        /// </summary>
        /// <param name="filterContext">给定的 HTTP 动作上下文。</param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);

            // 解析登陆链接
            string loginUrl = HttpContext.Session.ResolveLoginUrl();

            // 记录调试信息
            Logger.Debug(loginUrl);

            // 重定向登陆
            filterContext.HttpContext.Response.Redirect(loginUrl);
            return;
        }

        /// <summary>
        /// 认证核心。
        /// </summary>
        /// <param name="httpContext">给定的 HTTP 上下文。</param>
        /// <returns>返回布尔值。</returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            // 如果未启用认证功能
            if (!AuthorizeAdapter.AuthSettings.EnableAuthorize)
            {
                // 记录调试信息
                Logger.Debug("当前未启用认证功能，默认返回已认证");
                return true;
            }

            // 解析票根
            Ticket = httpContext.Session.ResolveTicket();
            if (Ticket == null || Ticket.Expired || string.IsNullOrEmpty(Ticket?.Token))
            {
                // 记录调试信息
                Logger.Debug("解析会话状态中的认证信息失败，可能未登录");
                return false;
            }

            return true;
        }

    }
}
