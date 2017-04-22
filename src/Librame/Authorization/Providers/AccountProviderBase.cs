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
    using Data;
    using Descriptors;
    using Utility;

    /// <summary>
    /// 帐户管道。
    /// </summary>
    /// <remarks>
    /// 需重写实现自己的帐户认证逻辑。
    /// </remarks>
    public class AccountProviderBase : AbstractAdapterManagerReference, IAccountProvider
    {
        /// <summary>
        /// 构造一个 <see cref="AccountProviderBase"/> 实例。
        /// </summary>
        /// <param name="adapters">给定的适配器管理器。</param>
        public AccountProviderBase(IAdapterManager adapters)
            : base(adapters)
        {
        }


        /// <summary>
        /// 认证用户。
        /// </summary>
        /// <remarks>
        /// 接口默认支持用户名/电邮/身份证号/手机等验证方式。
        /// </remarks>
        /// <param name="name">给定的名称。</param>
        /// <param name="passwd">给定的密码。</param>
        /// <returns>返回 <see cref="AuthenticateInfo"/>。</returns>
        public virtual AuthenticateInfo Authenticate(string name, string passwd)
        {
            var error = AuthenticateStatus.Default;
            IAccountDescriptor account = null;

            if (ValidationHelper.IsEmail(name))
            {
                error = AuthenticateStatus.EmailError;
                account = SeekEmail(name);
            }
            else if (ValidationHelper.IsPhone(name))
            {
                error = AuthenticateStatus.PhoneError;
                account = SeekPhone(name);
            }
            else if (ValidationHelper.IsIdCard(name))
            {
                error = AuthenticateStatus.IdCardError;
                account = SeekIdCard(name);
            }
            else if (ReferenceEquals(account, null))
            {
                error = AuthenticateStatus.NameError;
                account = SeekName(name);
            }

            // 用户名正确
            if (!ReferenceEquals(account, null))
            {
                // 验证密码
                if (!string.IsNullOrEmpty(passwd))
                {
                    var passwdFactory = Adapters.AuthorizationAdapter.Passwd;

                    // 密码错误
                    if (!passwdFactory.Equals(account.Passwd, passwd))
                    {
                        error = AuthenticateStatus.PasswdError;
                        return new AuthenticateInfo(account, EnumUtility.GetDescription(error), error);
                    }
                }
                
                // 检测帐户状态
                if (account.Status != AccountStatus.Active)
                {
                    error = AuthenticateStatus.StatusError;
                    return new AuthenticateInfo(account, EnumUtility.GetDescription(error), error);
                }

                // 登录成功
                return new AuthenticateInfo(account, "登录成功", AuthenticateStatus.Success);
            }
            else
            {
                //account = new AccountDescriptor(string.Empty, string.Empty, AccountStatus.Default);
                return new AuthenticateInfo(account, "未知错误", AuthenticateStatus.Unknown);
            }
        }

        /// <summary>
        /// 认证用户。
        /// </summary>
        /// <param name="account">给定的帐户。</param>
        /// <returns>返回 <see cref="AuthenticateInfo"/>。</returns>
        public virtual AuthenticateInfo Authenticate(IAccountDescriptor account)
        {
            return new AuthenticateInfo(account, "需要重写此方法");
        }


        /// <summary>
        /// 检索电邮。
        /// </summary>
        /// <param name="email">给定的电邮。</param>
        /// <returns>返回 <see cref="IAccountDescriptor"/>。</returns>
        protected virtual IAccountDescriptor SeekEmail(string email)
        {
            return new AccountDescriptor(email, string.Empty, AccountStatus.Active);
        }

        /// <summary>
        /// 检索身份证号。
        /// </summary>
        /// <param name="idCard">给定的身份证号。</param>
        /// <returns>返回 <see cref="IAccountDescriptor"/>。</returns>
        protected virtual IAccountDescriptor SeekIdCard(string idCard)
        {
            return new AccountDescriptor(idCard, string.Empty, AccountStatus.Active);
        }

        /// <summary>
        /// 检索手机。
        /// </summary>
        /// <param name="phone">给定的手机。</param>
        /// <returns>返回 <see cref="IAccountDescriptor"/>。</returns>
        protected virtual IAccountDescriptor SeekPhone(string phone)
        {
            return new AccountDescriptor(phone, string.Empty, AccountStatus.Active);
        }

        /// <summary>
        /// 检索名称。
        /// </summary>
        /// <param name="name">给定的用户。</param>
        /// <returns>返回 <see cref="IAccountDescriptor"/>。</returns>
        protected virtual IAccountDescriptor SeekName(string name)
        {
            return new AccountDescriptor(name, string.Empty, AccountStatus.Active);
        }

    }
}
