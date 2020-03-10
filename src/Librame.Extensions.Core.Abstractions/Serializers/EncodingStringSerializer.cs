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

namespace Librame.Extensions.Core.Serializers
{
    /// <summary>
    /// 字符编码字符串序列化器。
    /// </summary>
    public class EncodingStringSerializer : AbstractStringSerializer<Encoding>
    {
        /// <summary>
        /// 反序列化字符串为字符编码。
        /// </summary>
        /// <param name="target">给定的字符串。</param>
        /// <returns>返回类型。</returns>
        public override Encoding Deserialize(string target)
            => target.FromEncodingName();

        /// <summary>
        /// 序列化字符编码为字符串。
        /// </summary>
        /// <param name="source">给定的类型。</param>
        /// <returns>返回字符串。</returns>
        public override string Serialize(Encoding source)
            => source.AsName();
    }
}
