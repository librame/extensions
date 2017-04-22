#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Linq;
using System.Security.Principal;

namespace Librame.Authorization
{
    using Utility;

    /// <summary>
    /// 帐户用户。
    /// </summary>
    public class AccountPrincipal : GenericPrincipal, IPrincipal
    {
        ///// <summary>
        ///// 获取当前用户对应的角色集合。
        ///// </summary>
        //public string[] Roles { get; }

        /// <summary>
        /// 构造一个 <see cref="AccountPrincipal"/> 实例。
        /// </summary>
        /// <param name="identity">给定的 <see cref="IIdentity"/>。</param>
        /// <param name="roles">给定当前用户对应的角色集合。</param>
        public AccountPrincipal(IIdentity identity, string[] roles)
            : base(identity, roles)
        {
            //Roles = roles;
        }


        ///// <summary>
        ///// 确定当前用户是否属于指定的角色。
        ///// </summary>
        ///// <param name="role">要检查其成员资格的角色的名称。</param>
        ///// <returns>如果当前用户是指定角色的成员，则为 true；否则为 false。</returns>
        //public virtual bool IsInRole(string role)
        //{
        //    if (ReferenceEquals(Roles, null))
        //        return false;

        //    return Roles.Contains(role);
        //}

    }
}
