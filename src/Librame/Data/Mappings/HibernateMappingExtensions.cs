#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Data.Descriptors;
using Librame.Utility;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FluentNHibernate.Mapping
{
    /// <summary>
    /// NHibernate 映射静态扩展。
    /// </summary>
    public static class HibernateMappingExtensions
    {
        /// <summary>
        /// 映射主键属性（默认映射名为 Id 的自增长主键列）。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <typeparam name="TId">指定的主键类型。</typeparam>
        /// <param name="config">给定的实体类型配置实例。</param>
        /// <param name="columnName">给定的主键列名。</param>
        public static void MapId<TEntity, TId>(this ClassMap<TEntity> config,
            string columnName = null)
            where TEntity : class, IIdDescriptor<TId>
            where TId : struct
        {
            if (string.IsNullOrEmpty(columnName))
            {
                config.Id(k => k.Id).GeneratedBy.Identity();
            }
            else
            {
                config.Id(k => k.Id).Column(columnName).GeneratedBy.Identity();
            }
        }

        /// <summary>
        /// 映射创建属性。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <typeparam name="TId">指定的主键类型。</typeparam>
        /// <param name="config">给定的实体类型配置实例。</param>
        public static void MapCreate<TEntity, TId>(this ClassMap<TEntity> config)
            where TEntity : class, ICreateIdDescriptor<TId>
            where TId : struct
        {
            config.Map(p => p.CreatorId);
            config.Map(p => p.CreateTime);
        }

        /// <summary>
        /// 映射数据更新属性。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <typeparam name="TId">指定的主键类型。</typeparam>
        /// <param name="config">给定的实体类型配置实例。</param>
        public static void MapUpdateAndCreate<TEntity, TId>(this ClassMap<TEntity> config)
            where TEntity : class, IUpdateAndCreateIdDescriptor<TId>
            where TId : struct
        {
            config.Map(p => p.UpdatorId);
            config.Map(p => p.UpdateTime);
        }

        /// <summary>
        /// 映射数据排序、状态属性。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <typeparam name="TId">指定的主键类型。</typeparam>
        /// <param name="config">给定的实体类型配置实例。</param>
        public static void MapData<TEntity, TId>(this ClassMap<TEntity> config)
            where TEntity : class, IDataIdDescriptor<TId>
            where TId : struct
        {
            config.Map(p => p.DataRank);
            config.Map(p => p.DataStatus);
        }


        /// <summary>
        /// 引用属性映射。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <typeparam name="TTargetEntity">指定的目标实体类型。</typeparam>
        /// <typeparam name="TKey">指定的键类型。</typeparam>
        /// <param name="config">给定的实体类型配置实例。</param>
        /// <param name="requiredPropertyExpression">给定的引用属性表达式。</param>
        /// <param name="foreignKeyExpression">给定的外键表达式。</param>
        /// <param name="manyPropertyExpression">给定的对多关系属性表达式（可选）。</param>
        /// <param name="cascadeOnDelete">是否需要级联删除。</param>
        public static void HasRequireProperty<TEntity, TTargetEntity, TKey>(this ClassMap<TEntity> config,
            Expression<Func<TEntity, TTargetEntity>> requiredPropertyExpression,
            Expression<Func<TEntity, TKey>> foreignKeyExpression,
            Expression<Func<TTargetEntity, ICollection<TEntity>>> manyPropertyExpression = null,
            bool cascadeOnDelete = false)
            where TEntity : class
            where TTargetEntity : class
        {
            var foreignKey = ExpressionUtility.AsPropertyName(foreignKeyExpression);

            ManyToOnePart<TTargetEntity> mapping = null;
            if (ReferenceEquals(manyPropertyExpression, null))
            {
                mapping = config.References(requiredPropertyExpression).Nullable().Column(foreignKey);
            }
            else
            {
                mapping = config.References(requiredPropertyExpression).Nullable().Column(foreignKey);
            }

            if (cascadeOnDelete)
                mapping.Cascade.Delete();
            else
                mapping.Cascade.None();
        }

        /// <summary>
        /// 可选属性映射。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <typeparam name="TTargetEntity">指定的目标实体类型。</typeparam>
        /// <typeparam name="TKey">指定的键类型。</typeparam>
        /// <param name="config">给定的实体类型配置实例。</param>
        /// <param name="requiredPropertyExpression">给定的引用属性表达式。</param>
        /// <param name="foreignKeyExpression">给定的外键表达式。</param>
        /// <param name="manyPropertyExpression">给定的对多关系属性表达式（可选）。</param>
        /// <param name="cascadeOnDelete">是否需要级联删除。</param>
        public static void HasOptionalProperty<TEntity, TTargetEntity, TKey>(this ClassMap<TEntity> config,
            Expression<Func<TEntity, TTargetEntity>> requiredPropertyExpression,
            Expression<Func<TEntity, TKey>> foreignKeyExpression,
            Expression<Func<TTargetEntity, ICollection<TEntity>>> manyPropertyExpression = null,
            bool cascadeOnDelete = false)
            where TEntity : class
            where TTargetEntity : class
        {
            //if (ReferenceEquals(manyPropertyExpression, null))
            //{
            //    config.HasOptional(requiredPropertyExpression).WithMany().HasForeignKey(foreignKeyExpression).WillCascadeOnDelete(cascadeOnDelete);
            //}
            //else
            //{
            //    config.HasOptional(requiredPropertyExpression).WithMany(manyPropertyExpression).HasForeignKey(foreignKeyExpression).WillCascadeOnDelete(cascadeOnDelete);
            //}
        }

    }
}
