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

    /// <summary>
    /// 雪花标识生成器。
    /// </summary>
    /// <remarks>
    /// 参考：https://www.cnblogs.com/zhaoshujie/p/12010052.html
    /// </remarks>
    public class SnowflakeIdentityGenerator : AbstractIdentityGenerator<long>
    {
        // 唯一时间，这是一个避免重复的随机量，自行设定不要大于当前时间戳
        private const long _twepoch = 687888001020L;

        // 计数器字节数，10个字节用来保存计数码
        private const int _sequenceBits = 10;

        // 机器码字节数。4个字节用来保存机器码（定义为Long类型会出现最大偏移64位，所以左移64位没有意义）
        private const int _machineIdBits = 4;
        private const int _dataCenterIdBits = 4;

        private const long _maxMachineId = -1L ^ -1L << _machineIdBits;
        private const long _maxDatacenterId = -1L ^ (-1L << _dataCenterIdBits);

        // 机器码数据左移位数，就是后面计数器占用的位数
        private const int _machineIdShift = _sequenceBits;
        private const int _dataCenterIdShift = _sequenceBits + _machineIdBits;
        // 时间戳左移动位数就是机器码和计数器总字节数
        private const int _timestampLeftShift = _sequenceBits + _machineIdBits + _dataCenterIdBits;

        // 一微秒内可以产生计数，如果达到该值则等到下一微妙在进行生成
        private long _sequenceMask = -1L ^ -1L << _sequenceBits;

        private readonly long _machineId = 0L;
        private readonly long _dataCenterId = 0L;

        private long _sequence = 0L;
        private long _lastTimestamp = -1L;


        /// <summary>
        /// 构造一个 <see cref="SnowflakeIdentityGenerator"/>。
        /// </summary>
        /// <param name="machineId">给定的机器标识。</param>
        /// <param name="dataCenterId">给定的数据中心标识。</param>
        public SnowflakeIdentityGenerator(long machineId, long dataCenterId)
        {
            if (machineId >= 0)
                _machineId = machineId.NotGreater(_maxMachineId, nameof(machineId));

            if (dataCenterId >= 0)
                _dataCenterId = dataCenterId.NotGreater(_maxDatacenterId, nameof(dataCenterId));
        }


        /// <summary>
        /// 生成标识。
        /// </summary>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <returns>返回长整数。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public override long GenerateId(IClockService clock)
        {
            clock.NotNull(nameof(clock));

            var timestamp = GetTimestamp(clock);

            if (_lastTimestamp == timestamp)
            {
                // 同一微妙中生成ID
                // 用&运算计算该微秒内产生的计数是否已经到达上限
                _sequence = (_sequence + 1) & _sequenceMask;
                if (_sequence == 0)
                {
                    timestamp = GetNextTimestamp(clock);
                }
            }
            else
            {
                // 不同微秒生成ID
                // 计数清0
                _sequence = 0;
            }

            return GenerateCore(timestamp);
        }

        /// <summary>
        /// 异步生成标识。
        /// </summary>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含长整数的异步操作。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public override async Task<long> GenerateIdAsync(IClockService clock,
            CancellationToken cancellationToken = default)
        {
            clock.NotNull(nameof(clock));

            var timestamp = await GetTimestampAsync(clock, cancellationToken)
                .ConfigureAwait();

            if (_lastTimestamp == timestamp)
            {
                // 同一微妙中生成ID
                // 用&运算计算该微秒内产生的计数是否已经到达上限
                _sequence = (_sequence + 1) & _sequenceMask;
                if (_sequence == 0)
                {
                    timestamp = await GetNextTimestampAsync(clock, cancellationToken)
                        .ConfigureAwait();
                }
            }
            else
            {
                // 不同微秒生成ID
                // 计数清0
                _sequence = 0;
            }

            return GenerateCore(timestamp);
        }

        private long GenerateCore(long timestamp)
        {
            if (timestamp < _lastTimestamp)
            {
                // 如果当前时间戳比上一次生成ID时时间戳还小，抛出异常，因为不能保证现在生成的ID之前没有生成过
                throw new Exception($"Clock moved backwards. Refusing to generate id for {_lastTimestamp - timestamp} milliseconds");
            }

            return ExtensionSettings.Preference.RunLocker(() =>
            {
                var nextId = (timestamp - _twepoch << _timestampLeftShift)
                    | (_dataCenterId << _dataCenterIdShift)
                    | (_machineId << _machineIdShift)
                    | _sequence;

                _lastTimestamp = timestamp;

                // Length(18): 814352650875961344
                return nextId;
            });
        }


        private long GetNextTimestamp(IClockService clock)
        {
            var timestamp = GetTimestamp(clock);

            while (timestamp <= _lastTimestamp)
            {
                timestamp = GetTimestamp(clock);
            }

            return timestamp;
        }

        private async Task<long> GetNextTimestampAsync(IClockService clock,
            CancellationToken cancellationToken = default)
        {
            var timestamp = await GetTimestampAsync(clock, cancellationToken)
                .ConfigureAwait();

            while (timestamp <= _lastTimestamp)
            {
                timestamp = await GetTimestampAsync(clock, cancellationToken)
                    .ConfigureAwait();
            }

            return timestamp;
        }


        private static long GetTimestamp(IClockService clock)
        {
            var offsetNow = clock.GetNowOffset();

            var offsetBaseTime = new DateTimeOffset(ExtensionSettings.Preference.BaseDateTime, offsetNow.Offset);
            return (long)(offsetNow - offsetBaseTime).TotalMilliseconds;
        }

        private static async Task<long> GetTimestampAsync(IClockService clock,
            CancellationToken cancellationToken = default)
        {
            var offsetNow = await clock.GetNowOffsetAsync(cancellationToken: cancellationToken)
                .ConfigureAwait();

            var offsetBaseTime = new DateTimeOffset(ExtensionSettings.Preference.BaseDateTime, offsetNow.Offset);
            return (long)(offsetNow - offsetBaseTime).TotalMilliseconds;
        }


        /// <summary>
        /// 设备与数据中心为 0 的默认雪花标识符生成器。
        /// </summary>
        public static readonly SnowflakeIdentityGenerator Default
            = new SnowflakeIdentityGenerator(0, 0);
    }
}
