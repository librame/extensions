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
using System.Collections.Generic;

namespace Librame.Extensions.Data.Mappers
{
    using Core.Mappers;

    /// <summary>
    /// 访问器类型参数映射器。
    /// </summary>
    public class AccessorTypeParameterMapper : AbstractTypeParameterMapper
    {
        /// <summary>
        /// 构造一个 <see cref="AccessorTypeParameterMapper"/>。
        /// </summary>
        /// <param name="accessor">给定的访问器 <see cref="TypeParameterMapping"/>。</param>
        /// <param name="mappings">给定的类型参数映射字典集合。</param>
        public AccessorTypeParameterMapper(TypeParameterMapping accessor,
            Dictionary<string, TypeParameterMapping> mappings)
            : base(mappings)
        {
            Accessor = accessor.NotNull(nameof(accessor));
        }


        /// <summary>
        /// 访问器映射。
        /// </summary>
        public TypeParameterMapping Accessor { get; }


        /// <summary>
        /// 审计映射。
        /// </summary>
        public virtual TypeParameterMapping Audit
            => GetGenericMapping(nameof(Audit));

        /// <summary>
        /// 审计属性映射。
        /// </summary>
        public virtual TypeParameterMapping AuditProperty
            => GetGenericMapping(nameof(AuditProperty));

        /// <summary>
        /// 实体映射。
        /// </summary>
        public virtual TypeParameterMapping Entity
            => GetGenericMapping(nameof(Entity));

        /// <summary>
        /// 迁移映射。
        /// </summary>
        public virtual TypeParameterMapping Migration
            => GetGenericMapping(nameof(Migration));

        /// <summary>
        /// 租户映射。
        /// </summary>
        public virtual TypeParameterMapping Tenant
            => GetMapping($"T{nameof(Tenant)}");


        /// <summary>
        /// 生成式标识映射。
        /// </summary>
        public virtual TypeParameterMapping GenId
            => GetGenericMapping(nameof(GenId));

        /// <summary>
        /// 增量式标识映射。
        /// </summary>
        public virtual TypeParameterMapping IncremId
            => GetGenericMapping(nameof(IncremId));

        /// <summary>
        /// 创建者（同更新者、发表者）映射。
        /// </summary>
        public virtual TypeParameterMapping CreatedBy
            => GetGenericMapping(nameof(CreatedBy));

        /// <summary>
        /// 创建日期（同更新日期、发表日期）映射。
        /// </summary>
        public virtual TypeParameterMapping CreatedTime
            => GetGenericMapping(nameof(CreatedTime));


        /// <summary>
        /// 使用除创建日期外的所有类型参数映射集合填充泛型类型定义。
        /// </summary>
        /// <param name="typeDefinition">给定的类型定义。</param>
        /// <returns>返回类型。</returns>
        public virtual Type PopulateGenericTypeDefinitionByAllMappingWithoutCreatedTime(Type typeDefinition)
        {
            return PopulateGenericTypeDefinition(typeDefinition, () => new Type[]
            {
                Audit.ArgumentType,
                AuditProperty.ArgumentType,
                Entity.ArgumentType,
                Migration.ArgumentType,
                Tenant.ArgumentType,
                GenId.ArgumentType,
                IncremId.ArgumentType,
                CreatedBy.ArgumentType
            });
        }

    }
}
