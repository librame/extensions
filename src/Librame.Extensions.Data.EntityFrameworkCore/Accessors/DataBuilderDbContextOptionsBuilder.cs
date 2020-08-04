#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

//using Librame.Extensions;
//using Librame.Extensions.Data.Builders;
//using Microsoft.EntityFrameworkCore.Infrastructure;
//using System;
//using System.Diagnostics.CodeAnalysis;

namespace Microsoft.EntityFrameworkCore
{
    ///// <summary>
    ///// <see cref="IDataBuilder"/> 数据库上下文选项构建器。
    ///// </summary>
    //public class DataBuilderDbContextOptionsBuilder
    //{
    //    /// <summary>
    //    /// 构造一个 <see cref="DataBuilderDbContextOptionsBuilder"/>。
    //    /// </summary>
    //    /// <param name="optionsBuilder">给定的 <see cref="DbContextOptionsBuilder"/>。</param>
    //    public DataBuilderDbContextOptionsBuilder(DbContextOptionsBuilder optionsBuilder)
    //    {
    //        OptionsBuilder = optionsBuilder.NotNull(nameof(optionsBuilder));
    //    }


    //    /// <summary>
    //    /// 数据库上下文选项构建器。
    //    /// </summary>
    //    public DbContextOptionsBuilder OptionsBuilder { get; }


    //    /// <summary>
    //    /// 使用数据构建器。
    //    /// </summary>
    //    /// <param name="dataBuilder">给定的 <see cref="IDataBuilder"/>。</param>
    //    /// <returns>返回 <see cref="DataBuilderDbContextOptionsBuilder"/>。</returns>
    //    public virtual DataBuilderDbContextOptionsBuilder UseDataBuilder(IDataBuilder dataBuilder)
    //        => WithOption(e => e.WithDataBuilder(dataBuilder));

    //    /// <summary>
    //    /// 使用服务提供程序。
    //    /// </summary>
    //    /// <param name="serviceProvider">给定的 <see cref="IServiceProvider"/>。</param>
    //    /// <returns>返回 <see cref="DataBuilderDbContextOptionsBuilder"/>。</returns>
    //    public virtual DataBuilderDbContextOptionsBuilder UseServiceProvider(IServiceProvider serviceProvider)
    //        => WithOption(e => e.WithServiceProvider(serviceProvider));


    //    /// <summary>
    //    /// 带有选项。
    //    /// </summary>
    //    /// <param name="setAction">给定的设置选项动作。</param>
    //    /// <returns>返回 <see cref="DataBuilderDbContextOptionsBuilder"/>。</returns>
    //    [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
    //    protected virtual DataBuilderDbContextOptionsBuilder WithOption
    //        (Func<DataBuilderDbContextOptionsExtension, DataBuilderDbContextOptionsExtension> setAction)
    //    {
    //        var extension = DataBuilderDbContextOptionsExtensions.GetOrCreateExtension(OptionsBuilder);

    //        ((IDbContextOptionsBuilderInfrastructure)OptionsBuilder)
    //            .AddOrUpdateExtension(setAction.Invoke(extension));

    //        return this;
    //    }

    //}
}
