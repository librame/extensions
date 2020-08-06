#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Aspects
{
    using Core.Mediators;
    using Data.Accessors;
    using Data.Builders;
    using Data.Mediators;
    using Data.Stores;

    /// <summary>
    /// 表格迁移数据库上下文访问器截面。
    /// </summary>
    /// <typeparam name="TAudit">指定的审计类型。</typeparam>
    /// <typeparam name="TAuditProperty">指定的审计属性类型。</typeparam>
    /// <typeparam name="TMigration">指定的迁移类型。</typeparam>
    /// <typeparam name="TTabulation">指定的表格类型。</typeparam>
    /// <typeparam name="TTenant">指定的租户类型。</typeparam>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    /// <typeparam name="TIncremId">指定的增量式标识类型。</typeparam>
    /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
    public class TabulationMigrateDbContextAccessorAspect<TAudit, TAuditProperty, TMigration, TTabulation, TTenant, TGenId, TIncremId, TCreatedBy>
        : DbContextAccessorAspectBase<TAudit, TAuditProperty, TMigration, TTabulation, TTenant, TGenId, TIncremId, TCreatedBy>,
        IMigrateAccessorAspect
        where TAudit : DataAudit<TGenId, TCreatedBy>
        where TAuditProperty : DataAuditProperty<TIncremId, TGenId>
        where TMigration : DataMigration<TGenId, TCreatedBy>
        where TTabulation : DataTabulation<TGenId, TCreatedBy>
        where TTenant : DataTenant<TGenId, TCreatedBy>
        where TGenId : IEquatable<TGenId>
        where TIncremId : IEquatable<TIncremId>
        where TCreatedBy : IEquatable<TCreatedBy>
    {
        /// <summary>
        /// 构造一个实体迁移数据库上下文访问器截面。
        /// </summary>
        /// <param name="generator">给定的 <see cref="IStoreIdentificationGenerator"/>。</param>
        /// <param name="options">给定的 <see cref="IOptions{DataBuilderOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public TabulationMigrateDbContextAccessorAspect(IStoreIdentificationGenerator generator,
            IOptions<DataBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(generator, options, loggerFactory, priority: 2)
        {
        }


        /// <summary>
        /// 启用此截面。
        /// </summary>
        public override bool Enabled
            => Options.Stores.UseDataTabulation;


        /// <summary>
        /// 后置处理核心。
        /// </summary>
        /// <param name="dbContextAccessor">给定的数据库上下文访问器。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        protected override void PostProcessCore(DataDbContextAccessor<TAudit, TAuditProperty, TMigration, TTabulation, TTenant, TGenId, TIncremId, TCreatedBy> dbContextAccessor)
        {
            (var adds, var updates) = GetAddsOrUpdates(dbContextAccessor);

            if (adds.IsNotNull() || updates.IsNotNull())
            {
                var notification = new TabulationNotification<TTabulation>();
                notification.Adds = adds;
                notification.Updates = updates;

                var mediator = dbContextAccessor.GetService<IMediator>();
                mediator.Publish(notification).ConfigureAwaitCompleted();
            }
        }

        /// <summary>
        /// 异步后置处理核心。
        /// </summary>
        /// <param name="dbContextAccessor">给定的数据库上下文访问器。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        protected override async Task PostProcessCoreAsync(DataDbContextAccessor<TAudit, TAuditProperty, TMigration, TTabulation, TTenant, TGenId, TIncremId, TCreatedBy> dbContextAccessor,
            CancellationToken cancellationToken = default)
        {
            (var adds, var updates) = GetAddsOrUpdates(dbContextAccessor, cancellationToken);

            if (adds.IsNotNull() || updates.IsNotNull())
            {
                var notification = new TabulationNotification<TTabulation>();
                notification.Adds = adds;
                notification.Updates = updates;

                var mediator = dbContextAccessor.GetService<IMediator>();
                await mediator.Publish(notification).ConfigureAwait();
            }
        }


        /// <summary>
        /// 获取添加或更新的实体集合。
        /// </summary>
        /// <param name="dbContextAccessor">给定的数据库上下文访问器。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回包含添加、更新的 <see cref="List{DataEntity}"/> 元组。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        protected virtual (List<TTabulation> adds, List<TTabulation> updates) GetAddsOrUpdates
            (DataDbContextAccessor<TAudit, TAuditProperty, TMigration, TTabulation, TTenant, TGenId, TIncremId, TCreatedBy> dbContextAccessor,
            CancellationToken cancellationToken = default)
        {
            var manager = dbContextAccessor.TabulationsManager;

            List<TTabulation> adds = null;

            // Initialize
            var initialized = manager.TryInitializeRange(() =>
            {
                adds = dbContextAccessor.Model.GetEntityTypes()
                    .Select(s => CreateTabulation(s)).ToList();

                return adds;
            },
            post =>
            {
                if (!dbContextAccessor.RequiredSaveChanges)
                    dbContextAccessor.RequiredSaveChanges = true;
            });

            if (initialized)
                return (adds, null);

            List<TTabulation> updates = null;

            // AddOrUpdate
            foreach (var entityType in dbContextAccessor.Model.GetEntityTypes())
            {
                var tableName = GetEntityTableName(entityType);
                var schema = GetEntitySchema(entityType);

                manager.TryAddOrUpdate(p => p.Schema == schema && p.TableName == tableName,
                    () =>
                    {
                        var create = CreateTabulation(entityType, tableName, schema);

                        if (adds.IsNull())
                            adds = new List<TTabulation>();

                        adds.Add(create);

                        return create;
                    },
                    update =>
                    {
                        if (IsTabulationUpdated(entityType, update))
                        {
                            if (updates.IsNull())
                                updates = new List<TTabulation>();

                            updates.Add(update);
                            return true;
                        }

                        return false;
                    },
                    addedOrUpdatedPost =>
                    {
                        if (!dbContextAccessor.RequiredSaveChanges)
                            dbContextAccessor.RequiredSaveChanges = true;
                    });
            }

            return (adds, updates);
        }


        /// <summary>
        /// 创建表格。
        /// </summary>
        /// <param name="entityType">给定的 <see cref="IEntityType"/>。</param>
        /// <returns>返回 <typeparamref name="TTabulation"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        protected virtual TTabulation CreateTabulation(IEntityType entityType)
        {
            var tableName = GetEntityTableName(entityType);
            var schema = GetEntitySchema(entityType);

            return CreateTabulation(entityType, tableName, schema);
        }

        /// <summary>
        /// 创建表格。
        /// </summary>
        /// <param name="entityType">给定的 <see cref="IEntityType"/>。</param>
        /// <param name="tableName">给定的表名。</param>
        /// <param name="schema">给定的架构。</param>
        /// <returns>返回 <typeparamref name="TTabulation"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        protected virtual TTabulation CreateTabulation(IEntityType entityType, string tableName, string schema)
        {
            var entity = ObjectExtensions.EnsureCreate<TTabulation>();

            entity.Id = DataGenerator.GenerateTabulationId();

            entity.TableName = tableName;
            entity.Schema = schema;

            entity.EntityName = GetEntityName(entityType);
            entity.AssemblyName = GetEntityAssemblyName(entityType);
            entity.Description = GetEntityDescription(entityType);
            entity.IsSharding = IsEntitySharding(entityType);

            entity.PopulateCreation(Clock);

            return entity;
        }


        /// <summary>
        /// 表格是否已更新。
        /// </summary>
        /// <param name="entityType">给定的 <see cref="IEntityType"/>。</param>
        /// <param name="dbTabulation">给定的数据库实体。</param>
        /// <returns>返回是否更新的布尔值。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        protected virtual bool IsTabulationUpdated(IEntityType entityType, TTabulation dbTabulation)
        {
            var isUpdated = false;

            var entityName = GetEntityName(entityType);
            if (dbTabulation.EntityName != entityName)
            {
                dbTabulation.EntityName = entityName;
                isUpdated = true;
            }

            var assemblyName = GetEntityAssemblyName(entityType);
            if (dbTabulation.AssemblyName != assemblyName)
            {
                dbTabulation.AssemblyName = assemblyName;
                isUpdated = true;
            }

            var description = GetEntityDescription(entityType);
            if (dbTabulation.Description != description)
            {
                dbTabulation.Description = description;
                isUpdated = true;
            }

            var isSharding = IsEntitySharding(entityType);
            if (dbTabulation.IsSharding != isSharding)
            {
                dbTabulation.IsSharding = isSharding;
                isUpdated = true;
            }

            return isUpdated;
        }


        /// <summary>
        /// 获取实体类型表名。
        /// </summary>
        /// <param name="entityType">给定的 <see cref="IEntityType"/>。</param>
        /// <returns>返回字符串。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        protected virtual string GetEntityTableName(IEntityType entityType)
            => entityType.GetTableName().NotEmptyOrDefault(entityType.ClrType.Name);

        /// <summary>
        /// 获取实体类型架构。
        /// </summary>
        /// <param name="entityType">给定的 <see cref="IEntityType"/>。</param>
        /// <returns>返回字符串。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        protected virtual string GetEntitySchema(IEntityType entityType)
            => entityType.GetSchema().NotEmptyOrDefault(DataSettings.Preference.DefaultSchema);

        /// <summary>
        /// 获取实体类型名称。
        /// </summary>
        /// <param name="entityType">给定的 <see cref="IEntityType"/>。</param>
        /// <returns>返回字符串。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        protected virtual string GetEntityName(IEntityType entityType)
            => entityType.ClrType.GetDisplayNameWithNamespace();

        /// <summary>
        /// 获取实体类型程序集名称。
        /// </summary>
        /// <param name="entityType">给定的 <see cref="IEntityType"/>。</param>
        /// <returns>返回字符串。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        protected virtual string GetEntityAssemblyName(IEntityType entityType)
            => entityType.ClrType.GetAssemblyDisplayName();

        /// <summary>
        /// 获取实体类型描述。
        /// </summary>
        /// <param name="entityType">给定的 <see cref="IEntityType"/>。</param>
        /// <returns>返回字符串。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        protected virtual string GetEntityDescription(IEntityType entityType)
            => entityType.ClrType.TryGetCustomAttribute(out DescriptionAttribute descr)
            ? descr.Description
            : null;

        /// <summary>
        /// 实体类型是否分表。
        /// </summary>
        /// <param name="entityType">给定的 <see cref="IEntityType"/>。</param>
        /// <returns>返回布尔值。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        protected virtual bool IsEntitySharding(IEntityType entityType)
            => entityType.ClrType.IsDefined<ShardableAttribute>();

    }
}
