﻿#region License

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
        /// <param name="builderOptions">给定的 <see cref="DataBuilderOptions"/>（可选）。</param>
        /// <param name="configuration">给定的 <see cref="IConfiguration"/>（可选）。</param>
        /// <param name="postConfigureOptions">给定的 <see cref="Action{DataBuilderOptions}"/>（可选）。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddData(this IBuilder builder, DataBuilderOptions builderOptions = null,
            IConfiguration configuration = null, Action<DataBuilderOptions> postConfigureOptions = null)
        {
            return builder.AddData<DataBuilderOptions>(builderOptions ?? new DataBuilderOptions(),
                configuration, postConfigureOptions);
        }
        /// <summary>
        /// 添加数据扩展。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IBuilder"/>。</param>
        /// <param name="builderOptions">给定的构建器选项。</param>
        /// <param name="configuration">给定的 <see cref="IConfiguration"/>（可选）。</param>
        /// <param name="postConfigureOptions">给定的 <see cref="Action{TBuilderOptions}"/>（可选）。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddData<TBuilderOptions>(this IBuilder builder, TBuilderOptions builderOptions,
            IConfiguration configuration = null, Action<TBuilderOptions> postConfigureOptions = null)
            where TBuilderOptions : DataBuilderOptions
        {
            if (builderOptions.AuditPropertyTable.IsDefault())
                builderOptions.AuditPropertyTable = new EveryWeekShardingOptions();

            return builder.AddBuilder(b =>
            {
                return b.AsDataBuilder()
                    .AddAudits();
            },
            builderOptions, configuration, postConfigureOptions);
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
        /// 添加审计。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddAudits(this IDataBuilder builder)
        {
            builder.Services.AddSingleton<IAuditResolver, DefaultAuditResolver>();

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
            builder.Services.AddDbContext<TDbContext>((provider, optionsBuilder) =>
            {
                var options = provider.GetRequiredService<IOptions<DataBuilderOptions>>().Value;
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
            builder.AddDbContext<TDbContextImplementation>(configureOptions);

            builder.Services.AddScoped<TDbContextService>(provider =>
            {
                return provider.GetRequiredService<TDbContextImplementation>();
            });

            return builder;
        }

    }
}
