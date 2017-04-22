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
    /// 应用管道基类。
    /// </summary>
    /// <remarks>
    /// 需重写实现自己的系统应用认证逻辑。
    /// </remarks>
    public class ApplicationProviderBase : AbstractAdapterManagerReference, IApplicationProvider
    {
        /// <summary>
        /// 构造一个 <see cref="ApplicationProviderBase"/> 实例。
        /// </summary>
        /// <param name="adapters">给定的适配器管理器。</param>
        public ApplicationProviderBase(IAdapterManager adapters)
            : base(adapters)
        {
        }


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
            return application;
        }

    }
}
