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
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Accessors
{
    using Core.Services;
    using Core.Threads;
    using Data.Builders;
    using Data.Resources;
    using Data.Stores;

    /// <summary>
    /// 数据库上下文访问器基类。
    /// </summary>
    public class DbContextAccessorBase : DbContext, IAccessor
    {
        private static ITenant _currentTenant = null;
        private static string _currentConnectionString = null;


        /// <summary>
        /// 构造一个数据库上下文访问器基类。
        /// </summary>
        /// <param name="options">给定的 <see cref="DbContextOptions"/>。</param>
        protected DbContextAccessorBase(DbContextOptions options)
            : base(options)
        {
            InitializeMigrate();
        }


        private void InitializeMigrate()
        {
            // 验证数据库是否存在
            if (BuilderOptions.IsCreateDatabase)
                EnsureDatabaseCreated();
        }


        /// <summary>
        /// 内部服务提供程序。
        /// </summary>
        public IServiceProvider InternalServiceProvider
            => this.GetInfrastructure();

        /// <summary>
        /// 服务工厂。
        /// </summary>
        public ServiceFactory ServiceFactory
            => this.GetService<ServiceFactory>();

        /// <summary>
        /// 内存锁定器。
        /// </summary>
        public IMemoryLocker Locker
            => this.GetService<IMemoryLocker>();


        /// <summary>
        /// 构建器选项。
        /// </summary>
        public DataBuilderOptions BuilderOptions
            => this.GetService<IOptions<DataBuilderOptions>>().Value;

        /// <summary>
        /// 日志工厂。
        /// </summary>
        public ILoggerFactory LoggerFactory
            => this.GetService<ILoggerFactory>();

        /// <summary>
        /// 日志。
        /// </summary>
        protected ILogger Logger
            => LoggerFactory.CreateLogger(GetType());


        /// <summary>
        /// 当前类型。
        /// </summary>
        public Type CurrentType
            => GetType();

        /// <summary>
        /// 当前租户。
        /// </summary>
        /// <value>返回 <see cref="ITenant"/>。</value>
        public ITenant CurrentTenant
            => _currentTenant.NotNullOrDefault(() => BuilderOptions.DefaultTenant);

        /// <summary>
        /// 当前数据库连接字符串。
        /// </summary>
        public string CurrentConnectionString
            => _currentConnectionString.NotEmptyOrDefault(() => Database.GetDbConnection().ConnectionString);


        /// <summary>
        /// 是否为当前数据连接字符串。
        /// </summary>
        /// <param name="connectionString">给定的连接字符串。</param>
        /// <returns>返回布尔值。</returns>
        public virtual bool IsCurrentConnectionString(string connectionString)
            => CurrentConnectionString.Equals(connectionString, StringComparison.OrdinalIgnoreCase);


        /// <summary>
        /// 是写入请求。
        /// </summary>
        /// <returns>返回布尔值。</returns>
        public virtual bool IsWritingRequest()
            => !CurrentTenant.WritingSeparation || (CurrentTenant.WritingSeparation && IsCurrentConnectionString(CurrentTenant.WritingConnectionString));


        /// <summary>
        /// 确保已创建数据库。
        /// </summary>
        /// <param name="dbConnection">给定的 <see cref="DbConnection"/>（可选）。</param>
        /// <returns>返回是否已创建的布尔值。</returns>
        protected virtual bool EnsureDatabaseCreated(DbConnection dbConnection = null)
        {
            bool result;
            if (result = Database.EnsureCreated())
            {
                if (dbConnection.IsNull())
                    dbConnection = Database.GetDbConnection();

                _currentConnectionString = dbConnection.ConnectionString;
                Logger.LogInformation($"Database created: {_currentConnectionString}.");

                BuilderOptions.DatabaseCreatedAction?.Invoke(this);
            }
            return result;
        }

        /// <summary>
        /// 确保已创建数据库。
        /// </summary>
        /// <param name="dbConnection">给定的 <see cref="DbConnection"/>（可选）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含是否已创建的布尔值的异步操作。</returns>
        protected virtual async Task<bool> EnsureDatabaseCreatedAsync(DbConnection dbConnection = null,
            CancellationToken cancellationToken = default)
        {
            bool result;
            if (result = await Database.EnsureCreatedAsync(cancellationToken).ConfigureAndResultAsync())
            {
                if (dbConnection.IsNull())
                    dbConnection = Database.GetDbConnection();

                _currentConnectionString = dbConnection.ConnectionString;
                Logger.LogInformation($"Database created: {_currentConnectionString}.");

                BuilderOptions.DatabaseCreatedAction?.Invoke(this);
            }
            return result;
        }


        /// <summary>
        /// 执行 SQL 命令。
        /// </summary>
        /// <param name="sql">给定的 SQL 语句。</param>
        /// <param name="parameters">给定的参数集合。</param>
        /// <returns>返回受影响的行数。</returns>
        public virtual int ExecuteSqlRaw(string sql, params object[] parameters)
            => Database.ExecuteSqlRaw(sql, parameters);

        /// <summary>
        /// 异步执行 SQL 命令。
        /// </summary>
        /// <param name="sql">给定的 SQL 语句。</param>
        /// <param name="parameters">给定的参数集合。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含受影响行数的异步操作。</returns>
        public virtual Task<int> ExecuteSqlRawAsync(string sql, IEnumerable<object> parameters,
            CancellationToken cancellationToken = default)
            => Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);


        /// <summary>
        /// 迁移。
        /// </summary>
        public virtual void Migrate()
        {
        }

        /// <summary>
        /// 异步迁移。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        public virtual Task MigrateAsync(CancellationToken cancellationToken = default)
            => Task.CompletedTask;


        /// <summary>
        /// 改变数据库链接。
        /// </summary>
        /// <param name="changeFactory">给定改变租户数据库连接的工厂方法。</param>
        /// <returns>返回是否切换的布尔值。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "changeFactory")]
        public virtual bool ChangeDbConnection(Func<ITenant, string> changeFactory)
        {
            changeFactory.NotNull(nameof(changeFactory));

            var isSwitched = false;

            // 如果启用多租户
            if (BuilderOptions.TenantEnabled)
            {
                var switchTenant = GetSwitchTenant();
                if (switchTenant.IsNotNull() && !switchTenant.Equals(CurrentTenant))
                {
                    _currentTenant = switchTenant;
                    isSwitched = true;
                    Logger.LogTrace(InternalResource.SwitchNewTenantFormat.Format(switchTenant.ToString()));
                }
            }

            if (!isSwitched && !CurrentTenant.WritingSeparation)
                return false;

            var changeConnectionString = changeFactory.Invoke(CurrentTenant);
            if (changeConnectionString.IsEmpty())
            {
                Logger.LogTrace(InternalResource.ChangeConnectionStringIsEmpty);
                return false;
            }

            if (IsCurrentConnectionString(changeConnectionString))
            {
                Logger.LogTrace(InternalResource.ChangeConnectionStringSameAsCurrent);
                return false;
            }
            _currentConnectionString = changeConnectionString;

            // 不使用尝试捕获异常锁，否则会中断异常后面的方法执行，不适合局部异常处理
            return Locker.WaitFactory(ChangeDbConnectionCore);
        }

        /// <summary>
        /// 获取切换的租户。
        /// </summary>
        /// <returns>返回 <see cref="ITenant"/>。</returns>
        protected virtual ITenant GetSwitchTenant()
            => null;

        /// <summary>
        /// 改变数据库连接核心。
        /// </summary>
        /// <returns>返回布尔值。</returns>
        protected bool ChangeDbConnectionCore()
        {
            var connection = Database.GetDbConnection();
            if (connection.State != ConnectionState.Closed)
            {
                connection.Close();
                Logger.LogTrace(InternalResource.CloseConnectionFormat.Format(connection.ConnectionString));
            }

            // MYSql: System.InvalidOperationException
            //  HResult = 0x80131509
            //  Message = Cannot change connection string on a connection that has already been opened.
            //  Source = MySqlConnector
            // connection.ChangeDatabase(connectionString);
            connection.ConnectionString = CurrentConnectionString;
            Logger.LogInformation(InternalResource.ChangeNewConnectionFormat.Format(CurrentTenant.ToString(), CurrentConnectionString));

            // 先尝试创建数据库
            if (BuilderOptions.IsCreateDatabase)
                EnsureDatabaseCreated(connection);

            if (connection.State != ConnectionState.Open)
            {
                // 如果数据库不存在，打开时会抛出异常
                connection.Open();
                Logger.LogInformation(InternalResource.OpenConnectionFormat.Format(CurrentConnectionString));
            }

            BuilderOptions.PostChangedDbConnectionAction?.Invoke(this);
            return true;
        }


        /// <summary>
        /// 异步改变数据库链接。
        /// </summary>
        /// <param name="changeFactory">给定改变租户数据库连接的工厂方法。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含是否已切换的布尔值的异步操作。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "changeFactory")]
        public virtual async Task<bool> ChangeDbConnectionAsync(Func<ITenant, string> changeFactory,
            CancellationToken cancellationToken = default)
        {
            changeFactory.NotNull(nameof(changeFactory));

            var isSwitched = false;

            // 如果启用多租户
            if (BuilderOptions.TenantEnabled)
            {
                var switchTenant = await GetSwitchTenantAsync(cancellationToken).ConfigureAndResultAsync();
                if (switchTenant.IsNotNull() && !switchTenant.Equals(CurrentTenant))
                {
                    _currentTenant = switchTenant;
                    isSwitched = true;
                    Logger.LogTrace(InternalResource.SwitchNewTenantFormat.Format(switchTenant.ToString()));
                }
            }

            if (!isSwitched && !CurrentTenant.WritingSeparation)
                return false;

            var changeConnectionString = changeFactory.Invoke(CurrentTenant);
            if (changeConnectionString.IsEmpty())
            {
                Logger.LogTrace(InternalResource.ChangeConnectionStringIsEmpty);
                return false;
            }

            if (IsCurrentConnectionString(changeConnectionString))
            {
                Logger.LogTrace(InternalResource.ChangeConnectionStringSameAsCurrent);
                return false;
            }
            _currentConnectionString = changeConnectionString;

            // 不使用尝试捕获异常锁，否则会中断异常后面的方法执行，不适合局部异常处理
            return await Locker.WaitFactoryAsync(() => ChangeDbConnectionCoreAsync(cancellationToken),
                cancellationToken).ConfigureAndResultAsync();
        }

        /// <summary>
        /// 获取切换的租户。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回包含 <see cref="ITenant"/> 的异步操作。</returns>
        protected virtual Task<ITenant> GetSwitchTenantAsync(CancellationToken cancellationToken = default)
            => Task.FromResult((ITenant)null);

        /// <summary>
        /// 异步改变数据库连接核心。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回包含布尔值的异步操作。</returns>
        protected virtual async Task<bool> ChangeDbConnectionCoreAsync(CancellationToken cancellationToken = default)
        {
            var connection = Database.GetDbConnection();
            if (connection.State != ConnectionState.Closed)
            {
                await connection.CloseAsync().ConfigureAndWaitAsync();
                Logger.LogTrace(InternalResource.CloseConnectionFormat.Format(connection.ConnectionString));
            }

            // MYSql: System.InvalidOperationException
            //  HResult = 0x80131509
            //  Message = Cannot change connection string on a connection that has already been opened.
            //  Source = MySqlConnector
            // connection.ChangeDatabase(connectionString);
            connection.ConnectionString = CurrentConnectionString;
            Logger?.LogInformation(InternalResource.ChangeNewConnectionFormat.Format(CurrentTenant.ToString(), CurrentConnectionString));

            // 先尝试创建数据库
            if (BuilderOptions.IsCreateDatabase)
                await EnsureDatabaseCreatedAsync(connection, cancellationToken).ConfigureAndResultAsync();

            if (connection.State != ConnectionState.Open)
            {
                // 如果数据库不存在，打开时会抛出异常
                await connection.OpenAsync(cancellationToken).ConfigureAndWaitAsync();
                Logger?.LogInformation(InternalResource.OpenConnectionFormat.Format(CurrentConnectionString));
            }

            BuilderOptions.PostChangedDbConnectionAction?.Invoke(this);
            return true;
        }

    }
}
