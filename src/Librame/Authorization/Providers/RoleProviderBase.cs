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
    using Adaptation;
    using Descriptors;

    /// <summary>
    /// 角色管道。
    /// </summary>
    public class RoleProviderBase : AbstractAdapterManagerReference, IRoleProvider
    {
        /// <summary>
        /// 构造一个 <see cref="RoleProviderBase"/> 实例。
        /// </summary>
        /// <param name="adapters">给定的适配器管理器。</param>
        public RoleProviderBase(IAdapterManager adapters)
            : base(adapters)
        {
        }


        /// <summary>
        /// 获取角色集合。
        /// </summary>
        /// <param name="user">给定的用户。</param>
        /// <returns>返回字符串数组。</returns>
        public virtual string[] GetRoles(string user)
        {
            return null;
        }

        /// <summary>
        /// 获取角色。
        /// </summary>
        /// <param name="role">给定的用户。</param>
        /// <returns>返回角色。</returns>
        public virtual IRoleDescriptor GetRole(IRoleDescriptor role)
        {
            return role;
        }

    }
}
