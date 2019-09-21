#region License

/* **************************************************************************************
 * Copyright (c) zwbwl All rights reserved.
 * 
 * http://51zwb.com
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections.Generic;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 抽象域名组合器静态扩展。
    /// </summary>
    public static class AbstractionDomainNameCombinerExtensions
    {
        /// <summary>
        /// 转换为域名组合器。
        /// </summary>
        /// <param name="host">给定的主机。</param>
        /// <returns>返回 <see cref="DomainNameCombiner"/>。</returns>
        public static DomainNameCombiner AsDomainNameCombiner(this string host)
        {
            return (DomainNameCombiner)host;
        }

        /// <summary>
        /// 转换为域名组合器。
        /// </summary>
        /// <param name="allLevelSegments">给定的所有级别片段列表。</param>
        /// <returns>返回 <see cref="DomainNameCombiner"/>。</returns>
        public static DomainNameCombiner AsDomainNameCombiner(this List<string> allLevelSegments)
        {
            return new DomainNameCombiner(allLevelSegments);
        }

        /// <summary>
        /// 获取仅两级域名形式。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="DomainNameCombiner"/>。</param>
        /// <returns>返回包含子级与父级的两级元组。</returns>
        public static (string Child, string Parent) GetOnlyTwoLevels(this DomainNameCombiner combiner)
        {
            combiner.NotNull(nameof(combiner));

            if (combiner.TopLevelSegment.IsNullOrEmpty())
                return (null, combiner.Root);

            if (combiner.SecondLevelSegment.IsNullOrEmpty())
                return (null, combiner.TopLevel);

            var child = combiner.Source.TrimEnd($".{combiner.TopLevel}");
            return (child, combiner.TopLevel);
        }

    }
}
