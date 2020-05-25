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
using System.Globalization;

namespace Librame.Extensions.Storage.Capacities
{
    /// <summary>
    /// 单位定义信息。
    /// </summary>
    public class UnitDefinitionInfo
    {
        /// <summary>
        /// 构造一个 <see cref="UnitDefinitionInfo"/>。
        /// </summary>
        /// <param name="name">给定的名称（如：KibiByte/KiloByte）。</param>
        /// <param name="abbr">给定的缩写（如：KiB/KB）。</param>
        /// <param name="baseNumber">给定的底数（用于幂运算）。</param>
        /// <param name="exponent">给定的指数（用于幂运算）。</param>
        public UnitDefinitionInfo(string name, string abbr, int baseNumber, int exponent)
            : this(name, abbr)
        {
            Size = (long)Math.Pow(baseNumber, exponent);
        }

        /// <summary>
        /// 构造一个 <see cref="UnitDefinitionInfo"/>。
        /// </summary>
        /// <param name="name">给定的名称（如：KibiByte/KiloByte）。</param>
        /// <param name="abbr">给定的缩写（如：KiB/KB）。</param>
        /// <param name="size">给定的大小。</param>
        public UnitDefinitionInfo(string name, string abbr, long size)
            : this(name, abbr)
        {
            Size = size;
        }

        private UnitDefinitionInfo(string name, string abbr)
        {
            Name = name.NotEmpty(nameof(name));
            Abbr = abbr.NotEmpty(nameof(abbr));
        }


        /// <summary>
        /// 名称（如：KibiByte/KiloByte）。
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 简称（如：KiB/KB）。
        /// </summary>
        public string Abbr { get; }

        /// <summary>
        /// 大小。
        /// </summary>
        public long Size { get; }


        /// <summary>
        /// 格式化为带单位的字符串。
        /// </summary>
        /// <param name="size">给定要格式化的文件大小。</param>
        /// <returns>返回字符串。</returns>
        public string FormatString(long size)
        {
            if (size <= Size)
                return ToString();

            var str = string.Format(CultureInfo.CurrentCulture,
                "{0:0,0.00} " + Abbr,
                ((double)size) / Size);

            // 移除可能存在的前置0
            return str.TrimStart('0');
        }


        /// <summary>
        /// 转换为字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
            => $"{Size} {Abbr}";
    }
}
