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
using Microsoft.Extensions.Logging;
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
    using Data.Builders;
    using Data.Resources;
    using Data.Stores;

    /// <summary>
    /// 数据库上下文访问器基类。
    /// </summary>
    public class DbContextAccessorBase : DbContext, IAccessor
    {
        private bool _isSavingChanges = false;


        /// <summary>
        /// 构造一个数据库上下文访问器基类。
        /// </summary>
        /// <param name="options">给定的 <see cref="DbContextOptions"/>。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        protected DbContextAccessorBase(DbContextOptions options)
            : base(options)
        {
            InitializeAccessorBase(options);
        }


        private void InitializeAccessorBase(DbContextOptions options)
        {
            // Database.GetDbConnection().ConnectionString 的信息不一定完整
            CurrentConnectionString = Dependency.Options.OptionsExtensionConnectionStringFactory?.Invoke(options);
            CurrentConnectionString.NotEmpty("BuilderOptions.OptionsExtensionConnectionStringFactory");

            CurrentTenant = Dependency.Options.DefaultTenant.NotNull("BuilderOptions.DefaultTenant");
            
            if (Dependency.Options.SupportsCreateDatabase)
                EnsureDatabaseCreated();
        }


        /// <summary>
        /// 时钟服务。
        /// </summary>
        public IClockService Clock
            => GetService<IClockService>();

        /// <summary>
        /// 日志工厂。
        /// </summary>
        public ILoggerFactory LoggerFactory
            => GetService<ILoggerFactory>();

        /// <summary>
        /// 构建器依赖。
        /// </summary>
        public DataBuilderDependency Dependency
            => GetService<DataBuilderDependency>();


        /// <summary>
        /// 日志。
        /// </summary>
        protected ILogger Logger
            => LoggerFactory.CreateLogger(GetType());


        /// <summary>
        /// 当前时间戳。
        /// </summary>
        public DateTimeOffset CurrentTimestamp
            => Clock.GetNowOffsetAsync().ConfigureAndResult();

        /// <summary>
        /// 当前类型。
        /// </summary>
        public Type CurrentType
            => GetType();

        /// <summary>
        /// 当前租户。
        /// </summary>
        /// <value>返回 <see cref="ITenant"/>。</value>
        public ITenant CurrentTenant { get; private set; }

        /// <summary>
        /// 当前数据库连接字符串。
        /// </summary>
        public string CurrentConnectionString { get; private set; }


        /// <summary>
        /// 是当前数据连接字符串。
        /// </summary>
        /// <param name="connectionString">给定的连接字符串。</param>
        /// <returns>返回布尔值。</returns>
        public virtual bool IsCurrentConnectionString(string connectionString)
            => CurrentConnectionString.Equals(connectionString, StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// 是写入数据连接字符串（支持未启用读写分离）。
        /// </summary>
        /// <returns>返回布尔值。</returns>
        public virtual bool IsWritingConnectionString()
            => !CurrentTenant.WritingSeparation
            || (CurrentTenant.WritingSeparation && IsCurrentConnectionString(CurrentTenant.WritingConnectionString));

        /// <summary>
        /// 获取当前数据连接字符串标签。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public virtual string GetCurrentConnectionStringTag()
        {
            if (CurrentTenant.WritingSeparation && IsCurrentConnectionString(CurrentTenant.WritingConnectionString))
                return nameof(ITenant.WritingConnectionString);

            return nameof(ITenant.DefaultConnectionString);
        }


        /// <summary>
        /// 获取服务。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <returns>返回 <typeparamref name="TService"/>。</returns>
        public virtual TService GetService<TService>()
            => AccessorExtensions.GetService<TService>(this);

        /// <summary>
        /// 获取服务提供程序。
        /// </summary>
        /// <returns>返回 <see cref="IServiceProvider"/>。</returns>
        public virtual IServiceProvider GetServiceProvider()
            => AccessorExtensions.GetInfrastructure(this);


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
                Logger.LogInformation($"Database created: {CurrentConnectionString}.");
                Dependency.Options.PostDatabaseCreatedAction?.Invoke(this);
            }
            return result;
        }

        /// <summary>
        /// 确保已创建数据库。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含是否已创建的布尔值的异步操作。</returns>
        protected virtual async Task<bool> EnsureDatabaseCreatedAsync(CancellationToken cancellationToken = default)
        {
            bool result;
            if (result = await Database.EnsureCreatedAsync(cancellationToken).ConfigureAndResultAsync())
            {
                Logger.LogInformation($"Database created: {CurrentConnectionString}.");
                Dependency.Options.PostDatabaseCreatedAction?.Invoke(this);
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


        #region SaveChanges (WritingSeparation)

        /// <summary>
        /// 基础保存更改。
        /// </summary>
        /// <returns>返回受影响的行数。</returns>
        public int BaseSaveChanges()
            => base.SaveChanges();

        /// <summary>
        /// 基础保存更改。
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">指示是否在更改已成功发送到数据库之后调用。</param>
        /// <returns>返回受影响的行数。</returns>
        public int BaseSaveChanges(bool acceptAllChangesOnSuccess)
            => base.SaveChanges(acceptAllChangesOnSuccess);

        /// <summary>
        /// 基础异步保存更改。
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">指示是否在更改已成功发送到数据库之后调用。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含受影响行数的异步操作。</returns>
        public Task<int> BaseSaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
            => base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

        /// <summary>
        /// 基础异步保存更改。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含受影响行数的异步操作。</returns>
        public Task<int> BaseSaveChangesAsync(CancellationToken cancellationToken = default)
            => base.SaveChangesAsync(cancellationToken);


        /// <summary>
        /// 重载保存更改。
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">指示是否在更改已成功发送到数据库之后调用。</param>
        /// <returns>返回受影响的行数。</returns>
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            var count = 0;

            if (_isSavingChanges)
                return count;

            _isSavingChanges = true;

            // 试着改变为写入数据库连接
            var isChanged = ChangeConnectionString(tenant => tenant.WritingConnectionString);

            // 调用保存更改核心
            if (IsWritingConnectionString())
                count = SaveChangesCore(acceptAllChangesOnSuccess);

            // 试着还原为默认数据库连接
            if (isChanged)
                ChangeConnectionString(tenant => tenant.DefaultConnectionString);

            _isSavingChanges = false;

            return count;
        }

        /// <summary>
        /// 保存更改核心。
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">指示是否在更改已成功发送到数据库之后调用。</param>
        /// <returns>返回受影响的行数。</returns>
        protected virtual int SaveChangesCore(bool acceptAllChangesOnSuccess)
            => BaseSaveChanges(acceptAllChangesOnSuccess);

        /// <summary>
        /// 重载异步保存更改。
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">指示是否在更改已成功发送到数据库之后调用。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含受影响行数的异步操作。</returns>
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            return cancellationToken.RunFactoryOrCancellationAsync(async () =>
            {
                var count = 0;

                if (_isSavingChanges)
                    return count;

                _isSavingChanges = true;

                // 试着改变为写入数据库连接
                var isChanged = await ChangeConnectionStringAsync(tenant => tenant.WritingConnectionString, cancellationToken)
                    .ConfigureAndResultAsync();

                // 调用保存更改核心
                if (IsWritingConnectionString())
                    count = await SaveChangesCoreAsync(acceptAllChangesOnSuccess, cancellationToken).ConfigureAndResultAsync();

                // 试着还原为默认数据库连接
                if (isChanged)
                {
                    await ChangeConnectionStringAsync(tenant => tenant.DefaultConnectionString, cancellationToken: cancellationToken)
                        .ConfigureAndResultAsync();
                }

                _isSavingChanges = false;

                return count;
            });
        }

        /// <summary>
        /// 异步保存更改核心。
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">指示是否在更改已成功发送到数据库之后调用。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含受影响行数的异步操作。</returns>
        protected virtual Task<int> SaveChangesCoreAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
            => BaseSaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

        #endregion


        #region Migrate (MigrationEnabled)

        /// <summary>
        /// 迁移（如果选项已启用迁移，则调用迁移核心方法，反之则调用数据库默认迁移方法）。
        /// </summary>
        public virtual void Migrate()
        {
            if (Dependency.Options.Stores.UseDataMigration)
                MigrateCore();
            else
                Database.Migrate();
        }

        /// <summary>
        /// 迁移核心。
        /// </summary>
        protected virtual void MigrateCore()
        {
        }


        /// <summary>
        /// 异步迁移（如果选项已启用迁移，则调用迁移核心方法，反之则调用数据库默认迁移方法）。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        public virtual Task MigrateAsync(CancellationToken cancellationToken = default)
        {
            if (Dependency.Options.Stores.UseDataMigration)
                return MigrateCoreAsync(cancellationToken);
            else
                return Database.MigrateAsync(cancellationToken);
        }

        /// <summary>
        /// 异步迁移核心。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        protected virtual Task MigrateCoreAsync(CancellationToken cancellationToken = default)
            => Task.CompletedTask;

        #endregion


        #region ChangeConnectionString (TenantEnabled)

        /// <summary>
        /// 改变数据库链接。
        /// </summary>
        /// <param name="changeFactory">给定改变租户数据库连接的工厂方法。</param>
        /// <returns>返回是否切换的布尔值。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public virtual bool ChangeConnectionString(Func<ITenant, string> changeFactory)
        {
            changeFactory.NotNull(nameof(changeFactory));

            // 不使用尝试捕获异常锁，否则会中断异常后面的方法执行，不适合局部异常处理
            var isSwitched = false;

            // 如果启用多租户
            if (Dependency.Options.Stores.UseDataTenant)
            {
                var switchTenant = SwitchTenant();
                if (switchTenant.IsNotNull() && !switchTenant.Equals(CurrentTenant))
                {
                    CurrentTenant = switchTenant;
                    isSwitched = true;
                    Logger.LogTrace(InternalResource.SwitchNewTenantFormat.Format(switchTenant.ToString()));
                }
            }

            // 如果未进行租户切换且当前租户未启用读写分离，则退出
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

            CurrentConnectionString = changeConnectionString;

            return ChangeConnectionStringCore();
        }

        /// <summary>
        /// 改变数据库连接核心。
        /// </summary>
        /// <returns>返回布尔值。</returns>
        protected virtual bool ChangeConnectionStringCore()
        {
            var connection = Database.GetDbConnection();
            if (connection.State != ConnectionState.Closed)
            {
                connection.Close();
                Logger.LogTrace(InternalResource.CloseConnectionFormat.Format(connection.ConnectionString));
            }

            connection.ConnectionString = CurrentConnectionString;
            Logger.LogInformation(InternalResource.ChangeNewConnectionFormat.Format(CurrentTenant.ToString(), CurrentConnectionString));

            // 先尝试创建数据库
            if (Dependency.Options.SupportsCreateDatabase)
                EnsureDatabaseCreated(connection);

            if (connection.State != ConnectionState.Open)
            {
                // 如果数据库不存在，打开时会抛出异常
                connection.Open();
                Logger.LogInformation(InternalResource.OpenConnectionFormat.Format(CurrentConnectionString));
            }

            Dependency.Options.PostChangedDbConnectionAction?.Invoke(this);
            return true;
        }


        /// <summary>
        /// 异步改变数据库链接。
        /// </summary>
        /// <param name="changeFactory">给定改变租户数据库连接的工厂方法。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含是否已切换的布尔值的异步操作。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public virtual Task<bool> ChangeConnectionStringAsync(Func<ITenant, string> changeFactory,
            CancellationToken cancellationToken = default)
        {
            changeFactory.NotNull(nameof(changeFactory));

            // 不使用尝试捕获异常锁，否则会中断异常后面的方法执行，不适合局部异常处理
            return cancellationToken.RunFactoryOrCancellationAsync(async () =>
            {
                var isSwitched = false;

                // 如果启用多租户
                if (Dependency.Options.Stores.UseDataTenant)
                {
                    var switchTenant = await SwitchTenantAsync(cancellationToken).ConfigureAndResultAsync();
                    if (switchTenant.IsNotNull() && !switchTenant.Equals(CurrentTenant))
                    {
                        CurrentTenant = switchTenant;
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

                CurrentConnectionString = changeConnectionString;

                return await ChangeConnectionStringCoreAsync(cancellationToken).ConfigureAndResultAsync();
            });
        }

        /// <summary>
        /// 异步改变数据库连接核心。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回包含布尔值的异步操作。</returns>
        protected virtual async Task<bool> ChangeConnectionStringCoreAsync(CancellationToken cancellationToken = default)
        {
            var connection = Database.GetDbConnection();
            if (connection.State != ConnectionState.Closed)
            {
                await connection.CloseAsync().ConfigureAndWaitAsync();
                Logger.LogTrace(InternalResource.CloseConnectionFormat.Format(connection.ConnectionString));
            }

            connection.ConnectionString = CurrentConnectionString;
            Logger?.LogInformation(InternalResource.ChangeNewConnectionFormat.Format(CurrentTenant.ToString(), CurrentConnectionString));

            // 先尝试创建数据库
            if (Dependency.Options.SupportsCreateDatabase)
                await EnsureDatabaseCreatedAsync(cancellationToken).ConfigureAndResultAsync();

            if (connection.State != ConnectionState.Open)
            {
                // 如果数据库不存在，打开时会抛出异常
                await connection.OpenAsync(cancellationToken).ConfigureAndWaitAsync();
                Logger?.LogInformation(InternalResource.OpenConnectionFormat.Format(CurrentConnectionString));
            }

            Dependency.Options.PostChangedDbConnectionAction?.Invoke(this);
            return true;
        }


        /// <summary>
        /// 切换租户。
        /// </summary>
        /// <returns>返回 <see cref="ITenant"/>。</returns>
        protected virtual ITenant SwitchTenant()
            => null;

        /// <summary>
        /// 切换租户。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回包含 <see cref="ITenant"/> 的异步操作。</returns>
        protected virtual Task<ITenant> SwitchTenantAsync(CancellationToken cancellationToken = default)
            => Task.FromResult((ITenant)null);

        #endregion

    }
}
