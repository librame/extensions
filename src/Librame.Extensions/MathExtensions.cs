#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
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
            if (a == b) return a; // BUG: a,b 相同时，if(a < b) 为 false 仍会执行代码段

            if (a < b)
                a = a + b; b = a - b; a = a - b;

            return (a % b == 0) ? b : ComputeGCD(a % b, b);
        }

        /// <summary>
        /// 计算最小公倍数。
        /// </summary>
        /// <param name="a">给定的一个数。</param>
        /// <param name="b">给定的另一个数。</param>
        /// <returns>返回整数。</returns>
        public static int ComputeLCM(this int a, int b)
        {
            return a * b / ComputeGCD(a, b);
        }

    }
}
