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
    /// 帐户管道接口。
    /// </summary>
    public interface IAccountProvider : IAdapterManagerReference
    {
        /// <summary>
        /// 认证用户。
        /// </summary>
        /// <remarks>
        /// 接口默认支持用户名/电邮/身份证号/手机等验证方式。
        /// </remarks>
        /// <param name="name">给定的名称。</param>
        /// <param name="passwd">给定的密码。</param>
        /// <returns>返回 <see cref="AuthenticateInfo"/>。</returns>
        AuthenticateInfo Authenticate(string name, string passwd);

        /// <summary>
        /// 认证用户。
        /// </summary>
        /// <param name="account">给定的帐户。</param>
        /// <returns>返回 <see cref="AuthenticateInfo"/>。</returns>
        AuthenticateInfo Authenticate(IAccountDescriptor account);
    }
}
