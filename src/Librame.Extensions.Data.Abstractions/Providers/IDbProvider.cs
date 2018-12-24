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
    /// <summary>
    /// 数据库提供程序接口。
    /// </summary>
    public interface IDbProvider : IDisposable
    {
        /// <summary>
        /// 数据库已创建动作列表。
        /// </summary>
        IList<Action> DatabaseCreatedActions { get; set; }

        /// <summary>
        /// 提供程序名称。
        /// </summary>
        string DbProviderName { get; }

        /// <summary>
        /// 启用自动事务。
        /// </summary>
        bool AutoTransactionsEnabled { get; set; }

        /// <summary>
        /// 快照名称。
        /// </summary>
        string SnapshotName { get; set; }


        /// <summary>
        /// 创建数据库。
        /// </summary>
        void CreateDatabase();

        /// <summary>
        /// 创建数据库。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个异步操作。</returns>
        Task CreateDatabaseAsync(CancellationToken cancellationToken = default);


        /// <summary>
        /// 更新数据库。
        /// </summary>
        void UpdateDatabase();

        /// <summary>
        /// 更新数据库。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个异步操作。</returns>
        Task UpdateDatabaseAsync(CancellationToken cancellationToken = default);


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
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含受影响行数的异步操作。</returns>
        Task<int> ExecuteSqlCommandAsync(string sql, IEnumerable<object> parameters, CancellationToken cancellationToken = default);


        /// <summary>
        /// 保存变化。
        /// </summary>
        /// <returns>返回受影响的行数。</returns>
        int SaveChanges();


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
