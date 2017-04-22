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
        /// 认证类型。
        /// </summary>
        public const string AUTHORIZATION_TYPE = LibrameAssemblyConstants.NAME + "Authorization";
        /// <summary>
        /// 认证键名。
        /// </summary>
        public const string AUTHORIZATION_KEY = AUTHORIZATION_TYPE;
        
        /// <summary>
        /// 授权编号键名。
        /// </summary>
        public const string AUTH_ID_KEY = "authid";
        /// <summary>
        /// 应答链接键名。
        /// </summary>
        public const string RESPOND_URL_KEY = "respondurl";
        /// <summary>
        /// 认证票根键名。
        /// </summary>
        public const string TICKET_KEY = "ticket";


        private static readonly IAlgorithmAdapter _algo = null;

        static AuthorizeHelper()
        {
            if (ReferenceEquals(_algo, null))
            {
                // 引入算法
                _algo = LibrameArchitecture.AdapterManager.AlgorithmAdapter;
            }
        }


        /// <summary>
        /// 格式化 SSO 认证服务器登录 URL。
        /// </summary>
        /// <param name="serverUrl">给定的认证服务器链接。</param>
        /// <param name="authId">给定的授权编号。</param>
        /// <param name="respondUrl">给定的应答链接。</param>
        /// <returns>返回格式化后的字符串。</returns>
        public static string FormatSsoServerSignInUrl(string serverUrl, string authId, string respondUrl)
        {
            serverUrl.GuardNullOrEmpty(nameof(serverUrl));
            authId.GuardNullOrEmpty(nameof(authId));
            respondUrl.GuardNullOrEmpty(nameof(respondUrl));

            // 默认使用标准对称加密算法
            var sa = _algo.StandardAes;

            // 加密授权编号参数
            authId = sa.Encrypt(authId);

            // URL 编码
            authId = HttpUtility.UrlEncode(authId);
            respondUrl = HttpUtility.UrlEncode(respondUrl);

            // 准备格式化键名
            var authIdKey = KeyBuilder.BuildFormatKey(AUTH_ID_KEY);
            var respondUrlKey = KeyBuilder.BuildFormatKey(RESPOND_URL_KEY);

            // 格式化链接参数
            serverUrl = KeyBuilder.FormatKeyValue(serverUrl, authIdKey, authId);
            serverUrl = KeyBuilder.FormatKeyValue(serverUrl, respondUrlKey, respondUrl);

            return serverUrl;
        }

        /// <summary>
        /// 解析指定请求包含的 SSO 认证服务器登录 URL 参数。
        /// </summary>
        /// <param name="request">给定的请求。</param>
        /// <returns>返回依次为认证编号、应答链接的元组。</returns>
        public static Tuple<string, string> ResolveSsoServerSignInUrl(HttpRequest request)
        {
            request.GuardNull(nameof(request));

            var authId = request.QueryString[AUTH_ID_KEY];
            var respondUrl = request.QueryString[RESPOND_URL_KEY];

            return new Tuple<string, string>(authId, respondUrl);
        }
        /// <summary>
        /// 解析指定请求包含的 SSO 认证服务器登录 URL 参数。
        /// </summary>
        /// <param name="request">给定的请求。</param>
        /// <returns>返回依次为认证编号、应答链接的元组。</returns>
        public static Tuple<string, string> ResolveSsoServerSignInUrl(HttpRequestBase request)
        {
            request.GuardNull(nameof(request));

            var authId = request.QueryString[AUTH_ID_KEY];
            var respondUrl = request.QueryString[RESPOND_URL_KEY];

            return new Tuple<string, string>(authId, respondUrl);
        }


        /// <summary>
        /// 格式化 SSO 认证登录应答 URL。
        /// </summary>
        /// <param name="respondUrl">给定的认证服务器应答链接。</param>
        /// <param name="ticket">给定的认证票根。</param>
        /// <returns>返回格式化后的字符串。</returns>
        public static string FormatSsoSignInRespondUrl(string respondUrl, string ticket)
        {
            respondUrl.GuardNullOrEmpty(nameof(respondUrl));
            ticket.GuardNullOrEmpty(nameof(ticket));
            
            // URL 编码
            ticket = HttpUtility.UrlEncode(ticket);

            // 准备格式化键名
            var ticketKey = KeyBuilder.BuildFormatKey(TICKET_KEY);

            // 格式化链接参数
            respondUrl = KeyBuilder.FormatKeyValue(respondUrl, ticketKey, ticket);

            return respondUrl;
        }

        /// <summary>
        /// 解析指定请求包含的 SSO 认证登录应答 URL 参数。
        /// </summary>
        /// <param name="request">给定的请求。</param>
        /// <returns>返回认证票根。</returns>
        public static string ResolveSsoSignInRespondUrl(HttpRequest request)
        {
            request.GuardNull(nameof(request));

            var ticket = request.QueryString[TICKET_KEY];

            return ticket;
        }
        /// <summary>
        /// 解析指定请求包含的 SSO 认证登录应答 URL 参数。
        /// </summary>
        /// <param name="request">给定的请求。</param>
        /// <returns>返回认证票根。</returns>
        public static string ResolveSsoSignInRespondUrl(HttpRequestBase request)
        {
            request.GuardNull(nameof(request));

            var ticket = request.QueryString[TICKET_KEY];

            return ticket;
        }


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
            return guid.AsBit();
        }


        /// <summary>
        /// 生成令牌。
        /// </summary>
        /// <param name="authId">给定的授权编号（可选；默认为 <see cref="Adaptation.AdapterSettings.AuthId"/>）。</param>
        /// <param name="hash">给定的哈希算法（可选；默认为 <see cref="IAlgorithmAdapter.Hash"/>）。</param>
        /// <returns>返回 64 位长度字符串。</returns>
        public static string GenerateToken(string authId = null, IHashAlgorithm hash = null)
        {
            if (ReferenceEquals(authId, null))
                authId = _algo.Settings.AuthId;

            if (ReferenceEquals(hash, null))
                hash = _algo.Hash;
            
            var guid = authId.FromBitAsGuid();
            return hash.ToSha384(guid.ToString());
        }

    }
}
