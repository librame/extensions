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

namespace Librame.Utility
{
    /// <summary>
    /// <see cref="Guid"/> 实用工具。
    /// </summary>
    public class GuidUtility
    {
        /// <summary>
        /// 转换为十六进制字符串。
        /// </summary>
        /// <param name="guid">给定的全局唯一标识符。</param>
        /// <param name="hasConnector">是否包含连接符（可选；默认不包含连接符）。</param>
        /// <returns>返回 32 位长度的字符串。</returns>
        public static string AsBit(string guid, bool hasConnector = false)
        {
            return AsBit(Guid.Parse(guid), hasConnector);
        }
        /// <summary>
        /// 转换为十六进制字符串。
        /// </summary>
        /// <param name="guid">给定的 <see cref="Guid"/>。</param>
        /// <param name="hasConnector">是否包含连接符（可选；默认不包含连接符）。</param>
        /// <returns>返回 32 位长度的字符串。</returns>
        public static string AsBit(Guid guid, bool hasConnector = false)
        {
            var buffer = guid.ToByteArray();

            return ByteUtility.AsBit(buffer, hasConnector);
        }
        
        /// <summary>
        /// 还原为全局唯一标识符。
        /// </summary>
        /// <param name="bit">给定的十六进制字符串（支持有/无连接符）。</param>
        /// <returns>返回 <see cref="Guid"/>。</returns>
        public static Guid FromBit(string bit)
        {
            var buffer = ByteUtility.FromBit(bit);

            return new Guid(buffer);
        }

    }


    /// <summary>
    /// <see cref="GuidUtility"/> 静态扩展。
    /// </summary>
    public static class GuidUtilityExtensions
    {
        /// <summary>
        /// 转换为十六进制字符串。
        /// </summary>
        /// <param name="guid">给定的全局唯一标识符。</param>
        /// <param name="hasConnector">是否包含连接符（可选；默认不包含连接符）。</param>
        /// <returns>返回 32 位长度的字符串。</returns>
        public static string AsBit(this string guid, bool hasConnector = false)
        {
            return GuidUtility.AsBit(guid, hasConnector);
        }
        /// <summary>
        /// 转换为十六进制字符串。
        /// </summary>
        /// <param name="guid">给定的 <see cref="Guid"/>。</param>
        /// <param name="hasConnector">是否包含连接符（可选；默认不包含连接符）。</param>
        /// <returns>返回 32 位长度的字符串。</returns>
        public static string AsBit(this Guid guid, bool hasConnector = false)
        {
            return GuidUtility.AsBit(guid, hasConnector);
        }

        /// <summary>
        /// 还原为全局唯一标识符。
        /// </summary>
        /// <param name="bit">给定的十六进制字符串（同时支持有、无连接符）。</param>
        /// <returns>返回 <see cref="Guid"/>。</returns>
        public static Guid FromBitAsGuid(this string bit)
        {
            return GuidUtility.FromBit(bit);
        }

    }
}
