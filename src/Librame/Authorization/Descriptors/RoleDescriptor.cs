#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.ComponentModel;

namespace Librame.Authorization.Descriptors
{
    using Utility;

    /// <summary>
    /// 角色描述符。
    /// </summary>
    [Serializable]
    public class RoleDescriptor : IRoleDescriptor
    {
        /// <summary>
        /// 构造一个 <see cref="AccountDescriptor"/> 实例。
        /// </summary>
        public RoleDescriptor()
        {
        }
        /// <summary>
        /// 构造一个 <see cref="AccountDescriptor"/> 实例。
        /// </summary>
        /// <param name="name">给定的名称。</param>
        public RoleDescriptor(string name)
        {
            Name = name.NotNullOrEmpty(nameof(name));
        }


        /// <summary>
        /// 名称。
        /// </summary>
        [DisplayName("角色名称")]
        public virtual string Name { get; set; }
    }
}
