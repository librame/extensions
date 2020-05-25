#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Net;

namespace Librame.Extensions.Core.Combiners
{
    using Resources;

    /// <summary>
    /// 域名组合器。
    /// </summary>
    public class DomainNameCombiner : AbstractCombiner<string>
    {
        /// <summary>
        /// 分隔符。
        /// </summary>
        public const char Separator = '.';

        /// <summary>
        /// 端口界定符。
        /// </summary>
        public const char PortDelimiter = ':';


        /// <summary>
        /// 从根域名开始的顺序集合。
        /// </summary>
        private List<string> _allLevelSegments = null;


        /// <summary>
        /// 构造一个 <see cref="DomainNameCombiner"/>。
        /// </summary>
        /// <param name="host">给定的主机名（支持域名加端口形式）。</param>
        public DomainNameCombiner(string host)
            : base(host)
        {
            _allLevelSegments = new List<string>(GetAllLevelSegments(host, out var port));
            Port = port;
        }

        /// <summary>
        /// 构造一个 <see cref="DomainNameCombiner"/>。
        /// </summary>
        /// <param name="allLevelSegments">给定的所有级别片段列表。</param>
        /// <param name="port">给定的端口（可选）。</param>
        public DomainNameCombiner(List<string> allLevelSegments, ushort? port = null)
            : base(CombineParameters(allLevelSegments, port))
        {
            _allLevelSegments = allLevelSegments;
            Port = port;
        }


        /// <summary>
        /// 根域名（如：com/org...）。
        /// </summary>
        public string Root
        {
            get => _allLevelSegments.First();
            private set => AddOrUpdateAllLevelSegments(value);
        }

        /// <summary>
        /// 顶级/一级片段（如：top.com/top.org... 中的 top）。
        /// </summary>
        public string TopLevelSegment
        {
            get => _allLevelSegments.Count > 1 ? _allLevelSegments[1] : null;
            private set => AddOrUpdateAllLevelSegments(value, currentLevelIndex: 1);
        }

        /// <summary>
        /// 二级片段（如：second.top.com/second.top.org... 中的 second）。
        /// </summary>
        public string SecondLevelSegment
        {
            get => _allLevelSegments.Count > 2 ? _allLevelSegments[2] : null;
            private set => AddOrUpdateAllLevelSegments(value, currentLevelIndex: 2);
        }

        /// <summary>
        /// 三级片段（如：third.second.top.com/third.second.top.org... 中的 third）。
        /// </summary>
        public string ThirdLevelSegment
        {
            get => _allLevelSegments.Count > 3 ? _allLevelSegments[3] : null;
            private set => AddOrUpdateAllLevelSegments(value, currentLevelIndex: 3);
        }

        /// <summary>
        /// 其他级别片段集合（除三级外所有子级）。
        /// </summary>
        public IReadOnlyList<string> OtherLevelSegments
        {
            get => _allLevelSegments.Count > 4 ? _allLevelSegments.Skip(4).Reverse().AsReadOnlyList() : null;
            private set => value.ForEach((str, i) => _allLevelSegments[i + 4] = str);
        }


        /// <summary>
        /// 顶级/一级域名（如：top.com/top.org...）。
        /// </summary>
        public string TopLevel
            => TopLevelSegment.IsNotEmpty() ? $"{TopLevelSegment}{Separator}{Root}" : Root;

        /// <summary>
        /// 二级域名（如：second.top.com/second.top.org...）。
        /// </summary>
        public string SecondLevel
            => SecondLevelSegment.IsNotEmpty() ? $"{SecondLevelSegment}{Separator}{TopLevel}" : TopLevel;

        /// <summary>
        /// 三级域名（如：third.second.top.com/third.second.top.org...）。
        /// </summary>
        public string ThirdLevel
            => ThirdLevelSegment.IsNotEmpty() ? $"{ThirdLevelSegment}{Separator}{SecondLevel}" : SecondLevel;


        /// <summary>
        /// 端口。
        /// </summary>
        public ushort? Port { get; private set; }


        /// <summary>
        /// 重写源实例。
        /// </summary>
        public override string Source
            => CombineParameters(_allLevelSegments, Port);


