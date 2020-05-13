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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;

namespace Librame.Extensions.Core.Combiners
{
    /// <summary>
    /// <see cref="FilePathCombiner"/> 静态扩展。
    /// </summary>
    public static class AbstractionFilePathCombinerExtensions
    {
        /// <summary>
        /// 转换为文件路径组合器。
        /// </summary>
        /// <param name="fileName">给定的 <see cref="FileNameCombiner"/>。</param>
        /// <param name="basePath">给定的基础路径。</param>
        /// <returns>返回 <see cref="FilePathCombiner"/>。</returns>
        public static FilePathCombiner AsFilePathCombiner(this FileNameCombiner fileName, string basePath = null)
        {
            var combiner = new FilePathCombiner(fileName);

            if (!basePath.IsEmpty())
                combiner.ChangeBasePath(basePath);

            return combiner;
        }

        /// <summary>
        /// 转换为文件路径组合器。
        /// </summary>
        /// <param name="filePath">给定的文件路径。</param>
        /// <param name="basePath">给定的基础路径。</param>
        /// <returns>返回 <see cref="FilePathCombiner"/>。</returns>
        public static FilePathCombiner AsFilePathCombiner(this string filePath, string basePath = null)
        {
            var combiner = new FilePathCombiner(filePath);

            if (!basePath.IsEmpty())
                combiner.ChangeBasePath(basePath);

            return combiner;
        }

        /// <summary>
        /// 将文件名数组转换为文件路径组合器数组。
        /// </summary>
        /// <param name="filePaths">给定的文件路径数组。</param>
        /// <param name="basePath">给定的基础路径。</param>
        /// <returns>返回 <see cref="FilePathCombiner"/> 数组。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static FilePathCombiner[] AsFilePathCombiners(this string[] filePaths, string basePath = null)
        {
            filePaths.NotNull(nameof(filePaths));

            var combiners = new FilePathCombiner[filePaths.Length];

            for (var i = 0; i < filePaths.Length; i++)
                combiners[i] = filePaths[i].AsFilePathCombiner(basePath);

            return combiners;
        }


        /// <summary>
        /// 当作数组。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="FilePathCombiner"/>。</param>
        /// <returns>返回 <see cref="FilePathCombiner"/> 集合。</returns>
        public static FilePathCombiner[] AsArray(this FilePathCombiner combiner)
        {
            combiner.NotNull(nameof(combiner));
            return new FilePathCombiner[] { combiner };
        }

        /// <summary>
        /// 转换为字符串集合。
        /// </summary>
        /// <param name="combiners">给定的 <see cref="FilePathCombiner"/> 集合。</param>
        /// <returns>返回字符串集合。</returns>
        public static IEnumerable<string> ToStrings(this IEnumerable<FilePathCombiner> combiners)
            => combiners.NotNull(nameof(combiners)).Select(combiner => combiner?.ToString());


        /// <summary>
        /// 批量改变基础路径。
        /// </summary>
        /// <param name="combiners">给定的 <see cref="FilePathCombiner"/> 集合。</param>
        /// <param name="newBasePath">给定的新基础路径。</param>
        /// <returns>返回 <see cref="FilePathCombiner"/> 集合。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static IEnumerable<FilePathCombiner> ChangeBasePath(this IEnumerable<FilePathCombiner> combiners, string newBasePath)
        {
            combiners.NotNull(nameof(combiners));

            foreach (var combiner in combiners)
                combiner?.ChangeBasePath(newBasePath);

            return combiners;
        }


        #region FileInfo and DirectoryInfo

        /// <summary>
        /// 转换为 <see cref="FileInfo"/>。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="FilePathCombiner"/>。</param>
        /// <returns>返回 <see cref="FileInfo"/>。</returns>
        public static FileInfo AsFileInfo(this FilePathCombiner combiner)
            => new FileInfo(combiner);

        /// <summary>
        /// 将基础路径转换为 <see cref="DirectoryInfo"/>。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="FilePathCombiner"/>。</param>
        /// <returns>返回 <see cref="DirectoryInfo"/>。</returns>
        public static DirectoryInfo AsDirectoryInfo(this FilePathCombiner combiner)
            => new DirectoryInfo(combiner?.BasePath);


        /// <summary>
        /// 创建基础路径的目录。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="FilePathCombiner"/>。</param>
        /// <returns>返回 <see cref="DirectoryInfo"/>。</returns>
        public static DirectoryInfo CreateDirectory(this FilePathCombiner combiner)
            => Directory.CreateDirectory(combiner?.BasePath);


        /// <summary>
        /// 删除文件。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="FilePathCombiner"/>。</param>
        public static void Delete(this FilePathCombiner combiner)
            => File.Delete(combiner);

        /// <summary>
        /// 文件是否存在。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="FilePathCombiner"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool Exists(this FilePathCombiner combiner)
            => File.Exists(combiner);


