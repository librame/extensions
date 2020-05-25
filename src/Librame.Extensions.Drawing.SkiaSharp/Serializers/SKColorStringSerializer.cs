#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
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
        /// 构造一个 <see cref="SKColorStringSerializer"/>。
        /// </summary>
        public SKColorStringSerializer()
            : base(f => f.ToString(), r => SKColor.Parse(r))
        {
        }

    }
}
