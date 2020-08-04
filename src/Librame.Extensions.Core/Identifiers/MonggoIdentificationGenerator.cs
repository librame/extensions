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
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Core.Identifiers
{
    using Services;

    /// <summary>
    /// MonggoDB 标识生成器（可生成长度 24 位且包含数字、字母的字符串标识）。
    /// </summary>
    /// <remarks>
    /// 参考：https://mp.weixin.qq.com/s/ooquxazWRcFOqPj9XjF93Q。
    /// </remarks>
    public class MonggoIdentificationGenerator : AbstractIdentificationGenerator<string>
    {
        private static int _location
            = Environment.TickCount;

        private static readonly UTF8Encoding _encoding
            = new UTF8Encoding(false);


        private readonly byte[] _machineHash;
        private readonly byte[] _processIdHex;


        /// <summary>
        /// 构造一个 <see cref="MonggoIdentificationGenerator"/>。
        /// </summary>
        public MonggoIdentificationGenerator()
        {
            _machineHash = _encoding.GetBytes(Dns.GetHostName()).Md5();
            _processIdHex = BitConverter.GetBytes(Process.GetCurrentProcess().Id);
            Array.Reverse(_processIdHex);
        }


        /// <summary>
        /// 生成标识。
        /// </summary>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <returns>返回字符串。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public override string GenerateId(IClockService clock)
            => GenerateId(clock, out _);

        /// <summary>
        /// 异步生成标识。
        /// </summary>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含字符串的异步操作。</returns>
        public override async Task<string> GenerateIdAsync(IClockService clock,
            CancellationToken cancellationToken = default)
            => await GenerateIdDescriptorAsync(clock, cancellationToken).ConfigureAwait();


        /// <summary>
        /// 生成标识。
        /// </summary>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="result">输出 <see cref="MonggoIdentificationDescriptor"/>。</param>
        /// <returns>返回字符串。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public virtual string GenerateId(IClockService clock,
            out MonggoIdentificationDescriptor result)
        {
            clock.NotNull(nameof(clock));

            var timestamp = GetTimestamp(clock);

            var buffer = ComputeHex(timestamp);
            result = MonggoIdentificationDescriptor.Create(buffer);

            return result.ToString();
        }

        /// <summary>
        /// 异步生成标识。
        /// </summary>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含字符串的异步操作。</returns>
        public virtual Task<MonggoIdentificationDescriptor> GenerateIdDescriptorAsync(IClockService clock,
            CancellationToken cancellationToken = default)
            => cancellationToken.RunOrCancelAsync(() =>
            {
                var timestamp = GetTimestamp(clock);

                var buffer = ComputeHex(timestamp);
                return MonggoIdentificationDescriptor.Create(buffer);
            });


        private byte[] ComputeHex(int timestamp)
        {
            var buffer = new byte[12];

            var copyIndex = 0;

            var timestampBytes = BitConverter.GetBytes(timestamp);
            Array.Reverse(timestampBytes);
            Array.Copy(timestampBytes, 0, buffer, copyIndex, 4);
            
            copyIndex += 4;
            Array.Copy(_machineHash, 0, buffer, copyIndex, 3);

            copyIndex += 3;
            Array.Copy(_processIdHex, 2, buffer, copyIndex, 2);

            copyIndex += 2;
            var incrementBytes = BitConverter.GetBytes(Interlocked.Increment(ref _location));
            Array.Reverse(incrementBytes);
            Array.Copy(incrementBytes, 1, buffer, copyIndex, 3);

            return buffer;
        }

        private static int GetTimestamp(IClockService clock)
        {
            var ts = clock.GetNow() - ExtensionSettings.Preference.UnixEpoch;

            return Convert.ToInt32(Math.Floor(ts.TotalSeconds));
        }


        /// <summary>
        /// 默认 MonggoDB 标识生成器。
        /// </summary>
        public static readonly MonggoIdentificationGenerator Default
            = new MonggoIdentificationGenerator();
    }
}
