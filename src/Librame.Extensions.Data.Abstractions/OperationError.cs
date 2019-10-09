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
using System.Globalization;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 操作错误。
    /// </summary>
    public class OperationError : IEquatable<OperationError>
    {
        /// <summary>
        /// 构造一个 <see cref="OperationError"/>。
        /// </summary>
        public OperationError()
        {
        }

        /// <summary>
        /// 构造一个 <see cref="OperationError"/>。
        /// </summary>
        /// <param name="code">给定的代码。</param>
        /// <param name="description">给定的描述。</param>
        public OperationError(string code, string description)
        {
            Code = code;
            Description = description;
        }


        /// <summary>
        /// 代码。
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 描述。
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 异常。
        /// </summary>
        public Exception Exception { get; set; }


        /// <summary>
        /// 转换为操作错误。
        /// </summary>
        /// <param name="exception">给定的异常。</param>
        /// <returns>返回 <see cref="OperationError"/>。</returns>
        public static OperationError ToError(Exception exception)
        {
            exception.NotNull(nameof(exception));

            return new OperationError
            {
                Code = exception.HResult.ToString(CultureInfo.CurrentCulture),
                Description = exception.AsInnerMessage(),
                Exception = exception
            };
        }


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="other">给定的 <see cref="OperationError"/>。</param>
        /// <returns>返回布尔值。</returns>
        public bool Equals(OperationError other)
            => Code == other?.Code;

        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
            => (obj is OperationError other) ? Equals(other) : false;


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回 32 位整数。</returns>
        public override int GetHashCode()
            => Code.GetHashCode(StringComparison.OrdinalIgnoreCase);


        /// <summary>
        /// 转换为文件。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
            => $"{Code}: {Description}";


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="a">给定的 <see cref="OperationError"/>。</param>
        /// <param name="b">给定的 <see cref="OperationError"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator ==(OperationError a, OperationError b)
            => (a?.Equals(b)).Value;

        /// <summary>
        /// 是否不等。
        /// </summary>
        /// <param name="a">给定的 <see cref="OperationError"/>。</param>
        /// <param name="b">给定的 <see cref="OperationError"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator !=(OperationError a, OperationError b)
            => !(a?.Equals(b)).Value;
    }
}
