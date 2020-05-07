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

namespace Librame.Extensions.Data.Accessors
{
    using Core.Services;
    using Data.Stores;
    using Data.Services;

    /// <summary>
    /// 访问器接口。
    /// </summary>
    public interface IAccessor : ISaveChanges, IInfrastructureService, IDisposable
    {
        /// <summary>
        /// 时钟服务。
        /// </summary>
        IClockService Clock { get; }


        /// <summary>
        /// 当前时间戳。
        /// </summary>
        DateTimeOffset CurrentTimestamp { get; }

        /// <summary>
        /// 当前类型。
        /// </summary>
        Type CurrentType { get; }

        /// <summary>
        /// 当前租户。
        /// </summary>
        /// <value>返回 <see cref="ITenant"/>。</value>
        ITenant CurrentTenant { get; }

        /// <summary>
        /// 当前连接字符串。
        /// </summary>
        string CurrentConnectionString { get; }


        /// <summary>
        /// 是当前连接字符串。
        /// </summary>
        /// <param name="connectionString">给定的连接字符串。</param>
        /// <returns>返回布尔值。</returns>
        bool IsCurrentConnectionString(string connectionString);

        /// <summary>
        /// 是写入连接字符串。
        /// </summary>
        /// <returns>返回布尔值。</returns>
        bool IsWritingConnectionString();


        /// <summary>
        /// 执行 SQL 命令。
        /// </summary>
        /// <param name="sql">给定的 SQL 语句。</param>
        /// <param name="parameters">给定的参数集合。</param>
        /// <returns>返回受影响的行数。</returns>
        int ExecuteSqlRaw(string sql, params object[] parameters);

        /// <summary>
        /// 异步执行 SQL 命令。
        /// </summary>
        /// <param name="sql">给定的 SQL 语句。</param>
        /// <param name="parameters">给定的参数集合。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含受影响行数的异步操作。</returns>
        Task<int> ExecuteSqlRawAsync(string sql, IEnumerable<object> parameters, CancellationToken cancellationToken = default);


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
        /// 改变连接字符串。
        /// </summary>
        /// <param name="changeConnectionStringFactory">给定改变租户数据库连接的工厂方法。</param>
        /// <returns>返回已改变的布尔值。</returns>
        bool ChangeConnectionString(Func<ITenant, string> changeConnectionStringFactory);

        /// <summary>
        /// 异步改变连接字符串。
        /// </summary>
        /// <param name="changeConnectionStringFactory">给定改变租户数据库连接的工厂方法。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含已改变的布尔值的异步操作。</returns>
        Task<bool> ChangeConnectionStringAsync(Func<ITenant, string> changeConnectionStringFactory,
            CancellationToken cancellationToken = default);
    }
}
