#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Librame.Extensions.Data.Stores
{
    using Core.Identifiers;
    using Data.Resources;

    /// <summary>
    /// 数据租户。
    /// </summary>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
    [Description("数据租户")]
    public class DataTenant<TGenId, TCreatedBy> : AbstractEntityCreation<TGenId, TCreatedBy>,
        IGenerativeIdentifier<TGenId>, ITenant
        where TGenId : IEquatable<TGenId>
        where TCreatedBy : IEquatable<TCreatedBy>
    {
        /// <summary>
        /// 构造一个数据租户。
        /// </summary>
        public DataTenant()
            : base()
        {
        }

        /// <summary>
        /// 构造一个数据租户。
        /// </summary>
        /// <param name="name">给定的名称。</param>
        /// <param name="host">给定的主机。</param>
        public DataTenant(string name, string host)
            : base()
        {
            Name = name.NotEmpty(nameof(name));
            Host = host.NotEmpty(nameof(host));
        }


        /// <summary>
        /// 名称。
        /// </summary>
        [Display(Name = nameof(Name), ResourceType = typeof(DataTenantResource))]
        public virtual string Name { get; set; }

        /// <summary>
        /// 主机。
        /// </summary>
        [Display(Name = nameof(Host), ResourceType = typeof(DataTenantResource))]
        public virtual string Host { get; set; }

        /// <summary>
        /// 加密连接字符串（默认不启用）。
        /// </summary>
        [Display(Name = nameof(EncryptedConnectionStrings), ResourceType = typeof(DataTenantResource))]
        public virtual bool EncryptedConnectionStrings { get; set; }

        /// <summary>
        /// 默认连接字符串。
        /// </summary>
        [Display(Name = nameof(DefaultConnectionString), ResourceType = typeof(DataTenantResource))]
        [PrivacyData]
        public virtual string DefaultConnectionString { get; set; }

        /// <summary>
        /// 写入连接字符串。
        /// </summary>
        [Display(Name = nameof(WritingConnectionString), ResourceType = typeof(DataTenantResource))]
        [PrivacyData]
        public virtual string WritingConnectionString { get; set; }

        /// <summary>
        /// 写入连接分离（默认不启用）。
        /// </summary>
        [Display(Name = nameof(WritingSeparation), ResourceType = typeof(DataTenantResource))]
        public virtual bool WritingSeparation { get; set; }

        /// <summary>
        /// 数据同步（默认不启用）。
        /// </summary>
        [Display(Name = nameof(DataSynchronization), ResourceType = typeof(DataTenantResource))]
        public virtual bool DataSynchronization { get; set; }

        /// <summary>
        /// 结构同步（默认不启用）。
        /// </summary>
        [Display(Name = nameof(StructureSynchronization), ResourceType = typeof(DataTenantResource))]
        public virtual bool StructureSynchronization { get; set; }


        /// <summary>
        /// 除主键外的唯一索引相等比较（参见实体映射的唯一索引配置）。
        /// </summary>
        /// <param name="other">给定的 <see cref="ITenant"/>。</param>
        /// <returns>返回布尔值。</returns>
        public bool Equals(ITenant other)
            => Name == other?.Name && Host == other.Host;

        /// <summary>
        /// 重写是否相等。
        /// </summary>
        /// <param name="obj">给定要比较的对象。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
            => obj is ITenant other && Equals(other);


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回 32 位整数。</returns>
        public override int GetHashCode()
            => Name.CompatibleGetHashCode() ^ Host.CompatibleGetHashCode();


        /// <summary>
        /// 转换为字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
            => $"{base.ToString()};{nameof(Name)}={Name};{nameof(Host)}={Host}";

    }
}
