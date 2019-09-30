#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data
{
    using Core;

    /// <summary>
    /// 数据实体迁移访问器截面。
    /// </summary>
    public class DataEntityMigrateAccessorAspect : MigrateAccessorAspectBase
    {
        private static List<DataEntity> _cache = new List<DataEntity>();


        /// <summary>
        /// 构造一个 <see cref="DataEntityMigrateAccessorAspect"/>。
        /// </summary>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="identifier">给定的 <see cref="IStoreIdentifier"/>。</param>
        /// <param name="options">给定的 <see cref="IOptions{DataBuilderOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public DataEntityMigrateAccessorAspect(IClockService clock, IStoreIdentifier identifier,
            IOptions<DataBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(clock, identifier, options, loggerFactory)
        {
        }


        /// <summary>
        /// 数据实体缓存。
        /// </summary>
        public IReadOnlyList<DataEntity> Cache { get; }
            = _cache.AsReadOnlyList();


        /// <summary>
        /// 启用截面。
        /// </summary>
        public override bool Enabled
            => Options.EntityEnabled;


        /// <summary>
        /// 后置处理核心。
        /// </summary>
        /// <param name="dbContextAccessor">给定的 <see cref="DbContextAccessor"/>。</param>
        protected override void PostprocessCore(DbContextAccessor dbContextAccessor)
        {
            var diff = GetDifference(dbContextAccessor);
            if (diff.IsNotEmpty())
            {
                dbContextAccessor.Entities.AddRange(diff);
                _cache.AddRange(diff);
                RequiredSaveChanges = true;

                var mediator = dbContextAccessor.ServiceFactory.GetRequiredService<IMediator>();
                mediator.Publish(new DataEntityNotification { Entities = diff }).ConfigureAndWait();
            }
        }

        /// <summary>
        /// 异步后置处理核心。
        /// </summary>
        /// <param name="dbContextAccessor">给定的 <see cref="DbContextAccessor"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        protected override async Task PostprocessCoreAsync(DbContextAccessor dbContextAccessor,
            CancellationToken cancellationToken = default)
        {
            var diff = GetDifference(dbContextAccessor, cancellationToken);
            if (diff.IsNotEmpty())
            {
                await dbContextAccessor.Entities.AddRangeAsync(diff, cancellationToken).ConfigureAndWaitAsync();
                _cache.AddRange(diff);
                RequiredSaveChanges = true;

                var mediator = dbContextAccessor.ServiceFactory.GetRequiredService<IMediator>();
                await mediator.Publish(new DataEntityNotification { Entities = diff }).ConfigureAndWaitAsync();
            }
        }

        /// <summary>
        /// 获取差异的数据实体。
        /// </summary>
        /// <param name="dbContextAccessor">给定的 <see cref="DbContextAccessor"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="List{DataEntity}"/>。</returns>
        protected virtual List<DataEntity> GetDifference(DbContextAccessor dbContextAccessor,
            CancellationToken cancellationToken = default)
        {
            if (_cache.IsEmpty())
            {
                var entitis = dbContextAccessor.Entities.ToList();
                if (entitis.IsNotEmpty())
                    _cache.AddRange(entitis);
            }

            var diff = dbContextAccessor.Model.GetEntityTypes()
                .Select(s => GetEntity(s, cancellationToken))
                .ToList();

            if (_cache.IsNotEmpty())
                diff = diff.Except(_cache).ToList();

            return diff;
        }


        /// <summary>
        /// 获取数据实体。
        /// </summary>
        /// <param name="entityType">给定的 <see cref="IEntityType"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回 <see cref="DataEntity"/>。</returns>
        protected virtual DataEntity GetEntity(IEntityType entityType, CancellationToken cancellationToken)
        {
            var entity = new DataEntity
            {
                Id = Identifier.GetEntityIdAsync(cancellationToken).ConfigureAndResult(),
                EntityName = entityType.ClrType.GetSimpleFullName(),
                AssemblyName = entityType.ClrType.GetSimpleAssemblyName(),
                CreatedTime = Clock.GetOffsetNowAsync(DateTimeOffset.UtcNow, isUtc: true, cancellationToken).ConfigureAndResult(),
                CreatedBy = nameof(DataEntityMigrateAccessorAspect)
            };

            if (entityType.ClrType.TryGetCustomAttribute(out DescriptionAttribute descr)
                && descr.Description.IsNotEmpty())
            {
                entity.Description = descr.Description;
            }

            entity.Name = entityType.GetTableName();
            entity.Schema = entityType.GetSchema();

            if (entity.Name.IsEmpty())
                SetDefaultTableName(entity, entityType);

            if (entity.Schema.IsEmpty())
                SetDefaultTableSchema(entity, entityType);

            return entity;
        }


        /// <summary>
        /// 设置默认表名。
        /// </summary>
        /// <param name="entity">给定的 <see cref="DataEntity"/>。</param>
        /// <param name="entityType">给定的实体类型。</param>
        protected virtual void SetDefaultTableName(DataEntity entity, IEntityType entityType)
            => entity.Name = entityType.ClrType.Name;

        /// <summary>
        /// 设置默认表架构。
        /// </summary>
        /// <param name="entity">给定的 <see cref="DataEntity"/>。</param>
        /// <param name="entityType">给定的实体类型。</param>
        protected virtual void SetDefaultTableSchema(DataEntity entity, IEntityType entityType)
            => entity.Schema = Options.Tables.DefaultSchema.NotEmptyOrDefault("dbo");


        /// <summary>
        /// 释放实体表缓存。
        /// </summary>
        protected override void DisposeCore()
            => _cache.Clear();
    }
}
