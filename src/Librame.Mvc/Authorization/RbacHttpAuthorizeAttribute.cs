#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Web.Http.Controllers;

namespace Librame.Authorization
{
    /// <summary>
    /// RBAC HTTP 认证属性。
    /// </summary>
    public class RbacHttpAuthorizeAttribute : HttpAuthorizeAttribute
    {
        private readonly bool _enableRbac = false;
        private readonly bool _enableStatus = false;
        private readonly int _minRoleId = 0;

        /// <summary>
        /// 构造一个 <see cref="RbacHttpAuthorizeAttribute"/> 实例。
        /// </summary>
        /// <param name="enableRbac">启用 RBAC 认证（TRUE 表示启用每个动作的权限认证）。</param>
        /// <param name="enableStatus">启用状态认证（TRUE 表示启用用户状态异常的认证）。</param>
        /// <param name="minRoleId">给定的最小角色编号（大于 0 表示启用单独的角色认证；如是否为管理员 [2]）。</param>
        public RbacHttpAuthorizeAttribute(bool enableRbac = true,
            bool enableStatus = true, int minRoleId = 0)
            : base()
        {
            _enableRbac = enableRbac;
            _enableStatus = enableStatus;
            _minRoleId = minRoleId;
        }


        /// <summary>
        /// 开始认证。
        /// </summary>
        /// <param name="actionContext">给定的 HTTP 动作上下文。</param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            // 调用基础认证
            base.OnAuthorization(actionContext);

            // 用户名为空
            if (string.IsNullOrEmpty(CurrentUser?.Identity.Name))
            {
                base.HandleUnauthorizedRequest(actionContext);
                return;
            }
        }

    }
}
