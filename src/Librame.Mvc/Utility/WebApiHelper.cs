#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net;
using System.Web;
using System.Web.SessionState;

namespace Librame.Utility
{
    /// <summary>
    /// WebApi 助手。
    /// </summary>
    public static class WebApiHelper
    {
        private const string KEY_PREFIX = "WebApi:";

        /// <summary>
        /// 建立配置键名。
        /// </summary>
        /// <param name="name">给定的名称。</param>
        /// <returns>返回配置键名。</returns>
        public static string BuildConfigKey(this string name)
        {
            name.NotEmpty(nameof(name));

            if (!name.StartsWith(KEY_PREFIX))
                return (KEY_PREFIX + name);

            return name;
        }


        #region HttpSessionState

        /// <summary>
        /// 转换为 WebApi 对象（如果会话中不存在，则发起远程请求并解析对象）。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="session">给定的 <see cref="HttpSessionState"/>。</param>
        /// <param name="configKey">给定的配置（如 AppSettings）键名。</param>
        /// <param name="requestUrlFormat">给定的请求链接格式化方法。</param>
        /// <param name="token">给定的原始令牌（可选；默认解析认证信息）。</param>
        /// <param name="serializerSettings">序列化首选项（可选）。</param>
        /// <param name="converters">转换器数组（可选）。</param>
        /// <returns>返回经过解析的对象。</returns>
        public static T AsWebApiByJson<T>(this HttpSessionState session,
            string configKey, Func<string, string> requestUrlFormat, string token = null,
            JsonSerializerSettings serializerSettings = null,
            params JsonConverter[] converters)
            where T : class
        {
            return (T)session.AsWebApiByJson(configKey, requestUrlFormat,
                typeof(T), token, serializerSettings, converters);
        }
        /// <summary>
        /// 转换为 WebApi 对象（如果会话中不存在，则发起远程请求并解析对象）。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="session">给定的 <see cref="HttpSessionState"/>。</param>
        /// <param name="requestUrl">给定的请求链接。</param>
        /// <param name="token">给定的原始令牌（可选；默认解析认证信息）。</param>
        /// <param name="serializerSettings">序列化首选项（可选）。</param>
        /// <param name="converters">转换器数组（可选）。</param>
        /// <returns>返回经过解析的对象。</returns>
        public static T AsWebApiByJson<T>(this HttpSessionState session,
            string requestUrl, string token = null,
            JsonSerializerSettings serializerSettings = null,
            params JsonConverter[] converters)
        {
            return (T)new HttpSessionStateWrapper(session).AsWebApiByJson(requestUrl,
                typeof(T), token, serializerSettings, converters);
        }

        /// <summary>
        /// 转换为 WebApi 对象（如果会话中不存在，则发起远程请求并解析对象）。
        /// </summary>
        /// <param name="session">给定的 <see cref="HttpSessionState"/>。</param>
        /// <param name="configKey">给定的配置（如 AppSettings）键名。</param>
        /// <param name="requestUrlFormat">给定的请求链接格式化方法。</param>
        /// <param name="type">给定的类型。</param>
        /// <param name="token">给定的原始令牌（可选；默认解析认证信息）。</param>
        /// <param name="serializerSettings">序列化首选项（可选）。</param>
        /// <param name="converters">转换器数组（可选）。</param>
        /// <returns>返回经过解析的对象。</returns>
        public static object AsWebApiByJson(this HttpSessionState session,
            string configKey, Func<string, string> requestUrlFormat,
            Type type, string token = null,
            JsonSerializerSettings serializerSettings = null,
            params JsonConverter[] converters)
        {
            configKey.NotEmpty(nameof(configKey));

            var requestUrl = ConfigurationManager.AppSettings[configKey];
            requestUrl = requestUrlFormat(requestUrl.NotEmpty(nameof(requestUrl)));

            return new HttpSessionStateWrapper(session).AsWebApi(requestUrl,
                s => s.FromJson(type, true, serializerSettings, converters), token);
        }
        /// <summary>
        /// 转换为 WebApi 对象（如果会话中不存在，则发起远程请求并解析对象）。
        /// </summary>
        /// <param name="session">给定的 <see cref="HttpSessionState"/>。</param>
        /// <param name="requestUrl">给定的请求链接。</param>
        /// <param name="type">给定的类型。</param>
        /// <param name="token">给定的原始令牌（可选；默认解析认证信息）。</param>
        /// <param name="serializerSettings">序列化首选项（可选）。</param>
        /// <param name="converters">转换器数组（可选）。</param>
        /// <returns>返回经过解析的对象。</returns>
        public static object AsWebApiByJson(this HttpSessionState session,
            string requestUrl, Type type, string token = null,
            JsonSerializerSettings serializerSettings = null,
            params JsonConverter[] converters)
        {
            return new HttpSessionStateWrapper(session).AsWebApi(requestUrl,
                s => s.FromJson(type, true, serializerSettings, converters), token);
        }


