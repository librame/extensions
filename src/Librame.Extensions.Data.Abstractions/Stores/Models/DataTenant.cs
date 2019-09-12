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
using System.Linq.Expressions;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 数据租户。
    /// </summary>
    [Description("数据租户")]
    public class DataTenant : AbstractEntityUpdation<string>, ITenant, IRank, IEquatable<DataTenant>
    {
        /// <summary>
        /// 构造一个 <see cref="DataTenant"/>。
        /// </summary>
        public DataTenant()
            : base()
        {
        }

        /// <summary>
        /// 构造一个 <see cref="DataTenant"/>。
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
        public virtual string DefaultConnectionString { get; set; }

        /// <summary>
        /// 写入连接字符串。
        /// </summary>
        [Display(Name = nameof(WritingConnectionString), ResourceType = typeof(TenantResource))]
        public virtual string WritingConnectionString { get; set; }

        /// <summary>
        /// 写入连接分离（默认不启用）。
        /// </summary>
        [Display(Name = nameof(WritingSeparation), ResourceType = typeof(TenantResource))]
        public virtual bool WritingSeparation { get; set; }


        /// <summary>
        /// 唯一索引是否相等。
        /// </summary>
        /// <param name="other">给定的其他 <see cref="DataTenant"/>。</param>
        /// <returns>返回布尔值。</returns>
        public bool Equals(DataTenant other)
            => Name == other.Name && Host == other.Host;

        /// <summary>
        /// 重写是否相等。
        /// </summary>
        /// <param name="obj">给定要比较的对象。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
            => (obj is DataEntity other) ? Equals(other) : false;


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回 32 位整数。</returns>
        public override int GetHashCode()
            => ToString().GetHashCode();


        /// <summary>
        /// 转换为字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
            => $"{Name},{Host}";


        /// <summary>
        /// 获取唯一索引表达式。
        /// </summary>
        /// <typeparam name="TTenant">指定的租户类型。</typeparam>
        /// <param name="name">给定的名称。</param>
        /// <param name="host">给定的主机。</param>
        /// <returns>返回查询表达式。</returns>
        public static Expression<Func<TTenant, bool>> GetUniqueIndexExpression<TTenant>(string name, string host)
            where TTenant : DataTenant
        {
            name.NotNullOrEmpty(nameof(name));
            host.NotNullOrEmpty(nameof(host));

            return p => p.Name == name && p.Host == host;
        }
    }
}
