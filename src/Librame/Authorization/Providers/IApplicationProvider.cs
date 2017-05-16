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
    /// 应用管道接口。
    /// </summary>
    public interface IApplicationProvider
    {
        /// <summary>
        /// 密文管理器接口。
        /// </summary>
        ICiphertextManager Ciphertext { get; }


        /// <summary>
        /// 认证系统。
        /// </summary>
        /// <param name="authId">给定的授权编号。</param>
        /// <returns>返回 <see cref="IApplicationDescriptor"/>。</returns>
        IApplicationDescriptor Authenticate(string authId);

        /// <summary>
        /// 认证系统。
        /// </summary>
        /// <param name="application">给定的应用。</param>
        /// <returns>返回 <see cref="IApplicationDescriptor"/>。</returns>
        IApplicationDescriptor Authenticate(IApplicationDescriptor application);
    }
}