        /// <summary>
        /// 目录是否存在。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="FilePathCombiner"/>。</param>
        public static void DirectoryExists(this FilePathCombiner combiner)
            => Directory.Delete(combiner?.BasePath);

        #endregion


        #region Read and Write

        /// <summary>
        /// 读取所有字节数组。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="FilePathCombiner"/>。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] ReadAllBytes(this FilePathCombiner combiner)
            => File.ReadAllBytes(combiner);

        /// <summary>
        /// 写入所有字节数组。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="FilePathCombiner"/>。</param>
        /// <param name="bytes">给定的字节数组。</param>
        public static void WriteAllBytes(this FilePathCombiner combiner, byte[] bytes)
            => File.WriteAllBytes(combiner, bytes);


        /// <summary>
        /// 读取所有行集合。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="FilePathCombiner"/>。</param>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选）。</param>
        /// <returns>返回字符串数组。</returns>
        public static string[] ReadAllLines(this FilePathCombiner combiner, Encoding encoding = null)
            => File.ReadAllLines(combiner, encoding ?? ExtensionSettings.Preference.DefaultEncoding);

        /// <summary>
        /// 写入所有行集合。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="FilePathCombiner"/>。</param>
        /// <param name="contents">给定的行内容集合。</param>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选）。</param>
        /// <param name="autoCreateDirectory">自动创建目录（可选；默认启用）。</param>
        public static void WriteAllLines(this FilePathCombiner combiner, IEnumerable<string> contents,
            Encoding encoding = null, bool autoCreateDirectory = true)
        {
            if (autoCreateDirectory)
                combiner.CreateDirectory();

            File.WriteAllLines(combiner, contents, encoding ?? ExtensionSettings.Preference.DefaultEncoding);
        }

        /// <summary>
        /// 附加所有行集合。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="FilePathCombiner"/>。</param>
        /// <param name="contents">给定的内容集合。</param>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选）。</param>
        public static void AppendAllLines(this FilePathCombiner combiner, IEnumerable<string> contents,
            Encoding encoding = null)
            => File.AppendAllLines(combiner?.Source, contents, encoding ?? ExtensionSettings.Preference.DefaultEncoding);


        /// <summary>
        /// 读取所有文本。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="FilePathCombiner"/>。</param>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选）。</param>
        /// <returns>返回字符串。</returns>
        public static string ReadAllText(this FilePathCombiner combiner, Encoding encoding = null)
            => File.ReadAllText(combiner, encoding ?? ExtensionSettings.Preference.DefaultEncoding);

        /// <summary>
        /// 写入所有文本。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="FilePathCombiner"/>。</param>
        /// <param name="contents">给定的内容集合。</param>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选）。</param>
        /// <param name="autoCreateDirectory">自动创建目录（可选；默认启用）。</param>
        public static void WriteAllText(this FilePathCombiner combiner, string contents,
            Encoding encoding = null, bool autoCreateDirectory = true)
        {
            if (autoCreateDirectory)
                combiner.CreateDirectory();

            File.WriteAllText(combiner, contents, encoding ?? ExtensionSettings.Preference.DefaultEncoding);
        }

        /// <summary>
        /// 附加所有文本。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="FilePathCombiner"/>。</param>
        /// <param name="contents">给定的内容集合。</param>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选）。</param>
        public static void AppendAllText(this FilePathCombiner combiner, string contents,
            Encoding encoding = null)
            => File.AppendAllText(combiner?.Source, contents, encoding ?? ExtensionSettings.Preference.DefaultEncoding);

        #endregion


        #region ReadJson and WriteJson

        /// <summary>
        /// 读取 JSON。
        /// </summary>
        /// <typeparam name="T">指定的反序列化类型。</typeparam>
        /// <param name="combiner">给定的 <see cref="FilePathCombiner"/>。</param>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选）。</param>
        /// <param name="settings">给定的 <see cref="JsonSerializerSettings"/>（可选）。</param>
        /// <returns>返回反序列化对象。</returns>
        public static T ReadJson<T>(this FilePathCombiner combiner, Encoding encoding = null,
            JsonSerializerSettings settings = null)
        {
            var json = combiner.ReadAllText(encoding);
            
            if (settings.IsNull())
                return JsonConvert.DeserializeObject<T>(json);

            return JsonConvert.DeserializeObject<T>(json, settings);
        }

        /// <summary>
        /// 读取 JSON。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="FilePathCombiner"/>。</param>
        /// <param name="type">给定的反序列化对象类型。</param>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选）。</param>
        /// <param name="settings">给定的 <see cref="JsonSerializerSettings"/>（可选）。</param>
        /// <returns>返回反序列化对象。</returns>
        public static object ReadJson(this FilePathCombiner combiner, Type type, Encoding encoding = null,
            JsonSerializerSettings settings = null)
        {
            var json = combiner.ReadAllText(encoding);

            if (settings.IsNull())
                return JsonConvert.DeserializeObject(json, type);

            return JsonConvert.DeserializeObject(json, type, settings);
        }


