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
        /// <param name="serializerSettings">序列化首选项（可选）。</param>
        /// <param name="converters">转换器数组（可选）。</param>
        /// <returns>返回经过解析的对象。</returns>
        public static T AsWebApiByJson<T>(this HttpSessionState session,
            string configKey, Func<string, string> requestUrlFormat,
            JsonSerializerSettings serializerSettings = null,
            params JsonConverter[] converters)
            where T : class
        {
            return (T)session.AsWebApiByJson(configKey, requestUrlFormat,
                typeof(T), serializerSettings, converters);
        }
        /// <summary>
        /// 转换为 WebApi 对象（如果会话中不存在，则发起远程请求并解析对象）。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="session">给定的 <see cref="HttpSessionState"/>。</param>
        /// <param name="requestUrl">给定的请求链接。</param>
        /// <param name="serializerSettings">序列化首选项（可选）。</param>
        /// <param name="converters">转换器数组（可选）。</param>
        /// <returns>返回经过解析的对象。</returns>
        public static T AsWebApiByJson<T>(this HttpSessionState session,
            string requestUrl,
            JsonSerializerSettings serializerSettings = null,
            params JsonConverter[] converters)
        {
            return (T)new HttpSessionStateWrapper(session).AsWebApiByJson(requestUrl,
                typeof(T), serializerSettings, converters);
        }

        /// <summary>
        /// 转换为 WebApi 对象（如果会话中不存在，则发起远程请求并解析对象）。
        /// </summary>
        /// <param name="session">给定的 <see cref="HttpSessionState"/>。</param>
        /// <param name="configKey">给定的配置（如 AppSettings）键名。</param>
        /// <param name="requestUrlFormat">给定的请求链接格式化方法。</param>
        /// <param name="type">给定的类型。</param>
        /// <param name="serializerSettings">序列化首选项（可选）。</param>
        /// <param name="converters">转换器数组（可选）。</param>
        /// <returns>返回经过解析的对象。</returns>
        public static object AsWebApiByJson(this HttpSessionState session,
            string configKey, Func<string, string> requestUrlFormat, Type type,
            JsonSerializerSettings serializerSettings = null,
            params JsonConverter[] converters)
        {
            var requestUrl = ConfigurationManager.AppSettings[configKey];
            requestUrl = requestUrlFormat(requestUrl);

            return new HttpSessionStateWrapper(session).AsWebApi(requestUrl,
                s => s.FromJson(type, serializerSettings, converters));
        }
        /// <summary>
        /// 转换为 WebApi 对象（如果会话中不存在，则发起远程请求并解析对象）。
        /// </summary>
        /// <param name="session">给定的 <see cref="HttpSessionState"/>。</param>
        /// <param name="requestUrl">给定的请求链接。</param>
        /// <param name="type">给定的类型。</param>
        /// <param name="serializerSettings">序列化首选项（可选）。</param>
        /// <param name="converters">转换器数组（可选）。</param>
        /// <returns>返回经过解析的对象。</returns>
        public static object AsWebApiByJson(this HttpSessionState session,
            string requestUrl, Type type,
            JsonSerializerSettings serializerSettings = null,
            params JsonConverter[] converters)
        {
            return new HttpSessionStateWrapper(session).AsWebApi(requestUrl,
                s => s.FromJson(type, serializerSettings, converters));
        }


        /// <summary>
        /// 转换为 WebApi 对象（如果会话中不存在，则发起远程请求并解析对象）。
        /// </summary>
        /// <param name="session">给定的 <see cref="HttpSessionState"/>。</param>
        /// <param name="requestUrl">给定的请求链接。</param>
        /// <param name="resolveFactory">给定的解析工厂方法。</param>
        /// <returns>返回经过解析的对象。</returns>
        public static object AsWebApi(this HttpSessionState session,
            string requestUrl, Func<string, object> resolveFactory)
        {
            return new HttpSessionStateWrapper(session).AsWebApi(requestUrl, resolveFactory);
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
        /// <param name="serializerSettings">序列化首选项（可选）。</param>
        /// <param name="converters">转换器数组（可选）。</param>
        /// <returns>返回经过解析的对象。</returns>
        public static T AsWebApiByJson<T>(this HttpSessionStateBase session,
            string configKey, Func<string, string> requestUrlFormat,
            JsonSerializerSettings serializerSettings = null,
            params JsonConverter[] converters)
            where T : class
        {
            return (T)session.AsWebApiByJson(configKey, requestUrlFormat, typeof(T),
                serializerSettings, converters);
        }
        /// <summary>
        /// 转换为 WebApi 对象（如果会话中不存在，则发起远程请求并解析对象）。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="session">给定的 <see cref="HttpSessionStateBase"/>。</param>
        /// <param name="requestUrl">给定的请求链接。</param>
        /// <param name="serializerSettings">序列化首选项（可选）。</param>
        /// <param name="converters">转换器数组（可选）。</param>
        /// <returns>返回经过解析的对象。</returns>
        public static T AsWebApiByJson<T>(this HttpSessionStateBase session,
            string requestUrl,
            JsonSerializerSettings serializerSettings = null,
            params JsonConverter[] converters)
        {
            return (T)session.AsWebApiByJson(requestUrl, typeof(T), serializerSettings, converters);
        }

        /// <summary>
        /// 转换为 WebApi 对象（如果会话中不存在，则发起远程请求并解析对象）。
        /// </summary>
        /// <param name="session">给定的 <see cref="HttpSessionStateBase"/>。</param>
        /// <param name="configKey">给定的配置（如 AppSettings）键名。</param>
        /// <param name="requestUrlFormat">给定的请求链接格式化方法。</param>
        /// <param name="type">给定的类型。</param>
        /// <param name="serializerSettings">序列化首选项（可选）。</param>
        /// <param name="converters">转换器数组（可选）。</param>
        /// <returns>返回经过解析的对象。</returns>
        public static object AsWebApiByJson(this HttpSessionStateBase session,
            string configKey, Func<string, string> requestUrlFormat, Type type,
            JsonSerializerSettings serializerSettings = null,
            params JsonConverter[] converters)
        {
            var requestUrl = ConfigurationManager.AppSettings[configKey];
            requestUrl = requestUrlFormat(requestUrl);

            return session.AsWebApi(requestUrl, s => s.FromJson(type, serializerSettings, converters));
        }
        /// <summary>
        /// 转换为 WebApi 对象（如果会话中不存在，则发起远程请求并解析对象）。
        /// </summary>
        /// <param name="session">给定的 <see cref="HttpSessionStateBase"/>。</param>
        /// <param name="requestUrl">给定的请求链接。</param>
        /// <param name="type">给定的类型。</param>
        /// <param name="serializerSettings">序列化首选项（可选）。</param>
        /// <param name="converters">转换器数组（可选）。</param>
        /// <returns>返回经过解析的对象。</returns>
        public static object AsWebApiByJson(this HttpSessionStateBase session,
            string requestUrl, Type type,
            JsonSerializerSettings serializerSettings = null,
            params JsonConverter[] converters)
        {
            return session.AsWebApi(requestUrl, s => s.FromJson(type, serializerSettings, converters));
        }

        
        /// <summary>
        /// 转换为 WebApi 对象（如果会话中不存在，则发起远程请求并解析对象）。
        /// </summary>
        /// <param name="session">给定的 <see cref="HttpSessionStateBase"/>。</param>
        /// <param name="requestUrl">给定的请求链接。</param>
        /// <param name="resolveFactory">给定的解析工厂方法。</param>
        /// <returns>返回经过解析的对象。</returns>
        public static object AsWebApi(this HttpSessionStateBase session,
            string requestUrl, Func<string, object> resolveFactory)
        {
            // 以链接为缓存键名
            var obj = session[requestUrl];
            if (obj == null)
            {
                // 获取 API 链接的响应内容
                var content = UriUtility.ReadContent(requestUrl);

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
