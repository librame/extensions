#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions;
using Librame.Extensions.Data.Accessors;
using Librame.Extensions.Data.Builders;
using Librame.Extensions.Data.Migrations;
using Librame.Extensions.Data.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 访问器数据构建器静态扩展。
    /// </summary>
    public static class AccessorDataBuilderExtensions
    {
        private static readonly Type _dbContextAccessorTypeDefinition
            = typeof(IDbContextAccessor<,,,,>);


        /// <summary>
        /// 添加服务类型为 <see cref="IAccessor"/> 的数据库上下文访问器。
        /// </summary>
        /// <typeparam name="TAccessor">指定派生自 <see cref="DbContextAccessor{TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/> 的访问器类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <param name="setupAction">给定的 <see cref="Action{DataBuilderOptions, DbContextOptionsBuilder}"/>。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddAccessor<TAccessor>(this IDataBuilder builder,
            Action<ITenant, DbContextOptionsBuilder> setupAction)
            where TAccessor : DbContext, IAccessor
            => builder.AddAccessor<IAccessor, TAccessor>(setupAction);

        /// <summary>
        /// 添加数据库上下文访问器。
        /// </summary>
        /// <typeparam name="TAccessor">指定派生自 <see cref="IDbContextAccessor{TAudit, TAuditProperty, TEntity, TMigration, TTenant}"/> 的访问器类型。</typeparam>
        /// <typeparam name="TImplementation">指定派生自 <see cref="DbContextAccessor{TAudit, TAuditProperty, TEntity, TMigration, TTenant, TGenId, TIncremId}"/> 的访问器实现类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <param name="setupAction">给定的 <see cref="Action{ITenant, DbContextOptionsBuilder}"/>。</param>
        /// <param name="poolSize">设置池保留的最大实例数（可选；默认为128，如果小于1，将使用 AddDbContext() 注册）。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static IDataBuilder AddAccessor<TAccessor, TImplementation>(this IDataBuilder builder,
            Action<ITenant, DbContextOptionsBuilder> setupAction, int poolSize = 128)
            where TAccessor : class, IAccessor
            where TImplementation : DbContext, TAccessor
        {
            builder.NotNull(nameof(builder));
            setupAction.NotNull(nameof(setupAction));

            var implAccessorType = typeof(TImplementation);
            if (!implAccessorType.IsImplementedInterface(_dbContextAccessorTypeDefinition))
                throw new ArgumentException($"The accessor type '{implAccessorType}' does not implement interface '{_dbContextAccessorTypeDefinition}'");

            if (poolSize > 0)
            {
                builder.Services.AddDbContextPool<TAccessor, TImplementation>((sp, optionsBuilder) =>
                {
                    var options = sp.GetRequiredService<IOptions<DataBuilderOptions>>().Value;
                    setupAction.Invoke(options.DefaultTenant, optionsBuilder);

                    optionsBuilder.ReplaceServices();
                },
                poolSize);
            }
            else
            {
                builder.Services.AddDbContext<TAccessor, TImplementation>((sp, optionsBuilder) =>
                {
                    var options = sp.GetRequiredService<IOptions<DataBuilderOptions>>().Value;
                    setupAction.Invoke(options.DefaultTenant, optionsBuilder);

                    optionsBuilder.ReplaceServices();
                });
            }

            builder.Services.TryAddScoped(sp => (TImplementation)sp.GetRequiredService<TAccessor>());

            return builder.AddDesignTimeServices<TImplementation>();
        }


        private static IDataBuilder AddDesignTimeServices<TDbContext>(this IDataBuilder builder)
           where TDbContext : DbContext, IAccessor
        {
            builder.Services.AddTransient(sp => sp.GetRequiredService<TDbContext>().GetService<ICurrentDbContext>());
            builder.Services.AddTransient(sp => sp.GetRequiredService<TDbContext>().GetService<IDatabaseProvider>());
            builder.Services.AddTransient(sp => sp.GetRequiredService<TDbContext>().GetService<IDbContextOptions>());
            builder.Services.AddTransient(sp => sp.GetRequiredService<TDbContext>().GetService<IHistoryRepository>());
            builder.Services.AddTransient(sp => sp.GetRequiredService<TDbContext>().GetService<IMigrationsAssembly>());
            builder.Services.AddTransient(sp => sp.GetRequiredService<TDbContext>().GetService<IMigrationsIdGenerator>());
            builder.Services.AddTransient(sp => sp.GetRequiredService<TDbContext>().GetService<IMigrationsModelDiffer>());
            builder.Services.AddTransient(sp => sp.GetRequiredService<TDbContext>().GetService<IMigrator>());
            //builder.Services.AddTransient(sp => sp.GetRequiredService<TDbContext>().GetService<IRelationalTypeMappingSource>());
            builder.Services.AddTransient(sp => sp.GetRequiredService<TDbContext>().GetService<IModel>());

            return builder;
        }


        /// <summary>
        /// 替换默认服务集合（<see cref="DbContext.OnConfiguring(DbContextOptionsBuilder)"/> 配置不支持 <see cref="EntityFrameworkServiceCollectionExtensions.AddDbContextPool{TContext}(IServiceCollection, Action{DbContextOptionsBuilder}, int)"/> 的替换模式）。
        /// </summary>
        /// <param name="optionsBuilder">给定的 <see cref="DbContextOptionsBuilder"/>。</param>
        private static void ReplaceServices(this DbContextOptionsBuilder optionsBuilder)
        {
            // 在使用自定义迁移服务时，利用模型缓存键工厂使模型缓存失效后重新映射表名来达到分表的方法同样会变成重命名及修改的迁移操作而导致失败。
            //optionsBuilder.ReplaceService<IModelCacheKeyFactory, DbContextAccessorModelCacheKeyFactory>();

            optionsBuilder.ReplaceService<IMigrationsModelDiffer, ShardingMigrationsModelDiffer>();
        }

    }
}