        /// <summary>
        /// 添加或更新所有级别片段集合。
        /// </summary>
        /// <param name="value">给定的值。</param>
        /// <param name="currentLevelIndex">指定当前级别索引（可选；默认为 0）。</param>
        protected void AddOrUpdateAllLevelSegments(string value, int currentLevelIndex = 0)
        {
            switch (currentLevelIndex)
            {
                case 0:
                    _allLevelSegments[currentLevelIndex] = value.NotEmpty(nameof(value));
                    break;

                case 1:
                    {
                        value.NotEmpty(nameof(value));

                        if (_allLevelSegments.Count > currentLevelIndex)
                            _allLevelSegments[currentLevelIndex] = value;
                        else
                            _allLevelSegments.Add(value);
                    }
                    break;

                default:
                    {
                        if (_allLevelSegments.Count > currentLevelIndex)
                        {
                            if (value.IsEmpty())
                                _allLevelSegments.RemoveRange(currentLevelIndex, _allLevelSegments.Count - currentLevelIndex);
                            else
                                _allLevelSegments[currentLevelIndex] = value;
                        }
                        else
                        {
                            _allLevelSegments.Add(value);
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// 更新部分或所有级别片段集合。
        /// </summary>
        /// <param name="newLevelSegments">给定用于更新的级别片段集合。</param>
        protected void UpdateLevelSegments(IEnumerable<string> newLevelSegments)
            => newLevelSegments.ForEach((str, i) => _allLevelSegments[i] = str);


        /// <summary>
        /// 复制一个当前实例。
        /// </summary>
        /// <param name="port">给定的端口（可选；默认使用当前端口）。</param>
        /// <returns>返回 <see cref="DomainNameCombiner"/>。</returns>
        public DomainNameCombiner Copy(ushort? port = null)
            => new DomainNameCombiner(new List<string>(_allLevelSegments), port ?? Port);


        #region Change

        /// <summary>
        /// 改变域名。
        /// </summary>
        /// <param name="newDomainName">给定的新域名（支持包括根、一级、二级、三级、多级等在内的所有域名形式）。</param>
        /// <returns>返回 <see cref="DomainNameCombiner"/>。</returns>
        public DomainNameCombiner ChangeDomainName(string newDomainName)
        {
            UpdateLevelSegments(GetAllLevelSegments(newDomainName, out var port));
            Port = port;

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

        /// <summary>
        /// 改变端口。
        /// </summary>
        /// <param name="port">给定的端口（可选；如果为空表示清空端口）。</param>
        /// <returns>返回 <see cref="DomainNameCombiner"/>。</returns>
        public DomainNameCombiner ChangePort(ushort? port = null)
        {
            Port = port; // 允许清空当前端口
            return this;
        }

        #endregion


        #region With

        /// <summary>
        /// 带有域名。
        /// </summary>
        /// <param name="newDomainName">给定的新域名（支持包括根、一级、二级、三级、多级等在内的所有域名）。</param>
        /// <returns>返回 <see cref="DomainNameCombiner"/>。</returns>
        public DomainNameCombiner WithDomainName(string newDomainName)
        {
            var newLevelSegments = GetAllLevelSegments(newDomainName, out var port);

            var newCombiner = Copy(port);
            newCombiner.UpdateLevelSegments(newLevelSegments);
            
            return newCombiner;
        }

        /// <summary>
        /// 带有根。
        /// </summary>
        /// <param name="newRoot">给定的根（如：com/org...）。</param>
        /// <returns>返回 <see cref="DomainNameCombiner"/>。</returns>
        public DomainNameCombiner WithRoot(string newRoot)
        {
            newRoot.NotEmpty(nameof(newRoot));

            var newCombiner = Copy();
            newCombiner.Root = newRoot;

            return newCombiner;
        }

        /// <summary>
        /// 带有顶级/一级片段。
        /// </summary>
        /// <param name="newTopLevelSegment">给定的新顶级/一级片段（如：top.com/top.org... 中的 top）。</param>
        /// <returns>返回 <see cref="DomainNameCombiner"/>。</returns>
        public DomainNameCombiner WithTopLevelSegment(string newTopLevelSegment)
        {
            newTopLevelSegment.NotEmpty(nameof(newTopLevelSegment));

            var newCombiner = Copy();
            newCombiner.TopLevelSegment = newTopLevelSegment;

            return newCombiner;
        }

        /// <summary>
        /// 带有二级片段。
        /// </summary>
        /// <param name="newSecondLevelSegment">给定的二级片段（如：second.top.com/second.top.org... 中的 second）。</param>
        /// <returns>返回 <see cref="DomainNameCombiner"/>。</returns>
        public DomainNameCombiner WithSecondLevelSegment(string newSecondLevelSegment)
        {
            newSecondLevelSegment.NotEmpty(nameof(newSecondLevelSegment));

            var newCombiner = Copy();
            newCombiner.SecondLevelSegment = newSecondLevelSegment;

            return newCombiner;
        }

        /// <summary>
        /// 带有三级片段。
        /// </summary>
        /// <param name="newThirdLevelSegment">给定的三级片段（如：third.second.top.com/third.second.top.org... 中的 third）。</param>
        /// <returns>返回 <see cref="DomainNameCombiner"/>。</returns>
        public DomainNameCombiner WithThirdLevelSegment(string newThirdLevelSegment)
        {
            newThirdLevelSegment.NotEmpty(nameof(newThirdLevelSegment));

            var newCombiner = Copy();
            newCombiner.ThirdLevelSegment = newThirdLevelSegment;

            return newCombiner;
        }

        /// <summary>
        /// 带有其他级别片段集合（除三级外所有子级）。
        /// </summary>
        /// <param name="newOtherLevelSegments">给定的新其他级别片段集合（除三级外所有子级）。</param>
        /// <returns>返回 <see cref="DomainNameCombiner"/>。</returns>
        public DomainNameCombiner WithOtherLevelSegments(params string[] newOtherLevelSegments)
        {
            newOtherLevelSegments.NotEmpty(nameof(newOtherLevelSegments));

            var newCombiner = Copy();
            newCombiner.OtherLevelSegments = newOtherLevelSegments;

            return newCombiner;
        }

        /// <summary>
        /// 带有端口。
        /// </summary>
        /// <param name="port">给定的端口（可空）。</param>
        /// <returns>返回 <see cref="DomainNameCombiner"/>。</returns>
        public DomainNameCombiner WithPort(ushort? port)
            => Copy(port);

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
            => obj is DomainNameCombiner other && Equals(other);


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回 32 位整数。</returns>
        public override int GetHashCode()
            => Source.CompatibleGetHashCode();


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


        private static string CombineParameters(IEnumerable<string> segments, ushort? port = null)
        {
            var domain = segments?.Reverse().CompatibleJoinString(Separator);

            if (!port.HasValue)
                return domain;

            return $"{domain}{PortDelimiter}{port}";
        }

        private static IEnumerable<string> GetAllLevelSegments(string host, out ushort? port)
        {
            if (!TryParseParameters(host, out port,
                out IEnumerable<string> allLevelSegments)
                && allLevelSegments.IsEmpty())
            {
                // 不支持除本机环回地址外的 IP 地址
                throw new NotSupportedException(InternalResource.NotSupportedExceptionDomainName);
            }

            return allLevelSegments;
        }

        internal static bool TryParseParameters(string host, out ushort? port,
            out IEnumerable<string> allLevelSegments)
        {
            port = null;

            if (host.IsEmpty())
            {
                allLevelSegments = null;
                return false;
            }

            if (host.CompatibleContains(PortDelimiter))
            {
                var pair = host.SplitPair(PortDelimiter);
                host = pair.Key;
                port = ushort.Parse(pair.Value, CultureInfo.InvariantCulture);
            }

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
                    allLevelSegments = null;
                    return false;
                }
            }

            // 反转为顺序
            allLevelSegments = host.CompatibleSplit(Separator).Reverse();
            return true;
        }


        /// <summary>
        /// 尝试解析组合器。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="host"/> is null or empty.
        /// </exception>
        /// <param name="host">给定的主机。</param>
        /// <param name="result">输出 <see cref="TypeNameCombiner"/>。</param>
        /// <returns>返回布尔值。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        public static bool TryParseCombiner(string host, out DomainNameCombiner result)
        {
            if (TryParseParameters(host, out var port, out var allLevelSegments))
            {
                result = new DomainNameCombiner(allLevelSegments?.ToList(), port);
                return true;
            }

            result = null;
            return false;
        }

    }
}
