#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data
{
    using Core;

    /// <summary>
    /// 访问器接口。
    /// </summary>
    public interface IAccessor : IDisposable
    {
        /// <summary>
        /// 服务提供程序。
        /// </summary>
        IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// 服务工厂。
        /// </summary>
        ServiceFactoryDelegate ServiceFactory { get; }


        /// <summary>
        /// 执行 SQL 命令。
        /// </summary>
        /// <param name="sql">给定的 SQL 语句。</param>
        /// <param name="parameters">给定的参数集合。</param>
        /// <returns>返回受影响的行数。</returns>
        int ExecuteSqlCommand(string sql, params object[] parameters);

        /// <summary>
        /// 异步执行 SQL 命令。
        /// </summary>
        /// <param name="sql">给定的 SQL 语句。</param>
        /// <param name="parameters">给定的参数集合。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含受影响行数的异步操作。</returns>
        Task<int> ExecuteSqlCommandAsync(string sql, IEnumerable<object> parameters, CancellationToken cancellationToken = default);


        /// <summary>
        /// 重载保存更改。
        /// </summary>
        /// <returns>返回受影响的行数。</returns>
        int SaveChanges();

        /// <summary>
        /// 重载保存更改。
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">指示是否在更改已成功发送到数据库之后调用。</param>
        /// <returns>返回受影响的行数。</returns>
        int SaveChanges(bool acceptAllChangesOnSuccess);

        /// <summary>
        /// 重载异步保存更改。
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">指示是否在更改已成功发送到数据库之后调用。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含受影响行数的异步操作。</returns>
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);


        /// <summary>
        /// 迁移。
        /// </summary>
        void Migrate();

        /// <summary>
        /// 异步迁移。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        Task MigrateAsync(CancellationToken cancellationToken = default);


        /// <summary>
        /// 切换租户。
        /// </summary>
        /// <param name="changeConnectionStringFactory">给定改变租户数据库连接的工厂方法。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回是否切换的布尔值。</returns>
        bool ToggleTenant(Func<ITenant, string> changeConnectionStringFactory, CancellationToken cancellationToken = default);
    }
}
