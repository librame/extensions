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
    /// COMB 标识生成器。
    /// </summary>
    /// <remarks>
    /// 参考：https://mp.weixin.qq.com/s/C6xk42s-4SwyszJPTM0G6A。
    /// </remarks>
    public class CombIdentificationGenerator : AbstractIdentificationGenerator<Guid>
    {
        /// <summary>
        /// 构造一个 <see cref="CombIdentificationGenerator"/>。
        /// </summary>
        /// <param name="generation">给定的 <see cref="CombIdentificationGeneration"/>。</param>
        public CombIdentificationGenerator(CombIdentificationGeneration generation)
        {
            Generation = generation;
        }


        /// <summary>
        /// COMB 标识生成。
        /// </summary>
        public CombIdentificationGeneration Generation { get; }


        /// <summary>
        /// 生成标识。
        /// </summary>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <returns>返回 <see cref="Guid"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public override Guid GenerateId(IClockService clock)
        {
            clock.NotNull(nameof(clock));

            var timestampBytes = GetCurrentTimestamp(clock);

            return GenerateIdCore(timestampBytes);
        }

        /// <summary>
        /// 异步生成标识。
        /// </summary>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="Guid"/> 的异步操作。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public override async Task<Guid> GenerateIdAsync(IClockService clock,
            CancellationToken cancellationToken = default)
        {
            clock.NotNull(nameof(clock));

            var timestampBytes = await GetCurrentTimestampAsync(clock, cancellationToken)
                .ConfigureAwait();

            return GenerateIdCore(timestampBytes);
        }

        /// <summary>
        /// 生成标识核心。
        /// </summary>
        /// <param name="timestampBytes">给定的时间戳字节数组。</param>
        /// <returns>返回 <see cref="Guid"/>。</returns>
        protected virtual Guid GenerateIdCore(byte[] timestampBytes)
        {
            return ExtensionSettings.Preference.RunLocker(() =>
            {
                var randomBytes = RandomUtility.GenerateByteArray(10);
                var guidBytes = new byte[16];

                switch (Generation)
                {
                    case CombIdentificationGeneration.AsString:
                    case CombIdentificationGeneration.AsBinary:
                        Buffer.BlockCopy(timestampBytes, 2, guidBytes, 0, 6);
                        Buffer.BlockCopy(randomBytes, 0, guidBytes, 6, 10);

                        // If formatting as a string, we have to reverse the order
                        // of the Data1 and Data2 blocks on little-endian systems.
                        if (Generation == CombIdentificationGeneration.AsString && BitConverter.IsLittleEndian)
                        {
                            Array.Reverse(guidBytes, 0, 4);
                            Array.Reverse(guidBytes, 4, 2);
                        }
                        break;

                    case CombIdentificationGeneration.AtEnd:
                        Buffer.BlockCopy(randomBytes, 0, guidBytes, 0, 10);
                        Buffer.BlockCopy(timestampBytes, 2, guidBytes, 10, 6);
                        break;
                }

                return new Guid(guidBytes);
            });
        }


        /// <summary>
        /// 获取当前时间戳。
        /// </summary>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <returns>返回字节数组。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        protected virtual byte[] GetCurrentTimestamp(IClockService clock)
        {
            var now = clock.GetNowOffset();

            var buffer = BitConverter.GetBytes(now.Ticks / 10000L);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(buffer);

            return buffer;
        }

        /// <summary>
        /// 异步获取当前时间戳。
        /// </summary>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含字节数组的异步操作。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        protected virtual async Task<byte[]> GetCurrentTimestampAsync(IClockService clock,
            CancellationToken cancellationToken = default)
        {
            var now = await clock.GetNowOffsetAsync(cancellationToken: cancellationToken)
                .ConfigureAwait();

            var buffer = BitConverter.GetBytes(now.Ticks / 10000L);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(buffer);

            return buffer;
        }


        /// <summary>
        /// 支持 MySQL 排序类型的 COMB 标识生成器（char(36)）。
        /// </summary>
        public static readonly CombIdentificationGenerator MySQL
            = new CombIdentificationGenerator(CombIdentificationGeneration.AsString);

        /// <summary>
        /// 支持 Oracle 排序类型的 COMB 标识生成器（raw(16)）。
        /// </summary>
        public static readonly CombIdentificationGenerator Oracle
            = new CombIdentificationGenerator(CombIdentificationGeneration.AsBinary);

        /// <summary>
        /// 支持 SQLite 排序类型的 COMB 标识生成器（text）。
        /// </summary>
        public static readonly CombIdentificationGenerator SQLite
            = new CombIdentificationGenerator(CombIdentificationGeneration.AsString);

        /// <summary>
        /// 支持 SQL Server 排序类型的 COMB 标识生成器（uniqueidentifier）。
        /// </summary>
        public static readonly CombIdentificationGenerator SQLServer
            = new CombIdentificationGenerator(CombIdentificationGeneration.AtEnd);
    }
}
