#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 抽象标识符。
    /// </summary>
    public abstract class AbstractIdentifier
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractIdentifier"/> 实例。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        public AbstractIdentifier(byte[] buffer)
        {
            Buffer = buffer.NotNullOrEmpty(nameof(buffer));
        }


        /// <summary>
        /// 字节数组。
        /// </summary>
        public byte[] Buffer { get; }


        /// <summary>
        /// 转换为 BASE32 字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public virtual string ToBase32String()
            => Buffer.AsBase32String();


        /// <summary>
        /// 转换为 16 进制字符串。
        /// </summary>
        /// <returns></returns>
        public virtual string ToHexString()
            => Buffer.AsHexString();


        #region Overrrides

        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
        {
            if (obj is AbstractIdentifier other)
                return Equals(other);

            return false;
        }


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回整数。</returns>
        public override int GetHashCode()
            => ToString().GetHashCode();

        #endregion


        #region Compares

        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="other">给定的 <see cref="AbstractIdentifier"/>。</param>
        /// <returns>返回布尔值。</returns>
        public virtual bool Equals(AbstractIdentifier other)
            => this == other;


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="a">给定的 <see cref="AbstractIdentifier"/>。</param>
        /// <param name="b">给定的 <see cref="AbstractIdentifier"/>。</param>
        /// <returns>返回是否相等的布尔值。</returns>
        public static bool operator ==(AbstractIdentifier a, AbstractIdentifier b)
            => a.ToString() == b.ToString();

        /// <summary>
        /// 是否不等。
        /// </summary>
        /// <param name="a">给定的 <see cref="AbstractIdentifier"/>。</param>
        /// <param name="b">给定的 <see cref="AbstractIdentifier"/>。</param>
        /// <returns>返回是否不等的布尔值。</returns>
        public static bool operator !=(AbstractIdentifier a, AbstractIdentifier b)
            => !(a == b);

        #endregion


        /// <summary>
        /// 隐式转换为字符串。
        /// </summary>
        /// <param name="identifier">给定的 <see cref="AbstractIdentifier"/>。</param>
        public static implicit operator string(AbstractIdentifier identifier)
            => identifier?.ToString();
    }
}
