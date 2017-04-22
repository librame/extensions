#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Web.Mvc;
using System.Web.Routing;

namespace Librame.Authorization
{
    using Data;

    /// <summary>
    /// RBAC MVC 认证属性。
    /// </summary>
    public class RbacMvcAuthorizeAttribute : MvcAuthorizeAttribute
    {
        private readonly bool _enableRbac = false;
        private readonly bool _enableStatus = false;
        private readonly int _minRoleId = 0;

        /// <summary>
        /// 构造一个 <see cref="RbacMvcAuthorizeAttribute"/> 实例。
        /// </summary>
        /// <param name="enableRbac">启用 RBAC 认证（TRUE 表示启用每个动作的权限认证）。</param>
        /// <param name="enableStatus">启用状态认证（TRUE 表示启用用户状态异常的认证）。</param>
        /// <param name="minRoleId">给定的最小角色编号（大于 0 表示启用单独的角色认证；如是否为管理员 [2]）。</param>
        public RbacMvcAuthorizeAttribute(bool enableRbac = true,
            bool enableStatus = true, int minRoleId = 0)
            : base()
        {
            _enableRbac = enableRbac;
            _enableStatus = enableStatus;
            _minRoleId = minRoleId;
        }


        /// <summary>
        /// 获取或设置用户登陆路由描述符。
        /// </summary>
        public RouteDescriptor UserLogin { get; set; }

        /// <summary>
        /// 获取或设置用户注册路由描述符。
        /// </summary>
        public RouteDescriptor UserRegister { get; set; }

        /// <summary>
        /// 获取或设置用户状态路由描述符。
        /// </summary>
        public RouteDescriptor UserStatus { get; set; }

        /// <summary>
        /// 获取或设置用户拒绝路由描述符。
        /// </summary>
        public RouteDescriptor UserDeny { get; set; }


        /// <summary>
        /// 开始认证。
        /// </summary>
        /// <param name="filterContext">给定的认证上下文。</param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            // 调用基础认证
            base.OnAuthorization(filterContext);

            // 用户名为空
            if (string.IsNullOrEmpty(CurrentUser?.Identity.Name))
            {
                if (ReferenceEquals(UserLogin, null))
                {
                    UserLogin = new RouteDescriptor()
                    {
                        ActionName = "Login",
                        ControllerName = "User",
                        AreaName = string.Empty
                    };
                }

                // 绑定重写向用户登陆路由结果
                filterContext.Result = UserLogin.RedirectToRoute();
                return;
            }

            // 引入认证相关管道集合
            //var providers = Archit.Can.Adapters.AuthorizationAdapter.ProvCollection;

            //// 取得当前用户信息
            //var account = providers.
            //CurrentAccount = providers.Get(p => p.Name == name);

            //// 如果用户信息不存在
            //if (ReferenceEquals(CurrentAccount, null))
            //{
            //    filterContext.RedirectToUserRegister();
            //    return;
            //}

            //// 如需附加状态认证
            //if (EnableStatusAuthorize && CurrentAccount.Status != AccountStatus.Active)
            //{
            //    filterContext.RedirectToUserStatus();
            //    return;
            //}

            //// 如果不具备管理权限
            //if (!ServiceHelper.IsAdminPermission(CurrentAccount.AppId, CurrentAccount.Id, NeedRoleId))
            //{
            //    filterContext.RedirectToUserDeny();
            //    return;
            //}
        }

    }
}
