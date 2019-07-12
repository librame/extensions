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
        /// <returns>返回字符串。</returns>
        public virtual string ToHexString()
            => Buffer.AsHexString();


        /// <summary>
        /// 转换为 BASE64 字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
            => Buffer.AsBase64String();


        /// <summary>
        /// 隐式转换为字符串。
        /// </summary>
        /// <param name="identifier">给定的 <see cref="AbstractIdentifier"/>。</param>
        public static implicit operator string(AbstractIdentifier identifier)
            => identifier?.ToString();
    }
}
