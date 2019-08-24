#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Storage
{
    /// <summary>
    /// 容量单位信息。
    /// </summary>
    public class CapacityUnitInfo
    {
        /// <summary>
        /// 构造一个 <see cref="CapacityUnitInfo"/>。
        /// </summary>
        /// <param name="format">给定的格式。</param>
        public CapacityUnitInfo(CapacityUnitFormat format)
        {
            Format = format;
        }


        /// <summary>
        /// 格式。
        /// </summary>
        public CapacityUnitFormat Format { get; }

        /// <summary>
        /// 二进制。
        /// </summary>
        public CapacityUnitDescriptor Binary { get; set; }

        /// <summary>
        /// 十进制。
        /// </summary>
        public CapacityUnitDescriptor Decimal { get; set; }
    }
}
