#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Aspects
{
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
    /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
    public class DbContextAccessorAspectBase<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId, TCreatedBy>
        : AbstractAccessorAspect<DataBuilderOptions>
        where TAudit : DataAudit<TGenId, TCreatedBy>
        where TAuditProperty : DataAuditProperty<TIncremId, TGenId>
        where TEntity : DataEntity<TGenId, TCreatedBy>
        where TMigration : DataMigration<TGenId, TCreatedBy>
        where TTenant : DataTenant<TGenId, TCreatedBy>
        where TGenId : IEquatable<TGenId>
        where TIncremId : IEquatable<TIncremId>
        where TCreatedBy : IEquatable<TCreatedBy>
    {
        /// <summary>
        /// 构造一个数据库上下文访问器截面基类。
        /// </summary>
        /// <param name="identifierGenerator">给定的 <see cref="IStoreIdentifierGenerator"/>。</param>
        /// <param name="options">给定的 <see cref="IOptions{DataBuilderOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        /// <param name="priority">给定的优先级（数值越小越优先）。</param>
        protected DbContextAccessorAspectBase(IStoreIdentifierGenerator identifierGenerator,
            IOptions<DataBuilderOptions> options, ILoggerFactory loggerFactory, float priority)
            : base(identifierGenerator, options, loggerFactory, priority)
        {
            DataIdentifierGenerator = identifierGenerator.CastTo<IStoreIdentifierGenerator,
                AbstractDataStoreIdentifierGenerator<TGenId>>(nameof(identifierGenerator));
        }


        /// <summary>
        /// 数据存储标识符生成器。
        /// </summary>
        /// <value>返回 <see cref="AbstractDataStoreIdentifierGenerator{TGenId}"/>。</value>
        protected AbstractDataStoreIdentifierGenerator<TGenId> DataIdentifierGenerator { get; }


        #region PreProcess

        /// <summary>
        /// 前置处理。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        public override void PreProcess(IAccessor accessor)
        {
            accessor.NotNull(nameof(accessor));

            if (accessor is DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId, TCreatedBy> dbContextAccessor)
                PreProcessCore(dbContextAccessor);
        }

        /// <summary>
        /// 前置处理核心。
        /// </summary>
        /// <param name="dbContextAccessor">给定的数据库上下文访问器。</param>
        protected virtual void PreProcessCore
            (DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId, TCreatedBy> dbContextAccessor)
        {
        }


        /// <summary>
        /// 异步前置处理。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        public override Task PreProcessAsync(IAccessor accessor, CancellationToken cancellationToken = default)
        {
            accessor.NotNull(nameof(accessor));

            if (accessor is DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId, TCreatedBy> dbContextAccessor)
                return PreProcessCoreAsync(dbContextAccessor, cancellationToken);

            return Task.CompletedTask;
        }

        /// <summary>
        /// 异步前置处理核心。
        /// </summary>
        /// <param name="dbContextAccessor">给定的数据库上下文访问器。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        protected virtual Task PreProcessCoreAsync
            (DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId, TCreatedBy> dbContextAccessor,
            CancellationToken cancellationToken = default)
            => Task.CompletedTask;

        #endregion


        #region PostProcess

        /// <summary>
        /// 后置处理。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        public override void PostProcess(IAccessor accessor)
        {
            accessor.NotNull(nameof(accessor));

            if (accessor is DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId, TCreatedBy> dbContextAccessor)
                PostProcessCore(dbContextAccessor);
        }

        /// <summary>
        /// 后置处理核心。
        /// </summary>
        /// <param name="dbContextAccessor">给定的数据库上下文访问器。</param>
        protected virtual void PostProcessCore
            (DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId, TCreatedBy> dbContextAccessor)
        {
        }


        /// <summary>
        /// 异步后置处理。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        public override Task PostProcessAsync(IAccessor accessor, CancellationToken cancellationToken = default)
        {
            accessor.NotNull(nameof(accessor));

            if (accessor is DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId, TCreatedBy> dbContextAccessor)
                return PostProcessCoreAsync(dbContextAccessor, cancellationToken);

            return Task.CompletedTask;
        }

        /// <summary>
        /// 异步后置处理核心。
        /// </summary>
        /// <param name="dbContextAccessor">给定的数据库上下文访问器。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        protected virtual Task PostProcessCoreAsync
            (DbContextAccessor<TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId, TCreatedBy> dbContextAccessor,
            CancellationToken cancellationToken = default)
            => Task.CompletedTask;

        #endregion

    }
}
