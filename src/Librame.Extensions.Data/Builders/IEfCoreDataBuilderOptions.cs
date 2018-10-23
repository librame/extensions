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
using System;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// EntityFrameworkCore 数据构建器选项接口。
    /// </summary>
    public interface IEfCoreDataBuilderOptions : IDataBuilderOptions
    {
        /// <summary>
        /// 配置数据库上下文。
        /// </summary>
        Action<DbContextOptionsBuilder> ConfigureDbContext { get; set; }

        /// <summary>
        /// 解析数据库上下文选项。
        /// </summary>
        Action<IServiceProvider, DbContextOptionsBuilder> ResolveDbContextOptions { get; set; }
    }
}
