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
using System.Globalization;

namespace Librame.Utility
{
    /// <summary>
    /// <see cref="decimal"/> 实用工具。
    /// </summary>
    public static class DecimalUtility
    {
        /// <summary>
        /// 基数据类型连接符。
        /// </summary>
        public const string BIT_CONNECTOR = "-";


        /// <summary>
        /// 转换为十六进制字符串。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="hasConnector">是否包含连接符（可选；默认不包含连接符）。</param>
        /// <returns>返回字符串。</returns>
        public static string AsHex(this byte[] buffer, bool hasConnector = false)
        {
            var bit = BitConverter.ToString(buffer);

            if (!hasConnector)
            {
                // 清空连接符
                bit = bit.Replace(BIT_CONNECTOR, string.Empty);
            }

            return bit;
        }

        /// <summary>
        /// 还原为字节数组。
        /// </summary>
        /// <param name="hex">给定的十六进制字符串（支持有/无连接符）。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] FromHex(this string hex)
        {
            hex.NotNullOrEmpty(nameof(hex));

            byte[] buffer = null;

            if (hex.Contains(BIT_CONNECTOR))
            {
                // 包含连接符的情况
                var bits = hex.Split(BIT_CONNECTOR.ToCharArray());
                buffer = new byte[bits.Length];

                for (int i = 0; i < bits.Length; i++)
                {
                    buffer[i] = byte.Parse(bits[i], NumberStyles.HexNumber);
                }
            }
            else
            {
                // 无连接符的情况（以两个字符为一组）
                var bits = hex.ToCharArray();
                buffer = new byte[bits.Length / 2];

                var index = 0;
                for (int i = 0; i < bits.Length; i += 2)
                {
                    // 将顺序索引相连的字符组合为一个字符串
                    var part = new char[] { bits[i], bits[i + 1] };
                    var s = new string(part);

                    buffer[index] = byte.Parse(s, NumberStyles.HexNumber);
                    index++;
                }
            }

            return buffer;
        }

    }
}
