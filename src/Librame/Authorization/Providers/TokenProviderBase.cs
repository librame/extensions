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
    using Adaptation;
    using Descriptors;

    /// <summary>
    /// 令牌管道基类。
    /// </summary>
    public class TokenProviderBase : AbstractAdapterManagerReference, ITokenProvider
    {
        /// <summary>
        /// 构造一个 <see cref="TokenProviderBase"/> 实例。
        /// </summary>
        /// <param name="adapters">给定的适配器管理器。</param>
        public TokenProviderBase(IAdapterManager adapters)
            : base(adapters)
        {
        }


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
            return token;
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
            return token;
        }

    }
}
