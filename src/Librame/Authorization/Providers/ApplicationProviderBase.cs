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
    /// 应用管道基类。
    /// </summary>
    /// <remarks>
    /// 需重写实现自己的系统应用认证逻辑。
    /// </remarks>
    public class ApplicationProviderBase : IApplicationProvider
    {
        /// <summary>
        /// 构造一个应用管道基类实例。
        /// </summary>
        /// <param name="ciphertext">给定的密文管理器。</param>
        public ApplicationProviderBase(ICiphertextManager ciphertext)
        {
            Ciphertext = ciphertext.NotNull(nameof(ciphertext));
        }


        /// <summary>
        /// 密文管理器接口。
        /// </summary>
        public ICiphertextManager Ciphertext { get; }


        /// <summary>
        /// 认证系统。
        /// </summary>
        /// <param name="authId">给定的授权编号。</param>
        /// <returns>返回 <see cref="IApplicationDescriptor"/>。</returns>
        public virtual IApplicationDescriptor Authenticate(string authId)
        {
            return Authenticate(new ApplicationDescriptor(authId));
        }

        /// <summary>
        /// 认证系统。
        /// </summary>
        /// <param name="application">给定的应用。</param>
        /// <returns>返回 <see cref="IApplicationDescriptor"/>。</returns>
        public virtual IApplicationDescriptor Authenticate(IApplicationDescriptor application)
        {
            return application.NotNull(nameof(application));
        }

    }
}
