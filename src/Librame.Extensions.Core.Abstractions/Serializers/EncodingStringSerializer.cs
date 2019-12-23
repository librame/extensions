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
    public class EncodingStringSerializer : IObjectStringSerializer
    {
        internal const string DefaultName
            = nameof(Encoding);


        /// <summary>
        /// 名称。
        /// </summary>
        public string Name
            => DefaultName;


        /// <summary>
        /// 反序列化为原始对象。
        /// </summary>
        /// <param name="target">给定的字符串。</param>
        /// <returns>返回字符编码。</returns>
        public object Deserialize(string target)
            => target.FromName();

        /// <summary>
        /// 序列化为字符串。
        /// </summary>
        /// <param name="source">给定的字符编码。</param>
        /// <returns>返回字符串。</returns>
        public string Serialize(object source)
            => source is Encoding encoding ? encoding.AsName() : string.Empty;
    }
}
