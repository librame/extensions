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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Librame.Extensions.Core.Identifiers
{
    /// <summary>
    /// MonggoDB 标识描述符。
    /// </summary>
    public class MonggoIdentificationDescriptor : IEquatable<MonggoIdentificationDescriptor>,
        IEqualityComparer<MonggoIdentificationDescriptor>, IComparable<MonggoIdentificationDescriptor>, IComparable
    {
        /// <summary>
        /// 空标识描述符。
        /// </summary>
        public static readonly MonggoIdentificationDescriptor Empty
            = Parse("000000000000000000000000");


        /// <summary>
        /// 构造一个 <see cref="MonggoIdentificationDescriptor"/>。
        /// </summary>
        /// <param name="buffer">给定的缓冲区字节列表。</param>
        public MonggoIdentificationDescriptor(IReadOnlyList<byte> buffer)
        {
            Buffer = buffer.NotNull(nameof(buffer));
        }


        /// <summary>
        /// 缓冲区字节列表。
        /// </summary>
        public IReadOnlyList<byte> Buffer { get; }

        /// <summary>
        /// 增量。
        /// </summary>
        public int Increment { get; private set; }

        /// <summary>
        /// 主机标识。
        /// </summary>
        public int MachineId { get; private set; }

        /// <summary>
        /// 进程标识。
        /// </summary>
        public int ProcessId { get; private set; }

        /// <summary>
        /// 时间戳。
        /// </summary>
        public int Timestamp { get; private set; }


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="x">给定的 <see cref="MonggoIdentificationDescriptor"/>。</param>
        /// <param name="y">给定的 <see cref="MonggoIdentificationDescriptor"/>。</param>
        /// <returns>返回的布尔值。</returns>
        public virtual bool Equals(MonggoIdentificationDescriptor x, MonggoIdentificationDescriptor y)
            => x?.Equals(y) == true;

        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="other">给定的 <see cref="MonggoIdentificationDescriptor"/>。</param>
        /// <returns>返回布尔值。</returns>
        public virtual bool Equals(MonggoIdentificationDescriptor other)
            => Buffer.SequenceEqual(other?.Buffer);

        /// <summary>
        /// 重写是否相等。
        /// </summary>
        /// <param name="obj">给定要比较的对象。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
            => obj is MonggoIdentificationDescriptor other && Equals(other);


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回 32 位整数。</returns>
        public override int GetHashCode()
            => GetHashCode(this);

        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <param name="obj">给定的 <see cref="MonggoIdentificationDescriptor"/>。</param>
        /// <returns>返回整数。</returns>
        public virtual int GetHashCode(MonggoIdentificationDescriptor obj)
            => obj.IsNull() ? -1
            : obj.ToString().CompatibleGetHashCode();


        /// <summary>
        /// 比较大小。
        /// </summary>
        /// <param name="other">给定的 <see cref="MonggoIdentificationDescriptor"/>。</param>
        /// <returns>返回整数。</returns>
        public virtual int CompareTo(MonggoIdentificationDescriptor other)
            => string.CompareOrdinal(ToString(), other?.ToString());

        /// <summary>
        /// 比较大小。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回整数。</returns>
        public virtual int CompareTo(object obj)
            => obj is MonggoIdentificationDescriptor other ? CompareTo(other) : -1;


        /// <summary>
        /// 转换为字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            for (int i = 0; i < Buffer.Count; i++)
            {
                sb.Append(Buffer[i].ToString("x2", CultureInfo.InvariantCulture));
            }

            return sb.ToString();
        }


        /// <summary>
        /// 解析标识字符串。
        /// </summary>
        /// <param name="id">给定的标识字符串。</param>
        /// <returns>返回 <see cref="MonggoIdentificationDescriptor"/>。</returns>
        [SuppressMessage("Design", "CA1031:不捕获常规异常类型", Justification = "<挂起>")]
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        [SuppressMessage("Globalization", "CA1303:请不要将文本作为本地化参数传递", Justification = "<挂起>")]
        public static MonggoIdentificationDescriptor Parse(string id)
        {
            id.NotEmpty(nameof(id));

            if (id.Length != 24)
                throw new ArgumentOutOfRangeException(nameof(id), "value should be 24 characters");

            var buffer = new byte[12];

            for (int i = 0; i < id.Length; i += 2)
            {
                try
                {
                    buffer[i / 2] = Convert.ToByte(id.Substring(i, 2), 16);
                }
                catch (Exception)
                {
                    buffer[i / 2] = 0;
                }
            }

            return Create(buffer);
        }

        /// <summary>
        /// 创建描述符。
        /// </summary>
        /// <param name="buffer">给定的缓冲区字节数组。</param>
        /// <returns>返回 <see cref="MonggoIdentificationDescriptor"/>。</returns>
        internal static MonggoIdentificationDescriptor Create(byte[] buffer)
        {
            var descriptor = new MonggoIdentificationDescriptor(buffer);

            var copyIndex = 0;

            var timestampBytes = new byte[4];
            Array.Copy(buffer, copyIndex, timestampBytes, 0, 4);
            Array.Reverse(timestampBytes);
            descriptor.Timestamp = BitConverter.ToInt32(timestampBytes, 0);

            copyIndex += 4;
            var machineIdBytes = new byte[4];
            Array.Copy(buffer, copyIndex, machineIdBytes, 0, 3);
            descriptor.MachineId = BitConverter.ToInt32(machineIdBytes, 0);

            copyIndex += 3;
            var processIdBytes = new byte[4];
            Array.Copy(buffer, copyIndex, processIdBytes, 0, 2);
            Array.Reverse(processIdBytes);
            descriptor.ProcessId = BitConverter.ToInt32(processIdBytes, 0);

            copyIndex += 2;
            var incrementBytes = new byte[4];
            Array.Copy(buffer, copyIndex, incrementBytes, 0, 3);
            Array.Reverse(incrementBytes);

            descriptor.Increment = BitConverter.ToInt32(incrementBytes, 0);

            return descriptor;
        }


        /// <summary>
        /// 比较相等。
        /// </summary>
        /// <param name="left">给定的 <see cref="MonggoIdentificationDescriptor"/>。</param>
        /// <param name="right">给定的 <see cref="MonggoIdentificationDescriptor"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator ==(MonggoIdentificationDescriptor left, MonggoIdentificationDescriptor right)
        {
            if (left is null)
                return right is null;

            return left.Equals(right);
        }

        /// <summary>
        /// 比较不等。
        /// </summary>
        /// <param name="left">给定的 <see cref="MonggoIdentificationDescriptor"/>。</param>
        /// <param name="right">给定的 <see cref="MonggoIdentificationDescriptor"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator !=(MonggoIdentificationDescriptor left, MonggoIdentificationDescriptor right)
            => !(left == right);

        /// <summary>
        /// 比较小于。
        /// </summary>
        /// <param name="left">给定的 <see cref="MonggoIdentificationDescriptor"/>。</param>
        /// <param name="right">给定的 <see cref="MonggoIdentificationDescriptor"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator <(MonggoIdentificationDescriptor left, MonggoIdentificationDescriptor right)
            => left is null ? right is MonggoIdentificationDescriptor : left.CompareTo(right) < 0;

        /// <summary>
        /// 比较小于等于。
        /// </summary>
        /// <param name="left">给定的 <see cref="MonggoIdentificationDescriptor"/>。</param>
        /// <param name="right">给定的 <see cref="MonggoIdentificationDescriptor"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator <=(MonggoIdentificationDescriptor left, MonggoIdentificationDescriptor right)
            => left is null || left.CompareTo(right) <= 0;

        /// <summary>
        /// 比较大于。
        /// </summary>
        /// <param name="left">给定的 <see cref="MonggoIdentificationDescriptor"/>。</param>
        /// <param name="right">给定的 <see cref="MonggoIdentificationDescriptor"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator >(MonggoIdentificationDescriptor left, MonggoIdentificationDescriptor right)
            => left is MonggoIdentificationDescriptor && left.CompareTo(right) > 0;

        /// <summary>
        /// 比较大于等于。
        /// </summary>
        /// <param name="left">给定的 <see cref="MonggoIdentificationDescriptor"/>。</param>
        /// <param name="right">给定的 <see cref="MonggoIdentificationDescriptor"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator >=(MonggoIdentificationDescriptor left, MonggoIdentificationDescriptor right)
            => left is null ? right is null : left.CompareTo(right) >= 0;


        /// <summary>
        /// 隐式转换为字符串。
        /// </summary>
        /// <param name="descriptor">给定的 <see cref="MonggoIdentificationDescriptor"/>。</param>
        public static implicit operator string(MonggoIdentificationDescriptor descriptor)
            => descriptor?.ToString();
    }
}
