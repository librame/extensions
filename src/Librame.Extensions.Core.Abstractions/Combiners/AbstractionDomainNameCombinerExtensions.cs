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
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Core.Combiners
{
    /// <summary>
    /// <see cref="DomainNameCombiner"/> 静态扩展。
    /// </summary>
    public static class AbstractionDomainNameCombinerExtensions
    {
        /// <summary>
        /// 转换为域名组合器。
        /// </summary>
        /// <param name="host">给定的主机。</param>
        /// <returns>返回 <see cref="DomainNameCombiner"/>。</returns>
        public static DomainNameCombiner AsDomainNameCombiner(this string host)
            => new DomainNameCombiner(host);

        /// <summary>
        /// 转换为域名组合器。
        /// </summary>
        /// <param name="allLevelSegments">给定的所有级别片段列表。</param>
        /// <param name="port">给定的端口（可选）。</param>
        /// <returns>返回 <see cref="DomainNameCombiner"/>。</returns>
        public static DomainNameCombiner AsDomainNameCombiner(this List<string> allLevelSegments,
            ushort? port = null)
            => new DomainNameCombiner(allLevelSegments, port);

        /// <summary>
        /// 获取仅两级域名形式（如：null/com, null/top.com, www/top.com, n.www/top.com）。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="DomainNameCombiner"/>。</param>
        /// <returns>返回包含子级与父级的两级元组。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static (string Child, string Parent) GetOnlyTwoLevels(this DomainNameCombiner combiner)
        {
            combiner.NotNull(nameof(combiner));

            if (combiner.TopLevelSegment.IsEmpty())
                return (null, combiner.Root);

            if (combiner.SecondLevelSegment.IsEmpty())
                return (null, combiner.TopLevel);

            var child = combiner.Source.TrimEnd($".{combiner.TopLevel}");
            return (child, combiner.TopLevel);
        }

    }
}
