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
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data
{
    using Core;

    /// <summary>
    /// <see cref="DbContext"/> 访问器。
    /// </summary>
    public class DbContextAccessor : DbContext, IAccessor
    {
        /// <summary>
        /// 构造一个 <see cref="DbContextAccessor"/>。
        /// </summary>
        /// <param name="options">给定的 <see cref="DbContextOptions"/>。</param>
        public DbContextAccessor(DbContextOptions options)
            : base(options)
        {
            Initialize();
        }


        /// <summary>
        /// 初始化。
        /// </summary>
        protected void Initialize()
        {
            if (BuilderOptions.IsCreateDatabase)
                EnsureDatabaseCreated(Database.GetDbConnection());
        }


        #region Entities

        /// <summary>
        /// 审计数据集。
        /// </summary>
        public DbSet<DataAudit> Audits { get; set; }

        /// <summary>
        /// 审计属性数据集。
        /// </summary>
        public DbSet<DataAuditProperty> AuditProperties { get; set; }

        /// <summary>
        /// 实体数据集。
        /// </summary>
        public DbSet<DataEntity> Entities { get; set; }

        /// <summary>
        /// 迁移数据集。
        /// </summary>
        public DbSet<DataMigration> Migrations { get; set; }

        /// <summary>
        /// 租户数据集。
        /// </summary>
        public DbSet<DataTenant> Tenants { get; set; }

        #endregion
		
		
        /// <summary>
        /// 服务提供程序。
        /// </summary>
        public IServiceProvider ServiceProvider
            => this.GetInfrastructure();

        /// <summary>
        /// 服务工厂。
        /// </summary>
        public ServiceFactoryDelegate ServiceFactory
            => this.GetService<ServiceFactoryDelegate>();

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
        /// 确保已创建数据库。
        /// </summary>
        /// <param name="dbConnection">给定的 <see cref="DbConnection"/>。</param>
        /// <returns>返回是否已创建的布尔值。</returns>
        protected virtual bool EnsureDatabaseCreated(DbConnection dbConnection)
        {
            if (Database.EnsureCreated())
            {
                if (dbConnection.IsNotNull())
                    Logger.LogInformation($"Database created: {dbConnection.ConnectionString}.");
                
                BuilderOptions.DatabaseCreatedAction?.Invoke(this);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 确保已创建数据库。
        /// </summary>
        /// <param name="dbConnection">给定的 <see cref="DbConnection"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含是否已创建的布尔值的异步操作。</returns>
        protected virtual async Task<bool> EnsureDatabaseCreatedAsync(DbConnection dbConnection, CancellationToken cancellationToken)
        {
            bool result;
            if (result = await Database.EnsureCreatedAsync(cancellationToken).ConfigureAndResultAsync())
            {
                if (dbConnection.IsNotNull())
                    Logger.LogInformation($"Database created: {dbConnection.ConnectionString}.");

                BuilderOptions.DatabaseCreatedAction?.Invoke(this);
            }

            return result;
        }


        /// <summary>
        /// 开始模型创建。
        /// </summary>
        /// <param name="modelBuilder">给定的 <see cref="ModelBuilder"/>。</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ConfigureStoreHubBase(BuilderOptions);
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
        /// 重载保存更改。
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">指示是否在更改已成功发送到数据库之后调用。</param>
        /// <returns>返回受影响的行数。</returns>
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            // 读写分离：尝试切换为写入数据库
            if (BuilderOptions.TenantEnabled)
                SwitchTenant(tenant => tenant.WritingConnectionString);

            var aspects = ServiceFactory.GetRequiredService<IServicesManager<ISaveChangesAccessorAspect>>();
            // 前置处理保存变化
            aspects.ForEach(aspect => aspect.Preprocess(this));

            // 保存变化
            var count = base.SaveChanges(acceptAllChangesOnSuccess);

            // 后置处理保存变化
            aspects.ForEach(aspect => aspect.Postprocess(this));

            // 读写分离：尝试切换为默认数据库
            if (BuilderOptions.TenantEnabled)
                SwitchTenant(tenant => tenant.DefaultConnectionString);

            return count;
        }

        /// <summary>
        /// 重载异步保存更改。
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">指示是否在更改已成功发送到数据库之后调用。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含受影响行数的异步操作。</returns>
        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            // 读写分离：尝试切换为写入数据库
            if (BuilderOptions.TenantEnabled)
                await SwitchTenantAsync(tenant => tenant.WritingConnectionString, cancellationToken).ConfigureAndResultAsync();

            var aspects = ServiceFactory.GetRequiredService<IServicesManager<ISaveChangesAccessorAspect>>();
            // 前置处理保存变化
            aspects.ForEach(async aspect => await aspect.PreprocessAsync(this, cancellationToken).ConfigureAndWaitAsync());

            // 保存变化
            var count = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken).ConfigureAndResultAsync();

            // 后置处理保存变化
            aspects.ForEach(async aspect => await aspect.PostprocessAsync(this, cancellationToken).ConfigureAndWaitAsync());

            // 读写分离：尝试切换为默认数据库
            if (BuilderOptions.TenantEnabled)
                await SwitchTenantAsync(tenant => tenant.DefaultConnectionString, cancellationToken).ConfigureAndResultAsync();

            return count;
        }


        /// <summary>
        /// 迁移。
        /// </summary>
        public virtual void Migrate()
        {
            var migrationService = this.GetService<IDataMigrationService>();
            migrationService.Migrate(this);
        }

        /// <summary>
        /// 异步迁移。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        public virtual Task MigrateAsync(CancellationToken cancellationToken = default)
        {
            var migrationService = this.GetService<IDataMigrationService>();
            return migrationService.MigrateAsync(this, cancellationToken);
        }


        /// <summary>
        /// 切换租户。
        /// </summary>
        /// <param name="changeConnectionStringFactory">给定改变租户数据库连接的工厂方法。</param>
        /// <returns>返回是否切换的布尔值。</returns>
        public virtual bool SwitchTenant(Func<ITenant, string> changeConnectionStringFactory)
        {
            if (changeConnectionStringFactory.IsNull())
                return false;

            var tenantService = this.GetService<IDataTenantService>();
            var currentTenant = tenantService.GetCurrentTenant(this);
            if (currentTenant.IsNull())
                return false;

            if (!currentTenant.WritingSeparation)
            {
                Logger.LogTrace($"The tenant({currentTenant.Name}:{currentTenant.Host}) write separation is disable.");
                return false;
            }

            var changeConnectionString = changeConnectionStringFactory.Invoke(currentTenant);
            var connection = Database.GetDbConnection();
            if (connection.ConnectionString.Equals(changeConnectionString, StringComparison.OrdinalIgnoreCase))
            {
                Logger.LogTrace($"The tenant({currentTenant.Name}:{currentTenant.Host}) same as the current connection string.");
                return false;
            }

            var locker = this.GetService<IMemoryLocker>();
            return locker.WaitFactory(ChangeDbConnection, catchExceptionFactory: ex =>
            {
                Logger.LogError(ex, ex.AsInnerMessage());
                return false;
            });

            // 改变数据库连接
            bool ChangeDbConnection()
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                    Logger.LogTrace($"Close connection string: {connection.ConnectionString}");
                }

                // MYSql: System.InvalidOperationException
                //  HResult = 0x80131509
                //  Message = Cannot change connection string on a connection that has already been opened.
                //  Source = MySqlConnector
                // connection.ChangeDatabase(connectionString);
                connection.ConnectionString = changeConnectionString;
                Logger?.LogInformation($"The tenant({currentTenant.Name}:{currentTenant.Host}) change connection string: {changeConnectionString}");

                if (BuilderOptions.IsCreateDatabase)
                    EnsureDatabaseCreated(connection);

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                    Logger?.LogInformation($"Open connection string: {connection.ConnectionString}");
                }

                if (BuilderOptions.MigrationEnabled)
                    Migrate();

                return true;
            }
        }

        /// <summary>
        /// 异步切换租户。
        /// </summary>
        /// <param name="changeConnectionStringFactory">给定改变租户数据库连接的工厂方法。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含是否已切换的布尔值的异步操作。</returns>
        public virtual async Task<bool> SwitchTenantAsync(Func<ITenant, string> changeConnectionStringFactory,
            CancellationToken cancellationToken = default)
        {
            if (changeConnectionStringFactory.IsNull())
                return false;

            var tenantService = this.GetService<IDataTenantService>();
            var currentTenant = await tenantService.GetCurrentTenantAsync(this, cancellationToken).ConfigureAndResultAsync();
            if (currentTenant.IsNull())
                return false;

            if (!currentTenant.WritingSeparation)
            {
                Logger.LogTrace($"The tenant({currentTenant.Name}:{currentTenant.Host}) write separation is disable.");
                return false;
            }

            var changeConnectionString = changeConnectionStringFactory.Invoke(currentTenant);
            var connection = Database.GetDbConnection();
            if (connection.ConnectionString.Equals(changeConnectionString, StringComparison.OrdinalIgnoreCase))
            {
                Logger.LogTrace($"The tenant({currentTenant.Name}:{currentTenant.Host}) same as the current connection string.");
                return false;
            }

            var locker = this.GetService<IMemoryLocker>();
            return await locker.WaitFactoryAsync(ChangeDbConnectionAsync, catchExceptionFactory: ex =>
            {
                Logger.LogError(ex, ex.AsInnerMessage());
                return false;
            })
            .ConfigureAndResultAsync();

            // 异步改变数据库连接
            async Task<bool> ChangeDbConnectionAsync()
            {
                if (connection.State != ConnectionState.Closed)
                {
                    await connection.CloseAsync().ConfigureAndWaitAsync();
                    Logger.LogTrace($"Close connection string: {connection.ConnectionString}");
                }

                // MYSql: System.InvalidOperationException
                //  HResult = 0x80131509
                //  Message = Cannot change connection string on a connection that has already been opened.
                //  Source = MySqlConnector
                // connection.ChangeDatabase(connectionString);
                connection.ConnectionString = changeConnectionString;
                Logger?.LogInformation($"The tenant({currentTenant.Name}:{currentTenant.Host}) change connection string: {changeConnectionString}");

                if (BuilderOptions.IsCreateDatabase)
                    await EnsureDatabaseCreatedAsync(connection, cancellationToken).ConfigureAndResultAsync();

                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync(cancellationToken).ConfigureAndWaitAsync();
                    Logger?.LogInformation($"Open connection string: {connection.ConnectionString}");
                }

                if (BuilderOptions.MigrationEnabled)
                    await MigrateAsync(cancellationToken).ConfigureAndWaitAsync();

                return true;
            }
        }

    }
}
