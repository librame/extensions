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
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 数据库提供程序接口。
    /// </summary>
    public interface IDbProvider : IDisposable
    {
        /// <summary>
        /// 提供程序名称。
        /// </summary>
        string DbProviderName { get; }

        /// <summary>
        /// 启用自动事务。
        /// </summary>
        bool AutoTransactionsEnabled { get; set; }


        /// <summary>
        /// 确保数据库已创建。
        /// </summary>
        /// <returns>返回布尔值。</returns>
        bool DatabaseCreated();

        /// <summary>
        /// 异步确保数据库已创建。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含布尔值的异步操作。</returns>
        Task<bool> DatabaseCreatedAsync(CancellationToken cancellationToken = default);


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
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="parameters">给定的参数集合。</param>
        /// <returns>返回一个包含受影响行数的异步操作。</returns>
        Task<int> ExecuteSqlCommandAsync(string sql, IEnumerable<object> parameters, CancellationToken cancellationToken = default);


        /// <summary>
        /// 获取数据库连接。
        /// </summary>
        /// <returns>返回 <see cref="DbConnection"/>。</returns>
        DbConnection GetDbConnection();


        /// <summary>
        /// 保存变化。
        /// </summary>
        /// <returns>返回受影响的行数。</returns>
        int SaveChanges();

        /// <summary>
        /// 异步保存变化。
        /// </summary>
        /// <returns>返回一个包含受影响行数的异步操作。</returns>
        Task<int> SaveChangesAsync();


        /// <summary>
        /// 切换数据库连接执行。
        /// </summary>
        /// <param name="processAction">给定的处理动作。</param>
        void SwitchConnectionProcess(Action processAction);
        /// <summary>
        /// 切换数据库连接执行。
        /// </summary>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="processFactory">给定的处理工厂方法。</param>
        /// <returns>返回结果实例。</returns>
        TResult SwitchConnectionProcess<TResult>(Func<TResult> processFactory);


        /// <summary>
        /// 尝试切换数据库连接。
        /// </summary>
        /// <param name="connectionStringFactory">给定的数据库连接字符串工厂方法。</param>
        /// <returns>返回是否切换的布尔值。</returns>
        bool TrySwitchConnection(Func<IConnectionStrings, string> connectionStringFactory);
        /// <summary>
        /// 尝试切换数据库连接。
        /// </summary>
        /// <param name="connectionString">给定的数据库连接字符串。</param>
        /// <returns>返回是否切换的布尔值。</returns>
        bool TrySwitchConnection(string connectionString);
    }
}
