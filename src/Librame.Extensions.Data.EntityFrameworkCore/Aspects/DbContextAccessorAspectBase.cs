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
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Aspects
{
    using Core;
    using Core.Services;
    using Data.Accessors;
    using Data.Builders;
    using Data.Stores;

    /// <summary>
    /// 数据库上下文访问器截面基类。
    /// </summary>
    /// <typeparam name="TAudit">指定的审计类型。</typeparam>
    /// <typeparam name="TAuditProperty">指定的审计属性类型。</typeparam>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <typeparam name="TMigration">指定的迁移类型。</typeparam>
    /// <typeparam name="TTenant">指定的租户类型。</typeparam>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    /// <typeparam name="TIncremId">指定的增量式标识类型。</typeparam>
    public class DbContextAccessorAspectBase<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> : AbstractExtensionBuilderService<DataBuilderOptions>
        , IDbContextAccessorAspect<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId>
        where TAudit : DataAudit<TGenId>
        where TAuditProperty : DataAuditProperty<TIncremId, TGenId>
        where TEntity : DataEntity<TGenId>
        where TMigration : DataMigration<TGenId>
        where TTenant : DataTenant<TGenId>
        where TGenId : IEquatable<TGenId>
        where TIncremId : IEquatable<TIncremId>
    {
        /// <summary>
        /// 构造一个数据库上下文访问器截面基类。
        /// </summary>
        /// <param name="dependencies">给定的 <see cref="DbContextAccessorAspectDependencies{TGenId}"/>。</param>
        /// <param name="priority">给定的服务优先级（数值越小越优先）。</param>
        protected DbContextAccessorAspectBase(DbContextAccessorAspectDependencies<TGenId> dependencies, float priority)
            : base(dependencies)
        {
            Dependencies = dependencies;
            Priority = priority;
        }


        /// <summary>
        /// 依赖集合。
        /// </summary>
        /// <value>返回 <see cref="DbContextAccessorAspectDependencies{TGenId}"/>。</value>
        public DbContextAccessorAspectDependencies<TGenId> Dependencies { get; }

        /// <summary>
        /// 服务优先级（数值越小越优先）。
        /// </summary>
        public float Priority { get; set; }

        /// <summary>
        /// 启用截面。
        /// </summary>
        public virtual bool Enabled { get; }


        #region Preprocess

        /// <summary>
        /// 前置处理。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IDbContextAccessor{TAudit, TAuditProperty, TEntity, TMigration, TTenant}"/>。</param>
        public virtual void Preprocess(IDbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant> accessor)
        {
            accessor.NotNull(nameof(accessor));

            if (Enabled && accessor is DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> dbContextAccessor)
                PreprocessCore(dbContextAccessor);
        }

        /// <summary>
        /// 前置处理核心。
        /// </summary>
        /// <param name="dbContextAccessor">给定的 <see cref="DbContextAccessor{TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/>。</param>
        protected virtual void PreprocessCore(DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> dbContextAccessor)
        {
        }


        /// <summary>
        /// 异步前置处理。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IDbContextAccessor{TAudit, TAuditProperty, TEntity, TMigration, TTenant}"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        public virtual Task PreprocessAsync(IDbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant> accessor, CancellationToken cancellationToken = default)
        {
            accessor.NotNull(nameof(accessor));

            if (Enabled && accessor is DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> dbContextAccessor)
                return PreprocessCoreAsync(dbContextAccessor, cancellationToken);

            return Task.CompletedTask;
        }

        /// <summary>
        /// 异步前置处理核心。
        /// </summary>
        /// <param name="dbContextAccessor">给定的 <see cref="DbContextAccessor{TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        protected virtual Task PreprocessCoreAsync(DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> dbContextAccessor,
            CancellationToken cancellationToken = default)
            => Task.CompletedTask;

        #endregion


        #region Postprocess

        /// <summary>
        /// 后置处理。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IDbContextAccessor{TAudit, TAuditProperty, TEntity, TMigration, TTenant}"/>。</param>
        public virtual void Postprocess(IDbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant> accessor)
        {
            accessor.NotNull(nameof(accessor));

            if (Enabled && accessor is DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> dbContextAccessor)
                PostprocessCore(dbContextAccessor);
        }

        /// <summary>
        /// 后置处理核心。
        /// </summary>
        /// <param name="dbContextAccessor">给定的 <see cref="DbContextAccessor{TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/>。</param>
        protected virtual void PostprocessCore(DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> dbContextAccessor)
        {
        }


        /// <summary>
        /// 异步后置处理。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IDbContextAccessor{TAudit, TAuditProperty, TEntity, TMigration, TTenant}"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        public virtual Task PostprocessAsync(IDbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant> accessor, CancellationToken cancellationToken = default)
        {
            accessor.NotNull(nameof(accessor));

            if (Enabled && accessor is DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> dbContextAccessor)
                return PostprocessCoreAsync(dbContextAccessor, cancellationToken);

            return Task.CompletedTask;
        }

        /// <summary>
        /// 异步后置处理核心。
        /// </summary>
        /// <param name="dbContextAccessor">给定的 <see cref="DbContextAccessor{TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        protected virtual Task PostprocessCoreAsync(DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> dbContextAccessor,
            CancellationToken cancellationToken = default)
            => Task.CompletedTask;

        #endregion


        /// <summary>
        /// 比较优先级。
        /// </summary>
        /// <param name="other">给定的 <see cref="ISortable"/>。</param>
        /// <returns>返回整数。</returns>
        public virtual int CompareTo(ISortable other)
            => Priority.CompareTo((float)other?.Priority);


        /// <summary>
        /// 优先级相等。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
            => obj is DbContextAccessorAspectBase<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> sortable ? Priority == sortable?.Priority : false;


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回整数。</returns>
        public override int GetHashCode()
            => Priority.GetHashCode();


        /// <summary>
        /// 相等比较。
        /// </summary>
        /// <param name="left">给定的 <see cref="DbContextAccessorAspectBase{TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/>。</param>
        /// <param name="right">给定的 <see cref="DbContextAccessorAspectBase{TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator ==(DbContextAccessorAspectBase<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> left,
            DbContextAccessorAspectBase<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> right)
            => ReferenceEquals(left, null) ? !ReferenceEquals(right, null) : left.Equals(right);

        /// <summary>
        /// 不等比较。
        /// </summary>
        /// <param name="left">给定的 <see cref="DbContextAccessorAspectBase{TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/>。</param>
        /// <param name="right">给定的 <see cref="DbContextAccessorAspectBase{TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator !=(DbContextAccessorAspectBase<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> left,
            DbContextAccessorAspectBase<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> right)
            => !(left == right);

        /// <summary>
        /// 小于比较。
        /// </summary>
        /// <param name="left">给定的 <see cref="DbContextAccessorAspectBase{TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/>。</param>
        /// <param name="right">给定的 <see cref="DbContextAccessorAspectBase{TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator <(DbContextAccessorAspectBase<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> left,
            DbContextAccessorAspectBase<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> right)
            => ReferenceEquals(left, null) ? !ReferenceEquals(right, null) : left.CompareTo(right) < 0;

        /// <summary>
        /// 小于等于比较。
        /// </summary>
        /// <param name="left">给定的 <see cref="DbContextAccessorAspectBase{TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/>。</param>
        /// <param name="right">给定的 <see cref="DbContextAccessorAspectBase{TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator <=(DbContextAccessorAspectBase<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> left,
            DbContextAccessorAspectBase<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> right)
            => ReferenceEquals(left, null) || left.CompareTo(right) <= 0;

        /// <summary>
        /// 大于比较。
        /// </summary>
        /// <param name="left">给定的 <see cref="DbContextAccessorAspectBase{TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/>。</param>
        /// <param name="right">给定的 <see cref="DbContextAccessorAspectBase{TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator >(DbContextAccessorAspectBase<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> left,
            DbContextAccessorAspectBase<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> right)
            => !ReferenceEquals(left, null) && left.CompareTo(right) > 0;

        /// <summary>
        /// 大于等于比较。
        /// </summary>
        /// <param name="left">给定的 <see cref="DbContextAccessorAspectBase{TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/>。</param>
        /// <param name="right">给定的 <see cref="DbContextAccessorAspectBase{TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator >=(DbContextAccessorAspectBase<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> left,
            DbContextAccessorAspectBase<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId> right)
            => ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.CompareTo(right) >= 0;
    }
}