        /// <summary>
        /// 写入 JSON。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="FilePathCombiner"/>。</param>
        /// <param name="value"></param>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选）。</param>
        /// <param name="formatting">给定的 <see cref="Formatting"/>。</param>
        /// <param name="settings">给定的 <see cref="JsonSerializerSettings"/>（可选）。</param>
        /// <param name="autoCreateDirectory">自动创建目录（可选；默认启用）。</param>
        /// <returns>返回 JSON 字符串。</returns>
        public static string WriteJson(this FilePathCombiner combiner, object value, Encoding encoding = null,
            Formatting formatting = Formatting.Indented, JsonSerializerSettings settings = null, bool autoCreateDirectory = true)
        {
            var json = JsonConvert.SerializeObject(value, formatting, settings);
            combiner.WriteAllText(json, encoding, autoCreateDirectory);

            return json;
        }

        #endregion


        #region ReadSecureJson and WriteSecureJson

        /// <summary>
        /// 读取安全 JSON。
        /// </summary>
        /// <typeparam name="T">指定的反序列化类型。</typeparam>
        /// <param name="combiner">给定的 <see cref="FilePathCombiner"/>。</param>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选）。</param>
        /// <param name="settings">给定的 <see cref="JsonSerializerSettings"/>（可选）。</param>
        /// <returns>返回反序列化对象。</returns>
        public static T ReadSecureJson<T>(this FilePathCombiner combiner, Encoding encoding = null,
            JsonSerializerSettings settings = null)
        {
            var json = combiner.ReadSecureString(encoding);

            if (settings.IsNull())
                return JsonConvert.DeserializeObject<T>(json);

            return JsonConvert.DeserializeObject<T>(json, settings);
        }

        /// <summary>
        /// 读取安全 JSON。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="FilePathCombiner"/>。</param>
        /// <param name="type">给定的反序列化对象类型。</param>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选）。</param>
        /// <param name="settings">给定的 <see cref="JsonSerializerSettings"/>（可选）。</param>
        /// <returns>返回反序列化对象。</returns>
        public static object ReadSecureJson(this FilePathCombiner combiner, Type type, Encoding encoding = null,
            JsonSerializerSettings settings = null)
        {
            var json = combiner.ReadSecureString(encoding);

            if (settings.IsNull())
                return JsonConvert.DeserializeObject(json, type);

            return JsonConvert.DeserializeObject(json, type, settings);
        }

        private static string ReadSecureString(this FilePathCombiner combiner, Encoding encoding = null)
        {
            List<byte> buffer;

            if (encoding.IsNull())
                encoding = ExtensionSettings.Preference.DefaultEncoding;

            using (var fs = File.OpenRead(combiner))
            using (var br = new BinaryReader(fs, encoding))
            {
                if (fs.Length < int.MaxValue)
                    buffer = new List<byte>((int)fs.Length);
                else
                    buffer = new List<byte>();

                var chunk = new byte[1024];
                int readCount;

                do
                {
                    readCount = br.Read(chunk, 0, chunk.Length);
                    if (readCount > 0)
                    {
                        if (readCount < chunk.Length)
                            buffer.AddRange(chunk.Take(readCount));
                        else
                            buffer.AddRange(chunk);
                    }
                }
                while (readCount > 0);
            }

            return buffer.ToArray().FromAes().AsEncodingString(encoding);
        }


        /// <summary>
        /// 写入安全 JSON。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="FilePathCombiner"/>。</param>
        /// <param name="value"></param>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选）。</param>
        /// <param name="settings">给定的 <see cref="JsonSerializerSettings"/>（可选）。</param>
        /// <param name="autoCreateDirectory">自动创建目录（可选；默认启用）。</param>
        /// <returns>返回 JSON 字符串。</returns>
        public static string WriteSecureJson(this FilePathCombiner combiner, object value, Encoding encoding = null,
            JsonSerializerSettings settings = null, bool autoCreateDirectory = true)
        {
            var json = JsonConvert.SerializeObject(value, settings);

            if (autoCreateDirectory)
                combiner.CreateDirectory();

            if (combiner.Exists())
                combiner.Delete(); // 防止混合现有文件内容

            if (encoding.IsNull())
                encoding = ExtensionSettings.Preference.DefaultEncoding;

            using (var fs = File.OpenWrite(combiner))
            using (var bw = new BinaryWriter(fs, encoding))
            {
                var buffer = json.FromEncodingString(encoding);
                bw.Write(buffer.AsAes());
            }

            return json;
        }

        #endregion

    }
}
