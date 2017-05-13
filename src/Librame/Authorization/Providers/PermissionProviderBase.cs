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
    using Utility;

    /// <summary>
    /// 权限管道基类。
    /// </summary>
    /// <remarks>
    /// 需重写实现自己的系统应用认证逻辑。
    /// </remarks>
    public class PermissionProviderBase : IPermissionProvider
    {
        /// <summary>
        /// 构造一个权限管道基类实例。
        /// </summary>
        /// <param name="cryptogram">给定的密文管理器。</param>
        public PermissionProviderBase(ICryptogramManager cryptogram)
        {
            Cryptogram = cryptogram.NotNull(nameof(cryptogram));
        }


        /// <summary>
        /// 密文管理器接口。
        /// </summary>
        public ICryptogramManager Cryptogram { get; }


        /// <summary>
        /// 认证权限。
        /// </summary>
        /// <param name="actionName">给定的动作名。</param>
        /// <param name="controllerName">给定的控制器名。</param>
        /// <param name="areaName">给定的区域名。</param>
        /// <returns>返回 <see cref="IPermissionDescriptor"/>。</returns>
        public virtual IPermissionDescriptor Authenticate(string actionName, string controllerName, string areaName)
        {
            return new PermissionDescriptor();
        }

        /// <summary>
        /// 认证权限。
        /// </summary>
        /// <param name="permission">给定的权限。</param>
        /// <returns>返回 <see cref="IPermissionDescriptor"/>。</returns>
        public virtual IPermissionDescriptor Authenticate(IPermissionDescriptor permission)
        {
            return permission.NotNull(nameof(permission));
        }

    }
}
