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
    /// 角色管道基类。
    /// </summary>
    public class RoleProviderBase : IRoleProvider
    {
        /// <summary>
        /// 构造一个角色管道基类实例。
        /// </summary>
        /// <param name="cryptogram">给定的密文管理器。</param>
        public RoleProviderBase(ICryptogramManager cryptogram)
        {
            Cryptogram = cryptogram.NotNull(nameof(cryptogram));
        }


        /// <summary>
        /// 密文管理器接口。
        /// </summary>
        public ICryptogramManager Cryptogram { get; }


        /// <summary>
        /// 获取角色集合。
        /// </summary>
        /// <param name="user">给定的用户。</param>
        /// <returns>返回字符串数组。</returns>
        public virtual string[] GetRoles(string user)
        {
            user.NotNullOrEmpty(nameof(user));

            return null;
        }

        /// <summary>
        /// 获取角色。
        /// </summary>
        /// <param name="role">给定的用户。</param>
        /// <returns>返回角色。</returns>
        public virtual IRoleDescriptor GetRole(IRoleDescriptor role)
        {
            return role.NotNull(nameof(role));
        }

    }
}
