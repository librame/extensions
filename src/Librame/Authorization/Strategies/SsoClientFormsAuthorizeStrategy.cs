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
using System.Web;

namespace Librame.Authorization.Strategies
{
    using Descriptors;
    using Utility;

    ///// <summary>
    ///// SSO 窗体客户端应用程序认证策略。
    ///// </summary>
    //public class SsoClientFormsAuthorizeStrategy : FormsAuthorizeStrategy, IAuthorizeStrategy
    //{
    //    /// <summary>
    //    /// 构造一个 SSO 窗体客户端应用程序认证策略实例。
    //    /// </summary>
    //    /// <param name="authorize">给定的认证适配器接口。</param>
    //    public SsoClientFormsAuthorizeStrategy(IAuthorizeAdapter authorize)
    //        : base(authorize)
    //    {
    //    }


    //    #region Authentication

    //    /// <summary>
    //    /// 认证失败。
    //    /// </summary>
    //    protected override void OnAuthenticationFailed()
    //    {
    //        // 加密授权编号
    //        var encryptAuthId = Authorize.Managers.Ciphertext.Encode(Authorize.Settings.AuthId);

    //        var serverSignInUrl = AuthorizeHelper.FormatServerSignInUrl(encryptAuthId,
    //            Authorize.AuthSettings.SsoSignInRespondUrl,
    //            Authorize.AuthSettings.SsoServerSignInUrl);
            
    //        // 客户端模式则需重定向到认证服务器
    //        HttpContext.Current?.Response.Redirect(serverSignInUrl);
    //    }

    //    #endregion


    //    #region Sign

    //    /// <summary>
    //    /// 用户登入。
    //    /// </summary>
    //    /// <param name="name">给定的用户名（此参数无效）。</param>
    //    /// <param name="passwd">给定的密码（此参数无效）。</param>
    //    /// <param name="isPersistent">是否持久化存储（此参数无效）。</param>
    //    /// <param name="bindPrincipal">用于绑定键名与用户的方法（此参数有效）。</param>
    //    /// <returns>返回认证信息。</returns>
    //    public override AuthenticateInfo SignIn(string name, string passwd, bool isPersistent,
    //        Action<string, IPrincipal> bindPrincipal)
    //    {
    //        bindPrincipal.NotNull(nameof(bindPrincipal));

    //        try
    //        {
    //            IAccountDescriptor account = null;

    //            if (!Authorize.AuthSettings.EnableAuthorize)
    //            {
    //                // 禁用认证，默认模拟已认证用户
    //                account = new AccountDescriptor("VirtualUser");
    //                return new AuthenticateInfo(account, "当前已禁用认证，默认使用模拟认证用户");
    //            }

    //            // 解析认证服务器应答链接参数
    //            var request = HttpContext.Current?.Request;

    //            // 解析用户令牌
    //            var encryptToken = AuthorizeHelper.ResolveEncryptToken(request);
    //            var token = Authorize.Managers.Ciphertext.Decode(encryptToken);

    //            // 检索令牌，得到票根
    //            var tokenDescriptor = Authorize.Adapters.Authorization
    //                .Providers.Token.Authenticate(token);
    //            tokenDescriptor.NotNull(nameof(tokenDescriptor));

    //            // 还原票根
    //            var ticket = Authorize.Managers.Storage.FromString<AuthenticateTicket>(tokenDescriptor.Ticket);
    //            var identity = new AccountIdentity(ticket.NotNull(nameof(ticket)));
    //            var principal = new AccountPrincipal(identity, null);
    //            bindPrincipal?.Invoke(AuthorizeHelper.AUTHORIZATION_KEY, principal);

    //            // 模拟帐户实例
    //            account = new AccountDescriptor(identity.Name);
    //            var authInfo = new AuthenticateInfo(account, "服务端认证成功");

    //            // 认证成功
    //            SignInSuccess(authInfo, principal);

    //            return authInfo;
    //        }
    //        catch (Exception ex)
    //        {
    //            Log.Error(ex.InnerMessage(), ex);

    //            return null;
    //        }
    //    }


    //    /// <summary>
    //    /// 用户登出。
    //    /// </summary>
    //    /// <param name="removePrincipal">移除用户方法。</param>
    //    /// <return>返回用户票根。</return>
    //    public override AuthenticateTicket SignOut(Func<string, AuthenticateTicket> removePrincipal)
    //    {
    //        var ticket = base.SignOut(removePrincipal);

    //        var encryptToken = Authorize.Managers.Ciphertext.Encode(ticket.Token);
    //        var encryptAuthId = Authorize.Managers.Ciphertext.Encode(Authorize.Settings.AuthId);
            
    //        var signOutUrl = AuthorizeHelper.FormatServerSignOutUrl(encryptToken, encryptAuthId,
    //            Authorize.AuthSettings.SsoSignInRespondUrl,
    //            Authorize.AuthSettings.SsoServerSignInUrl);

    //        HttpContext.Current?.Response?.Redirect(signOutUrl);

    //        return ticket;
    //    }

    //    #endregion

    //}
}
