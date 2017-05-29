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
using Librame.Authorization.Descriptors;
using Librame.Utility;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;

namespace System.Web.Http
{
    /// <summary>
    /// API 认证属性。
    /// </summary>
    public class ApiAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly ILog _logger = null;

        /// <summary>
        /// 构造一个 <see cref="ApiAuthorizeAttribute"/> 实例。
        /// </summary>
        public ApiAuthorizeAttribute()
            : base()
        {
            if (_logger == null)
                _logger = LibrameArchitecture.Logging.GetLogger<ApiAuthorizeAttribute>();
        }


        /// <summary>
        /// 启用认证功能。
        /// </summary>
        public bool EnableAuthorized { get; set; }

        /// <summary>
        /// 当前日志接口。
        /// </summary>
        public ILog Logger => _logger;

        /// <summary>
        /// 当前认证适配器接口。
        /// </summary>
        public IAuthorizeAdapter Adapter => LibrameArchitecture.Adapters.Authorization;

        /// <summary>
        /// 当前 HTTP 上下文。
        /// </summary>
        public HttpContextBase HttpContext { get; protected set; }


        /// <summary>
        /// 解析 HTTP 上下文信息。
        /// </summary>
        /// <param name="actionContext">给定的 HTTP 动作上下文。</param>
        /// <returns>返回 HTTP 上下文信息。</returns>
        protected virtual HttpContextBase ResolveHttpContext(HttpActionContext actionContext)
        {
            return (HttpContextBase)actionContext.Request.Properties["MS_HttpContext"];
        }


        /// <summary>
        /// 开始认证。
        /// </summary>
        /// <param name="actionContext">给定的 HTTP 动作上下文。</param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            // 支持预请求
            if (actionContext.Request.Method == HttpMethod.Options)
            {
                // 响应 202
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Accepted);
                return;
            }

            HttpContext = ResolveHttpContext(actionContext);

            if (IsAuthorized(actionContext))
            {
                base.IsAuthorized(actionContext);
            }
            else
            {
                HandleUnauthorizedRequest(actionContext);
            }
        }

        /// <summary>
        /// 处理未授权请求。
        /// </summary>
        /// <param name="filterContext">给定的 HTTP 动作上下文。</param>
        protected override void HandleUnauthorizedRequest(HttpActionContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);

            // 解析登陆链接
            string loginUrl = HttpContext.Session.ResolveLoginUrl();

            var obj = new
            {
                LoginUrl = loginUrl,
                Message = "当前请求未获得授权！",
                // 401 未授权会导致重定向
                StatusCode = HttpStatusCode.Forbidden
            };

            // 记录调试信息
            Logger.Debug(obj.AsPairsString());

            filterContext.Response = new HttpResponseMessage()
            {
                StatusCode = obj.StatusCode,
                Content = new StringContent(obj.AsJson(), Archit.Adapters.Settings.Encoding, "application/json")
            };
        }

        /// <summary>
        /// 当前动作是否已认证。
        /// </summary>
        /// <param name="actionContext">给定的 HTTP 动作上下文。</param>
        /// <returns>返回布尔值。</returns>
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            // 如果未启用认证功能
            if (!EnableAuthorized)
            {
                // 记录调试信息
                Logger.Debug("当前未启用认证功能，默认返回已认证");
                return true;
            }

            // 认证令牌
            var token = string.Empty;

            // 尝试从 HTTP 请求标头中包含的认证信息（Basic 基础认证）
            var authorization = actionContext.Request.Headers.Authorization;
            if (authorization != null && !string.IsNullOrEmpty(authorization.Parameter))
            {
                // 解码令牌
                token = DecodeToken(authorization.Parameter);
            }
            else
            {
                // 如果启用 SSO 且处于服务器模式
                if (Adapter.AuthSettings.EnableSso && Adapter.AuthSettings.IsSsoServerMode)
                {
                    // 尝试解析会话中存储的认证信息
                    return HttpContext.Session.IsAuthenticated();
                }

                // 其它模式则尝试从票根中解析令牌（兼容 .NET WEB 平台；未加密）
                token = HttpContext.Session.ResolveTicket()?.Token;
            }

            // 解析令牌失败
            if (string.IsNullOrEmpty(token))
            {
                // 记录调试信息
                Logger.Debug("获取 HTTP 标头或会话状态中的认证信息失败，可能未登录");
                return false;
            }

            // 验证令牌
            var isAuthorized = ValidateToken(token);
            if (!isAuthorized)
            {
                // 记录调试信息
                Logger.DebugFormat("验证认证信息失败，请确认令牌信息 {0} 是否正确", token);
            }

            return isAuthorized;
        }

        /// <summary>
        /// 解码令牌。
        /// </summary>
        /// <param name="encodeToken">给定经过编码的令牌字符串。</param>
        /// <returns>返回字符串。</returns>
        protected virtual string DecodeToken(string encodeToken)
        {
            return Adapter.Managers.Ciphertext.Decode(encodeToken);
        }

        /// <summary>
        /// 验证令牌是否有效。
        /// </summary>
        /// <param name="token">给定的令牌。</param>
        /// <returns>返回布尔值。</returns>
        protected virtual bool ValidateToken(string token)
        {
            ITokenDescriptor descriptor = null;
            return ValidateToken(token, out descriptor);
        }
        /// <summary>
        /// 验证令牌是否有效。
        /// </summary>
        /// <param name="token">给定的令牌。</param>
        /// <param name="descriptor">输出令牌描述符。</param>
        /// <returns>返回布尔值。</returns>
        protected virtual bool ValidateToken(string token, out ITokenDescriptor descriptor)
        {
            // 默认通过管道进行数据库查询
            descriptor = Adapter.Providers.Token.Authenticate(token);

            return (descriptor != null && !string.IsNullOrEmpty(descriptor.Name)
                && !string.IsNullOrEmpty(descriptor.Ticket));
        }

    }
}
