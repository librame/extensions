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
    using Algorithm;
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
        internal const string AUTHORIZATION_KEY = AUTHORIZATION_TYPE;

        /// <summary>
        /// 授权编号键名。
        /// </summary>
        internal const string AUTH_ID_KEY = "authid";
        /// <summary>
        /// 应答链接键名。
        /// </summary>
        internal const string RESPOND_URL_KEY = "respondurl";
        /// <summary>
        /// 认证票根键名。
        /// </summary>
        internal const string TICKET_KEY = "ticket";


        #region Resolve

        /// <summary>
        /// 解析指定请求包含的加密票根。
        /// </summary>
        /// <param name="request">给定的请求。</param>
        /// <returns>返回经过加密票根字符串。</returns>
        public static string ResolveEncryptTicket(HttpRequest request)
        {
            request.NotNull(nameof(request));

            return ResolveEncryptTicket(new HttpRequestWrapper(request));
        }
        /// <summary>
        /// 解析指定请求包含的加密票根。
        /// </summary>
        /// <param name="request">给定的请求。</param>
        /// <returns>返回经过加密票根字符串。</returns>
        public static string ResolveEncryptTicket(HttpRequestBase request)
        {
            request.NotNull(nameof(request));

            var encryptTicket = request.QueryString[TICKET_KEY];
            return encryptTicket;
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
        public static string FormatServerSignInUrl(string encryptAuthId,
            string respondUrl, string serverSignInUrl)
        {
            serverSignInUrl.NotNullOrEmpty(nameof(serverSignInUrl));
            
            encryptAuthId.NotNullOrEmpty(nameof(encryptAuthId));
            respondUrl.NotNullOrEmpty(nameof(respondUrl));

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
        /// <param name="encryptTicket">给定的已加密的票根字符串。</param>
        /// <param name="encryptAuthId">给定的加密授权编号。</param>
        /// <param name="respondUrl">给定的应答链接。</param>
        /// <param name="serverSignOutUrl">给定的服务器登出链接。</param>
        /// <returns>返回格式化后的字符串。</returns>
        public static string FormatServerSignOutUrl(string encryptTicket, string encryptAuthId,
            string respondUrl, string serverSignOutUrl)
        {
            serverSignOutUrl.NotNullOrEmpty(nameof(serverSignOutUrl));

            encryptTicket.NotNullOrEmpty(nameof(encryptTicket));
            encryptAuthId.NotNullOrEmpty(nameof(encryptAuthId));
            respondUrl.NotNullOrEmpty(nameof(respondUrl));

            // URL 编码
            encryptTicket = HttpUtility.UrlEncode(encryptTicket);
            encryptAuthId = HttpUtility.UrlEncode(encryptAuthId);
            respondUrl = HttpUtility.UrlEncode(respondUrl);

            var formatDictionary = new Dictionary<string, string>()
            {
                { TICKET_KEY, encryptTicket },
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
        /// <param name="encryptTicket">给定的已加密的票根字符串。</param>
        /// <param name="respondUrl">给定的应答链接（支持登入与登出应答链接）。</param>
        /// <returns>返回格式化后的字符串。</returns>
        public static string FormatRespondUrl(string encryptTicket, string respondUrl)
        {
            respondUrl.NotNullOrEmpty(nameof(respondUrl));

            encryptTicket.NotNullOrEmpty(nameof(encryptTicket));

            // URL 编码
            encryptTicket = HttpUtility.UrlEncode(encryptTicket);

            // 格式化链接
            respondUrl = KeyBuilder.Formatting(respondUrl, TICKET_KEY, encryptTicket);
            return respondUrl;
        }

        #endregion


        /// <summary>
        /// 生成授权编号（默认使用 <see cref="Guid.NewGuid"/>）。
        /// </summary>
        /// <returns>返回 32 位长度的字符串。</returns>
        public static string GenerateAuthId()
        {
            return GenerateAuthId(Guid.NewGuid());
        }
        /// <summary>
        /// 生成授权编号。
        /// </summary>
        /// <param name="guid">给定的全局唯一标识符。</param>
        /// <returns>返回 32 位长度的字符串。</returns>
        public static string GenerateAuthId(Guid guid)
        {
            return GuidUtility.AsHex(guid);
        }


        /// <summary>
        /// 生成令牌。
        /// </summary>
        /// <param name="algoAdapter">给定的算法适配器接口。</param>
        /// <returns>返回 64 位长度字符串。</returns>
        public static string GenerateToken(IAlgorithmAdapter algoAdapter)
        {
            algoAdapter.NotNull(nameof(algoAdapter));

            string authId = algoAdapter.Settings.AuthId;
            return GenerateToken(authId, algoAdapter);
        }
        /// <summary>
        /// 生成令牌。
        /// </summary>
        /// <param name="authId">给定的授权编号。</param>
        /// <param name="algoAdapter">给定的算法适配器接口。</param>
        /// <returns>返回 64 位长度字符串。</returns>
        public static string GenerateToken(string authId, IAlgorithmAdapter algoAdapter)
        {
            var guid = GuidUtility.FromHex(authId.NotNullOrEmpty(nameof(authId)));
            return algoAdapter.Hash.ToSha384(guid.ToString());
        }

    }
}
