#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Authorization
{
    /// <summary>
    /// 认证首选项。
    /// </summary>
    public class AuthorizeSettings : Adaptation.AbstractAdapterSettings, Adaptation.IAdapterSettings
    {
        /// <summary>
        /// 启用认证功能。
        /// </summary>
        public bool EnableAuthorize { get; set; }

        /// <summary>
        /// 过期天数。
        /// </summary>
        public int ExpirationDays { get; set; }

        /// <summary>
        /// 启用 SSO。
        /// </summary>
        public bool EnableSso { get; set; }

        /// <summary>
        /// 是否处于 SSO 服务器模式。
        /// </summary>
        public bool IsSsoServerMode { get; set; }

        /// <summary>
        /// SSO 认证服务器登录 URL。
        /// </summary>
        public string SsoServerSignInUrl { get; set; }
        /// <summary>
        /// SSO 认证登录应答 URL。
        /// </summary>
        public string SsoSignInRespondUrl { get; set; }

        /// <summary>
        /// SSO 认证服务器登出 URL。
        /// </summary>
        public string SsoServerSignOutUrl { get; set; }
        /// <summary>
        /// SSO 认证登出应答 URL。
        /// </summary>
        public string SsoSignOutRespondUrl { get; set; }

        /// <summary>
        /// SSO 认证服务器权限 URL。
        /// </summary>
        public string SsoServerPermitUrl { get; set; }
        /// <summary>
        /// SSO 认证权限应答 URL。
        /// </summary>
        public string SsoPermitRespondUrl { get; set; }
    }
}
