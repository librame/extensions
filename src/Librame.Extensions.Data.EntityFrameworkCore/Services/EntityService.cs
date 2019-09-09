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
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data
{
    using Core;

    /// <summary>
    /// 实体服务。
    /// </summary>
    public class EntityService : ExtensionBuilderServiceBase<DataBuilderOptions>, IEntityService
    {
        private static readonly List<DataEntity> _cache
            = new List<DataEntity>();


        /// <summary>
        /// 构造一个 <see cref="EntityService"/>。
        /// </summary>
        /// <param name="identifier">给定的 <see cref="IStoreIdentifier"/>。</param>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="options">给定的 <see cref="IOptions{DataBuilderOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public EntityService(IStoreIdentifier identifier, IClockService clock,
            IOptions<DataBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(options, loggerFactory)
        {
            Identifier = identifier;
            Clock = clock;
        }


        /// <summary>
        /// 时钟服务。
        /// </summary>
        protected IClockService Clock { get; }

        /// <summary>
        /// 标识符服务。
        /// </summary>
        protected IStoreIdentifier Identifier { get; }


        /// <summary>
        /// 异步注册。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="List{Table}"/> 的异步操作。</returns>
        public async Task<List<DataEntity>> RegistAsync(IAccessor accessor,
            CancellationToken cancellationToken = default)
        {
            if (accessor.IsNotNull() && accessor is DbContextAccessor dbContextAccessor)
            {
                if (_cache.IsNullOrEmpty())
                {
                    var tables = dbContextAccessor.Tables.ToList();
                    if (tables.IsNotNullOrEmpty())
                        _cache.AddRange(tables);
                }

                var diff = dbContextAccessor.Model.GetEntityTypes()
                    .Where(et => !et.IsQueryType)
                    .Select(s => ToTable(s, cancellationToken))
                    .ToList();

                if (_cache.IsNotNullOrEmpty())
                    diff = diff.Except(_cache).ToList();

                if (diff.IsNotNullOrEmpty())
                    await AddAsync(dbContextAccessor, diff, cancellationToken);

                return diff;
            }

            return null;
        }

        /// <summary>
        /// 异步增加。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="DbContextAccessor"/>。</param>
        /// <param name="tables">给定要处理的实体列表。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个异步操作。</returns>
        protected virtual async Task AddAsync(DbContextAccessor accessor, List<DataEntity> tables,
            CancellationToken cancellationToken = default)
        {
            await accessor.Tables.AddRangeAsync(tables, cancellationToken);
            _cache.AddRange(tables);

            var mediator = accessor.ServiceFactory.GetRequiredService<IMediator>();
            await mediator.Publish(new EntityNotification { Entities = tables });
        }

        /// <summary>
        /// 转换为实体表。
        /// </summary>
        /// <param name="entityType">给定的 <see cref="IEntityType"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回 <see cref="DataEntity"/>。</returns>
        protected virtual DataEntity ToTable(IEntityType entityType, CancellationToken cancellationToken)
        {
            var table = new DataEntity
            {
                Id = Identifier.GetEntityIdAsync(cancellationToken).Result,
                EntityName = entityType.ClrType.GetSimpleFullName(),
                AssemblyName = entityType.ClrType.GetSimpleAssemblyName(),
                CreatedTime = Clock.GetOffsetNowAsync(cancellationToken: cancellationToken).Result,
                CreatedBy = nameof(EntityService)
            };

            if (entityType.ClrType.TryGetCustomAttribute(out DescriptionAttribute descr)
                && descr.Description.IsNotNullOrEmpty())
            {
                table.Description = descr.Description;
            }

            var relational = entityType?.Relational();
            if (relational.IsNotNull())
            {
                table.Name = relational.TableName;

                if (relational.Schema.IsNotNullOrEmpty())
                    table.Schema = relational.Schema;
            }

            if (table.Name.IsNullOrEmpty())
                SetDefaultTableName(table, entityType);

            if (table.Schema.IsNullOrEmpty())
                SetDefaultTableSchema(table, entityType);

            return table;
        }

        /// <summary>
        /// 设置默认表名。
        /// </summary>
        /// <param name="table">给定的 <see cref="DataEntity"/>。</param>
        /// <param name="entityType">给定的实体类型。</param>
        protected virtual void SetDefaultTableName(DataEntity table, IEntityType entityType)
            => table.Name = entityType.ClrType.Name;

        /// <summary>
        /// 设置默认架构。
        /// </summary>
        /// <param name="table">给定的 <see cref="DataEntity"/>。</param>
        /// <param name="entityType">给定的实体类型。</param>
        protected virtual void SetDefaultTableSchema(DataEntity table, IEntityType entityType)
            => table.Schema = Options.Tables.DefaultSchema.EnsureString("dbo");


        /// <summary>
        /// 释放实体表缓存。
        /// </summary>
        protected override void DisposeCore()
            => _cache.Clear();

    }
}
