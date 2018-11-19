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
using System.ComponentModel.DataAnnotations;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 租户。
    /// </summary>
    [Description("租户")]
    public class Tenant : AbstractEntity<string, DateTimeOffset, DataStatus>
    {
        /// <summary>
        /// 构造一个 <see cref="Tenant"/> 实例。
        /// </summary>
        public Tenant()
        {
            DataStatus = DataStatus.Public;
            CreateTime = UpdateTime = DateTimeOffset.Now;
        }


        /// <summary>
        /// 名称。
        /// </summary>
        [Display(Name = nameof(Name), ResourceType = typeof(TenantResource))]
        public virtual string Name { get; set; }

        /// <summary>
        /// 主机。
        /// </summary>
        [Display(Name = nameof(Host), ResourceType = typeof(TenantResource))]
        public virtual string Host { get; set; }
    }
}
