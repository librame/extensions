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
    using Utility;

    /// <summary>
    /// 默认认证适配器。
    /// </summary>
    public class DefaultAuthorizeAdapter : AbstractAuthorizeAdapter, IAuthorizeAdapter
    {
        /// <summary>
        /// 获取或设置认证首选项。
        /// </summary>
        public AuthorizeSettings AuthSettings { get; set; }

        /// <summary>
        /// 获取或设置管道集合。
        /// </summary>
        public IProviderCollection ProvCollection { get; set; }


        /// <summary>
        /// 获取密码管理器。
        /// </summary>
        public virtual IPasswdManager Passwd
        {
            get { return SingletonManager.Resolve<IPasswdManager>(key => new PasswdManager()); }
        }

        
        /// <summary>
        /// 获取认证策略。
        /// </summary>
        public virtual IAuthorizeStrategy Strategy
        {
            get
            {
                return SingletonManager.Regist<IAuthorizeStrategy>(key =>
                {
                    if (AuthSettings.EnableSso)
                    {
                        if (AuthSettings.IsSsoServerMode)
                            return new SsoServerFormsAuthorizeStrategy(AuthSettings, ProvCollection);

                        return new SsoClientFormsAuthorizeStrategy(AuthSettings, ProvCollection);
                    }
                    else
                    {
                        return new FormsAuthorizeStrategy(AuthSettings, ProvCollection);
                    }
                });
            }
        }

    }
}
