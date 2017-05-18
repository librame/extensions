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
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Librame.Utility
{
    /// <summary>
    /// <see cref="byte"/> 实用工具。
    /// </summary>
    public static class ByteUtility
    {

        #region Encoding

        /// <summary>
        /// 将字符串编码为字节序列。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="encoding">给定的字符编码。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] EncodeBytes(this string str, Encoding encoding = null)
        {
            encoding = encoding.AsOrDefault(Encoding.UTF8);

            return encoding.GetBytes(str);
        }

        /// <summary>
        /// 将字节序列还原为字符串。
        /// </summary>
        /// <param name="bytes">给定的字节数组。</param>
        /// <param name="encoding">给定的字符编码。</param>
        /// <returns>返回字符串。</returns>
        public static string DecodeBytes(this byte[] bytes, Encoding encoding = null)
        {
            encoding = encoding.AsOrDefault(Encoding.UTF8);

            return encoding.GetString(bytes);
        }

        #endregion


        #region Marshal

        /// <summary>
        /// 将对象转换为字节数组。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] AsBytes(this object obj)
        {
            obj.NotNull(nameof(obj));

            // 对象类名需设置 [StructLayout(LayoutKind.Sequential)] 属性特性，否则会抛出异常
            var size = Marshal.SizeOf(obj);
            var ip = Marshal.AllocHGlobal(size);

            // 此方法比 [Serializable] 序列化方法生成的字节数组短了很多，非常节省空间
            var buffer = new byte[size];

            try
            {
                Marshal.StructureToPtr(obj, ip, false /* 为 True 会出现进程卡死并退出 */);
                Marshal.Copy(ip, buffer, 0, size);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Marshal.FreeHGlobal(ip);
            }

            return buffer;
        }

        /// <summary>
        /// 将字节数组还原为对象。
        /// </summary>
        /// <typeparam name="T">给定的类型。</typeparam>
        /// <param name="bytes">给定的字节数组。</param>
        /// <returns>返回对象。</returns>
        public static T FromBytes<T>(this byte[] bytes)
        {
            return (T)bytes.FromBytes(typeof(T));
        }
        /// <summary>
        /// 将字节数组还原为对象。
        /// </summary>
        /// <param name="bytes">给定的字节数组。</param>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回对象。</returns>
        public static object FromBytes(this byte[] bytes, Type type)
        {
            var size = Marshal.SizeOf(type);
            var ip = Marshal.AllocHGlobal(size);

            object obj = null;

            try
            {
                Marshal.Copy(bytes, 0, ip, size);
                obj = Marshal.PtrToStructure(ip, type);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Marshal.FreeHGlobal(ip);
            }

            return obj;
        }

        #endregion


        #region Serialize

        /// <summary>
        /// 序列化对象为字节数组。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] SerializeBytes(this object obj)
        {
            obj.NotNull(nameof(obj));

            try
            {
                byte[] buffer = null;

                var bf = new BinaryFormatter();
                using (var ms = new MemoryStream())
                {
                    bf.Serialize(ms, obj);
                    buffer = ms.GetBuffer();
                }

                return buffer;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 反序列化字节数组为对象。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <returns>返回对象。</returns>
        public static T DeserializeBytes<T>(this byte[] buffer)
        {
            return (T)DeserializeBytes(buffer);
        }
        /// <summary>
        /// 反序列化字节数组为对象。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <returns>返回对象。</returns>
        public static object DeserializeBytes(this byte[] buffer)
        {
            buffer.NotNull(nameof(buffer));

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

        #endregion


        #region Base64

        /// <summary>
        /// 转换字节序列为 BASE64。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <returns>返回 BASE64 字符串。</returns>
        public static string AsBase64(this byte[] buffer)
        {
            buffer.NotNull(nameof(buffer));

            try
            {
                return Convert.ToBase64String(buffer);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 还原 BASE64 为字节序列。
        /// </summary>
        /// <param name="base64">给定的 BASE64 字符串。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] FromBase64(this string base64)
        {
            base64.NotEmpty(nameof(base64));

            try
            {
                return Convert.FromBase64String(base64);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


        #region Hex

        /// <summary>
        /// 转换字节序列为十六进制。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <returns>返回十六进制字符串。</returns>
        public static string AsHex(this byte[] buffer)
        {
            buffer.NotNull(nameof(buffer));

            try
            {
                var sb = new StringBuilder();

                if (buffer != null || buffer.Length == 0)
                {
                    // 同 BitConverter.ToString(buffer).Replace("-", string.Empty);
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        sb.Append(buffer[i].ToString("X2"));
                    }
                }

                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 还原十六进制为字节序列。
        /// </summary>
        /// <param name="hex">给定的十六进制字符串。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] FromHex(this string hex)
        {
            hex.NotNull(nameof(hex));

            if (hex.Length % 2 != 0)
            {
                throw new ArgumentException("hex length must be in multiples of 2.");
            }

            try
            {
                int length = hex.Length / 2;
                var buffer = new byte[length];

                for (int i = 0; i < length; i++)
                {
                    buffer[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
                }

                return buffer;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

    }
}
