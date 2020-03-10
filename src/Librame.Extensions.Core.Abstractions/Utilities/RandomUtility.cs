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

namespace Librame.Extensions.Core.Utilities
{
    /// <summary>
    /// 随机实用工具。
    /// </summary>
    public static class RandomUtility
    {
        private static readonly RandomNumberGenerator _generator
            = RandomNumberGenerator.Create();


        /// <summary>
        /// 生成随机数。
        /// </summary>
        /// <param name="length">给定的字节数组元素长度。</param>
        /// <returns>返回生成的字节数组。</returns>
        public static byte[] GenerateNumber(int length)
        {
            var buffer = new byte[length];
            _generator.GetBytes(buffer);

            return buffer;
        }

    }
}
