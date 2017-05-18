#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.Web;

namespace Librame.Authorization
{
    using Utility;

    /// <summary>
    /// 认证助手。
    /// </summary>
    public class AuthorizeHelper
    {
        /// <summary>
        /// 默认密码。
        /// </summary>
        internal const string DEFAULT_PASSWD = "123456";

        /// <summary>
        /// 认证类型。
        /// </summary>
        internal const string AUTHORIZATION_TYPE = LibrameAssemblyConstants.NAME + "Authorization";
        /// <summary>
        /// 认证键名。
        /// </summary>
        public const string AUTHORIZATION_KEY = AUTHORIZATION_TYPE;

        /// <summary>
        /// 授权编号键名。
        /// </summary>
        internal const string AUTH_ID_KEY = "authid";
        /// <summary>
        /// 应答链接键名。
        /// </summary>
        internal const string RESPOND_URL_KEY = "respondurl";
        /// <summary>
        /// 认证令牌键名。
        /// </summary>
        internal const string TOKEN_KEY = "token";


        #region Resolve

        /// <summary>
        /// 解析指定请求包含的加密令牌。
        /// </summary>
        /// <param name="request">给定的请求。</param>
        /// <returns>返回经过加密令牌字符串。</returns>
        public static string ResolveEncryptToken(HttpRequest request)
        {
            request.NotNull(nameof(request));

            return ResolveEncryptToken(new HttpRequestWrapper(request));
        }
        /// <summary>
        /// 解析指定请求包含的加密令牌。
        /// </summary>
        /// <param name="request">给定的请求。</param>
        /// <returns>返回经过加密令牌字符串。</returns>
        public static string ResolveEncryptToken(HttpRequestBase request)
        {
            request.NotNull(nameof(request));

            var encryptToken = request.QueryString[TOKEN_KEY];
            return encryptToken;
        }


        /// <summary>
        /// 解析指定请求包含的加密授权编号与应答链接。
        /// </summary>
        /// <param name="request">给定的请求。</param>
        /// <returns>返回依次为加密授权编号、应答链接的元组。</returns>
        public static Tuple<string, string> ResolveEncryptAuthIdAndRespondUrl(HttpRequest request)
        {
            request.NotNull(nameof(request));

            return ResolveEncryptAuthIdAndRespondUrl(new HttpRequestWrapper(request));
        }
        /// <summary>
        /// 解析指定请求包含的加密授权编号与应答链接。
        /// </summary>
        /// <param name="request">给定的请求。</param>
        /// <returns>返回依次为加密授权编号、应答链接的元组。</returns>
        public static Tuple<string, string> ResolveEncryptAuthIdAndRespondUrl(HttpRequestBase request)
        {
            request.NotNull(nameof(request));

            var encryptAuthId = request.QueryString[AUTH_ID_KEY];
            var respondUrl = request.QueryString[RESPOND_URL_KEY];

            return new Tuple<string, string>(encryptAuthId, respondUrl);
        }

        #endregion


        #region Format

        /// <summary>
        /// 格式化认证服务器登入链接。
        /// </summary>
        /// <param name="encryptAuthId">给定的加密授权编号。</param>
        /// <param name="respondUrl">给定的应答链接。</param>
        /// <param name="serverSignInUrl">给定的服务器登入链接。</param>
        /// <returns>返回格式化后的字符串。</returns>
        internal static string FormatServerSignInUrl(string encryptAuthId,
            string respondUrl, string serverSignInUrl)
        {
            serverSignInUrl.NotEmpty(nameof(serverSignInUrl));
            
            encryptAuthId.NotEmpty(nameof(encryptAuthId));
            respondUrl.NotEmpty(nameof(respondUrl));

            // URL 编码
            encryptAuthId = HttpUtility.UrlEncode(encryptAuthId);
            respondUrl = HttpUtility.UrlEncode(respondUrl);

            var formatDictionary = new Dictionary<string, string>()
            {
                { AUTH_ID_KEY, encryptAuthId },
                { RESPOND_URL_KEY, respondUrl }
            };

            // 格式化链接
            serverSignInUrl = KeyBuilder.Formatting(serverSignInUrl, formatDictionary);
            return serverSignInUrl;
        }


        /// <summary>
        /// 格式化认证服务器登出链接。
        /// </summary>
        /// <param name="encryptToken">给定的已加密的令牌字符串。</param>
        /// <param name="encryptAuthId">给定的加密授权编号。</param>
        /// <param name="respondUrl">给定的应答链接。</param>
        /// <param name="serverSignOutUrl">给定的服务器登出链接。</param>
        /// <returns>返回格式化后的字符串。</returns>
        internal static string FormatServerSignOutUrl(string encryptToken, string encryptAuthId,
            string respondUrl, string serverSignOutUrl)
        {
            serverSignOutUrl.NotEmpty(nameof(serverSignOutUrl));

            encryptToken.NotEmpty(nameof(encryptToken));
            encryptAuthId.NotEmpty(nameof(encryptAuthId));
            respondUrl.NotEmpty(nameof(respondUrl));

            // URL 编码
            encryptToken = HttpUtility.UrlEncode(encryptToken);
            encryptAuthId = HttpUtility.UrlEncode(encryptAuthId);
            respondUrl = HttpUtility.UrlEncode(respondUrl);

            var formatDictionary = new Dictionary<string, string>()
            {
                { TOKEN_KEY, encryptToken },
                { AUTH_ID_KEY, encryptAuthId },
                { RESPOND_URL_KEY, respondUrl }
            };

            // 格式化链接
            serverSignOutUrl = KeyBuilder.Formatting(serverSignOutUrl, formatDictionary);
            return serverSignOutUrl;
        }


        /// <summary>
        /// 格式化应答链接。
        /// </summary>
        /// <param name="encryptToken">给定的已加密的令牌字符串。</param>
        /// <param name="respondUrl">给定的应答链接（支持登入与登出应答链接）。</param>
        /// <returns>返回格式化后的字符串。</returns>
        public static string FormatRespondUrl(string encryptToken, string respondUrl)
        {
            respondUrl.NotEmpty(nameof(respondUrl));

            encryptToken.NotEmpty(nameof(encryptToken));

            // URL 编码
            encryptToken = HttpUtility.UrlEncode(encryptToken);

            // 格式化链接
            respondUrl = KeyBuilder.Formatting(respondUrl, TOKEN_KEY, encryptToken);
            return respondUrl;
        }

        #endregion

    }
}
