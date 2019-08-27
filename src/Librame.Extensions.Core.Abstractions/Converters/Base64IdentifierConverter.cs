﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// BASE64 标识符转换器。
    /// </summary>
    public class Base64IdentifierConverter : IIdentifierConverter<byte>
    {
        /// <summary>
        /// 转换为字节只读存储器。
        /// </summary>
        /// <param name="output">给定的 BASE64 字符串。</param>
        /// <returns>返回 <see cref="ReadOnlyMemory{Byte}"/>。</returns>
        public ReadOnlyMemory<byte> From(string output)
        {
            return output.FromBase64String();
        }

        /// <summary>
        /// 转换为 BASE64 字符串。
        /// </summary>
        /// <param name="input">给定的 <see cref="ReadOnlyMemory{Byte}"/>。</param>
        /// <returns>返回字符串。</returns>
        public string To(ReadOnlyMemory<byte> input)
        {
            return input.ToArray().AsBase64String();
        }
    }
}
