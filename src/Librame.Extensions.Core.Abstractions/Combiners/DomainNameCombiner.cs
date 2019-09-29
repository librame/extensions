#region License

/* **************************************************************************************
 * Copyright (c) zwbwl All rights reserved.
 * 
 * http://51zwb.com
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 域名组合器。
    /// </summary>
    public class DomainNameCombiner : AbstractCombiner<string>
    {
        private List<string> _allLevelSegments = null;


        /// <summary>
        /// 构造一个 <see cref="DomainNameCombiner"/>。
        /// </summary>
        /// <param name="domainName">给定的域名。</param>
        public DomainNameCombiner(string domainName)
            : base(domainName)
        {
            _allLevelSegments = new List<string>(ParseAllLevelSegments(domainName));
        }

        /// <summary>
        /// 构造一个 <see cref="DomainNameCombiner"/>。
        /// </summary>
        /// <param name="allLevelSegments">给定的所有级别片段列表。</param>
        public DomainNameCombiner(List<string> allLevelSegments)
            : base(CombineString(allLevelSegments))
        {
            _allLevelSegments = allLevelSegments;
        }


        /// <summary>
        /// 根域名（如：com/org...）。
        /// </summary>
        public string Root
        {
            get => _allLevelSegments.First();
            private set => _allLevelSegments[0] = value;
        }

        /// <summary>
        /// 顶级/一级片段（如：top.com/top.org... 中的 top）。
        /// </summary>
        public string TopLevelSegment
        {
            get => _allLevelSegments.Count > 1 ? _allLevelSegments[1] : null;
            private set
            {
                if (_allLevelSegments.Count > 1)
                    _allLevelSegments[1] = value;
                else
                    _allLevelSegments.Add(value);
            }
        }

        /// <summary>
        /// 二级片段（如：second.top.com/second.top.org... 中的 second）。
        /// </summary>
        public string SecondLevelSegment
        {
            get => _allLevelSegments.Count > 2 ? _allLevelSegments[2] : null;
            private set
            {
                if (_allLevelSegments.Count > 2)
                    _allLevelSegments[2] = value;
                else if (_allLevelSegments.Count < 2)
                    throw new ArgumentException("Can't set second level name for root.");
                else
                    _allLevelSegments.Add(value);
            }
        }

        /// <summary>
        /// 三级片段（如：third.second.top.com/third.second.top.org... 中的 third）。
        /// </summary>
        public string ThirdLevelSegment
        {
            get => _allLevelSegments.Count > 3 ? _allLevelSegments[3] : null;
            private set
            {
                if (_allLevelSegments.Count > 3)
                    _allLevelSegments[3] = value;
                else if (_allLevelSegments.Count < 3)
                    throw new ArgumentException("Can't set third level name for top level name.");
                else
                    _allLevelSegments.Add(value);
            }
        }

        /// <summary>
        /// 其他级别片段集合（除三级外所有子级）。
        /// </summary>
        public string[] OtherLevelSegments
        {
            get => _allLevelSegments.Count > 4 ? _allLevelSegments.Skip(4).Reverse().ToArray() : null;
            private set => value.ForEach((str, i) => _allLevelSegments[i + 4] = str);
        }


        /// <summary>
        /// 顶级/一级域名（如：top.com/top.org...）。
        /// </summary>
        public string TopLevel
            => TopLevelSegment.IsNotEmpty() ? $"{TopLevelSegment}.{Root}" : Root;

        /// <summary>
        /// 二级域名（如：second.top.com/second.top.org...）。
        /// </summary>
        public string SecondLevel
            => SecondLevelSegment.IsNotEmpty() ? $"{SecondLevelSegment}.{TopLevel}" : TopLevel;

        /// <summary>
        /// 三级域名（如：third.second.top.com/third.second.top.org...）。
        /// </summary>
        public string ThirdLevel
            => ThirdLevelSegment.IsNotEmpty() ? $"{ThirdLevelSegment}.{SecondLevel}" : SecondLevel;


        /// <summary>
        /// 重写源实例。
        /// </summary>
        public override string Source
            => CombineString(_allLevelSegments);


        /// <summary>
        /// 更新部分或所有级别片段集合。
        /// </summary>
        /// <param name="newLevelSegments">给定用于更新的级别片段集合。</param>
        protected void UpdateLevelSegments(IEnumerable<string> newLevelSegments)
            => newLevelSegments.ForEach((str, i) => _allLevelSegments[i] = str);


        #region Changes

        /// <summary>
        /// 改变域名。
        /// </summary>
        /// <param name="newDomainName">给定的新域名（支持包括根、一级、二级、三级、多级等在内的所有域名形式）。</param>
        /// <returns>返回 <see cref="DomainNameCombiner"/>。</returns>
        public DomainNameCombiner ChangeDomainName(string newDomainName)
        {
            UpdateLevelSegments(ParseAllLevelSegments(newDomainName));
            return this;
        }

        /// <summary>
        /// 改变根。
        /// </summary>
        /// <param name="newRoot">给定的根（如：com/org...）。</param>
        /// <returns>返回 <see cref="DomainNameCombiner"/>。</returns>
        public DomainNameCombiner ChangeRoot(string newRoot)
        {
            Root = newRoot.NotEmpty(nameof(newRoot));
            return this;
        }

        /// <summary>
        /// 改变顶级/一级片段。
        /// </summary>
        /// <param name="newTopLevelSegment">给定的新顶级/一级片段（如：top.com/top.org... 中的 top）。</param>
        /// <returns>返回 <see cref="DomainNameCombiner"/>。</returns>
        public DomainNameCombiner ChangeTopLevelSegment(string newTopLevelSegment)
        {
            TopLevelSegment = newTopLevelSegment.NotEmpty(nameof(newTopLevelSegment));
            return this;
        }

        /// <summary>
        /// 改变二级片段。
        /// </summary>
        /// <param name="newSecondLevelSegment">给定的二级片段（如：second.top.com/second.top.org... 中的 second）。</param>
        /// <returns>返回 <see cref="DomainNameCombiner"/>。</returns>
        public DomainNameCombiner ChangeSecondLevelSegment(string newSecondLevelSegment)
        {
            SecondLevelSegment = newSecondLevelSegment.NotEmpty(nameof(newSecondLevelSegment));
            return this;
        }

        /// <summary>
        /// 改变三级片段。
        /// </summary>
        /// <param name="newThirdLevelSegment">给定的三级片段（如：third.second.top.com/third.second.top.org... 中的 third）。</param>
        /// <returns>返回 <see cref="DomainNameCombiner"/>。</returns>
        public DomainNameCombiner ChangeThirdLevelSegment(string newThirdLevelSegment)
        {
            ThirdLevelSegment = newThirdLevelSegment.NotEmpty(nameof(newThirdLevelSegment));
            return this;
        }

        /// <summary>
        /// 改变其他级别片段集合（除三级外所有子级）。
        /// </summary>
        /// <param name="newOtherLevelSegments">给定的新其他级别片段集合（除三级外所有子级）。</param>
        /// <returns>返回 <see cref="DomainNameCombiner"/>。</returns>
        public DomainNameCombiner ChangeOtherLevelSegments(params string[] newOtherLevelSegments)
        {
            OtherLevelSegments = newOtherLevelSegments.NotEmpty(nameof(newOtherLevelSegments));
            return this;
        }

        #endregion


        #region News

        /// <summary>
        /// 通过指定的新域名与当前已有其他级别域名集合组合的方式新建域名。
        /// </summary>
        /// <param name="newDomainName">给定的新域名（支持包括根、一级、二级、三级、多级等在内的所有域名）。</param>
        /// <returns>返回 <see cref="DomainNameCombiner"/>。</returns>
        public DomainNameCombiner NewDomainName(string newDomainName)
        {
            var newLevelSegments = ParseAllLevelSegments(newDomainName);

            var copyCombiner = new DomainNameCombiner(new List<string>(_allLevelSegments));
            copyCombiner.UpdateLevelSegments(newLevelSegments);

            return copyCombiner;
        }

        /// <summary>
        /// 新建根。
        /// </summary>
        /// <param name="newRoot">给定的根（如：com/org...）。</param>
        /// <returns>返回 <see cref="DomainNameCombiner"/>。</returns>
        public DomainNameCombiner NewRoot(string newRoot)
        {
            newRoot.NotEmpty(nameof(newRoot));

            var copyCombiner = new DomainNameCombiner(new List<string>(_allLevelSegments));
            copyCombiner.Root = newRoot;

            return copyCombiner;
        }

        /// <summary>
        /// 新建顶级/一级片段。
        /// </summary>
        /// <param name="newTopLevelSegment">给定的新顶级/一级片段（如：top.com/top.org... 中的 top）。</param>
        /// <returns>返回 <see cref="DomainNameCombiner"/>。</returns>
        public DomainNameCombiner NewTopLevelSegment(string newTopLevelSegment)
        {
            newTopLevelSegment.NotEmpty(nameof(newTopLevelSegment));

            var copyCombiner = new DomainNameCombiner(new List<string>(_allLevelSegments));
            copyCombiner.TopLevelSegment = newTopLevelSegment;

            return copyCombiner;
        }

        /// <summary>
        /// 新建二级片段。
        /// </summary>
        /// <param name="newSecondLevelSegment">给定的二级片段（如：second.top.com/second.top.org... 中的 second）。</param>
        /// <returns>返回 <see cref="DomainNameCombiner"/>。</returns>
        public DomainNameCombiner NewSecondLevelSegment(string newSecondLevelSegment)
        {
            newSecondLevelSegment.NotEmpty(nameof(newSecondLevelSegment));

            var copyCombiner = new DomainNameCombiner(new List<string>(_allLevelSegments));
            copyCombiner.SecondLevelSegment = newSecondLevelSegment;

            return copyCombiner;
        }

        /// <summary>
        /// 新建三级片段。
        /// </summary>
        /// <param name="newThirdLevelSegment">给定的三级片段（如：third.second.top.com/third.second.top.org... 中的 third）。</param>
        /// <returns>返回 <see cref="DomainNameCombiner"/>。</returns>
        public DomainNameCombiner NewThirdLevelSegment(string newThirdLevelSegment)
        {
            newThirdLevelSegment.NotEmpty(nameof(newThirdLevelSegment));

            var copyCombiner = new DomainNameCombiner(new List<string>(_allLevelSegments));
            copyCombiner.ThirdLevelSegment = newThirdLevelSegment;

            return copyCombiner;
        }

        /// <summary>
        /// 新建其他级别片段集合（除三级外所有子级）。
        /// </summary>
        /// <param name="newOtherLevelSegments">给定的新其他级别片段集合（除三级外所有子级）。</param>
        /// <returns>返回 <see cref="DomainNameCombiner"/>。</returns>
        public DomainNameCombiner NewOtherLevelSegments(params string[] newOtherLevelSegments)
        {
            newOtherLevelSegments.NotEmpty(nameof(newOtherLevelSegments));

            var copyCombiner = new DomainNameCombiner(new List<string>(_allLevelSegments));
            copyCombiner.OtherLevelSegments = newOtherLevelSegments;

            return copyCombiner;
        }

        #endregion


        /// <summary>
        /// 是指定的根域名（忽略大小写）。
        /// </summary>
        /// <param name="root">给定的根域名（如：com/org...）。</param>
        /// <returns>返回布尔值。</returns>
        public bool IsRoot(string root)
            => Root.Equals(root, StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// 是指定的顶级/一级域名（忽略大小写）。
        /// </summary>
        /// <param name="topLevel">给定的顶级/一级域名（如：top.com/top.org...）。</param>
        /// <returns>返回布尔值。</returns>
        public bool IsTopLevel(string topLevel)
            => TopLevel.Equals(topLevel, StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// 是指定的二级域名（忽略大小写）。
        /// </summary>
        /// <param name="secondLevel">给定的二级域名（如：second.top.com/second.top.org...）。</param>
        /// <returns>返回布尔值。</returns>
        public bool IsSecondLevel(string secondLevel)
            => SecondLevel.Equals(secondLevel, StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// 是指定的三级域名（忽略大小写）。
        /// </summary>
        /// <param name="thirdLevel">给定的三级域名（如：third.second.top.com/third.second.top.org...）。</param>
        /// <returns>返回布尔值。</returns>
        public bool IsThirdLevel(string thirdLevel)
            => ThirdLevel.Equals(thirdLevel, StringComparison.OrdinalIgnoreCase);


        /// <summary>
        /// 是否相等（忽略大小写）。
        /// </summary>
        /// <param name="other">给定的域名。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(string other)
            => Source.Equals(other, StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// 是否相等（忽略大小写）。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
            => (obj is DomainNameCombiner other) ? Equals(other) : false;


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回 32 位整数。</returns>
        public override int GetHashCode()
            => Source.GetHashCode();


        /// <summary>
        /// 转换为字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
            => Source;


        /// <summary>
        /// 是否相等（忽略大小写）。
        /// </summary>
        /// <param name="a">给定的 <see cref="DomainNameCombiner"/>。</param>
        /// <param name="b">给定的 <see cref="DomainNameCombiner"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator ==(DomainNameCombiner a, DomainNameCombiner b)
            => (a?.Equals(b)).Value;

        /// <summary>
        /// 是否不等（忽略大小写）。
        /// </summary>
        /// <param name="a">给定的 <see cref="DomainNameCombiner"/>。</param>
        /// <param name="b">给定的 <see cref="DomainNameCombiner"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator !=(DomainNameCombiner a, DomainNameCombiner b)
            => !(a?.Equals(b)).Value;


        /// <summary>
        /// 隐式转换为字符串。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="DomainNameCombiner"/>。</param>
        public static implicit operator string(DomainNameCombiner combiner)
            => combiner?.ToString();

        /// <summary>
        /// 隐式转换为域名组合器。
        /// </summary>
        /// <param name="domainName">给定的域名。</param>
        public static implicit operator DomainNameCombiner(string domainName)
            => new DomainNameCombiner(domainName);


        /// <summary>
        /// 组合字符串。
        /// </summary>
        /// <exception cref="ArgumentException">
        /// <paramref name="segments"/> is null or empty.
        /// </exception>
        /// <param name="segments">给定的部分集合。</param>
        /// <returns>返回字符串。</returns>
        public static string CombineString(IEnumerable<string> segments)
            => string.Join(".", segments.NotEmpty(nameof(segments)).Reverse());


        /// <summary>
        /// 解析指定域名包含的所有级别片段正序集合（如：org/domain/...）。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="domainName"/> is null or empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="domainName"/> IP address are not supported except loopback address.
        /// </exception>
        /// <param name="domainName">给定的域名。</param>
        /// <returns>返回 <see cref="IEnumerable{String}"/>。</returns>
        public static IEnumerable<string> ParseAllLevelSegments(string domainName)
        {
            if (!TryParseAllLevelSegmentsFromHost(domainName, out IEnumerable<string> allLevelSegments)
                && allLevelSegments.IsNotNull() && !allLevelSegments.Any())
            {
                // 不支持除本机环回地址外的 IP 地址
                throw new NotSupportedException("IP address are not supported except loopback address.");
            }

            return allLevelSegments;
        }

        /// <summary>
        /// 尝试从主机解析包含的所有级别片段集合。
        /// </summary>
        /// <param name="host">给定的主机名。</param>
        /// <param name="allLevelSegments">输出 <see cref="IEnumerable{String}"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool TryParseAllLevelSegmentsFromHost(string host, out IEnumerable<string> allLevelSegments)
        {
            if (host.IsEmpty())
            {
                allLevelSegments = null;
                return false;
            }

            if (host.Contains(":"))
                host = host.SplitPair(":").Key;

            if (host.IsIPAddress(out IPAddress address))
            {
                if (IPAddress.IsLoopback(address))
                {
                    // 本机环回地址解析为根域名
                    allLevelSegments = "localhost".YieldEnumerable();
                    return true;
                }
                else
                {
                    allLevelSegments = Enumerable.Empty<string>();
                    return false;
                }
            }

            allLevelSegments = host.Split('.').Reverse();
            return true;
        }
    }
}
