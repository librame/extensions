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

namespace Librame.Extensions.Storage
{
    /// <summary>
    /// 容量单位描述符。
    /// </summary>
    public class CapacityUnitDescriptor : IEquatable<CapacityUnitDescriptor>
    {
        /// <summary>
        /// 构造一个 <see cref="CapacityUnitDescriptor"/> 实例。
        /// </summary>
        /// <param name="name">给定的名称。</param>
        /// <param name="abbr">给定的缩写。</param>
        /// <param name="baseNumber">给定的底数（用于幂运算）。</param>
        /// <param name="exponent">给定的指数（用于幂运算）。</param>
        public CapacityUnitDescriptor(string name, string abbr, int baseNumber, int exponent)
            : this(name, abbr)
        {
            Size = (long)Math.Pow(baseNumber, exponent);
        }
        /// <summary>
        /// 构造一个 <see cref="CapacityUnitDescriptor"/> 实例。
        /// </summary>
        /// <param name="name">给定的名称。</param>
        /// <param name="abbr">给定的缩写。</param>
        /// <param name="size">给定的大小。</param>
        public CapacityUnitDescriptor(string name, string abbr, long size)
            : this(name, abbr)
        {
            Size = size;
        }
        
        private CapacityUnitDescriptor(string name, string abbr)
        {
            Name = name.NotNullOrEmpty(nameof(name));
            Abbr = abbr.NotNullOrEmpty(nameof(abbr));
        }

        
        /// <summary>
        /// 名称。
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// 简称。
        /// </summary>
        public string Abbr { get; protected set; }

        /// <summary>
        /// 大小。
        /// </summary>
        public long Size { get; protected set; }


        /// <summary>
        /// 比较相等。
        /// </summary>
        /// <param name="other">给定的 <see cref="CapacityUnitDescriptor"/>。</param>
        /// <returns>返回布尔值。</returns>
        public bool Equals(CapacityUnitDescriptor other)
        {
            return Name == other?.Name;
        }

        /// <summary>
        /// 比较相等。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
        {
            if (obj is CapacityUnitDescriptor other)
                return Equals(other);

            return false;
        }


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回 32 位带符号整数。</returns>
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }


        /// <summary>
        /// 转换为字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
        {
            return $"{Name},{Abbr},{Size}";
        }

    }
}
