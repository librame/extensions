#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Librame.Extensions
{
    /// <summary>
    /// 序列化静态扩展。
    /// </summary>
    public static class SerializationExtensions
    {
        private static readonly Lazy<BinaryFormatter> _binaryFormatter
            = new Lazy<BinaryFormatter>(() => new BinaryFormatter());


        /// <summary>
        /// 序列化二进制为字节数组。
        /// </summary>
        /// <param name="graph">给定的对象。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] SerializeBinary(this object graph)
        {
            using (var ms = new MemoryStream())
            {
                SerializeBinary(graph, ms);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// 序列化二进制为文件。
        /// </summary>
        /// <param name="graph">给定的对象。</param>
        /// <param name="fileFullName">给定的文件完整名。</param>
        public static void SerializeBinary(this object graph, string fileFullName)
        {
            using (var fs = new FileStream(fileFullName, FileMode.Create))
            {
                SerializeBinary(graph, fs);
                fs.Flush();
            }
        }

        /// <summary>
        /// 序列化二进制。
        /// </summary>
        /// <param name="graph">给定的对象。</param>
        /// <param name="stream">给定的 <see cref="Stream"/>。</param>
        public static void SerializeBinary(this object graph, Stream stream)
            => _binaryFormatter.Value.Serialize(stream, graph);


        /// <summary>
        /// 反序列化二进制字节数组。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <returns>返回对象。</returns>
        public static object DeserializeBinary(this byte[] buffer)
        {
            using (var ms = new MemoryStream(buffer))
                return DeserializeBinary(ms);
        }

        /// <summary>
        /// 反序列化二进制文件。
        /// </summary>
        /// <param name="fileFullName">给定的文件完整名。</param>
        /// <returns>返回对象。</returns>
        public static object DeserializeBinary(this string fileFullName)
        {
            using (var fs = new FileStream(fileFullName, FileMode.Open))
                return DeserializeBinary(fs);
        }

        /// <summary>
        /// 反序列化二进制。
        /// </summary>
        /// <param name="stream">给定的 <see cref="Stream"/>。</param>
        /// <returns>返回对象。</returns>
        public static object DeserializeBinary(this Stream stream)
            => _binaryFormatter.Value.Deserialize(stream);
    }
}
