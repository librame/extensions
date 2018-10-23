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

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 格式化描述符。
    /// </summary>
    public class FormattingDescriptor : IEquatable<FormattingDescriptor>
    {
        /// <summary>
        /// 构造一个 <see cref="FormattingDescriptor"/> 实例。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// key is empty. / value or format is empty.
        /// </exception>
        /// <param name="key">给定的键名。</param>
        /// <param name="format">给定的格式（可选；格式与值的参数至少二选一）。</param>
        /// <param name="value">给定的值（可选；格式与值的参数至少二选一）。</param>
        public FormattingDescriptor(string key, string value = null, string format = null)
        {
            Key = key.NotEmpty(nameof(key));

            if (value.IsEmpty() && format.IsEmpty())
                throw new ArgumentNullException("value/format");

            Value = value;
            Format = format;
        }


        /// <summary>
        /// 键名。
        /// </summary>
        public string Key { get; }


        /// <summary>
        /// 值。
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 格式。
        /// </summary>
        public string Format { get; set; }


        /// <summary>
        /// 转换为字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
        {
            var pairs = new string[]
            {
                $"{nameof(Key)}:{Key}",
                $"{nameof(Format)}:{Format}",
                $"{nameof(Value)}:{Value}"
            };

            return string.Join(",", pairs);
        }


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回整数。</returns>
        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }


        /// <summary>
        /// 比较指定的对象是否与当前实例相等。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回是否相等的布尔值。</returns>
        public override bool Equals(object obj)
        {
            if (obj is FormattingDescriptor descriptor)
                return Equals(descriptor);

            return false;
        }
        /// <summary>
        /// 比较指定的 <see cref="FormattingDescriptor"/> 是否与当前实例相等。
        /// </summary>
        /// <param name="other">给定的 <see cref="FormattingDescriptor"/>。</param>
        /// <returns>返回是否相等的布尔值。</returns>
        public bool Equals(FormattingDescriptor other)
        {
            return this == other;
        }


        /// <summary>
        /// 比较两个 <see cref="FormattingDescriptor"/> 是否相等。
        /// </summary>
        /// <param name="a">给定的 <see cref="FormattingDescriptor"/>。</param>
        /// <param name="b">给定的 <see cref="FormattingDescriptor"/>。</param>
        /// <returns>返回是否相等的布尔值。</returns>
        public static bool operator ==(FormattingDescriptor a, FormattingDescriptor b)
        {
            return a.Key == b.Key;
        }

        /// <summary>
        /// 比较两个 <see cref="FormattingDescriptor"/> 是否不等。
        /// </summary>
        /// <param name="a">给定的 <see cref="FormattingDescriptor"/>。</param>
        /// <param name="b">给定的 <see cref="FormattingDescriptor"/>。</param>
        /// <returns>返回是否不等的布尔值。</returns>
        public static bool operator !=(FormattingDescriptor a, FormattingDescriptor b)
        {
            return !(a == b);
        }

    }
}
