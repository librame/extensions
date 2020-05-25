#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections.Generic;
using System.Linq;

namespace Librame.Extensions.Storage.Capacities
{
    /// <summary>
    /// 单位定义助手。
    /// </summary>
    public static class UnitDefinitionHelper
    {
        /// <summary>
        /// 支持的单位定义描述符集合。
        /// </summary>
        /// <value>返回 <see cref="IReadOnlyList{UnitDefinitionDescriptor}"/>。</value>
        public static IReadOnlyList<UnitDefinitionDescriptor> SupportDescriptors
            => InitializeSupportDescriptors();

        /// <summary>
        /// 支持的二进制单位定义信息集合。
        /// </summary>
        /// <value>返回 <see cref="IReadOnlyList{UnitDefinitionInfo}"/>。</value>
        public static IReadOnlyList<UnitDefinitionInfo> SupportBinaryInfos
            => InitializeSupportBinaryInfos();

        /// <summary>
        /// 支持的十进制单位定义信息集合。
        /// </summary>
        /// <value>返回 <see cref="IReadOnlyList{UnitDefinitionInfo}"/>。</value>
        public static IReadOnlyList<UnitDefinitionInfo> SupportDecimalInfos
            => InitializeSupportDecimalInfos();


        /// <summary>
        /// 获取指定单位格式的单位定义描述符。
        /// </summary>
        /// <param name="format">给定的 <see cref="UnitFormat"/>。</param>
        /// <returns>返回 <see cref="UnitDefinitionDescriptor"/>。</returns>
        public static UnitDefinitionDescriptor GetDescriptor(UnitFormat format)
            => SupportDescriptors.First(info => info.Format == format);


        /// <summary>
        /// 获取单位定义信息。
        /// </summary>
        /// <param name="format">给定的 <see cref="UnitFormat"/>。</param>
        /// <param name="notation">给定的 <see cref="UnitNotation"/>。</param>
        /// <returns>返回 <see cref="UnitDefinitionInfo"/>。</returns>
        public static UnitDefinitionInfo GetInfo(UnitFormat format, UnitNotation notation)
        {
            var info = GetDescriptor(format);
            return notation == UnitNotation.BinarySystem ? info.BinaryInfo : info.DecimalInfo;
        }

        /// <summary>
        /// 获取指定单位计数制的单位定义信息集合。
        /// </summary>
        /// <param name="notation">给定的 <see cref="UnitNotation"/>。</param>
        /// <returns>返回 <see cref="IReadOnlyList{UnitDefinitionInfo}"/>。</returns>
        public static IReadOnlyList<UnitDefinitionInfo> GetInfos(UnitNotation notation)
            => notation == UnitNotation.BinarySystem ? SupportBinaryInfos : SupportDecimalInfos;


        private static List<UnitDefinitionInfo> InitializeSupportBinaryInfos()
            => SupportDescriptors.Select(descr => descr.BinaryInfo).ToList();

        private static List<UnitDefinitionInfo> InitializeSupportDecimalInfos()
            => SupportDescriptors.Select(descr => descr.DecimalInfo).ToList();

        private static List<UnitDefinitionDescriptor> InitializeSupportDescriptors()
        {
            var byteString = UnitFormat.Byte.AsEnumName();

            return new List<UnitDefinitionDescriptor>
            {
                // Byte
                new UnitDefinitionDescriptor(UnitFormat.Byte)
                {
                    BinaryInfo = new UnitDefinitionInfo(byteString, byteString, 0),
                    DecimalInfo = new UnitDefinitionInfo(byteString, byteString, 0)
                },

                // KByte
                new UnitDefinitionDescriptor(UnitFormat.KByte)
                {
                    BinaryInfo = new UnitDefinitionInfo("KibiByte", "KiB", 2, 10),
                    DecimalInfo = new UnitDefinitionInfo("KiloByte", "KB", 10, 3)
                },

                // MByte
                new UnitDefinitionDescriptor(UnitFormat.MByte)
                {
                    BinaryInfo = new UnitDefinitionInfo("MebiByte", "MiB", 2, 20),
                    DecimalInfo = new UnitDefinitionInfo("MegaByte", "MB", 10, 6)
                },

                // GByte
                new UnitDefinitionDescriptor(UnitFormat.GByte)
                {
                    BinaryInfo = new UnitDefinitionInfo("GibiByte", "GiB", 2, 30),
                    DecimalInfo = new UnitDefinitionInfo("GigaByte", "GB", 10, 9)
                },

                // TByte
                new UnitDefinitionDescriptor(UnitFormat.TByte)
                {
                    BinaryInfo = new UnitDefinitionInfo("TebiByte", "TiB", 2, 40),
                    DecimalInfo = new UnitDefinitionInfo("TeraByte", "TB", 10, 12)
                },

                // PByte
                new UnitDefinitionDescriptor(UnitFormat.PByte)
                {
                    BinaryInfo = new UnitDefinitionInfo("PebiByte", "PiB", 2, 50),
                    DecimalInfo = new UnitDefinitionInfo("PetaByte", "PB", 10, 15)
                },

                // EByte
                new UnitDefinitionDescriptor(UnitFormat.EByte)
                {
                    BinaryInfo = new UnitDefinitionInfo("ExbiByte", "EiB", 2, 60),
                    DecimalInfo = new UnitDefinitionInfo("ExaByte", "EB", 10, 18)
                },

                // ZByte
                new UnitDefinitionDescriptor(UnitFormat.ZByte)
                {
                    BinaryInfo = new UnitDefinitionInfo("ZebiByte", "ZiB", 2, 70),
                    DecimalInfo = new UnitDefinitionInfo("ZettaByte", "ZB", 10, 21)
                },

                // YByte
                new UnitDefinitionDescriptor(UnitFormat.YByte)
                {
                    BinaryInfo = new UnitDefinitionInfo("YobiByte", "YiB", 2, 80),
                    DecimalInfo = new UnitDefinitionInfo("YottaByte", "YB", 10, 24)
                }
            };
        }

    }
}
