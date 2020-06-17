#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections.Generic;

namespace Librame.Extensions.Data.Accessors
{
    using Core;

    /// <summary>
    /// 访问器泛型类型映射描述符。
    /// </summary>
    public class AccessorGenericTypeMappingDescriptor : AbstractGenericTypeMappingDescriptor
    {
        /// <summary>
        /// 构造一个 <see cref="AccessorGenericTypeMappingDescriptor"/>。
        /// </summary>
        /// <param name="accessor">给定的访问器 <see cref="GenericTypeMapping"/>。</param>
        /// <param name="mappings">给定的泛型类型映射字典集合。</param>
        public AccessorGenericTypeMappingDescriptor(GenericTypeMapping accessor,
            Dictionary<string, GenericTypeMapping> mappings)
            : base(mappings)
        {
            Accessor = accessor.NotNull(nameof(accessor));
        }


        /// <summary>
        /// 访问器。
        /// </summary>
        public GenericTypeMapping Accessor { get; }


        /// <summary>
        /// 审计。
        /// </summary>
        public GenericTypeMapping Audit
            => GetMappingOrDefault(nameof(Audit));

        /// <summary>
        /// 审计属性。
        /// </summary>
        public GenericTypeMapping AuditProperty
            => GetMappingOrDefault(nameof(AuditProperty));

        /// <summary>
        /// 实体。
        /// </summary>
        public GenericTypeMapping Entity
            => GetMappingOrDefault(nameof(Entity));

        /// <summary>
        /// 迁移。
        /// </summary>
        public GenericTypeMapping Migration
            => GetMappingOrDefault(nameof(Migration));

        /// <summary>
        /// 租户。
        /// </summary>
        public GenericTypeMapping Tenant
            => GetMappingOrDefault($"T{nameof(Tenant)}");


        /// <summary>
        /// 生成式标识。
        /// </summary>
        public GenericTypeMapping GenId
            => GetMappingOrDefault(nameof(GenId));

        /// <summary>
        /// 增量式标识。
        /// </summary>
        public GenericTypeMapping IncremId
            => GetMappingOrDefault(nameof(IncremId));

        /// <summary>
        /// 创建者（同更新者、发表者）。
        /// </summary>
        public GenericTypeMapping CreatedBy
            => GetMappingOrDefault(nameof(CreatedBy));

        /// <summary>
        /// 创建日期（同更新日期、发表日期）。
        /// </summary>
        public GenericTypeMapping CreatedTime
            => GetMappingOrDefault(nameof(CreatedTime));
    }
}
