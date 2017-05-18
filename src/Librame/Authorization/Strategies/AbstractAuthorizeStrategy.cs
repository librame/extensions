#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Security.Principal;

namespace Librame.Authorization.Strategies
{
    using Descriptors;
    using Utility;

    /// <summary>
    /// 抽象认证策略。
    /// </summary>
    public abstract class AbstractAuthorizeStrategy : LibrameBase<AbstractAuthorizeStrategy>, IAuthorizeStrategy
    {
        /// <summary>
        /// 获取认证适配器接口。
        /// </summary>
        public IAuthorizeAdapter Authorize { get; }


        /// <summary>
        /// 构造一个抽象认证策略实例。
        /// </summary>
        /// <param name="authorize">给定的认证适配器接口。</param>
        public AbstractAuthorizeStrategy(IAuthorizeAdapter authorize)
        {
            Authorize = authorize.NotNull(nameof(authorize));
        }

        /// <summary>
        /// 获取模拟用户认证信息。
        /// </summary>
        /// <returns>返回认证信息。</returns>
        protected virtual AuthenticateInfo GetSimulateAuthenticateInfo()
        {
            var account = new AccountDescriptor("VirtualUser");

            return new AuthenticateInfo(account, "模拟用户认证（通常为禁用认证功能后发生）");
        }


        #region Authentication

        /// <summary>
        /// 是否已认证。
        /// </summary>
        /// <param name="principalFactory">给定取得用户的方法。</param>
        /// <returns>返回是否已认证的布尔值。</returns>
        public virtual bool IsAuthenticated(Func<string, IPrincipal> principalFactory)
        {
            try
            {
                if (!Authorize.AuthSettings.EnableAuthorize)
                    return true; // 禁用认证，默认模拟已认证

                principalFactory.NotNull(nameof(principalFactory));

                // 解析当前用户
                var principal = principalFactory.Invoke(AuthorizeHelper.AUTHORIZATION_KEY);

                return (!ReferenceEquals(principal, null) && principal.Identity.IsAuthenticated);
            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerMessage(), ex);

                return false;
            }
        }
        
        /// <summary>
        /// 开始认证。
        /// </summary>
        /// <param name="principalFactory">给定取得用户的方法。</param>
        public virtual void OnAuthentication(Func<string, IPrincipal> principalFactory)
        {
            // 如果认证未通过
            if (!IsAuthenticated(principalFactory))
            {
                try
                {
                    OnAuthenticationFailed();
                }
                catch (Exception ex)
                {
                    Log.Error(ex.InnerMessage(), ex);
                }
            }
        }

        /// <summary>
        /// 认证失败。
        /// </summary>
        protected abstract void OnAuthenticationFailed();

        #endregion


        #region Sign

        /// <summary>
        /// 用户登入。
        /// </summary>
        /// <param name="name">给定的用户名。</param>
        /// <param name="passwd">给定的密码。</param>
        /// <param name="isPersistent">是否持久化存储。</param>
        /// <param name="bindPrincipal">用于绑定键名与用户的方法。</param>
        /// <returns>返回认证信息。</returns>
        public virtual AuthenticateInfo SignIn(string name, string passwd, bool isPersistent,
            Action<string, IPrincipal> bindPrincipal)
        {
            try
            {
                if (!Authorize.AuthSettings.EnableAuthorize)
                {
                    // 禁用认证，默认模拟已认证用户
                    return GetSimulateAuthenticateInfo();
                }

                // 认证用户
                var authInfo = Authorize.Providers.Account.Authenticate(name, passwd);

                // 认证失败
                if (ReferenceEquals(authInfo.Account, null) || authInfo.Status != AuthenticateStatus.Success)
                    return authInfo;

                // 创建票根
                var token = Authorize.Managers.Token.Generate();
                var ticket = new AuthenticateTicket(authInfo.Account, token, DateTime.Now, isPersistent,
                    now => now.AddDays(Authorize.AuthSettings.ExpirationDays));

                // 绑定用户
                var identity = new AccountIdentity(ticket);
                var principal = new AccountPrincipal(identity, null);
                bindPrincipal?.Invoke(AuthorizeHelper.AUTHORIZATION_KEY, principal);

                // 认证成功
                SignInSuccess(authInfo, principal);

                return authInfo;
            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerMessage(), ex);

                return null;
            }
        }

        /// <summary>
        /// 登录成功。
        /// </summary>
        /// <param name="authInfo">给定的认证信息。</param>
        /// <param name="principal">给定的帐户用户。</param>
        protected abstract void SignInSuccess(AuthenticateInfo authInfo, AccountPrincipal principal);


        /// <summary>
        /// 用户登出。
        /// </summary>
        /// <param name="removePrincipal">移除用户方法。</param>
        /// <returns>返回用户票根。</returns>
        public abstract AuthenticateTicket SignOut(Func<string, AuthenticateTicket> removePrincipal);

        #endregion

    }
}
