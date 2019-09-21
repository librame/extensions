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
    /// 实体错误。
    /// </summary>
    public class EntityError : IEquatable<EntityError>
    {
        /// <summary>
        /// 构造一个 <see cref="EntityError"/> 默认实例。
        /// </summary>
        public EntityError()
        {
        }

        /// <summary>
        /// 构造一个 <see cref="EntityError"/>。
        /// </summary>
        /// <param name="code">给定的代码。</param>
        /// <param name="description">给定的描述。</param>
        public EntityError(string code, string description)
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
        /// 转换为实体错误。
        /// </summary>
        /// <param name="exception">给定的 <see cref="Exception"/>。</param>
        /// <returns>返回 <see cref="EntityError"/>。</returns>
        public static EntityError ToError(Exception exception)
        {
            exception.NotNull(nameof(exception));

            return new EntityError
            {
                Code = exception.HResult.ToString(),
                Description = exception.AsInnerMessage()
            };
        }


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="other">给定的 <see cref="EntityError"/>。</param>
        /// <returns>返回布尔值。</returns>
        public bool Equals(EntityError other)
            => Code == other?.Code;

        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
            => (obj is EntityError other) ? Equals(other) : false;


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回 32 位整数。</returns>
        public override int GetHashCode()
            => Code.GetHashCode();


        /// <summary>
        /// 转换为文件。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
            => $"{Code}: {Description}";


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="a">给定的 <see cref="EntityError"/>。</param>
        /// <param name="b">给定的 <see cref="EntityError"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator ==(EntityError a, EntityError b)
            => (a?.Equals(b)).Value;

        /// <summary>
        /// 是否不等。
        /// </summary>
        /// <param name="a">给定的 <see cref="EntityError"/>。</param>
        /// <param name="b">给定的 <see cref="EntityError"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator !=(EntityError a, EntityError b)
            => !(a?.Equals(b)).Value;
    }
}
