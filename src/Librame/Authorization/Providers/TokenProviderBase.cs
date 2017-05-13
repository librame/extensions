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
    using Utility;

    /// <summary>
    /// 令牌管道基类。
    /// </summary>
    public class TokenProviderBase : ITokenProvider
    {
        /// <summary>
        /// 构造一个令牌管道基类实例。
        /// </summary>
        /// <param name="cryptogram">给定的密文管理器。</param>
        public TokenProviderBase(ICryptogramManager cryptogram)
        {
            Cryptogram = cryptogram.NotNull(nameof(cryptogram));
        }


        /// <summary>
        /// 密文管理器接口。
        /// </summary>
        public ICryptogramManager Cryptogram { get; }


        /// <summary>
        /// 认证令牌。
        /// </summary>
        /// <param name="token">给定的令牌。</param>
        /// <param name="ticket">给定的票根。</param>
        /// <returns>返回 <see cref="ITokenDescriptor"/>。</returns>
        public virtual ITokenDescriptor Authenticate(string token, string ticket)
        {
            return Authenticate(new TokenDescriptor(token, ticket));
        }

        /// <summary>
        /// 认证令牌。
        /// </summary>
        /// <param name="token">给定的令牌。</param>
        /// <returns>返回 <see cref="ITokenDescriptor"/>。</returns>
        public virtual ITokenDescriptor Authenticate(ITokenDescriptor token)
        {
            return token.NotNull(nameof(token));
        }


        /// <summary>
        /// 创建令牌。
        /// </summary>
        /// <param name="token">给定的令牌。</param>
        /// <param name="ticket">给定的票根。</param>
        /// <returns>返回 <see cref="ITokenDescriptor"/>。</returns>
        public virtual ITokenDescriptor Create(string token, string ticket)
        {
            return Create(new TokenDescriptor(token, ticket));
        }

        /// <summary>
        /// 创建令牌。
        /// </summary>
        /// <param name="token">给定的令牌。</param>
        /// <returns>返回 <see cref="ITokenDescriptor"/>。</returns>
        public virtual ITokenDescriptor Create(ITokenDescriptor token)
        {
            return token.NotNull(nameof(token));
        }

    }
}
