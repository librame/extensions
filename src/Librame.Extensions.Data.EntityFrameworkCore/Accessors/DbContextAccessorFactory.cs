#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 数据库上下文访问器工厂。
    /// </summary>
    /// <typeparam name="TDbContextAccessor">指定的数据库上下文访问器类型。</typeparam>
    public class DbContextAccessorFactory<TDbContextAccessor> : IDesignTimeDbContextFactory<TDbContextAccessor>
        where TDbContextAccessor : DbContextAccessor
    {
        private readonly IServiceProvider _serviceProvider;


        /// <summary>
        /// 构造一个 <see cref="DbContextAccessorFactory{TDbContextAccessor}"/>。
        /// </summary>
        /// <param name="serviceProvider">给定的 <see cref="IServiceProvider"/>。</param>
        public DbContextAccessorFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider.NotNull(nameof(serviceProvider));
        }


        /// <summary>
        /// 创建数据库上下文。
        /// </summary>
        /// <param name="args">给定的参数数组。</param>
        /// <returns>返回 <typeparamref name="TDbContextAccessor"/>。</returns>
        public virtual TDbContextAccessor CreateDbContext(string[] args)
            => _serviceProvider.GetRequiredService<TDbContextAccessor>();
    }
}
