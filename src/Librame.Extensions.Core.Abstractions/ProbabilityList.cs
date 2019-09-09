#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 概率列表。
    /// </summary>
    public class ProbabilityList : IEnumerable<WeightRange>
    {
        private readonly IList<WeightRange> _weightRanges;


        /// <summary>
        /// 构造一个 <see cref="ProbabilityList"/>。
        /// </summary>
        /// <param name="weights">给定的权重集合。</param>
        /// <param name="startWeight">给定的起始权重（可选；默认以 0.0 开始）。</param>
        public ProbabilityList(IEnumerable<double> weights, double startWeight = 0d)
        {
            weights.NotNullOrEmpty(nameof(weights));

            _weightRanges = new List<WeightRange>();

            var minWeight = startWeight;
            weights.ForEach(weight =>
            {
                minWeight = MaxWeight;
                MaxWeight = MaxWeight + weight;

                _weightRanges.Add(new WeightRange(MaxWeight, minWeight));
            });
        }

        /// <summary>
        /// 构造一个 <see cref="ProbabilityList"/>。
        /// </summary>
        /// <param name="weightRanges">给定的权重范围列表。</param>
        public ProbabilityList(IList<WeightRange> weightRanges)
        {
            _weightRanges = weightRanges.NotNull(nameof(weightRanges));
            MaxWeight = _weightRanges.Select(range => range.Max).Max();
        }


        /// <summary>
        /// 最大权重。
        /// </summary>
        public double MaxWeight { get; private set; }
            = 0d;

        /// <summary>
        /// 权重范围列表。
        /// </summary>
        public IReadOnlyList<WeightRange> WeightRanges
            => _weightRanges.AsReadOnlyList();


        /// <summary>
        /// 得到随机索引。
        /// </summary>
        /// <returns>返回整数。</returns>
        public int GetRandomIndex()
        {
            var index = -1;
            var r = new Random();

            // 生成 0-1 间的随机数
            var weight = r.NextDouble() * MaxWeight;
            if (weight == 0d)
            {
                // 防止生成 0.0
                weight = r.NextDouble() * MaxWeight;
            }

            for (var i = 0; i < _weightRanges.Count; i++)
            {
                var range = _weightRanges[i];
                if (range.IsRange(weight))
                {
                    index = i;
                    break;
                }
            }

            return index;
        }


        /// <summary>
        /// 获取枚举器。
        /// </summary>
        /// <returns>返回 <see cref="IEnumerator{WeightRange}"/>。</returns>
        public IEnumerator<WeightRange> GetEnumerator()
            => _weightRanges.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

    }
}
