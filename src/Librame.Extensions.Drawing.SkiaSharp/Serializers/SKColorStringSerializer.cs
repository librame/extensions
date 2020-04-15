#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using SkiaSharp;

namespace Librame.Extensions.Drawing.Serializers
{
    using Core.Serializers;

    /// <summary>
    /// <see cref="SKColor"/> 字符串序列化器。
    /// </summary>
    public class SKColorStringSerializer : AbstractStringSerializer<SKColor>
    {
        /// <summary>
        /// 反序列化字符串为字符编码。
        /// </summary>
        /// <param name="target">给定的字符串。</param>
        /// <returns>返回类型。</returns>
        public override SKColor Deserialize(string target)
            => SKColor.Parse(target);

        /// <summary>
        /// 序列化字符编码为字符串。
        /// </summary>
        /// <param name="source">给定的类型。</param>
        /// <returns>返回字符串。</returns>
        public override string Serialize(SKColor source)
            => source.ToString();
    }
}
