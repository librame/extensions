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
using System.Runtime.InteropServices;

namespace Librame.Utility
{
    /// <summary>
    /// <see cref="byte"/> 实用工具。
    /// </summary>
    public static class ByteUtility
    {
        /// <summary>
        /// 将对象转换为字节数组。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] AsBytes(this object obj)
        {
            obj.NotNull(nameof(obj));

            var buffer = new byte[Marshal.SizeOf(obj)];
            var ip = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);

            // 此方法比 BinaryFormatter 序列化方法生成的字节数组短了很多，非常节省空间
            Marshal.StructureToPtr(obj, ip, true);

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
            var ptr = Marshal.UnsafeAddrOfPinnedArrayElement(bytes, 0);

            return Marshal.PtrToStructure(ptr, type);
        }

    }
}
