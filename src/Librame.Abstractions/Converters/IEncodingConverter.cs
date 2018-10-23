#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Text;

namespace Librame.Converters
{
    using Buffers;

    /// <summary>
    /// 字符编码转换器接口。
    /// </summary>
    public interface IEncodingConverter : IConverter<string, IBuffer<byte>>
    {
        /// <summary>
        /// 字符编码。
        /// </summary>
        Encoding Encoding { get; set; }
    }
}
