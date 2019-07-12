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
    /// 随机数生成器标识符。
    /// </summary>
    public class RngIdentifier : AbstractIdentifier
    {
        /// <summary>
        /// 构造一个 <see cref="RngIdentifier"/> 实例。
        /// </summary>
        /// <param name="generator">给定的 <see cref="RandomNumberGenerator"/> 。</param>
        /// <param name="length">给定要生成的字节数组长度（可选；默认 32 位）。</param>
        public RngIdentifier(RandomNumberGenerator generator, int length = 32)
            : this(GenerateBuffer(generator, length))
        {
        }

        private RngIdentifier(byte[] buffer)
            : base(buffer)
        {
        }


        private static byte[] GenerateBuffer(RandomNumberGenerator generator, int length = 32)
        {
            generator.NotNull(nameof(generator));

            var buffer = new byte[length];
            generator.GetBytes(buffer);

            return buffer;
        }


        /// <summary>
        /// 显式转换为 <see cref="RngIdentifier"/>。
        /// </summary>
        /// <param name="identifier">给定的随机数生成器标识符。</param>
        public static explicit operator RngIdentifier(string identifier)
            => new RngIdentifier(identifier.FromBase64String());


        /// <summary>
        /// 新建实例。
        /// </summary>
        /// <param name="length">给定要生成的字节数组长度（可选；默认 32 位）。</param>
        /// <returns>返回 <see cref="GuIdentifier"/>。</returns>
        public static RngIdentifier New(int length = 32)
            => new RngIdentifier(RandomNumberGenerator.Create(), length);
    }
}
