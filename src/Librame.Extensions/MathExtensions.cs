﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions
{
    /// <summary>
    /// 数学静态扩展。
    /// </summary>
    public static class MathExtensions
    {
        /// <summary>
        /// 计算最大公约数。
        /// </summary>
        /// <param name="a">给定的一个数。</param>
        /// <param name="b">给定的另一个数。</param>
        /// <returns>返回整数。</returns>
        public static int ComputeGCD(this int a, int b)
        {
            if (a == b) return a;

            if (a < b)
            {
                a += b;
                b = a - b;
                a -= b;
            }

            return (a % b == 0) ? b : ComputeGCD(a % b, b);
        }

        /// <summary>
        /// 计算最小公倍数。
        /// </summary>
        /// <param name="a">给定的一个数。</param>
        /// <param name="b">给定的另一个数。</param>
        /// <returns>返回整数。</returns>
        public static int ComputeLCM(this int a, int b)
            => a * b / ComputeGCD(a, b);
    }
}
