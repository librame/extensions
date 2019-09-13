#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Threading;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 雪花标识符生成器。
    /// </summary>
    public class SnowflakeIdentifierGenerator
    {
        private static long _machineId = 0L;
        private static long _dataCenterId = 0L;
        private static long _sequence = 0L;

        private static long _twepoch = 687888001020L;

        private static long _machineIdBits = 5L;
        private static long _dataCenterIdBits = 5L;
        private static long _maxMachineId = -1L ^ -1L << (int)_machineIdBits;
        private static long _maxDatacenterId = -1L ^ (-1L << (int)_dataCenterIdBits);

        private static long _sequenceBits = 12L;
        private static long _machineIdShift = _sequenceBits;
        private static long _dataCenterIdShift = _sequenceBits + _machineIdBits;
        private static long _timestampLeftShift = _sequenceBits + _machineIdBits + _dataCenterIdBits;
        private static long _sequenceMask = -1L ^ -1L << (int)_sequenceBits;
        private static long _lastTimestamp = -1L;

        private static byte[] _locker = new byte[0];


        /// <summary>
        /// 构造一个 <see cref="SnowflakeIdentifierGenerator"/>。
        /// </summary>
        /// <param name="machineId">给定的机器标识。</param>
        /// <param name="dataCenterId">给定的数据中心标识。</param>
        public SnowflakeIdentifierGenerator(long machineId, long dataCenterId)
        {
            if (machineId >= 0)
                _machineId = machineId.NotGreater(_maxMachineId, nameof(machineId));

            if (dataCenterId >= 0)
                _dataCenterId = dataCenterId.NotGreater(_maxDatacenterId, nameof(dataCenterId));
        }
        

        /// <summary>
        /// 生成标识符。
        /// </summary>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <returns>返回长整数。</returns>
        public long Generate(IClockService clock)
        {
            clock.NotNull(nameof(clock));

            lock (_locker)
            {
                var timestamp = GetCurrentTimestamp(clock);
                timestamp.NotLesser(_lastTimestamp, nameof(timestamp));

                if (_lastTimestamp == timestamp)
                {
                    _sequence = (_sequence + 1) & _sequenceMask;
                    if (_sequence == 0)
                        timestamp = GetNextTimestamp(_lastTimestamp, clock);
                }
                else
                {
                    _sequence = 0L;
                }

                _lastTimestamp = timestamp;

                var id = ((timestamp - _twepoch) << (int)_timestampLeftShift)
                    | (_dataCenterId << (int)_dataCenterIdShift)
                    | (_machineId << (int)_machineIdShift)
                    | _sequence;

                // Length(19): 5557114366106533888
                return id;
            }
        }

        private long GetCurrentTimestamp(IClockService clock)
        {
            return clock.GetOffsetNowAsync(DateTimeOffset.UtcNow, true).Result.ToFileTime();
        }

        private long GetNextTimestamp(long lastTimestamp, IClockService clock)
        {
            var timestamp = GetCurrentTimestamp(clock);

            if (timestamp == lastTimestamp)
            {
                Thread.Sleep(1);
                return GetCurrentTimestamp(clock);
            }

            if (timestamp < lastTimestamp)
            {
                // 将 100 纳秒转换为毫秒
                var msec = (lastTimestamp - timestamp) / 10000;
                Thread.Sleep(Convert.ToInt32(msec) + 1);
                return GetCurrentTimestamp(clock);
            }

            return timestamp;
        }


        /// <summary>
        /// 默认生成器。
        /// </summary>
        public static readonly SnowflakeIdentifierGenerator Default
            = new SnowflakeIdentifierGenerator(0L, 0L);
    }
}
