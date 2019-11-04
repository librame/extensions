#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections.Generic;
using System.Linq;

namespace Librame.Extensions.Storage
{
    /// <summary>
    /// 容量单位管理器。
    /// </summary>
    public static class CapacityUnitManager
    {
        static CapacityUnitManager()
        {
            if (Infos.IsEmpty())
            {
                var baseFormatString = CapacityUnitFormat.Byte.AsEnumName();

                Infos = new List<CapacityUnitInfo>
                {
                    // Byte
                    new CapacityUnitInfo(CapacityUnitFormat.Byte)
                    {
                        BinaryUnit = new CapacityUnitDescriptor(baseFormatString, baseFormatString, 0),
                        DecimalUnit = new CapacityUnitDescriptor(baseFormatString, baseFormatString, 0)
                    },

                    // KByte
                    new CapacityUnitInfo(CapacityUnitFormat.KByte)
                    {
                        BinaryUnit = new CapacityUnitDescriptor("KibiByte", "KiB", 2, 10),
                        DecimalUnit = new CapacityUnitDescriptor("KiloByte", "KB", 10, 3)
                    },

                    // MByte
                    new CapacityUnitInfo(CapacityUnitFormat.MByte)
                    {
                        BinaryUnit = new CapacityUnitDescriptor("MebiByte", "MiB", 2, 20),
                        DecimalUnit = new CapacityUnitDescriptor("MegaByte", "MB", 10, 6)
                    },

                    // GByte
                    new CapacityUnitInfo(CapacityUnitFormat.GByte)
                    {
                        BinaryUnit = new CapacityUnitDescriptor("GibiByte", "GiB", 2, 30),
                        DecimalUnit = new CapacityUnitDescriptor("GigaByte", "GB", 10, 9)
                    },

                    // TByte
                    new CapacityUnitInfo(CapacityUnitFormat.TByte)
                    {
                        BinaryUnit = new CapacityUnitDescriptor("TebiByte", "TiB", 2, 40),
                        DecimalUnit = new CapacityUnitDescriptor("TeraByte", "TB", 10, 12)
                    },

                    // PByte
                    new CapacityUnitInfo(CapacityUnitFormat.PByte)
                    {
                        BinaryUnit = new CapacityUnitDescriptor("PebiByte", "PiB", 2, 50),
                        DecimalUnit = new CapacityUnitDescriptor("PetaByte", "PB", 10, 15)
                    },

                    // EByte
                    new CapacityUnitInfo(CapacityUnitFormat.EByte)
                    {
                        BinaryUnit = new CapacityUnitDescriptor("ExbiByte", "EiB", 2, 60),
                        DecimalUnit = new CapacityUnitDescriptor("ExaByte", "EB", 10, 18)
                    },

                    // ZByte
                    new CapacityUnitInfo(CapacityUnitFormat.ZByte)
                    {
                        BinaryUnit = new CapacityUnitDescriptor("ZebiByte", "ZiB", 2, 70),
                        DecimalUnit = new CapacityUnitDescriptor("ZettaByte", "ZB", 10, 21)
                    },

                    // YByte
                    new CapacityUnitInfo(CapacityUnitFormat.YByte)
                    {
                        BinaryUnit = new CapacityUnitDescriptor("YobiByte", "YiB", 2, 80),
                        DecimalUnit = new CapacityUnitDescriptor("YottaByte", "YB", 10, 24)
                    }
                };
            }
        }


        /// <summary>
        /// 信息集合。
        /// </summary>
        public static IList<CapacityUnitInfo> Infos { get; }


        /// <summary>
        /// 获取信息。
        /// </summary>
        /// <param name="format">给定的单位格式。</param>
        /// <returns>返回 <see cref="CapacityUnitInfo"/>。</returns>
        public static CapacityUnitInfo GetInfo(CapacityUnitFormat format)
            => Infos.First(info => info.Format == format);


        /// <summary>
        /// 获取描述符。
        /// </summary>
        /// <param name="format">给定的 <see cref="CapacityUnitFormat"/>。</param>
        /// <param name="notation">给定的 <see cref="CapacityUnitNotation"/>。</param>
        /// <returns>返回 <see cref="CapacityUnitDescriptor"/>。</returns>
        public static CapacityUnitDescriptor GetDescriptor(CapacityUnitFormat format, CapacityUnitNotation notation)
        {
            var info = GetInfo(format);
            return notation == CapacityUnitNotation.BinaryUnit ? info.BinaryUnit : info.DecimalUnit;
        }

    }
}
