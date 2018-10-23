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
    /// 默认 EntityFrameworkCore 数据构建器选项。
    /// </summary>
    public class DefaultEfCoreDataBuilderOptions : DefaultDataBuilderOptions, IEfCoreDataBuilderOptions
    {
        /// <summary>
        /// 配置数据库上下文。
        /// </summary>
        public Action<DbContextOptionsBuilder> ConfigureDbContext { get; set; }

        /// <summary>
        /// 解析数据库上下文选项。
        /// </summary>
        public Action<IServiceProvider, DbContextOptionsBuilder> ResolveDbContextOptions { get; set; }
    }
}
