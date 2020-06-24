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

namespace Librame.Extensions
{
    /// <summary>
    /// GUID 静态扩展。
    /// </summary>
    public static class GuidExtensions
    {
        /// <summary>
        /// 转换为长度为 15 的短字符串形式。
        /// </summary>
        /// <param name="g">给定的 <see cref="Guid"/>。</param>
        /// <param name="timestamp">给定的时间戳（可选；默认为当前时间的 UTC 形式）。</param>
        /// <returns>返回字符串。</returns>
        public static string AsShortString(this Guid g, DateTime? timestamp = null)
            => g.ToByteArray().FormatString(timestamp.NotNullOrDefault(DateTime.UtcNow).Ticks);

        /// <summary>
        /// 转换为长度为 15 的短字符串形式。
        /// </summary>
        /// <param name="g">给定的 <see cref="Guid"/>。</param>
        /// <param name="timestamp">给定的时间戳。</param>
        /// <returns>返回字符串。</returns>
        public static string AsShortString(this Guid g, DateTimeOffset timestamp)
            => g.ToByteArray().FormatString(timestamp.Ticks);


        /// <summary>
        /// 转换为有顺序的 GUID。
        /// </summary>
        /// <param name="g">给定的 <see cref="Guid"/>。</param>
        /// <param name="timestamp">给定的时间戳（可选；默认不能小于等于 <see cref="IExtensionPreferenceSetting.BaseDateTime"/>）。</param>
        /// <returns>返回 <see cref="Guid"/>。</returns>
        public static Guid AsCombGuid(this Guid g, DateTime? timestamp = null)
        {
            var buffer = g.ToByteArray();

            var stampDate = timestamp.NotNullOrDefault(DateTime.UtcNow);
            var baseDate = ExtensionSettings.Preference.BaseDateTime;
            stampDate.NotLesser(baseDate, nameof(timestamp), equals: true);

            // Get the days and milliseconds which will be used to build the byte string 
            var days = new TimeSpan(stampDate.Ticks - baseDate.Ticks);
            var msecs = new TimeSpan(stampDate.Ticks - stampDate.Date.Ticks);

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
        /// 转换为有顺序的 GUID。
        /// </summary>
        /// <param name="g">给定的 <see cref="Guid"/>。</param>
        /// <param name="timestamp">给定的时间戳（不能小于等于 <see cref="IExtensionPreferenceSetting.BaseDateTime"/>）。</param>
        /// <returns>返回 <see cref="Guid"/>。</returns>
        public static Guid AsCombGuid(this Guid g, DateTimeOffset timestamp)
        {
            var buffer = g.ToByteArray();

            var baseDate = new DateTimeOffset(ExtensionSettings.Preference.BaseDateTime, timestamp.Offset);
            timestamp.NotLesser(baseDate, nameof(timestamp), equals: true);

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

    }
}
