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
    /// 抽象域名定位器。
    /// </summary>
    public abstract class AbstractDomainNameLocator : AbstractLocator<string>, IDomainNameLocator
    {
        private readonly List<string> _allLevelSegments = null;


        /// <summary>
        /// 构造一个 <see cref="AbstractDomainNameLocator"/>。
        /// </summary>
        /// <param name="domainName">给定的域名。</param>
        public AbstractDomainNameLocator(string domainName)
            : base(domainName)
        {
            _allLevelSegments = new List<string>(ParseAllLevelSegments(domainName));
        }

        /// <summary>
        /// 构造一个 <see cref="AbstractDomainNameLocator"/>。
        /// </summary>
        /// <param name="allLevelSegments">给定的所有级别片段列表。</param>
        protected AbstractDomainNameLocator(List<string> allLevelSegments)
            : base(string.Join(".", allLevelSegments.NotNullOrEmpty(nameof(allLevelSegments)).Reverse()))
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
            => TopLevelSegment.IsNotNullOrEmpty() ? $"{TopLevelSegment}.{Root}" : Root;

        /// <summary>
        /// 二级域名（如：second.top.com/second.top.org...）。
        /// </summary>
        public string SecondLevel
            => SecondLevelSegment.IsNotNullOrEmpty() ? $"{SecondLevelSegment}.{TopLevel}" : TopLevel;

        /// <summary>
        /// 三级域名（如：third.second.top.com/third.second.top.org...）。
        /// </summary>
        public string ThirdLevel
            => ThirdLevelSegment.IsNotNullOrEmpty() ? $"{ThirdLevelSegment}.{SecondLevel}" : SecondLevel;


        /// <summary>
        /// 重写源实例。
        /// </summary>
        public override string Source
            => string.Join(".", _allLevelSegments.Reverse<string>());


        /// <summary>
        /// 创建定位器。
        /// </summary>
        /// <param name="copyAllLevelSegments">给定的所有级别片段列表副本。</param>
        /// <returns>返回 <see cref="IDomainNameLocator"/>。</returns>
        protected abstract AbstractDomainNameLocator CreateLocator(List<string> copyAllLevelSegments);


        /// <summary>
        /// 更新部分或所有级别片段集合。
        /// </summary>
        /// <param name="newLevelSegments">给定用于更新的级别片段集合。</param>
        protected void UpdateLevelSegments(IEnumerable<string> newLevelSegments)
        {
            newLevelSegments.ForEach((str, i) => _allLevelSegments[i] = str);
        }


        #region Changes

        /// <summary>
        /// 改变域名。
        /// </summary>
        /// <param name="newDomainName">给定的新域名（支持包括根、一级、二级、三级、多级等在内的所有域名形式）。</param>
        /// <returns>返回 <see cref="IDomainNameLocator"/>。</returns>
        public IDomainNameLocator ChangeDomainName(string newDomainName)
        {
            UpdateLevelSegments(ParseAllLevelSegments(newDomainName));
            return this;
        }

        /// <summary>
        /// 改变根。
        /// </summary>
        /// <param name="newRoot">给定的根（如：com/org...）。</param>
        /// <returns>返回 <see cref="IDomainNameLocator"/>。</returns>
        public IDomainNameLocator ChangeRoot(string newRoot)
        {
            Root = newRoot.NotNullOrEmpty(nameof(newRoot));
            return this;
        }

        /// <summary>
        /// 改变顶级/一级片段。
        /// </summary>
        /// <param name="newTopLevelSegment">给定的新顶级/一级片段（如：top.com/top.org... 中的 top）。</param>
        /// <returns>返回 <see cref="IDomainNameLocator"/>。</returns>
        public IDomainNameLocator ChangeTopLevelSegment(string newTopLevelSegment)
        {
            TopLevelSegment = newTopLevelSegment.NotNullOrEmpty(nameof(newTopLevelSegment));
            return this;
        }

        /// <summary>
        /// 改变二级片段。
        /// </summary>
        /// <param name="newSecondLevelSegment">给定的二级片段（如：second.top.com/second.top.org... 中的 second）。</param>
        /// <returns>返回 <see cref="IDomainNameLocator"/>。</returns>
        public IDomainNameLocator ChangeSecondLevelSegment(string newSecondLevelSegment)
        {
            SecondLevelSegment = newSecondLevelSegment.NotNullOrEmpty(nameof(newSecondLevelSegment));
            return this;
        }

        /// <summary>
        /// 改变三级片段。
        /// </summary>
        /// <param name="newThirdLevelSegment">给定的三级片段（如：third.second.top.com/third.second.top.org... 中的 third）。</param>
        /// <returns>返回 <see cref="IDomainNameLocator"/>。</returns>
        public IDomainNameLocator ChangeThirdLevelSegment(string newThirdLevelSegment)
        {
            ThirdLevelSegment = newThirdLevelSegment.NotNullOrEmpty(nameof(newThirdLevelSegment));
            return this;
        }

        /// <summary>
        /// 改变其他级别片段集合（除三级外所有子级）。
        /// </summary>
        /// <param name="newOtherLevelSegments">给定的新其他级别片段集合（除三级外所有子级）。</param>
        /// <returns>返回 <see cref="IDomainNameLocator"/>。</returns>
        public IDomainNameLocator ChangeOtherLevelSegments(params string[] newOtherLevelSegments)
        {
            OtherLevelSegments = newOtherLevelSegments.NotNullOrEmpty(nameof(newOtherLevelSegments));
            return this;
        }

        #endregion


        #region News

        /// <summary>
        /// 通过指定的新域名与当前已有其他级别域名集合组合的方式新建域名。
        /// </summary>
        /// <param name="newDomainName">给定的新域名（支持包括根、一级、二级、三级、多级等在内的所有域名）。</param>
        /// <returns>返回 <see cref="IDomainNameLocator"/>。</returns>
        public IDomainNameLocator NewDomainName(string newDomainName)
        {
            var newLevelSegments = ParseAllLevelSegments(newDomainName);

            var copyLocator = CreateLocator(new List<string>(_allLevelSegments));
            copyLocator.UpdateLevelSegments(newLevelSegments);

            return copyLocator;
        }

        /// <summary>
        /// 新建根。
        /// </summary>
        /// <param name="newRoot">给定的根（如：com/org...）。</param>
        /// <returns>返回 <see cref="IDomainNameLocator"/>。</returns>
        public IDomainNameLocator NewRoot(string newRoot)
        {
            newRoot.NotNullOrEmpty(nameof(newRoot));

            var copyLocator = CreateLocator(new List<string>(_allLevelSegments));
            copyLocator.Root = newRoot;

            return copyLocator;
        }

        /// <summary>
        /// 新建顶级/一级片段。
        /// </summary>
        /// <param name="newTopLevelSegment">给定的新顶级/一级片段（如：top.com/top.org... 中的 top）。</param>
        /// <returns>返回 <see cref="IDomainNameLocator"/>。</returns>
        public IDomainNameLocator NewTopLevelSegment(string newTopLevelSegment)
        {
            newTopLevelSegment.NotNullOrEmpty(nameof(newTopLevelSegment));

            var copyLocator = CreateLocator(new List<string>(_allLevelSegments));
            copyLocator.TopLevelSegment = newTopLevelSegment;

            return copyLocator;
        }

        /// <summary>
        /// 新建二级片段。
        /// </summary>
        /// <param name="newSecondLevelSegment">给定的二级片段（如：second.top.com/second.top.org... 中的 second）。</param>
        /// <returns>返回 <see cref="IDomainNameLocator"/>。</returns>
        public IDomainNameLocator NewSecondLevelSegment(string newSecondLevelSegment)
        {
            newSecondLevelSegment.NotNullOrEmpty(nameof(newSecondLevelSegment));

            var copyLocator = CreateLocator(new List<string>(_allLevelSegments));
            copyLocator.SecondLevelSegment = newSecondLevelSegment;

            return copyLocator;
        }

        /// <summary>
        /// 新建三级片段。
        /// </summary>
        /// <param name="newThirdLevelSegment">给定的三级片段（如：third.second.top.com/third.second.top.org... 中的 third）。</param>
        /// <returns>返回 <see cref="IDomainNameLocator"/>。</returns>
        public IDomainNameLocator NewThirdLevelSegment(string newThirdLevelSegment)
        {
            newThirdLevelSegment.NotNullOrEmpty(nameof(newThirdLevelSegment));

            var copyLocator = CreateLocator(new List<string>(_allLevelSegments));
            copyLocator.ThirdLevelSegment = newThirdLevelSegment;

            return copyLocator;
        }

        /// <summary>
        /// 新建其他级别片段集合（除三级外所有子级）。
        /// </summary>
        /// <param name="newOtherLevelSegments">给定的新其他级别片段集合（除三级外所有子级）。</param>
        /// <returns>返回 <see cref="IDomainNameLocator"/>。</returns>
        public IDomainNameLocator NewOtherLevelSegments(params string[] newOtherLevelSegments)
        {
            newOtherLevelSegments.NotNullOrEmpty(nameof(newOtherLevelSegments));

            var copyLocator = CreateLocator(new List<string>(_allLevelSegments));
            copyLocator.OtherLevelSegments = newOtherLevelSegments;

            return copyLocator;
        }

        #endregion


        /// <summary>
        /// 比较是否相等。
        /// </summary>
        /// <param name="other">给定的 <see cref="IDomainNameLocator"/>。</param>
        /// <returns>返回布尔值。</returns>
        public virtual bool Equals(IDomainNameLocator other)
        {
            return Source == other.Source;
        }


        /// <summary>
        /// 转换为文件名。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
        {
            return Source;
        }


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
            if (host.IsNullOrEmpty())
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
