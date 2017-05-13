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
    using Descriptors;
    using Managers;

    /// <summary>
    /// 权限管道接口。
    /// </summary>
    public interface IPermissionProvider
    {
        /// <summary>
        /// 密文管理器接口。
        /// </summary>
        ICryptogramManager Cryptogram { get; }


        /// <summary>
        /// 认证权限。
        /// </summary>
        /// <param name="actionName">给定的动作名。</param>
        /// <param name="controllerName">给定的控制器名。</param>
        /// <param name="areaName">给定的区域名。</param>
        /// <returns>返回 <see cref="IPermissionDescriptor"/>。</returns>
        IPermissionDescriptor Authenticate(string actionName, string controllerName, string areaName);

        /// <summary>
        /// 认证权限。
        /// </summary>
        /// <param name="permission">给定的权限。</param>
        /// <returns>返回 <see cref="IPermissionDescriptor"/>。</returns>
        IPermissionDescriptor Authenticate(IPermissionDescriptor permission);
    }
}
