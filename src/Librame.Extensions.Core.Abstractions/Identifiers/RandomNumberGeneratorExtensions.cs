#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Security.Cryptography;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// <see cref="RandomNumberGenerator"/> 静态扩展。
    /// </summary>
    public static class RandomNumberGeneratorExtensions
    {
        /// <summary>
        /// 生成字节数组。
        /// </summary>
        /// <param name="generator">给定的 <see cref="RandomNumberGenerator"/>。</param>
        /// <param name="length">给定要生成的字节数组长度。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] GenerateByteArray(this RandomNumberGenerator generator, int length)
        {
            generator.NotNull(nameof(generator));

            var buffer = new byte[length];
            generator.GetBytes(buffer);

            return buffer;
        }

    }
}
