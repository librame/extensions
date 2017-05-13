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
            name.NotNullOrEmpty(nameof(name));

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
        /// <param name="defaultValue">给定的默认值（可选）。</param>
        /// <param name="serializerSettings">序列化首选项（可选）。</param>
        /// <param name="converters">转换器数组（可选）。</param>
        /// <returns>返回经过解析的对象。</returns>
        public static T AsWebApi<T>(this HttpSessionState session,
            string configKey, T defaultValue = null,
            JsonSerializerSettings serializerSettings = null,
            params JsonConverter[] converters)
            where T : class
        {
            // 获取对象
            var obj = session.AsWebApi(configKey, typeof(T), serializerSettings, converters);

            // 如果对象不为空，则直接返回
            if (obj != null)
                return (T)obj;

            return defaultValue;
        }

        /// <summary>
        /// 转换为 WebApi 对象（如果会话中不存在，则发起远程请求并解析对象）。
        /// </summary>
        /// <param name="session">给定的 <see cref="HttpSessionState"/>。</param>
        /// <param name="configKey">给定的配置（如 AppSettings）键名。</param>
        /// <param name="type">给定的类型。</param>
        /// <param name="serializerSettings">序列化首选项（可选）。</param>
        /// <param name="converters">转换器数组（可选）。</param>
        /// <returns>返回经过解析的对象。</returns>
        public static object AsWebApi(this HttpSessionState session,
            string configKey, Type type,
            JsonSerializerSettings serializerSettings = null,
            params JsonConverter[] converters)
        {
            var webApi = ConfigurationManager.AppSettings[configKey];

            return session.AsWebApi(webApi, s => s.FromJson(type, serializerSettings, converters));
        }

        /// <summary>
        /// 转换为 WebApi 对象（如果会话中不存在，则发起远程请求并解析对象）。
        /// </summary>
        /// <param name="session">给定的 <see cref="HttpSessionState"/>。</param>
        /// <param name="webApiUrl">给定的 WebApi 链接。</param>
        /// <param name="resolveFactory">给定的解析工厂方法。</param>
        /// <returns>返回经过解析的对象。</returns>
        public static object AsWebApi(this HttpSessionState session,
            string webApiUrl, Func<string, object> resolveFactory)
        {
            // 以链接为缓存键名
            var obj = session[webApiUrl];
            if (obj == null)
            {
                // 获取 API 链接的响应内容
                var content = UriUtility.ReadContent(webApiUrl);

                // 解析对象
                obj = resolveFactory.Invoke(content);

                // 缓存对象
                session[webApiUrl] = obj;
            }

            return obj;
        }

        #endregion


        #region HttpSessionStateBase

        /// <summary>
        /// 转换为 WebApi 对象（如果会话中不存在，则发起远程请求并解析对象）。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="session">给定的 <see cref="HttpSessionStateBase"/>。</param>
        /// <param name="configKey">给定的配置（如 AppSettings）键名。</param>
        /// <param name="defaultValue">给定的默认值（可选）。</param>
        /// <param name="serializerSettings">序列化首选项（可选）。</param>
        /// <param name="converters">转换器数组（可选）。</param>
        /// <returns>返回经过解析的对象。</returns>
        public static T AsWebApi<T>(this HttpSessionStateBase session,
            string configKey, T defaultValue = null,
            JsonSerializerSettings serializerSettings = null,
            params JsonConverter[] converters)
            where T : class
        {
            // 获取对象
            var obj = session.AsWebApi(configKey, typeof(T), serializerSettings, converters);

            // 如果对象不为空，则直接返回
            if (obj != null)
                return (T)obj;

            return defaultValue;
        }

        /// <summary>
        /// 转换为 WebApi 对象（如果会话中不存在，则发起远程请求并解析对象）。
        /// </summary>
        /// <param name="session">给定的 <see cref="HttpSessionStateBase"/>。</param>
        /// <param name="configKey">给定的配置（如 AppSettings）键名。</param>
        /// <param name="type">给定的类型。</param>
        /// <param name="serializerSettings">序列化首选项（可选）。</param>
        /// <param name="converters">转换器数组（可选）。</param>
        /// <returns>返回经过解析的对象。</returns>
        public static object AsWebApi(this HttpSessionStateBase session,
            string configKey, Type type,
            JsonSerializerSettings serializerSettings = null,
            params JsonConverter[] converters)
        {
            var webApi = ConfigurationManager.AppSettings[configKey];

            return session.AsWebApi(webApi, s => s.FromJson(type, serializerSettings, converters));
        }

        /// <summary>
        /// 转换为 WebApi 对象（如果会话中不存在，则发起远程请求并解析对象）。
        /// </summary>
        /// <param name="session">给定的 <see cref="HttpSessionStateBase"/>。</param>
        /// <param name="webApiUrl">给定的 WebApi 链接。</param>
        /// <param name="resolveFactory">给定的解析工厂方法。</param>
        /// <returns>返回经过解析的对象。</returns>
        public static object AsWebApi(this HttpSessionStateBase session,
            string webApiUrl, Func<string, object> resolveFactory)
        {
            // 以链接为缓存键名
            var obj = session[webApiUrl];
            if (obj == null)
            {
                // 获取 API 链接的响应内容
                var content = UriUtility.ReadContent(webApiUrl);

                // 解析对象
                obj = resolveFactory.Invoke(content);

                // 缓存对象
                session[webApiUrl] = obj;
            }

            return obj;
        }

        #endregion

    }
}
