#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Configuration;
using System;

namespace Librame.Extensions.Data
{
    using Core;

    /// <summary>
    /// 数据构建器静态扩展。
    /// </summary>
    public static class DataBuilderExtensions
    {
        /// <summary>
        /// 添加数据扩展。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IBuilder"/>。</param>
        /// <param name="configureOptions">给定的 <see cref="Action{DataBuilderOptions}"/>（可选；高优先级）。</param>
        /// <param name="configuration">给定的 <see cref="IConfiguration"/>（可选；次优先级）。</param>
        /// <param name="configureBinderOptions">给定的配置绑定器选项动作（可选）。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddData(this IBuilder builder,
            Action<DataBuilderOptions> configureOptions = null,
            IConfiguration configuration = null,
            Action<BinderOptions> configureBinderOptions = null)
        {
            var options = builder.Configure(configureOptions,
                configuration, configureBinderOptions);

            var dataBuilder = new InternalDataBuilder(builder, options);

            return dataBuilder
                .AddMediators()
                .AddServices();
        }


        ///// <summary>
        ///// 添加数据库上下文。
        ///// </summary>
        ///// <typeparam name="TDbContext">指定的数据库上下文类型。</typeparam>
        ///// <param name="builder">给定的 <see cref="DataBuilder"/>。</param>
        ///// <param name="configureOptions">给定的 <see cref="Action{DataBuilderOptions, DbContextOptionsBuilder}"/>（可选）。</param>
        ///// <returns>返回 <see cref="DataBuilder"/>。</returns>
        //public static DataBuilder AddDbContext<TDbContext>(this DataBuilder builder,
        //    Action<DataBuilderOptions, DbContextOptionsBuilder> configureOptions = null)
        //    where TDbContext : DbContext, IDbContext
        //{
        //    return builder.AddDbContext<TDbContext, DataBuilderOptions>(configureOptions);
        //}
        ///// <summary>
        ///// 添加数据库上下文。
        ///// </summary>
        ///// <typeparam name="TDbContext">指定的数据库上下文类型。</typeparam>
        ///// <typeparam name="TBuilderOptions">指定的构建器选项类型。</typeparam>
        ///// <param name="builder">给定的 <see cref="DataBuilder"/>。</param>
        ///// <param name="configureOptions">给定的 <see cref="Action{TBuilderOptions, DbContextOptionsBuilder}"/>（可选）。</param>
        ///// <returns>返回 <see cref="DataBuilder"/>。</returns>
        //public static DataBuilder AddDbContext<TDbContext, TBuilderOptions>(this DataBuilder builder,
        //    Action<TBuilderOptions, DbContextOptionsBuilder> configureOptions = null)
        //    where TDbContext : DbContext, IDbContext
        //    where TBuilderOptions : DataBuilderOptions, new()
        //{
        //    builder.Services.AddDbContext<TDbContext>((provider, optionsBuilder) =>
        //    {
        //        var options = provider.GetRequiredService<IOptions<TBuilderOptions>>().Value;
        //        configureOptions?.Invoke(options, optionsBuilder);
        //    });

        //    builder.Services.AddEntityFrameworkDesignTimeServices();

        //    return builder;
        //}

        ///// <summary>
        ///// 添加数据库上下文。
        ///// </summary>
        ///// <typeparam name="TDbContextService">指定的数据库上下文服务类型。</typeparam>
        ///// <typeparam name="TDbContextImplementation">指定的数据库上下文实现类型。</typeparam>
        ///// <param name="builder">给定的 <see cref="DataBuilder"/>。</param>
        ///// <param name="configureOptions">给定的 <see cref="Action{DataBuilderOptions, DbContextOptionsBuilder}"/>（可选）。</param>
        ///// <returns>返回 <see cref="DataBuilder"/>。</returns>
        //public static DataBuilder AddDbContext<TDbContextService, TDbContextImplementation>(this DataBuilder builder,
        //    Action<DataBuilderOptions, DbContextOptionsBuilder> configureOptions = null)
        //    where TDbContextService : class, IDbContext
        //    where TDbContextImplementation : DbContext, TDbContextService
        //{
        //    return builder.AddDbContext<TDbContextService, TDbContextImplementation, DataBuilderOptions>(configureOptions);
        //}
        ///// <summary>
        ///// 添加数据库上下文。
        ///// </summary>
        ///// <typeparam name="TDbContextService">指定的数据库上下文服务类型。</typeparam>
        ///// <typeparam name="TDbContextImplementation">指定的数据库上下文实现类型。</typeparam>
        ///// <typeparam name="TBuilderOptions">指定的构建器选项类型。</typeparam>
        ///// <param name="builder">给定的 <see cref="DataBuilder"/>。</param>
        ///// <param name="configureOptions">给定的 <see cref="Action{TBuilderOptions, DbContextOptionsBuilder}"/>（可选）。</param>
        ///// <returns>返回 <see cref="DataBuilder"/>。</returns>
        //public static DataBuilder AddDbContext<TDbContextService, TDbContextImplementation, TBuilderOptions>(this DataBuilder builder,
        //    Action<TBuilderOptions, DbContextOptionsBuilder> configureOptions = null)
        //    where TDbContextService : class, IDbContext
        //    where TDbContextImplementation : DbContext, TDbContextService
        //    where TBuilderOptions : DataBuilderOptions, new()
        //{
        //    builder.AddDbContext<TDbContextImplementation, TBuilderOptions>(configureOptions);

        //    builder.Services.AddScoped<TDbContextService>(provider =>
        //    {
        //        return provider.GetRequiredService<TDbContextImplementation>();
        //    });

        //    return builder;
        //}

    }
}
