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
    ///// <see cref="DbContextOptionsBuilder"/> 静态扩展。
    ///// </summary>
    //public static class DataBuilderDbContextOptionsExtensions
    //{
    //    /// <summary>
    //    /// 使用数据构建器。
    //    /// </summary>
    //    /// <param name="optionsBuilder">给定的 <see cref="DbContextOptionsBuilder"/>。</param>
    //    /// <param name="dataBuilder">给定的 <see cref="IDataBuilder"/>。</param>
    //    /// <param name="serviceProvider">给定的 <see cref="IServiceProvider"/>。</param>
    //    /// <param name="dataBuilderOptionsAction">给定的配置动作（可选）。</param>
    //    /// <returns>返回 <see cref="DataBuilderDbContextOptionsExtension"/>。</returns>
    //    [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
    //    public static DbContextOptionsBuilder UseDataBuilder(this DbContextOptionsBuilder optionsBuilder,
    //        IDataBuilder dataBuilder, IServiceProvider serviceProvider,
    //        Action<DataBuilderDbContextOptionsBuilder> dataBuilderOptionsAction = null)
    //    {
    //        optionsBuilder.NotNull(nameof(optionsBuilder));
    //        dataBuilder.NotNull(nameof(dataBuilder));

    //        var extension = GetOrCreateExtension(optionsBuilder).WithDataBuilder(dataBuilder, serviceProvider);
    //        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);

    //        dataBuilderOptionsAction?.Invoke(new DataBuilderDbContextOptionsBuilder(optionsBuilder));

    //        return optionsBuilder;
    //    }


    //    /// <summary>
    //    /// 获取或创建扩展。
    //    /// </summary>
    //    /// <param name="optionsBuilder">给定的 <see cref="DbContextOptionsBuilder"/>。</param>
    //    /// <returns>返回 <see cref="DataBuilderDbContextOptionsExtension"/>。</returns>
    //    internal static DataBuilderDbContextOptionsExtension GetOrCreateExtension(DbContextOptionsBuilder optionsBuilder)
    //        => optionsBuilder.Options.FindExtension<DataBuilderDbContextOptionsExtension>()
    //            ?? new DataBuilderDbContextOptionsExtension();

    //}
}
