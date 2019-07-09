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
    /// 数据租户。
    /// </summary>
    [Description("数据租户")]
    public class DataTenant : DataTenant<float, DataStatus>, IRank, IStatus
    {
        /// <summary>
        /// 构造一个 <see cref="DataTenant"/> 的默认实例。
        /// </summary>
        public DataTenant()
            : base()
        {
        }

        /// <summary>
        /// 构造一个 <see cref="DataTenant"/> 的实例。
        /// </summary>
        /// <param name="name">给定的名称。</param>
        /// <param name="host">给定的主机。</param>
        public DataTenant(string name, string host)
            : base(name, host)
        {
        }
    }


    /// <summary>
    /// 数据租户。
    /// </summary>
    /// <typeparam name="TRank">指定的排序类型（兼容整数、单双精度的排序字段）。</typeparam>
    /// <typeparam name="TStatus">指定的状态类型（兼容不支持枚举类型的实体框架）。</typeparam>
    [Description("数据租户")]
    public class DataTenant<TRank, TStatus> : AbstractEntity<TRank, TStatus>, ITenant
        where TRank : struct
        where TStatus : struct
    {
        /// <summary>
        /// 构造一个 <see cref="DataTenant{TRank, TStatus}"/> 的默认实例。
        /// </summary>
        public DataTenant()
            : base()
        {
        }
        /// <summary>
        /// 构造一个 <see cref="DataTenant{TRank, TStatus}"/> 的实例。
        /// </summary>
        /// <param name="name">给定的名称。</param>
        /// <param name="host">给定的主机。</param>
        public DataTenant(string name, string host)
            : base()
        {
            Name = name.NotNullOrEmpty(nameof(name));
            Host = host.NotNullOrEmpty(nameof(host));
        }


        /// <summary>
        /// 名称。
        /// </summary>
        [Display(Name = nameof(Name), ResourceType = typeof(DataTenantResource))]
        public virtual string Name { get; set; } = "Local";

        /// <summary>
        /// 主机。
        /// </summary>
        [Display(Name = nameof(Host), ResourceType = typeof(DataTenantResource))]
        public virtual string Host { get; set; } = "localhost";

        /// <summary>
        /// 默认连接字符串。
        /// </summary>
        [Display(Name = nameof(DefaultConnectionString), ResourceType = typeof(DataTenantResource))]
        public virtual string DefaultConnectionString { get; set; } = "librame_default";

        /// <summary>
        /// 写入连接字符串。
        /// </summary>
        [Display(Name = nameof(WriteConnectionString), ResourceType = typeof(DataTenantResource))]
        public virtual string WriteConnectionString { get; set; } = "librame_writer";

        /// <summary>
        /// 写入连接分离（默认不启用）。
        /// </summary>
        [Display(Name = nameof(WriteConnectionSeparation), ResourceType = typeof(DataTenantResource))]
        public virtual bool WriteConnectionSeparation { get; set; } = false;
    }
}
