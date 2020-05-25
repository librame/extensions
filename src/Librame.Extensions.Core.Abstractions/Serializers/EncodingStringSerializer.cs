#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Text;

namespace Librame.Extensions.Core.Serializers
{
    /// <summary>
    /// 字符编码字符串序列化器。
    /// </summary>
    public class EncodingStringSerializer : AbstractStringSerializer<Encoding>
    {
        /// <summary>
        /// 构造一个 <see cref="EncodingStringSerializer"/>。
        /// </summary>
        public EncodingStringSerializer()
            : base(f => f.AsName(), r => r.FromEncodingName())
        {
        }

    }
}
