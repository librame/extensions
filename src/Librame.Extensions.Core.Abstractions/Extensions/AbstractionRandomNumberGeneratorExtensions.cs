﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace System.Security.Cryptography
{
    /// <summary>
    /// 抽象随机数生成器静态扩展。
    /// </summary>
    public static class AbstractionRandomNumberGeneratorExtensions
    {
        /// <summary>
        /// 生成字节数组。
        /// </summary>
        /// <param name="generator">给定的 <see cref="RandomNumberGenerator"/>。</param>
        /// <param name="length">给定要生成的字节数组长度。</param>
        /// <returns>返回字节数组。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static byte[] GenerateByteArray(this RandomNumberGenerator generator, int length)
        {
            generator.NotNull(nameof(generator));

            var buffer = new byte[length];
            generator.GetBytes(buffer);

            return buffer;
        }

    }
}
