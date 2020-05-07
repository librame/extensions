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

namespace Librame.Extensions.Core.Combiners
{
    using Resources;

    /// <summary>
    /// 签名令牌组合器。
    /// </summary>
    public class SignedTokenCombiner : AbstractCombiner<string>
    {
        /// <summary>
        /// 分隔符。
        /// </summary>
        public const char Separator = '.';

        /// <summary>
        /// 默认签名工厂方法。
        /// </summary>
        private static readonly Func<string, string> DefaultSignatureFactory
            = s => s.Sha256Base64String();

        private List<string> _dataSegments = null;
        private Func<string, string> _signatureFactory = null;


        /// <summary>
        /// 构造一个 <see cref="SignedTokenCombiner"/>。
        /// </summary>
        /// <param name="token">给定的令牌。</param>
        /// <param name="signatureFactory">给定的签名工厂方法（可选）。</param>
        public SignedTokenCombiner(string token,
            Func<string, string> signatureFactory = null)
            : base(token)
        {
            if (!TryParseParameters(token, out var dataSegments,
                EnsureSignatureFactory(out var result, signatureFactory)))
            {
                throw new ArgumentException($"The token '{token}' is not a valid signed token.");
            }

            _dataSegments = dataSegments.ToList();
            _signatureFactory = signatureFactory;
        }

        /// <summary>
        /// 构造一个 <see cref="SignedTokenCombiner"/>。
        /// </summary>
        /// <param name="dataSegments">给定的数据片段集合。</param>
        /// <param name="signatureFactory">给定的签名工厂方法（可选）。</param>
        public SignedTokenCombiner(IEnumerable<string> dataSegments,
            Func<string, string> signatureFactory = null)
            : base(CombineParameters(dataSegments,
                EnsureSignatureFactory(out var result, signatureFactory)))
        {
            _dataSegments = dataSegments.ToList();
            _signatureFactory = signatureFactory;
        }


        /// <summary>
        /// 数据片段。
        /// </summary>
        public IReadOnlyList<string> DataSegments
            => _dataSegments;

        /// <summary>
        /// 数据片段数。
        /// </summary>
        public int Count
            => _dataSegments.Count;

        /// <summary>
        /// 重写源实例。
        /// </summary>
        public override string Source
            => CombineParameters(_dataSegments, _signatureFactory);


        /// <summary>
        /// 改变数据片段。
        /// </summary>
        /// <param name="oldDataSegment">给定的旧数据片段。</param>
        /// <param name="newDataSegment">给定的新数据片段。</param>
        /// <returns>返回 <see cref="SignedTokenCombiner"/>。</returns>
        public SignedTokenCombiner ChangeDataSegment(string oldDataSegment, string newDataSegment)
        {
            oldDataSegment.NotEmpty(nameof(oldDataSegment));

            var index = _dataSegments.FindIndex(s => s == oldDataSegment);
            return ChangeDataSegment(index, newDataSegment);
        }

        /// <summary>
        /// 改变数据片段。
        /// </summary>
        /// <param name="index">给定的旧数据索引。</param>
        /// <param name="newDataSegment">给定的新数据片段。</param>
        /// <returns>返回 <see cref="SignedTokenCombiner"/>。</returns>
        public SignedTokenCombiner ChangeDataSegment(int index, string newDataSegment)
        {
            newDataSegment.NotEmpty(nameof(newDataSegment));

            _dataSegments.RemoveAt(index);
            _dataSegments.Insert(index, newDataSegment);
            return this;
        }

        /// <summary>
        /// 改变数据片段列表。
        /// </summary>
        /// <param name="newDataSegments">给定的新数据片段列表。</param>
        /// <returns>返回 <see cref="SignedTokenCombiner"/>。</returns>
        public SignedTokenCombiner ChangeDataSegments(List<string> newDataSegments)
        {
            _dataSegments = newDataSegments.NotEmpty(nameof(newDataSegments));
            return this;
        }


        /// <summary>
        /// 添加数据片段。
        /// </summary>
        /// <param name="dataSegment">给定的数据片段。</param>
        /// <returns>返回 <see cref="SignedTokenCombiner"/>。</returns>
        public SignedTokenCombiner AddDataSegment(string dataSegment)
        {
            dataSegment.NotEmpty(nameof(dataSegment));

            _dataSegments.Add(dataSegment);
            return this;
        }

        /// <summary>
        /// 添加数据片段集合。
        /// </summary>
        /// <param name="dataSegments">给定的数据片段集合。</param>
        /// <returns>返回 <see cref="SignedTokenCombiner"/>。</returns>
        public SignedTokenCombiner AddDataSegments(IList<string> dataSegments)
        {
            _dataSegments.AddRange(dataSegments);
            return this;
        }


