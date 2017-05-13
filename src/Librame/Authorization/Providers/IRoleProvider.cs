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
    /// 角色管道接口。
    /// </summary>
    public interface IRoleProvider
    {
        /// <summary>
        /// 密文管理器接口。
        /// </summary>
        ICryptogramManager Cryptogram { get; }


        /// <summary>
        /// 获取角色集合。
        /// </summary>
        /// <param name="user">给定的用户。</param>
        /// <returns>返回字符串数组。</returns>
        string[] GetRoles(string user);

        /// <summary>
        /// 获取角色。
        /// </summary>
        /// <param name="role">给定的用户。</param>
        /// <returns>返回角色。</returns>
        IRoleDescriptor GetRole(IRoleDescriptor role);
    }
}
