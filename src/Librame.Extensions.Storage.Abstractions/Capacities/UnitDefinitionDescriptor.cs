#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Storage.Capacities
{
    /// <summary>
    /// 单位定义描述符。
    /// </summary>
    public class UnitDefinitionDescriptor
    {
        /// <summary>
        /// 构造一个 <see cref="UnitDefinitionDescriptor"/>。
        /// </summary>
        /// <param name="format">给定的 <see cref="UnitFormat"/>。</param>
        /// <param name="binaryInfo">给定的二进制 <see cref="UnitDefinitionInfo"/>。</param>
        /// <param name="decimalInfo">给定的十进制 <see cref="UnitDefinitionInfo"/>。</param>
        public UnitDefinitionDescriptor(UnitFormat format,
            UnitDefinitionInfo binaryInfo, UnitDefinitionInfo decimalInfo)
        {
            Format = format;
            BinaryInfo = binaryInfo.NotNull(nameof(binaryInfo));
            DecimalInfo = decimalInfo.NotNull(nameof(decimalInfo));
        }

        internal UnitDefinitionDescriptor(UnitFormat format)
        {
            Format = format;
        }


        /// <summary>
        /// 格式。
        /// </summary>
        public UnitFormat Format { get; }

        /// <summary>
        /// 二进制定义信息。
        /// </summary>
        public UnitDefinitionInfo BinaryInfo { get; internal set; }

        /// <summary>
        /// 十进制定义信息。
        /// </summary>
        public UnitDefinitionInfo DecimalInfo { get; internal set; }


        /// <summary>
        /// 转换为单位格式名称。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
            => Format.AsEnumName();
    }
}
