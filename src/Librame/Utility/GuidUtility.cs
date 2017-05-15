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
    public static class GuidUtility
    {
        /// <summary>
        /// 转换为十六进制字符串。
        /// </summary>
        /// <param name="guid">给定的全局唯一标识符。</param>
        /// <param name="hasConnector">是否包含连接符（可选；默认不包含连接符）。</param>
        /// <returns>返回 32 位长度的字符串。</returns>
        public static string AsHex(string guid, bool hasConnector = false)
        {
            return AsHex(Guid.Parse(guid), hasConnector);
        }
        /// <summary>
        /// 转换为十六进制字符串。
        /// </summary>
        /// <param name="guid">给定的 <see cref="Guid"/>。</param>
        /// <param name="hasConnector">是否包含连接符（可选；默认不包含连接符）。</param>
        /// <returns>返回 32 位长度的字符串。</returns>
        public static string AsHex(Guid guid, bool hasConnector = false)
        {
            var buffer = guid.ToByteArray();

            return buffer.AsHex(hasConnector);
        }
        
        /// <summary>
        /// 还原为全局唯一标识符。
        /// </summary>
        /// <param name="hex">给定的十六进制字符串（支持有/无连接符）。</param>
        /// <returns>返回 <see cref="Guid"/>。</returns>
        public static Guid FromHex(string hex)
        {
            var buffer = hex.FromHex();

            return new Guid(buffer);
        }

    }
}
