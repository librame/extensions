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
        /// 添加数据。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IBuilder"/>。</param>
        /// <param name="configuration">给定的 <see cref="IConfiguration"/>。</param>
        /// <param name="configureOptions">给定的 <see cref="Action{IDataBuilderOptions}"/>。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddData(this IBuilder builder,
            IConfiguration configuration = null, Action<IDataBuilderOptions> configureOptions = null)
        {
            return builder.AddData<DefaultDataBuilderOptions>(configuration, configureOptions);
        }
        /// <summary>
        /// 添加数据。
        /// </summary>
        /// <typeparam name="TBuilderOptions">指定的构建器选项类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IBuilder"/>。</param>
        /// <param name="configuration">给定的 <see cref="IConfiguration"/>。</param>
        /// <param name="configureOptions">给定的 <see cref="Action{TBuilderOptions}"/>。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddData<TBuilderOptions>(this IBuilder builder,
            IConfiguration configuration = null, Action<TBuilderOptions> configureOptions = null)
            where TBuilderOptions : class, IDataBuilderOptions
        {
            if (configuration.IsNotDefault())
                builder.Services.Configure<TBuilderOptions>(configuration);

            if (configureOptions.IsNotDefault())
                builder.Services.Configure(configureOptions);
            
            var dataBuilder = builder.AsDataBuilder();

            dataBuilder.AddAudit();

            return dataBuilder;
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
        public static IDataBuilder AddAudit(this IDataBuilder builder)
        {
            builder.Services.AddSingleton<IAuditResolverService, DefaultAuditResolverService>();

            return builder;
        }

        /// <summary>
        /// 添加数据库上下文。
        /// </summary>
        /// <typeparam name="TIDbContext">指定的数据库上下文接口类型。</typeparam>
        /// <typeparam name="TDbContext">指定的数据库上下文类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <param name="configureOptions">给定的 <see cref="Action{IEfCoreDataBuilderOptions}"/>（可选）。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddDbContext<TIDbContext, TDbContext>(this IDataBuilder builder,
            Action<IEfCoreDataBuilderOptions> configureOptions = null)
            where TIDbContext : class, IDbContext
            where TDbContext : DbContext, TIDbContext
        {
            var options = new DefaultEfCoreDataBuilderOptions();
            builder.Services.AddSingleton<IEfCoreDataBuilderOptions>(options);

            configureOptions?.Invoke(options);
            
            return builder.AddDbContext<TIDbContext, TDbContext>(options);
        }
        /// <summary>
        /// 添加数据库上下文。
        /// </summary>
        /// <typeparam name="TIDbContext">指定的数据库上下文接口类型。</typeparam>
        /// <typeparam name="TDbContext">指定的数据库上下文类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <param name="options">给定的 <see cref="IEfCoreDataBuilderOptions"/>。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddDbContext<TIDbContext, TDbContext>(this IDataBuilder builder,
            IEfCoreDataBuilderOptions options)
            where TIDbContext : class, IDbContext
            where TDbContext : DbContext, TIDbContext
        {
            if (options.ResolveDbContextOptions != null)
            {
                builder.Services.AddDbContext<TDbContext>(options.ResolveDbContextOptions);
            }
            else
            {
                builder.Services.AddDbContext<TDbContext>(dbContextBuilder =>
                {
                    options.ConfigureDbContext?.Invoke(dbContextBuilder);
                });
            }

            builder.Services.AddScoped<TIDbContext, TDbContext>();

            return builder;
        }

    }
}
