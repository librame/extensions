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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Librame.Utility
{
    /// <summary>
    /// 序列化器。
    /// </summary>
    public class Serializer
    {
        /// <summary>
        /// 序列化对象为 Base64 字符串。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回字符串。</returns>
        public static string SerializeBase64(object obj)
        {
            var buffer = SerializeBytes(obj);
            
            return Convert.ToBase64String(buffer);
        }

        /// <summary>
        /// 序列化对象为十六进制字符串。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回字符串。</returns>
        public static string SerializeBit(object obj)
        {
            var buffer = SerializeBytes(obj);
            
            return buffer.AsBit();
        }

        /// <summary>
        /// 序列化对象为字节数组。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] SerializeBytes(object obj)
        {
            if (ReferenceEquals(obj, null))
                return null;
            
            try
            {
                byte[] buffer = null;

                var bf = new BinaryFormatter();
                using (var ms = new MemoryStream())
                {
                    bf.Serialize(ms, obj);

                    buffer = ms.GetBuffer();

                    //buffer = new byte[ms.Length];
                    //ms.Position = 0;
                    //ms.Read(buffer, 0, buffer.Length);
                    //ms.Flush();
                }

                return buffer;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 反序列化 Base64 字符串为类型实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="base64">给定的 Base64 字符串。</param>
        /// <returns>返回类型实例。</returns>
        public static T DeserializeBase64<T>(string base64)
        {
            return (T)DeserializeBase64(base64);
        }
        /// <summary>
        /// 反序列化 Base64 字符串为对象。
        /// </summary>
        /// <param name="base64">给定的 Base64 字符串。</param>
        /// <returns>返回对象。</returns>
        public static object DeserializeBase64(string base64)
        {
            if (string.IsNullOrEmpty(base64))
                return null;

            var buffer = Convert.FromBase64String(base64);

            return Deserialize(buffer);
        }

        /// <summary>
        /// 反序列化十六进制字符串为类型实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="bit">给定的 Bit 字符串。</param>
        /// <returns>返回类型实例。</returns>
        public static T DeserializeBit<T>(string bit)
        {
            return (T)DeserializeBit(bit);
        }
        /// <summary>
        /// 反序列化十六进制字符串为对象。
        /// </summary>
        /// <param name="bit">给定的 Bit 字符串。</param>
        /// <returns>返回对象。</returns>
        public static object DeserializeBit(string bit)
        {
            if (string.IsNullOrEmpty(bit))
                return null;

            var buffer = bit.FromBitAsBytes();

            return Deserialize(buffer);
        }

        /// <summary>
        /// 反序列化字节数组为对象。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <returns>返回对象。</returns>
        public static object Deserialize(byte[] buffer)
        {
            if (ReferenceEquals(buffer, null))
                return null;
            
            try
            {
                object obj = null;

                var bf = new BinaryFormatter();
                using (var ms = new MemoryStream(buffer))
                {
                    obj = bf.Deserialize(ms);

                    ms.Flush();
                }

                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }


    /// <summary>
    /// <see cref="Serializer"/> 静态扩展。
    /// </summary>
    public static class SerializerExtensions
    {
        /// <summary>
        /// 序列化对象为 Base64 字符串。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回字符串。</returns>
        public static string AsBase64(this object obj)
        {
            return Serializer.SerializeBase64(obj);
        }

        /// <summary>
        /// 序列化对象为十六进制字符串。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回字符串。</returns>
        public static string AsBit(this object obj)
        {
            return Serializer.SerializeBit(obj);
        }

        /// <summary>
        /// 序列化对象为字节数组。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] AsBytes(this object obj)
        {
            return Serializer.SerializeBytes(obj);
        }


        /// <summary>
        /// 反序列化 Base64 字符串为类型实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="base64">给定的 Base64 字符串。</param>
        /// <returns>返回类型实例。</returns>
        public static T FromBase64<T>(string base64)
        {
            return Serializer.DeserializeBase64<T>(base64);
        }
        /// <summary>
        /// 反序列化 Base64 字符串为对象。
        /// </summary>
        /// <param name="base64">给定的 Base64 字符串。</param>
        /// <returns>返回对象。</returns>
        public static object FromBase64(string base64)
        {
            return Serializer.DeserializeBase64(base64);
        }

        /// <summary>
        /// 反序列化十六进制字符串为类型实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="bit">给定的 Bit 字符串。</param>
        /// <returns>返回类型实例。</returns>
        public static T FromBit<T>(string bit)
        {
            return Serializer.DeserializeBit<T>(bit);
        }
        /// <summary>
        /// 反序列化十六进制字符串为对象。
        /// </summary>
        /// <param name="bit">给定的 Bit 字符串。</param>
        /// <returns>返回对象。</returns>
        public static object FromBit(string bit)
        {
            return Serializer.DeserializeBit(bit);
        }

        /// <summary>
        /// 反序列化字节数组为对象。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <returns>返回对象。</returns>
        public static object FromBytes(byte[] buffer)
        {
            return Serializer.Deserialize(buffer);
        }

    }
}
