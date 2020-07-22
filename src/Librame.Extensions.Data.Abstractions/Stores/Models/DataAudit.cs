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
    /// 数据审计。
    /// </summary>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
    [Description("数据审计")]
    [NonAudited]
    public class DataAudit<TGenId, TCreatedBy> : AbstractCreation<TGenId, TCreatedBy>,
        IGenerativeIdentifier<TGenId>,
        IEquatable<DataAudit<TGenId, TCreatedBy>>
        where TGenId : IEquatable<TGenId>
        where TCreatedBy : IEquatable<TCreatedBy>
    {
        /// <summary>
        /// 表名。
        /// </summary>
        [Display(Name = nameof(TableName), ResourceType = typeof(DataAuditResource))]
        public virtual string TableName { get; set; }

        /// <summary>
        /// 实体标识。
        /// </summary>
        [Display(Name = nameof(EntityId), ResourceType = typeof(DataAuditResource))]
        public virtual string EntityId { get; set; }

        /// <summary>
        /// 实体类型名。
        /// </summary>
        [Display(Name = nameof(EntityTypeName), ResourceType = typeof(DataAuditResource))]
        public virtual string EntityTypeName { get; set; }

        /// <summary>
        /// 操作状态。
        /// </summary>
        [Display(Name = nameof(State), ResourceType = typeof(AbstractEntityResource))]
        public virtual int State { get; set; }

        /// <summary>
        /// 状态名称。
        /// </summary>
        [Display(Name = nameof(StateName), ResourceType = typeof(DataAuditResource))]
        public virtual string StateName { get; set; }

        /// <summary>
        /// 属性表名（记录属性分表名称）。
        /// </summary>
        public virtual string PropertyTableName { get; set; }


        /// <summary>
        /// 除主键外的唯一索引相等比较（参见实体映射的唯一索引配置）。
        /// </summary>
        /// <param name="other">给定的其他 <see cref="DataAudit{TGenId, TCreatedBy}"/>。</param>
        /// <returns>返回布尔值。</returns>
        public bool Equals(DataAudit<TGenId, TCreatedBy> other)
            => TableName == other?.TableName && EntityId == other.EntityId && State == other.State;

        /// <summary>
        /// 重写是否相等。
        /// </summary>
        /// <param name="obj">给定要比较的对象。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
            => obj is DataAudit<TGenId, TCreatedBy> other && Equals(other);


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回 32 位整数。</returns>
        public override int GetHashCode()
            => TableName.CompatibleGetHashCode() ^ EntityId.CompatibleGetHashCode() ^ State.GetHashCode();


        /// <summary>
        /// 转换为字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
            => $"{base.ToString()};{nameof(TableName)}={TableName};{nameof(EntityId)}={EntityId}";

    }
}