        /// <summary>
        /// 转换为 WebApi 对象（如果会话中不存在，则发起远程请求并解析对象）。
        /// </summary>
        /// <param name="session">给定的 <see cref="HttpSessionState"/>。</param>
        /// <param name="requestUrl">给定的请求链接。</param>
        /// <param name="resolveFactory">给定的解析工厂方法。</param>
        /// <param name="token">给定的原始令牌（可选；默认解析认证信息）。</param>
        /// <returns>返回经过解析的对象。</returns>
        public static object AsWebApi(this HttpSessionState session,
            string requestUrl, Func<string, object> resolveFactory, string token = null)
        {
            return new HttpSessionStateWrapper(session).AsWebApi(requestUrl, resolveFactory, token);
        }

        #endregion


        #region HttpSessionStateBase

        /// <summary>
        /// 转换为 WebApi 对象（如果会话中不存在，则发起远程请求并解析对象）。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="session">给定的 <see cref="HttpSessionStateBase"/>。</param>
        /// <param name="configKey">给定的配置（如 AppSettings）键名。</param>
        /// <param name="requestUrlFormat">给定的请求链接格式化方法。</param>
        /// <param name="token">给定的原始令牌（可选；默认解析认证信息）。</param>
        /// <param name="serializerSettings">序列化首选项（可选）。</param>
        /// <param name="converters">转换器数组（可选）。</param>
        /// <returns>返回经过解析的对象。</returns>
        public static T AsWebApiByJson<T>(this HttpSessionStateBase session,
            string configKey, Func<string, string> requestUrlFormat, string token = null,
            JsonSerializerSettings serializerSettings = null,
            params JsonConverter[] converters)
            where T : class
        {
            return (T)session.AsWebApiByJson(configKey, requestUrlFormat, typeof(T), token,
                serializerSettings, converters);
        }
        /// <summary>
        /// 转换为 WebApi 对象（如果会话中不存在，则发起远程请求并解析对象）。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="session">给定的 <see cref="HttpSessionStateBase"/>。</param>
        /// <param name="requestUrl">给定的请求链接。</param>
        /// <param name="token">给定的原始令牌（可选；默认解析认证信息）。</param>
        /// <param name="serializerSettings">序列化首选项（可选）。</param>
        /// <param name="converters">转换器数组（可选）。</param>
        /// <returns>返回经过解析的对象。</returns>
        public static T AsWebApiByJson<T>(this HttpSessionStateBase session,
            string requestUrl, string token = null,
            JsonSerializerSettings serializerSettings = null,
            params JsonConverter[] converters)
        {
            return (T)session.AsWebApiByJson(requestUrl, typeof(T), token,
                serializerSettings, converters);
        }

