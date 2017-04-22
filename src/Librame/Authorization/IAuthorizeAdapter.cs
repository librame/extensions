#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Authorization
{
    using Strategies;

    /// <summary>
    /// 认证适配器接口。
    /// </summary>
    public interface IAuthorizeAdapter : Adaptation.IAdapter
    {
        /// <summary>
        /// 获取或设置认证首选项。
        /// </summary>
        AuthorizeSettings AuthSettings { get; set; }

        /// <summary>
        /// 获取或设置管道集合。
        /// </summary>
        IProviderCollection ProvCollection { get; set; }

        /// <summary>
        /// 获取密码管理器。
        /// </summary>
        IPasswdManager Passwd { get; }
        
        /// <summary>
        /// 获取认证策略。
        /// </summary>
        IAuthorizeStrategy Strategy { get; }
    }
}
