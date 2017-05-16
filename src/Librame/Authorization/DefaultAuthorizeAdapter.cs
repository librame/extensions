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
        /// 获取或设置管理器集合。
        /// </summary>
        public IAuthorizeManagerCollection Managers { get; set; }

        /// <summary>
        /// 获取或设置管道集合。
        /// </summary>
        public IAuthorizeProviderCollection Providers { get; set; }

        
        /// <summary>
        /// 获取认证策略。
        /// </summary>
        public virtual IAuthorizeStrategy Strategy
        {
            get
            {
                return SingletonManager.Regist<IAuthorizeStrategy>(key =>
                {
                    if (AuthSettings.EnableSso && AuthSettings.IsSsoServerMode)
                    {
                        return new SsoServerFormsAuthorizeStrategy(this);

                        // SSO 客户端只能通过 WEBAPI 进行远程数据查询
                        //return new SsoClientFormsAuthorizeStrategy(this);
                    }
                    else
                    {
                        return new FormsAuthorizeStrategy(this);
                    }
                });
            }
        }

    }
}
