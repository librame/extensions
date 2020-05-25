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
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Core.Identifiers
{
    using Services;
    using Utilities;

    /// <summary>
    /// COMB 标识符生成器。
    /// </summary>
    /// <remarks>
    /// 参考：https://mp.weixin.qq.com/s/C6xk42s-4SwyszJPTM0G6A。
    /// </remarks>
    public class CombIdentifierGenerator : IIdentifierGenerator<Guid>
    {
        /// <summary>
        /// 构造一个 <see cref="CombIdentifierGenerator"/>。
        /// </summary>
        /// <param name="identifierType">给定的 <see cref="CombIdentifierType"/>。</param>
        public CombIdentifierGenerator(CombIdentifierType identifierType)
        {
            IdentifierType = identifierType;
        }


        /// <summary>
        /// 标识符类型。
        /// </summary>
        public CombIdentifierType IdentifierType { get; }


        /// <summary>
        /// 异步生成标识符。
        /// </summary>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Guid"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public virtual async Task<Guid> GenerateAsync(IClockService clock,
            CancellationToken cancellationToken = default)
        {
            clock.NotNull(nameof(clock));

            var timestampBytes = await GetCurrentTimestampAsync(clock, cancellationToken)
                .ConfigureAndResultAsync();

            return ExtensionSettings.Preference.RunLockerResult(() =>
            {
                var randomBytes = RandomUtility.GenerateByteArray(10);
                var guidBytes = new byte[16];

                switch (IdentifierType)
                {
                    case CombIdentifierType.AsString:
                    case CombIdentifierType.AsBinary:
                        Buffer.BlockCopy(timestampBytes, 2, guidBytes, 0, 6);
                        Buffer.BlockCopy(randomBytes, 0, guidBytes, 6, 10);

                        // If formatting as a string, we have to reverse the order
                        // of the Data1 and Data2 blocks on little-endian systems.
                        if (IdentifierType == CombIdentifierType.AsString && BitConverter.IsLittleEndian)
                        {
                            Array.Reverse(guidBytes, 0, 4);
                            Array.Reverse(guidBytes, 4, 2);
                        }
                        break;

                    case CombIdentifierType.AtEnd:
                        Buffer.BlockCopy(randomBytes, 0, guidBytes, 0, 10);
                        Buffer.BlockCopy(timestampBytes, 2, guidBytes, 10, 6);
                        break;
                }

                return new Guid(guidBytes);
            });
        }

        private static async Task<byte[]> GetCurrentTimestampAsync(IClockService clock,
            CancellationToken cancellationToken = default)
        {
            var now = await clock.GetNowOffsetAsync(cancellationToken: cancellationToken)
                .ConfigureAndResultAsync();

            var buffer = BitConverter.GetBytes(now.Ticks / 10000L);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(buffer);

            return buffer;
        }


        /// <summary>
        /// 支持 MySQL 排序类型的生成器（char(36)）。
        /// </summary>
        public static readonly CombIdentifierGenerator MySQL
            = new CombIdentifierGenerator(CombIdentifierType.AsString);

        /// <summary>
        /// 支持 Oracle 排序类型的生成器（raw(16)）。
        /// </summary>
        public static readonly CombIdentifierGenerator Oracle
            = new CombIdentifierGenerator(CombIdentifierType.AsBinary);

        /// <summary>
        /// 支持 SQLite 排序类型的生成器（text）。
        /// </summary>
        public static readonly CombIdentifierGenerator SQLite
            = new CombIdentifierGenerator(CombIdentifierType.AsString);

        /// <summary>
        /// 支持 SQL Server 排序类型的生成器（uniqueidentifier）。
        /// </summary>
        public static readonly CombIdentifierGenerator SQLServer
            = new CombIdentifierGenerator(CombIdentifierType.AtEnd);
    }
}
