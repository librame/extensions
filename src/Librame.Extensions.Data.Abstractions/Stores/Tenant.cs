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
    public class Tenant : AbstractEntity<string, DateTimeOffset, DataStatus>, ITenant
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

        /// <summary>
        /// 默认连接字符串。
        /// </summary>
        [Display(Name = nameof(DefaultConnectionString), ResourceType = typeof(TenantResource))]
        public virtual string DefaultConnectionString { get; set; } = "librame_default";

        /// <summary>
        /// 写入连接字符串。
        /// </summary>
        [Display(Name = nameof(WriteConnectionString), ResourceType = typeof(TenantResource))]
        public virtual string WriteConnectionString { get; set; } = "librame_writer";

        /// <summary>
        /// 写入连接分离（默认不启用）。
        /// </summary>
        [Display(Name = nameof(WriteConnectionSeparation), ResourceType = typeof(TenantResource))]
        public virtual bool WriteConnectionSeparation { get; set; } = false;
    }
}
