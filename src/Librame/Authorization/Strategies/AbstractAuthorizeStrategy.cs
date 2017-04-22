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
        /// 获取认证首选项。
        /// </summary>
        public AuthorizeSettings AuthSettings { get; }

        /// <summary>
        /// 获取管道集合。
        /// </summary>
        public IProviderCollection ProvCollection { get; }


        /// <summary>
        /// 构造一个 <see cref="AbstractAuthorizeStrategy"/> 实例。
        /// </summary>
        /// <param name="authSettings">给定的认证首选项。</param>
        /// <param name="provCollection">给定的管道集合。</param>
        public AbstractAuthorizeStrategy(AuthorizeSettings authSettings,
            IProviderCollection provCollection)
        {
            AuthSettings = authSettings.NotNull(nameof(authSettings));
            ProvCollection = provCollection.NotNull(nameof(provCollection));
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
                if (!AuthSettings.EnableAuthorize)
                    return true; // 禁用认证，默认模拟已认证

                principalFactory.GuardNull(nameof(principalFactory));

                // 解析当前用户
                var principal = principalFactory.Invoke(AuthorizeHelper.AUTHORIZATION_KEY);

                return (!ReferenceEquals(principal, null) && principal.Identity.IsAuthenticated);
            }
            catch (Exception ex)
            {
                Log.Error(ex.AsOrInnerMessage(), ex);

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
                    Log.Error(ex.AsOrInnerMessage(), ex);
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
                if (!AuthSettings.EnableAuthorize)
                {
                    // 禁用认证，默认模拟已认证用户
                    var account = new AccountDescriptor("VirtualUser", string.Empty, AccountStatus.Active);
                    return new AuthenticateInfo(account, "当前已禁用认证，默认使用模拟认证用户");
                }

                // 认证用户
                var authInfo = ProvCollection.Account.Authenticate(name, passwd);

                // 认证失败
                if (ReferenceEquals(authInfo.Account, null) || authInfo.Status != AuthenticateStatus.Success)
                    return authInfo;

                // 认证成功
                SignInSuccess(authInfo, isPersistent, bindPrincipal);

                return authInfo;
            }
            catch (Exception ex)
            {
                Log.Error(ex.AsOrInnerMessage(), ex);

                return null;
            }
        }

        /// <summary>
        /// 登录成功。
        /// </summary>
        /// <param name="authInfo">给定的认证信息。</param>
        /// <param name="isPersistent">是否持久化存储。</param>
        /// <param name="bindPrincipal">用于绑定键名与用户的方法。</param>
        protected abstract void SignInSuccess(AuthenticateInfo authInfo, bool isPersistent,
            Action<string, IPrincipal> bindPrincipal);


        /// <summary>
        /// 用户登出。
        /// </summary>
        /// <param name="removePrincipal">移除用户方法。</param>
        public abstract void SignOut(Action<string> removePrincipal);

        #endregion

    }
}