        /// <summary>
        /// 转换为 WebApi 对象（如果会话中不存在，则发起远程请求并解析对象）。
        /// </summary>
        /// <param name="session">给定的 <see cref="HttpSessionStateBase"/>。</param>
        /// <param name="configKey">给定的配置（如 AppSettings）键名。</param>
        /// <param name="requestUrlFormat">给定的请求链接格式化方法。</param>
        /// <param name="type">给定的类型。</param>
        /// <param name="token">给定的原始令牌（可选；默认解析认证信息）。</param>
        /// <param name="serializerSettings">序列化首选项（可选）。</param>
        /// <param name="converters">转换器数组（可选）。</param>
        /// <returns>返回经过解析的对象。</returns>
        public static object AsWebApiByJson(this HttpSessionStateBase session,
            string configKey, Func<string, string> requestUrlFormat,
            Type type, string token = null,
            JsonSerializerSettings serializerSettings = null,
            params JsonConverter[] converters)
        {
            configKey.NotEmpty(nameof(configKey));

            var requestUrl = ConfigurationManager.AppSettings[configKey];
            requestUrl = requestUrlFormat(requestUrl.NotEmpty(nameof(requestUrl)));

            return session.AsWebApi(requestUrl,
                s => s.FromJson(type, true, serializerSettings, converters), token);
        }
        /// <summary>
        /// 转换为 WebApi 对象（如果会话中不存在，则发起远程请求并解析对象）。
        /// </summary>
        /// <param name="session">给定的 <see cref="HttpSessionStateBase"/>。</param>
        /// <param name="requestUrl">给定的请求链接。</param>
        /// <param name="type">给定的类型。</param>
        /// <param name="token">给定的原始令牌（可选；默认解析认证信息）。</param>
        /// <param name="serializerSettings">序列化首选项（可选）。</param>
        /// <param name="converters">转换器数组（可选）。</param>
        /// <returns>返回经过解析的对象。</returns>
        public static object AsWebApiByJson(this HttpSessionStateBase session,
            string requestUrl, Type type, string token = null,
            JsonSerializerSettings serializerSettings = null,
            params JsonConverter[] converters)
        {
            return session.AsWebApi(requestUrl,
                s => s.FromJson(type, true, serializerSettings, converters), token);
        }

        
        /// <summary>
        /// 转换为 WebApi 对象（如果会话中不存在，则发起远程请求并解析对象）。
        /// </summary>
        /// <param name="session">给定的 <see cref="HttpSessionStateBase"/>。</param>
        /// <param name="requestUrl">给定的请求链接。</param>
        /// <param name="resolveFactory">给定的解析工厂方法。</param>
        /// <param name="token">给定的原始令牌（可选；默认解析认证信息）。</param>
        /// <returns>返回经过解析的对象。</returns>
        public static object AsWebApi(this HttpSessionStateBase session,
            string requestUrl, Func<string, object> resolveFactory, string token = null)
        {
            // 以链接为缓存键名
            var obj = session[requestUrl];

            if (obj == null)
            {
                if (string.IsNullOrEmpty(token))
                {
                    // 尝试解析票根中的令牌
                    var ticket = session.ResolveTicket();
                    if (ticket == null || string.IsNullOrEmpty(ticket.Token))
                    {
                        var logger = LibrameArchitecture.Logging.GetLogger<HttpSessionStateBase>();
                        logger.Warn("解析认证信息失败，不能发起 WebApi 请求");
                        return obj;
                    }

                    token = ticket.Token;
                }

                // 加密令牌用于传输
                token = LibrameArchitecture.Adapters.Authorization.Managers.Ciphertext.Encode(token);

                // 获取 API 链接的响应内容
                var content = UriUtility.ReadContent(requestUrl, hwr =>
                {
                    hwr.Headers.Add(HttpRequestHeader.Authorization, token);
                    hwr.PreAuthenticate = true;
                });

                // 如果响应内容为空
                if (string.IsNullOrEmpty(content))
                    return obj;

                // 解析对象
                obj = resolveFactory.Invoke(content);

                // 缓存对象
                session[requestUrl] = obj;
            }

            return obj;
        }

        #endregion

    }
}