        /// <summary>
        /// 插入数据片段。
        /// </summary>
        /// <param name="index">给定的开始索引。</param>
        /// <param name="dataSegment">给定的数据片段。</param>
        /// <returns>返回 <see cref="SignedTokenCombiner"/>。</returns>
        public SignedTokenCombiner InsertDataSegment(int index, string dataSegment)
        {
            dataSegment.NotEmpty(nameof(dataSegment));

            _dataSegments.Insert(index, dataSegment);
            return this;
        }

        /// <summary>
        /// 插入数据片段集合。
        /// </summary>
        /// <param name="index">给定的开始索引。</param>
        /// <param name="dataSegments">给定的数据片段集合。</param>
        /// <returns>返回 <see cref="SignedTokenCombiner"/>。</returns>
        public SignedTokenCombiner InsertDataSegments(int index, List<string> dataSegments)
        {
            _dataSegments.InsertRange(index, dataSegments);
            return this;
        }


        /// <summary>
        /// 移除数据片段。
        /// </summary>
        /// <param name="dataSegment">给定的数据片段。</param>
        /// <returns>返回 <see cref="SignedTokenCombiner"/>。</returns>
        public SignedTokenCombiner RemoveDataSegment(string dataSegment)
        {
            dataSegment.NotEmpty(nameof(dataSegment));

            if (_dataSegments.Count <= 1)
                throw new InvalidOperationException(InternalResource.InvalidOperationExceptionDataSegment);

            _dataSegments.Remove(dataSegment);
            return this;
        }

        /// <summary>
        /// 移除数据片段集合。
        /// </summary>
        /// <param name="oldDataSegment">给定的旧数据片段。</param>
        /// <param name="count">给定要移除的条数。</param>
        /// <returns>返回 <see cref="SignedTokenCombiner"/>。</returns>
        public SignedTokenCombiner RemoveDataSegments(string oldDataSegment, int count)
        {
            var index = _dataSegments.FindIndex(s => s == oldDataSegment);
            return RemoveDataSegments(index, count);
        }

        /// <summary>
        /// 移除数据片段集合。
        /// </summary>
        /// <param name="index">给定的开始索引。</param>
        /// <param name="count">给定要移除的条数。</param>
        /// <returns>返回 <see cref="SignedTokenCombiner"/>。</returns>
        public SignedTokenCombiner RemoveDataSegments(int index, int count)
        {
            if (_dataSegments.Count <= count)
                throw new InvalidOperationException($"The current collection of data fragments to be removed cannot be greater than or equal to the current count '{_dataSegments.Count}'.");

            _dataSegments.RemoveRange(index, count);
            return this;
        }


        /// <summary>
        /// 复制当前实例。
        /// </summary>
        /// <returns>返回 <see cref="SignedTokenCombiner"/>。</returns>
        public SignedTokenCombiner Copy()
            => new SignedTokenCombiner(_dataSegments, _signatureFactory);


        /// <summary>
        /// 复制当前实例并添加数据片段。
        /// </summary>
        /// <param name="dataSegment">给定的数据片段。</param>
        /// <returns>返回 <see cref="SignedTokenCombiner"/>。</returns>
        public SignedTokenCombiner WithAddDataSegment(string dataSegment)
            => Copy().AddDataSegment(dataSegment);

        /// <summary>
        /// 复制当前实例并添加数据片段集合。
        /// </summary>
        /// <param name="dataSegments">给定的数据片段集合。</param>
        /// <returns>返回 <see cref="SignedTokenCombiner"/>。</returns>
        public SignedTokenCombiner WithAddDataSegments(IList<string> dataSegments)
            => Copy().AddDataSegments(dataSegments);


        /// <summary>
        /// 复制当前实例并插入数据片段。
        /// </summary>
        /// <param name="index">给定的开始索引。</param>
        /// <param name="dataSegment">给定的数据片段。</param>
        /// <returns>返回 <see cref="SignedTokenCombiner"/>。</returns>
        public SignedTokenCombiner WithInsertDataSegment(int index, string dataSegment)
            => Copy().InsertDataSegment(index, dataSegment);

        /// <summary>
        /// 复制当前实例并插入数据片段集合。
        /// </summary>
        /// <param name="index">给定的开始索引。</param>
        /// <param name="dataSegments">给定的数据片段集合。</param>
        /// <returns>返回 <see cref="SignedTokenCombiner"/>。</returns>
        public SignedTokenCombiner WithInsertDataSegments(int index, List<string> dataSegments)
            => Copy().InsertDataSegments(index, dataSegments);


