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
        private static byte[] _locker = new byte[0];


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
        protected virtual void Initialize()
        {
            if (BuilderOptions.IsCreateDatabase)
                EnsureDatabaseCreated(Database.GetDbConnection());
        }


        #region Entities

        /// <summary>
        /// 审计。
        /// </summary>
        public DbSet<Audit> Audits { get; set; }

        /// <summary>
        /// 审计属性。
        /// </summary>
        public DbSet<AuditProperty> AuditProperties { get; set; }

        /// <summary>
        /// 租户。
        /// </summary>
        public DbSet<Tenant> Tenants { get; set; }

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
        /// 记录器工厂。
        /// </summary>
        public ILoggerFactory LoggerFactory
            => this.GetService<ILoggerFactory>();

        /// <summary>
        /// 记录器。
        /// </summary>
        protected ILogger Logger
            => LoggerFactory.CreateLogger(GetType());


        /// <summary>
        /// 确保已创建数据库。
        /// </summary>
        /// <param name="dbConnection">给定的 <see cref="DbConnection"/>。</param>
        /// <returns>返回是否已创建的布尔值。</returns>
        protected virtual void EnsureDatabaseCreated(DbConnection dbConnection)
        {
            if (Database.EnsureCreated())
            {
                if (dbConnection.IsNotNull())
                    Logger.LogInformation($"Database created: {dbConnection.ConnectionString}.");

                BuilderOptions.DatabaseCreatedAction?.Invoke(this);
            }
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
        public virtual int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return Database.ExecuteSqlCommand(sql, parameters);
        }

        /// <summary>
        /// 异步执行 SQL 命令。
        /// </summary>
        /// <param name="sql">给定的 SQL 语句。</param>
        /// <param name="parameters">给定的参数集合。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含受影响行数的异步操作。</returns>
        public virtual Task<int> ExecuteSqlCommandAsync(string sql, IEnumerable<object> parameters,
            CancellationToken cancellationToken = default)
        {
            return Database.ExecuteSqlCommandAsync(sql, parameters, cancellationToken);
        }


        /// <summary>
        /// 重载保存更改。
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">指示是否在更改已成功发送到数据库之后调用。</param>
        /// <returns>返回受影响的行数。</returns>
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            // 读写分离：尝试切换为写入数据库
            if (BuilderOptions.Tenants.Enabled)
                ToggleTenant(tenant => tenant.WritingConnectionString);

            // 审计实体
            if (BuilderOptions.Audits.Enabled)
                AuditAsync(default).Wait();

            var count = base.SaveChanges(acceptAllChangesOnSuccess);

            // 读写分离：尝试切换为默认数据库
            if (BuilderOptions.Tenants.Enabled)
                ToggleTenant(tenant => tenant.DefaultConnectionString);

            return count;
        }

        /// <summary>
        /// 重载异步保存更改。
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">指示是否在更改已成功发送到数据库之后调用。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含受影响行数的异步操作。</returns>
        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            // 读写分离：尝试切换为写入数据库
            if (BuilderOptions.Tenants.Enabled)
                ToggleTenant(tenant => tenant.WritingConnectionString, cancellationToken);

            // 审计实体
            if (BuilderOptions.Audits.Enabled)
                await AuditAsync(cancellationToken);

            var count = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

            // 读写分离：尝试切换为默认数据库
            if (BuilderOptions.Tenants.Enabled)
                ToggleTenant(tenant => tenant.DefaultConnectionString, cancellationToken);

            return count;
        }

        /// <summary>
        /// 异步审计。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        protected virtual async Task AuditAsync(CancellationToken cancellationToken)
        {
            var auditService = this.GetService<IAuditService>();

            await auditService.AuditAsync(this, cancellationToken);
        }


        /// <summary>
        /// 切换租户。
        /// </summary>
        /// <param name="changeConnectionStringFactory">给定改变租户数据库连接的工厂方法。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回是否切换的布尔值。</returns>
        public virtual bool ToggleTenant(Func<ITenant, string> changeConnectionStringFactory,
            CancellationToken cancellationToken = default)
        {
            if (changeConnectionStringFactory.IsNull())
                return false;

            var tenantService = this.GetService<ITenantService>();
            var currentTenant = tenantService.GetCurrentTenantAsync(this, cancellationToken).Result;
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

            try
            {
                lock (_locker)
                {
                    ChangeDbConnection();
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.AsInnerMessage());
                return false;
            }

            void ChangeDbConnection()
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

                if (connection.State != ConnectionState.Open)
                {
                    if (BuilderOptions.IsCreateDatabase)
                        EnsureDatabaseCreated(connection);

                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                        Logger?.LogInformation($"Open connection string: {connection.ConnectionString}");
                    }
                }
            }
        }

    }
}
