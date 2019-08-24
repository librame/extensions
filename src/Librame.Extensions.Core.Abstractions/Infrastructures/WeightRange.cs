#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 权重范围。
    /// </summary>
    public class WeightRange
    {
        /// <summary>
        /// 构造一个 <see cref="WeightRange"/>。
        /// </summary>
        /// <param name="max">给定的最大权重。</param>
        /// <param name="min">给定的最小权重。</param>
        public WeightRange(double max, double min = 0d)
        {
            max.NotLesser(min, nameof(max));

            Min = min;
            Max = max;
        }


        /// <summary>
        /// 最小权重。
        /// </summary>
        public double Min { get; }

        /// <summary>
        /// 最大权重。
        /// </summary>
        public double Max { get; }


        /// <summary>
        /// 权重属于当前权重范围。
        /// </summary>
        /// <param name="weight">指定的权重。</param>
        /// <returns>返回布尔值。</returns>
        public bool IsRange(double weight)
        {
            return weight > Min && weight <= Max;
        }

    }
}
