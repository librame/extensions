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
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Core.Tokens
{
    using Combiners;
    using Converters;

    /// <summary>
    /// 安全令牌。
    /// </summary>
    public class SecurityToken
    {
        private readonly byte[] _buffer;


        /// <summary>
        /// 构造一个 <see cref="SecurityToken"/>。
        /// </summary>
        /// <param name="g">给定的 <see cref="Guid"/>。</param>
        /// <param name="converter">给定的 <see cref="IAlgorithmConverter"/>（可选；默认使用 <see cref="HexStringConverter"/>）。</param>
        public SecurityToken(Guid g, IAlgorithmConverter converter = null)
            : this(g.ToByteArray())
        {
            EnsureConverter(converter);
        }

        /// <summary>
        /// 构造一个 <see cref="SecurityToken"/>。
        /// </summary>
        /// <param name="token">给定的令牌字符串。</param>
        /// <param name="converter">给定的 <see cref="IAlgorithmConverter"/>（可选；默认使用 <see cref="HexStringConverter"/>）。</param>
        public SecurityToken(string token, IAlgorithmConverter converter = null)
        {
            if (!TryParseToken(EnsureConverter(converter), token, out var buffer))
                throw new ArgumentException($"The token '{token}' is not a valid security token.");

            _buffer = buffer;
        }

        private SecurityToken(byte[] buffer)
        {
            _buffer = buffer;
        }


        private IAlgorithmConverter EnsureConverter(IAlgorithmConverter converter = null)
        {
            Converter = converter ?? ConverterManager.GetAlgorithm<HexStringConverter>();
            return Converter;
        }


        /// <summary>
        /// 算法转换器。
        /// </summary>
        /// <value>返回 <see cref="IAlgorithmConverter"/>。</value>
        public IAlgorithmConverter Converter { get; private set; }


        /// <summary>
        /// 转换为 GUID。
        /// </summary>
        /// <returns>返回 <see cref="Guid"/>。</returns>
        public Guid ToGuid()
            => new Guid(_buffer);


        /// <summary>
        /// 转换为只读内存字节。
        /// </summary>
        /// <returns>返回 <see cref="ReadOnlyMemory{Byte}"/>。</returns>
        public ReadOnlyMemory<byte> ToReadOnlyMemory()
            => _buffer;


        /// <summary>
        /// 转换为长度为 15 的短字符串（不可解析还原，可当作标识）。
        /// </summary>
        /// <param name="timestamp">给定的 <see cref="DateTime"/>。</param>
        /// <returns>返回字符串。</returns>
        public string ToShortString(DateTime timestamp)
            => _buffer.FormatString(timestamp.Ticks);

        /// <summary>
        /// 转换为长度为 15 的短字符串（不可解析还原，可当作标识）。
        /// </summary>
        /// <param name="timestamp">给定的 <see cref="DateTimeOffset"/>。</param>
        /// <returns>返回字符串。</returns>
        public string ToShortString(DateTimeOffset timestamp)
            => _buffer.FormatString(timestamp.Ticks);


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="other">给定的 <see cref="SecurityToken"/>。</param>
        /// <returns>返回布尔值。</returns>
        public bool Equals(SecurityToken other)
            => ToString() == other?.ToString();

        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
            => obj is SecurityToken other && Equals(other);


        /// <summary>
        /// 定义比较相等静态方法需强制重写此方法。
        /// </summary>
        /// <returns>返回 32 位带符号整数。</returns>
        public override int GetHashCode()
            => ToString().CompatibleGetHashCode();


        /// <summary>
        /// 转换为 BASE64 字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
        {
            return new SignedTokenCombiner(new string[]
            {
                Converter.ConvertTo(_buffer)
            },
            s => s.Sha256HexString());
        }

        [SuppressMessage("Design", "CA1031:不捕获常规异常类型")]
        private static bool TryParseToken(IAlgorithmConverter converter, string token, out byte[] buffer)
        {
            if (TryParseSignedToken(token, out var result))
            {
                buffer = converter.ConvertFrom(result.DataSegments[0]);
                return true;
            }

            buffer = null;
            return false;
        }

        private static bool TryParseSignedToken(string token, out SignedTokenCombiner result)
            => SignedTokenCombiner.TryParseCombiner(token, out result, s => s.Sha256HexString());


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="a">给定的 <see cref="SecurityToken"/>。</param>
        /// <param name="b">给定的 <see cref="SecurityToken"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator ==(SecurityToken a, SecurityToken b)
            => (a?.Equals(b)).Value;

        /// <summary>
        /// 是否不等。
        /// </summary>
        /// <param name="a">给定的 <see cref="SecurityToken"/>。</param>
        /// <param name="b">给定的 <see cref="SecurityToken"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator !=(SecurityToken a, SecurityToken b)
            => !(a?.Equals(b)).Value;


        /// <summary>
        /// 隐式转换为 GUID。
        /// </summary>
        /// <param name="token">给定的 <see cref="SecurityToken"/>。</param>
        public static implicit operator Guid(SecurityToken token)
            => token.NotNull(nameof(token)).ToGuid();

        /// <summary>
        /// 隐式转换为字符串形式。
        /// </summary>
        /// <param name="token">给定的 <see cref="SecurityToken"/>。</param>
        public static implicit operator string(SecurityToken token)
            => token?.ToString();


        /// <summary>
        /// 空安全令牌。
        /// </summary>
        public static readonly SecurityToken Empty
            = new SecurityToken(Guid.Empty);


        /// <summary>
        /// 新建安全令牌。
        /// </summary>
        /// <returns>返回 <see cref="SecurityToken"/>。</returns>
        public static SecurityToken New()
            => new SecurityToken(Guid.NewGuid());

        /// <summary>
        /// 新建安全令牌数组。
        /// </summary>
        /// <param name="count">给定要生成的实例数量。</param>
        /// <returns>返回 <see cref="SecurityToken"/> 数组。</returns>
        public static SecurityToken[] NewArray(int count)
        {
            var tokens = new SecurityToken[count];
            for (var i = 0; i < count; i++)
                tokens[i] = New();

            return tokens;
        }


        /// <summary>
        /// 尝试获取安全令牌。
        /// </summary>
        /// <param name="token">给定的令牌字符串。</param>
        /// <param name="result">输出 <see cref="SecurityToken"/>。</param>
        /// <param name="converter">给定的 <see cref="IAlgorithmConverter"/>（可选；默认使用 <see cref="HexStringConverter"/>）。</param>
        /// <returns>返回布尔值。</returns>
        public static bool TryGetToken(string token,
            out SecurityToken result, IAlgorithmConverter converter = null)
        {
            if (TryParseSignedToken(token, out _))
            {
                result = new SecurityToken(token, converter);
                return true;
            }

            result = null;
            return false;
        }

    }
}
