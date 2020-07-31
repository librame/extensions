#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Core.Identifiers
{
    using Services;

    /// <summary>
    /// 字符串标识生成器。
    /// </summary>
    public class StringIdentityGenerator : AbstractIdentityGenerator<string>
    {
        /// <summary>
        /// 构造一个 <see cref="StringIdentityGenerator"/>。
        /// </summary>
        /// <param name="identityGenerator">给定的 <see cref="IIdentityGenerator{Int64}"/>。</param>
        public StringIdentityGenerator(IIdentityGenerator<long> identityGenerator)
        {
            IdentityGenerator = identityGenerator.NotNull(nameof(identityGenerator));
        }


        /// <summary>
        /// 长整数标识生成器。
        /// </summary>
        public IIdentityGenerator<long> IdentityGenerator { get; }


        /// <summary>
        /// 转换进制（默认为 52 进制）。
        /// </summary>
        public int System { get; set; }
            = 52;


        /// <summary>
        /// 生成标识。
        /// </summary>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <returns>返回长整数。</returns>
        public override string GenerateId(IClockService clock)
            => IdentityGenerator.GenerateId(clock).AsSystemString(System);

        /// <summary>
        /// 异步生成标识。
        /// </summary>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含长整数的异步操作。</returns>
        public override async Task<string> GenerateIdAsync(IClockService clock,
            CancellationToken cancellationToken = default)
            => (await IdentityGenerator.GenerateIdAsync(clock, cancellationToken).ConfigureAwait())
                .AsSystemString(System);


        /// <summary>
        /// 生成短标识字符串。
        /// </summary>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="shortLength">给定的短标识长度（不小于 6）。</param>
        /// <returns>返回字符串。</returns>
        public virtual string GenerateShortId(IClockService clock, int shortLength)
            => SubIdString(GenerateId(clock), shortLength);

        /// <summary>
        /// 异步生成短标识字符串。
        /// </summary>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="shortLength">给定的短标识长度（不小于 6）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含字符串的异步操作。</returns>
        public virtual async Task<string> GenerateShortIdAsync(IClockService clock, int shortLength,
            CancellationToken cancellationToken = default)
            => SubIdString(await GenerateIdAsync(clock, cancellationToken).ConfigureAwait(), shortLength);


        private static string SubIdString(string id, int shortLength)
        {
            shortLength.NotOutOfRange(6, id.Length, nameof(shortLength));

            int index = id.Length - shortLength;
            if (index <= 0)
                return id;

            return id.Substring(index);
        }


        /// <summary>
        /// 默认字符串标识符生成器。
        /// </summary>
        public static readonly StringIdentityGenerator Default
            = new StringIdentityGenerator(SnowflakeIdentityGenerator.Default);
    }
}
