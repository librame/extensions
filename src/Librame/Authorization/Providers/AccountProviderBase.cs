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
    using Data;
    using Descriptors;
    using Managers;
    using Utility;

    /// <summary>
    /// 帐户管道基类。
    /// </summary>
    /// <remarks>
    /// 需重写实现自己的帐户认证逻辑。
    /// </remarks>
    public class AccountProviderBase : IAccountProvider
    {
        /// <summary>
        /// 构造一个帐户管道基类实例。
        /// </summary>
        /// <param name="passwdManager">给定的密码管理器。</param>
        public AccountProviderBase(IPasswdManager passwdManager)
        {
            PasswdManager = passwdManager.NotNull(nameof(passwdManager));
        }


        /// <summary>
        /// 密码管理器接口。
        /// </summary>
        public IPasswdManager PasswdManager { get; }


        /// <summary>
        /// 验证密码是否相同。
        /// </summary>
        /// <param name="left">给定左边的密码。</param>
        /// <param name="right">给定右边的密码。</param>
        /// <returns>返回布尔值。</returns>
        protected virtual bool PasswdEquals(string left, string right)
        {
            // 原始密码对比（未编码）
            return PasswdManager.RawEquals(left, right);
        }


        /// <summary>
        /// 认证用户。
        /// </summary>
        /// <remarks>
        /// 接口默认支持用户名/电邮/身份证号/手机等验证方式。
        /// </remarks>
        /// <param name="name">给定的名称。</param>
        /// <param name="passwd">给定的密码。</param>
        /// <returns>返回认证信息。</returns>
        public virtual AuthenticateInfo Authenticate(string name, string passwd)
        {
            name.NotEmpty(nameof(name));

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
                    // 密码验证失败
                    if (!PasswdEquals(account.Passwd, passwd))
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

                // 创建帐户镜像
                var accountMirror = (IAccountDescriptor)TypeUtility.CopyToCreate((object)account);

                // 清空密码
                accountMirror.ResetPasswd();

                // 登录成功
                return new AuthenticateInfo(accountMirror, "登录成功", AuthenticateStatus.Success);
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
        /// <returns>返回认证信息。</returns>
        public virtual AuthenticateInfo Authenticate(IAccountDescriptor account)
        {
            return new AuthenticateInfo(account, "需要重写此方法");
        }


        /// <summary>
        /// 检索电邮。
        /// </summary>
        /// <param name="email">给定的电邮。</param>
        /// <returns>返回帐户描述符。</returns>
        protected virtual IAccountDescriptor SeekEmail(string email)
        {
            return new AccountDescriptor(email);
        }

        /// <summary>
        /// 检索身份证号。
        /// </summary>
        /// <param name="idCard">给定的身份证号。</param>
        /// <returns>返回帐户描述符。</returns>
        protected virtual IAccountDescriptor SeekIdCard(string idCard)
        {
            return new AccountDescriptor(idCard);
        }

        /// <summary>
        /// 检索手机。
        /// </summary>
        /// <param name="phone">给定的手机。</param>
        /// <returns>返回帐户描述符。</returns>
        protected virtual IAccountDescriptor SeekPhone(string phone)
        {
            return new AccountDescriptor(phone);
        }

        /// <summary>
        /// 检索名称。
        /// </summary>
        /// <param name="name">给定的用户。</param>
        /// <returns>返回帐户描述符。</returns>
        protected virtual IAccountDescriptor SeekName(string name)
        {
            return new AccountDescriptor(name);
        }

    }
}