        /// <summary>
        /// 复制当前实例并移除数据片段。
        /// </summary>
        /// <param name="dataSegment">给定的数据片段。</param>
        /// <returns>返回 <see cref="SignedTokenCombiner"/>。</returns>
        public SignedTokenCombiner WithRemoveDataSegment(string dataSegment)
            => Copy().RemoveDataSegment(dataSegment);

        /// <summary>
        /// 复制当前实例并移除数据片段集合。
        /// </summary>
        /// <param name="oldDataSegment">给定的旧数据片段。</param>
        /// <param name="count">给定要移除的条数。</param>
        /// <returns>返回 <see cref="SignedTokenCombiner"/>。</returns>
        public SignedTokenCombiner WithRemoveDataSegments(string oldDataSegment, int count)
            => Copy().RemoveDataSegments(oldDataSegment, count);

        /// <summary>
        /// 复制当前实例并移除数据片段集合。
        /// </summary>
        /// <param name="index">给定的开始索引。</param>
        /// <param name="count">给定要移除的条数。</param>
        /// <returns>返回 <see cref="SignedTokenCombiner"/>。</returns>
        public SignedTokenCombiner WithRemoveDataSegments(int index, int count)
            => Copy().RemoveDataSegments(index, count);


        /// <summary>
        /// 是否相等（忽略大小写）。
        /// </summary>
        /// <param name="other">给定的令牌。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(string other)
            => Source.Equals(other, StringComparison.Ordinal);

        /// <summary>
        /// 是否相等（忽略大小写）。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
            => obj is SignedTokenCombiner other && Equals(other);


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
        /// <param name="a">给定的 <see cref="SignedTokenCombiner"/>。</param>
        /// <param name="b">给定的 <see cref="SignedTokenCombiner"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator ==(SignedTokenCombiner a, SignedTokenCombiner b)
            => (a?.Equals(b)).Value;

        /// <summary>
        /// 是否不等（忽略大小写）。
        /// </summary>
        /// <param name="a">给定的 <see cref="SignedTokenCombiner"/>。</param>
        /// <param name="b">给定的 <see cref="SignedTokenCombiner"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator !=(SignedTokenCombiner a, SignedTokenCombiner b)
            => !(a?.Equals(b)).Value;


        /// <summary>
        /// 隐式转换为字符串。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="SignedTokenCombiner"/>。</param>
        public static implicit operator string(SignedTokenCombiner combiner)
            => combiner?.ToString();


        private static Func<string, string> EnsureSignatureFactory(out Func<string, string> result,
            Func<string, string> signatureFactory = null)
        {
            result = signatureFactory ?? DefaultSignatureFactory;
            return result;
        }


        private static string CombineParameters(IEnumerable<string> dataSegments,
            Func<string, string> signatureFactory = null)
        {
            dataSegments.NotEmpty(nameof(dataSegments));

            if (signatureFactory.IsNull())
                signatureFactory = DefaultSignatureFactory;

            var data = dataSegments.CompatibleJoinString(Separator);
            var dataSignature = signatureFactory.Invoke(data);

            return $"{data}{Separator}{dataSignature}";
        }

        private static bool TryParseParameters(string token, out string[] dataSegments,
            Func<string, string> signatureFactory = null)
        {
            dataSegments = token.CompatibleSplit(Separator);
            if (dataSegments.Length <= 1)
                return false; // 不能仅包含单个数据或签名片段

            // 提取末尾的签名片段
            var tokenSignature = dataSegments.Last();
            dataSegments = dataSegments.Take(dataSegments.Length - 1).ToArray();

            // 重新生成数据的签名片段
            if (signatureFactory.IsNull())
                signatureFactory = DefaultSignatureFactory;

            var data = dataSegments.CompatibleJoinString(Separator);
            var dataSignature = signatureFactory.Invoke(data);

            // 严格对比
            return dataSignature.Equals(tokenSignature, StringComparison.Ordinal);
        }


        /// <summary>
        /// 尝试解析组合器。
        /// </summary>
        /// <param name="token">给定的令牌。</param>
        /// <param name="result">输出 <see cref="SignedTokenCombiner"/>。</param>
        /// <param name="signatureFactory">给定的签名工厂方法（可选）。</param>
        /// <returns>返回布尔值。</returns>
        public static bool TryParseCombiner(string token, out SignedTokenCombiner result,
            Func<string, string> signatureFactory = null)
        {
            if (TryParseParameters(token, out var dataSegments, signatureFactory))
            {
                result = new SignedTokenCombiner(dataSegments, signatureFactory);
                return true;
            }

            result = null;
            return false;
        }

    }
}
