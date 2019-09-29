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
    public class CapacityUnitDescriptor
    {
        /// <summary>
        /// 构造一个 <see cref="CapacityUnitDescriptor"/>。
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
        /// 构造一个 <see cref="CapacityUnitDescriptor"/>。
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
            Name = name.NotEmpty(nameof(name));
            Abbr = abbr.NotEmpty(nameof(abbr));
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
    }
}
