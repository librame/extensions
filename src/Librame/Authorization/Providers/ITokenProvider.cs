#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Authorization.Providers
{
    using Descriptors;
    using Managers;

    /// <summary>
    /// 令牌管道接口。
    /// </summary>
    public interface ITokenProvider
    {
        /// <summary>
        /// 密文管理器接口。
        /// </summary>
        ICryptogramManager Cryptogram { get; }


        /// <summary>
        /// 认证令牌。
        /// </summary>
        /// <param name="token">给定的令牌。</param>
        /// <param name="ticket">给定的票根。</param>
        /// <returns>返回 <see cref="ITokenDescriptor"/>。</returns>
        ITokenDescriptor Authenticate(string token, string ticket);

        /// <summary>
        /// 认证令牌。
        /// </summary>
        /// <param name="token">给定的令牌。</param>
        /// <returns>返回 <see cref="ITokenDescriptor"/>。</returns>
        ITokenDescriptor Authenticate(ITokenDescriptor token);


        /// <summary>
        /// 创建令牌。
        /// </summary>
        /// <param name="token">给定的令牌。</param>
        /// <param name="ticket">给定的票根。</param>
        /// <returns>返回 <see cref="ITokenDescriptor"/>。</returns>
        ITokenDescriptor Create(string token, string ticket);

        /// <summary>
        /// 创建令牌。
        /// </summary>
        /// <param name="token">给定的令牌。</param>
        /// <returns>返回 <see cref="ITokenDescriptor"/>。</returns>
        ITokenDescriptor Create(ITokenDescriptor token);
    }
}
