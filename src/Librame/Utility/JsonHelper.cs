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

namespace Librame.Utility
{
    /// <summary>
    /// JSON 助手。
    /// </summary>
    public class JsonHelper
    {
        /// <summary>
        /// 将指定值类型实例序列化为 JSON 字符串形式。
        /// </summary>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="value">给定的值。</param>
        /// <param name="formatting">要格式化的方式（可选）。</param>
        /// <param name="serializerSettings">序列化首选项（可选）。</param>
        /// <param name="converters">转换器数组（可选）。</param>
        /// <returns>返回 JSON 字符串。</returns>
        public static string AsJson<TValue>(TValue value, Formatting formatting = Formatting.None,
            JsonSerializerSettings serializerSettings = null, params JsonConverter[] converters)
        {
            return AsJson((object)value, formatting, serializerSettings, converters);
        }

        /// <summary>
        /// 将指定的对象序列化为 JSON 字符串形式。
        /// </summary>
        /// <param name="value">给定的值。</param>
        /// <param name="formatting">要格式化的方式（可选）。</param>
        /// <param name="serializerSettings">序列化首选项（可选）。</param>
        /// <param name="converters">转换器数组（可选）。</param>
        /// <returns>返回 JSON 字符串。</returns>
        public static string AsJson(object value, Formatting formatting = Formatting.None,
            JsonSerializerSettings serializerSettings = null, params JsonConverter[] converters)
        {
            try
            {
                if (ReferenceEquals(value, null))
                    return string.Empty;

                if (value.GetType().IsValueType)
                    return value.ToString();

                if (!ReferenceEquals(converters, null) && converters.Length > 0)
                    return JsonConvert.SerializeObject(value, formatting, converters);

                if (!ReferenceEquals(serializerSettings, null))
                    return JsonConvert.SerializeObject(value, formatting, serializerSettings);

                return JsonConvert.SerializeObject(value, formatting);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 将 JSON 字符串形式反序列化为指定值类型实例。
        /// </summary>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="json">给定的 JSON 字符串。</param>
        /// <param name="serializerSettings">序列化首选项（可选）。</param>
        /// <param name="converters">转换器数组（可选）。</param>
        /// <returns>返回值类型实例。</returns>
        public static TValue FromJson<TValue>(string json,
            JsonSerializerSettings serializerSettings = null, params JsonConverter[] converters)
        {
            return (TValue)FromJson(json, typeof(TValue), serializerSettings, converters);
        }

        /// <summary>
        /// 将 JSON 字符串形式反序列化为指定类型对象。
        /// </summary>
        /// <param name="json">给定的 JSON 字符串。</param>
        /// <param name="type">给定的类型。</param>
        /// <param name="serializerSettings">序列化首选项（可选）。</param>
        /// <param name="converters">转换器数组（可选）。</param>
        /// <returns>返回值类型实例。</returns>
        public static object FromJson(string json, Type type,
            JsonSerializerSettings serializerSettings = null, params JsonConverter[] converters)
        {
            try
            {
                if (!ReferenceEquals(serializerSettings, null))
                    return JsonConvert.DeserializeObject(json, type, serializerSettings);

                if (!ReferenceEquals(converters, null) && converters.Length > 0)
                    return JsonConvert.DeserializeObject(json, type, converters);

                return JsonConvert.DeserializeObject(json, type);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }


    /// <summary>
    /// <see cref="JsonHelper"/> 静态扩展。
    /// </summary>
    public static class JsonHelperExtensions
    {
        /// <summary>
        /// 将指定值类型实例序列化为 JSON 字符串形式。
        /// </summary>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="value">给定的值。</param>
        /// <param name="formatting">要格式化的方式（可选）。</param>
        /// <param name="serializerSettings">序列化首选项（可选）。</param>
        /// <param name="converters">转换器数组（可选）。</param>
        /// <returns>返回 JSON 字符串。</returns>
        public static string AsJson<TValue>(this TValue value, Formatting formatting = Formatting.None,
            JsonSerializerSettings serializerSettings = null, params JsonConverter[] converters)
        {
            return JsonHelper.AsJson(value, formatting, serializerSettings, converters);
        }

        /// <summary>
        /// 将指定的对象序列化为 JSON 字符串形式。
        /// </summary>
        /// <param name="value">给定的值。</param>
        /// <param name="formatting">要格式化的方式（可选）。</param>
        /// <param name="serializerSettings">序列化首选项（可选）。</param>
        /// <param name="converters">转换器数组（可选）。</param>
        /// <returns>返回 JSON 字符串。</returns>
        public static string AsJson(this object value, Formatting formatting = Formatting.None,
            JsonSerializerSettings serializerSettings = null, params JsonConverter[] converters)
        {
            return JsonHelper.AsJson(value, formatting, serializerSettings, converters);
        }


        /// <summary>
        /// 将 JSON 字符串形式反序列化为指定值类型实例。
        /// </summary>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="json">给定的 JSON 字符串。</param>
        /// <param name="serializerSettings">序列化首选项（可选）。</param>
        /// <param name="converters">转换器数组（可选）。</param>
        /// <returns>返回值类型实例。</returns>
        public static TValue FromJson<TValue>(this string json,
            JsonSerializerSettings serializerSettings = null, params JsonConverter[] converters)
        {
            return JsonHelper.FromJson<TValue>(json, serializerSettings, converters);
        }

        /// <summary>
        /// 将 JSON 字符串形式反序列化为指定类型对象。
        /// </summary>
        /// <param name="json">给定的 JSON 字符串。</param>
        /// <param name="type">给定的类型。</param>
        /// <param name="serializerSettings">序列化首选项（可选）。</param>
        /// <param name="converters">转换器数组（可选）。</param>
        /// <returns>返回值类型实例。</returns>
        public static object FromJson(this string json, Type type,
            JsonSerializerSettings serializerSettings = null, params JsonConverter[] converters)
        {
            return JsonHelper.FromJson(json, type, serializerSettings, converters);
        }

    }

}
