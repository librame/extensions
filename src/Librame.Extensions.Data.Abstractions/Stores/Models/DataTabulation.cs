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
    /// 数据表格。
    /// </summary>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
    [Description("数据表格")]
    public class DataTabulation<TGenId, TCreatedBy> : AbstractCreation<TGenId, TCreatedBy>,
        IGenerativeIdentifier<TGenId>,
        IEquatable<DataTabulation<TGenId, TCreatedBy>>
        where TGenId : IEquatable<TGenId>
        where TCreatedBy : IEquatable<TCreatedBy>
    {
        /// <summary>
        /// 架构。
        /// </summary>
        [Display(Name = nameof(Schema), ResourceType = typeof(DataTabulationResource))]
        public virtual string Schema { get; set; }

        /// <summary>
        /// 表名。
        /// </summary>
        [Display(Name = nameof(TableName), ResourceType = typeof(DataTabulationResource))]
        public virtual string TableName { get; set; }

        /// <summary>
        /// 是否分表。
        /// </summary>
        [Display(Name = nameof(IsSharding), ResourceType = typeof(DataTabulationResource))]
        public virtual bool IsSharding { get; set; }

        /// <summary>
        /// 描述。
        /// </summary>
        [Display(Name = nameof(Description), ResourceType = typeof(DataTabulationResource))]
        public virtual string Description { get; set; }

        /// <summary>
        /// 实体名。
        /// </summary>
        [Display(Name = nameof(EntityName), ResourceType = typeof(DataTabulationResource))]
        public virtual string EntityName { get; set; }

        /// <summary>
        /// 程序集名。
        /// </summary>
        [Display(Name = nameof(AssemblyName), ResourceType = typeof(DataTabulationResource))]
        public virtual string AssemblyName { get; set; }


        /// <summary>
        /// 除主键外的唯一索引相等比较（参见实体映射的唯一索引配置）。
        /// </summary>
        /// <param name="other">给定的 <see cref="DataTabulation{TGenId, TCreatedBy}"/>。</param>
        /// <returns>返回布尔值。</returns>
        public bool Equals(DataTabulation<TGenId, TCreatedBy> other)
            => Schema == other?.Schema && TableName == other.TableName;

        /// <summary>
        /// 重写是否相等。
        /// </summary>
        /// <param name="obj">给定要比较的对象。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
            => obj is DataTabulation<TGenId, TCreatedBy> other && Equals(other);


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回 32 位整数。</returns>
        public override int GetHashCode()
            => Schema.CompatibleGetHashCode() ^ TableName.CompatibleGetHashCode();


        /// <summary>
        /// 转换为字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
            => $"{base.ToString()};Table={new TableDescriptor(TableName, Schema)};{nameof(EntityName)}={EntityName};{nameof(IsSharding)}={IsSharding}";

    }
}
