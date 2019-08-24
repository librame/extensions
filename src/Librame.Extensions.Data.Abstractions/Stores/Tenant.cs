#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 租户。
    /// </summary>
    [Description("租户")]
    public class Tenant : AbstractEntityWithGenId, IRank, IStatus, ITenant
    {
        /// <summary>
        /// 构造一个 <see cref="Tenant"/> 的默认实例。
        /// </summary>
        public Tenant()
            : base()
        {
        }

        /// <summary>
        /// 构造一个 <see cref="Tenant"/> 的实例。
        /// </summary>
        /// <param name="name">给定的名称。</param>
        /// <param name="host">给定的主机。</param>
        public Tenant(string name, string host)
            : base()
        {
            Name = name.NotNullOrEmpty(nameof(name));
            Host = host.NotNullOrEmpty(nameof(host));
        }


        /// <summary>
        /// 名称。
        /// </summary>
        [Display(Name = nameof(Name), ResourceType = typeof(TenantResource))]
        public virtual string Name { get; set; } = "Local";

        /// <summary>
        /// 主机。
        /// </summary>
        [Display(Name = nameof(Host), ResourceType = typeof(TenantResource))]
        public virtual string Host { get; set; } = "localhost";

        /// <summary>
        /// 默认连接字符串。
        /// </summary>
        [Display(Name = nameof(DefaultConnectionString), ResourceType = typeof(TenantResource))]
        public virtual string DefaultConnectionString { get; set; } = "librame_data_default";

        /// <summary>
        /// 写入连接字符串。
        /// </summary>
        [Display(Name = nameof(WritingConnectionString), ResourceType = typeof(TenantResource))]
        public virtual string WritingConnectionString { get; set; } = "librame_data_writing";

        /// <summary>
        /// 写入连接分离（默认不启用）。
        /// </summary>
        [Display(Name = nameof(WritingSeparation), ResourceType = typeof(TenantResource))]
        public virtual bool WritingSeparation { get; set; } = false;
    }
}
