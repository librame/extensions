#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace Librame.Extensions
{
    /// <summary>
    /// <see cref="JsonConvert"/> 静态扩展。
    /// </summary>
    public static class JsonConvertExtensions
    {

        #region ReadJson and WriteJson

        /// <summary>
        /// 读取 JSON。
        /// </summary>
        /// <typeparam name="T">指定的反序列化类型。</typeparam>
        /// <param name="filePath">给定的文件路径。</param>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选）。</param>
        /// <param name="settings">给定的 <see cref="JsonSerializerSettings"/>（可选）。</param>
        /// <returns>返回反序列化对象。</returns>
        public static T ReadJson<T>(this string filePath, Encoding encoding = null,
            JsonSerializerSettings settings = null)
        {
            var json = File.ReadAllText(filePath, encoding ?? ExtensionSettings.UTF8Encoding);

            if (settings.IsNull())
                return JsonConvert.DeserializeObject<T>(json);

            return JsonConvert.DeserializeObject<T>(json, settings);
        }

        /// <summary>
        /// 读取 JSON。
        /// </summary>
        /// <param name="filePath">给定的文件路径。</param>
        /// <param name="type">给定的反序列化对象类型。</param>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选）。</param>
        /// <param name="settings">给定的 <see cref="JsonSerializerSettings"/>（可选）。</param>
        /// <returns>返回反序列化对象。</returns>
        public static object ReadJson(this string filePath, Type type, Encoding encoding = null,
            JsonSerializerSettings settings = null)
        {
            var json = File.ReadAllText(filePath, encoding ?? ExtensionSettings.UTF8Encoding);

            if (settings.IsNull())
                return JsonConvert.DeserializeObject(json, type);

            return JsonConvert.DeserializeObject(json, type, settings);
        }


        /// <summary>
        /// 写入 JSON。
        /// </summary>
        /// <param name="filePath">给定的文件路径。</param>
        /// <param name="value"></param>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选）。</param>
        /// <param name="formatting">给定的 <see cref="Formatting"/>。</param>
        /// <param name="settings">给定的 <see cref="JsonSerializerSettings"/>（可选）。</param>
        /// <param name="autoCreateDirectory">自动创建目录（可选；默认启用）。</param>
        /// <returns>返回 JSON 字符串。</returns>
        public static string WriteJson(this string filePath, object value, Encoding encoding = null,
            Formatting formatting = Formatting.Indented, JsonSerializerSettings settings = null, bool autoCreateDirectory = true)
        {
            var json = JsonConvert.SerializeObject(value, formatting, settings);

            if (autoCreateDirectory)
            {
                filePath.GetFileNameWithoutPath(out var basePath);
                Directory.CreateDirectory(basePath);
            }

            File.WriteAllText(filePath, json, encoding ?? ExtensionSettings.UTF8Encoding);
            return json;
        }

        #endregion

    }
}
