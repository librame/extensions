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
using System.Collections.Generic;
using System.Linq;

namespace Librame.Extensions
{
    /// <summary>
    /// GUID 静态扩展。
    /// </summary>
    public static class GuidExtensions
    {
        /// <summary>
        /// 转换为有顺序的 GUID 集合。
        /// </summary>
        /// <param name="guids">给定的 <see cref="IEnumerable{Guid}"/>。</param>
        /// <param name="timestamp">给定的时间戳。</param>
        /// <returns>返回 <see cref="IEnumerable{Guid}"/>。</returns>
        public static IEnumerable<Guid> AsCombGuids(this IEnumerable<Guid> guids, DateTimeOffset timestamp)
            => guids.Select(id => id.AsCombGuid(timestamp));

        /// <summary>
        /// 转换为有顺序的 GUID。
        /// </summary>
        /// <param name="g">给定的 <see cref="Guid"/>。</param>
        /// <param name="timestamp">给定的时间戳。</param>
        /// <returns>返回 <see cref="Guid"/>。</returns>
        public static Guid AsCombGuid(this Guid g, DateTimeOffset timestamp)
        {
            var buffer = g.ToByteArray();
            var baseDate = new DateTimeOffset(ExtensionSettings.BaseDateTime, timestamp.Offset);

            // Get the days and milliseconds which will be used to build the byte string 
            var days = new TimeSpan(timestamp.Ticks - baseDate.Ticks);
            var msecs = new TimeSpan(timestamp.Ticks - timestamp.Date.Ticks);

            // Convert to a byte array
            // SQL Server is accurate to 1/300th of a millisecond so we divide by 3.333333 
            var daysArray = BitConverter.GetBytes(days.Days);
            var msecsArray = BitConverter.GetBytes((long)(msecs.TotalMilliseconds / 3.333333));

            // Reverse the bytes to match SQL Servers ordering
            Array.Reverse(daysArray);
            Array.Reverse(msecsArray);

            for (int i = 15; i >= 6; i--)
                buffer[i] = buffer[i - 6];

            Array.Copy(daysArray, daysArray.Length - 2, buffer, 0, 2);
            Array.Copy(msecsArray, msecsArray.Length - 4, buffer, 2, 4);

            return new Guid(buffer);
        }


        /// <summary>
        /// 生成 GUID 集合。
        /// </summary>
        /// <param name="count">给定要生成的数量。</param>
        /// <returns>返回 <see cref="IEnumerable{Guid}"/>。</returns>
        public static IEnumerable<Guid> GenerateGuids(this int count)
        {
            for (var i = 0; i < count; i++)
                yield return Guid.NewGuid();
        }

        /// <summary>
        /// 生成有顺序的 GUID 集合。
        /// </summary>
        /// <param name="count">给定要生成的数量。</param>
        /// <param name="timestamp">给定的时间戳。</param>
        /// <returns>返回有顺序的 <see cref="IEnumerable{Guid}"/>。</returns>
        public static IEnumerable<Guid> GenerateCombIds(this int count, DateTimeOffset timestamp)
            => count.GenerateCombIds(timestamp, out _);

        /// <summary>
        /// 生成有顺序的 GUID 集合。
        /// </summary>
        /// <param name="count">给定要生成的数量。</param>
        /// <param name="timestamp">给定的时间戳。</param>
        /// <param name="guids">输出原始 <see cref="IEnumerable{Guid}"/>。</param>
        /// <returns>返回有顺序的 <see cref="IEnumerable{Guid}"/>。</returns>
        public static IEnumerable<Guid> GenerateCombIds(this int count, DateTimeOffset timestamp, out IEnumerable<Guid> guids)
        {
            guids = count.GenerateGuids();
            return guids.AsCombGuids(timestamp);
        }

    }
}
