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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Librame.Builders
{
    using Extensions;
    using Extensions.Data;

    /// <summary>
    /// 数据构建器静态扩展。
    /// </summary>
    public static class DataBuilderExtensions
    {

        /// <summary>
        /// 添加数据扩展。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IBuilder"/>。</param>
        /// <param name="configureOptions">给定的 <see cref="Action{DataBuilderOptions}"/>（可选）。</param>
        /// <param name="configuration">给定的 <see cref="IConfiguration"/>（可选）。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddData(this IBuilder builder,
            Action<DataBuilderOptions> configureOptions = null, IConfiguration configuration = null)
        {
            return builder.AddData<DataBuilderOptions>(configureOptions, configuration);
        }
        /// <summary>
        /// 添加数据扩展。
        /// </summary>
        /// <typeparam name="TBuilderOptions">指定的构建器选项类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IBuilder"/>。</param>
        /// <param name="configureOptions">给定的构建器选项配置动作（可选）。</param>
        /// <param name="configuration">给定的 <see cref="IConfiguration"/>（可选）。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddData<TBuilderOptions>(this IBuilder builder,
            Action<TBuilderOptions> configureOptions = null, IConfiguration configuration = null)
            where TBuilderOptions : DataBuilderOptions, new()
        {
            Action<TBuilderOptions> _configureOptions = options =>
            {
                options.AuditTable = new TableSchema<Audit>();
                options.AuditPropertyTable = new EveryWeekShardingSchema();
                options.TenantTable = new TableSchema<Tenant>();

                configureOptions?.Invoke(options);
            };

            return builder.AddBuilder(_configureOptions, configuration, _builder =>
            {
                return _builder.AsDataBuilder()
                    .AddChangeHandlers();
            });
        }


        /// <summary>
        /// 转换为数据构建器。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IBuilder"/>。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AsDataBuilder(this IBuilder builder)
        {
            return new InternalDataBuilder(builder);
        }

        /// <summary>
        /// 添加变化处理程序集合。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddChangeHandlers(this IDataBuilder builder)
        {
            builder.Services.AddScoped<IChangeHandler, AuditChangeHandler>();
            builder.Services.AddScoped<IChangeHandler, TenantChangeHandler>();

            builder.Services.AddScoped<IChangeTrackerContext, ChangeTrackerContext>();
            builder.Services.AddScoped<ITenantContext, TenantContext>();

            return builder;
        }


        /// <summary>
        /// 添加数据库上下文。
        /// </summary>
        /// <typeparam name="TDbContext">指定的数据库上下文类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <param name="configureOptions">给定的 <see cref="Action{DataBuilderOptions, DbContextOptionsBuilder}"/>（可选）。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddDbContext<TDbContext>(this IDataBuilder builder,
            Action<DataBuilderOptions, DbContextOptionsBuilder> configureOptions = null)
            where TDbContext : DbContext, IDbContext
        {
            return builder.AddDbContext<TDbContext, DataBuilderOptions>(configureOptions);
        }
        /// <summary>
        /// 添加数据库上下文。
        /// </summary>
        /// <typeparam name="TDbContext">指定的数据库上下文类型。</typeparam>
        /// <typeparam name="TBuilderOptions">指定的构建器选项类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <param name="configureOptions">给定的 <see cref="Action{TBuilderOptions, DbContextOptionsBuilder}"/>（可选）。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddDbContext<TDbContext, TBuilderOptions>(this IDataBuilder builder,
            Action<TBuilderOptions, DbContextOptionsBuilder> configureOptions = null)
            where TDbContext : DbContext, IDbContext
            where TBuilderOptions : DataBuilderOptions, new()
        {
            builder.Services.AddDbContext<TDbContext>((provider, optionsBuilder) =>
            {
                var options = provider.GetRequiredService<IOptions<TBuilderOptions>>().Value;
                configureOptions?.Invoke(options, optionsBuilder);
            });

            return builder;
        }

        /// <summary>
        /// 添加数据库上下文。
        /// </summary>
        /// <typeparam name="TDbContextService">指定的数据库上下文服务类型。</typeparam>
        /// <typeparam name="TDbContextImplementation">指定的数据库上下文实现类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <param name="configureOptions">给定的 <see cref="Action{DataBuilderOptions, DbContextOptionsBuilder}"/>（可选）。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddDbContext<TDbContextService, TDbContextImplementation>(this IDataBuilder builder,
            Action<DataBuilderOptions, DbContextOptionsBuilder> configureOptions = null)
            where TDbContextService : class, IDbContext
            where TDbContextImplementation : DbContext, TDbContextService
        {
            return builder.AddDbContext<TDbContextService, TDbContextImplementation, DataBuilderOptions>(configureOptions);
        }
        /// <summary>
        /// 添加数据库上下文。
        /// </summary>
        /// <typeparam name="TDbContextService">指定的数据库上下文服务类型。</typeparam>
        /// <typeparam name="TDbContextImplementation">指定的数据库上下文实现类型。</typeparam>
        /// <typeparam name="TBuilderOptions">指定的构建器选项类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <param name="configureOptions">给定的 <see cref="Action{TBuilderOptions, DbContextOptionsBuilder}"/>（可选）。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddDbContext<TDbContextService, TDbContextImplementation, TBuilderOptions>(this IDataBuilder builder,
            Action<TBuilderOptions, DbContextOptionsBuilder> configureOptions = null)
            where TDbContextService : class, IDbContext
            where TDbContextImplementation : DbContext, TDbContextService
            where TBuilderOptions : DataBuilderOptions, new()
        {
            builder.AddDbContext<TDbContextImplementation, TBuilderOptions>(configureOptions);

            builder.Services.AddScoped<TDbContextService>(provider =>
            {
                return provider.GetRequiredService<TDbContextImplementation>();
            });

            return builder;
        }

    }
}
