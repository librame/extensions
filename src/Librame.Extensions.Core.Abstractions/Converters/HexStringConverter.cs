#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core.Converters
{
    /// <summary>
    /// HEX 字符串转换器。
    /// </summary>
    public class HexStringConverter : AbstractAlgorithmConverter
    {
        /// <summary>
        /// 构造一个 <see cref="HexStringConverter"/>。
        /// </summary>
        public HexStringConverter()
            : base(f => f.AsHexString(), r => r.FromHexString())
        {
        }

    }
}
