#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Encryption.Generators
{
    using Core.Utilities;

    /// <summary>
    /// 生成器助手。
    /// </summary>
    public static class GeneratorHelper
    {
        /// <summary>
        /// 生成指定长度的字节数组。
        /// </summary>
        /// <param name="initialBytes">给定的初始化字节数组。</param>
        /// <param name="length">给定要生成的长度。</param>
        /// <param name="isRandom">是随机生成。</param>
        /// <returns>返回字节数组。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        public static byte[] GenerateBytes(byte[] initialBytes, int length, bool isRandom)
        {
            initialBytes.NotEmpty(nameof(initialBytes));

            var resultKey = new byte[length];

            // 计算最大公约数
            var gcd = initialBytes.Length.ComputeGCD(length);

            if (isRandom)
            {
                // 得到最大索引长度
                var maxIndexLength = (gcd <= initialBytes.Length) ? initialBytes.Length : gcd;

                RandomUtility.Run(r =>
                {
                    for (var i = 0; i < length; i++)
                    {
                        resultKey[i] = initialBytes[r.Next(maxIndexLength)];
                    }
                });
            }
            else
            {
                for (var i = 0; i < length; i++)
                {
                    if (i >= initialBytes.Length)
                    {
                        var multiples = (i + 1) / initialBytes.Length;
                        resultKey[i] = initialBytes[i + 1 - multiples * initialBytes.Length];
                    }
                    else
                    {
                        resultKey[i] = initialBytes[i];
                    }
                }
            }

            return resultKey;
        }

    }
}
