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
using System.Linq;
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
        /// 构造一个 <see cref="DbContextAccessor"/> 实例。
        /// </summary>
        /// <param name="options">给定的 <see cref="DbContextOptions"/>。</param>
        public DbContextAccessor(DbContextOptions options)
            : base(options)
        {
            if (BuilderOptions.EnsureDatabase)
                Database.EnsureCreated();
        }


        /// <summary>
        /// 基础审计。
        /// </summary>
        public DbSet<BaseAudit> BaseAudits { get; set; }

        /// <summary>
        /// 基础审计属性。
        /// </summary>
        public DbSet<BaseAuditProperty> BaseAuditProperties { get; set; }

        /// <summary>
        /// 基础租户。
        /// </summary>
        public DbSet<BaseTenant> BaseTenants { get; set; }


        /// <summary>
        /// 核心选项扩展。
        /// </summary>
        protected CoreOptionsExtension CoreOptions
            => this.GetService<IDbContextOptions>()
                .Extensions.OfType<CoreOptionsExtension>()
                .FirstOrDefault();

        /// <summary>
        /// 记录器。
        /// </summary>
        protected ILogger Logger
            => (CoreOptions?.LoggerFactory ?? CoreOptions.ApplicationServiceProvider.GetRequiredService<ILoggerFactory>())
                .CreateLogger<DbContextAccessor>();

        /// <summary>
        /// 服务提供程序。
        /// </summary>
        protected IServiceProvider ServiceProvider
            => CoreOptions?.InternalServiceProvider ?? CoreOptions.ApplicationServiceProvider;

        /// <summary>
        /// 构建器选项。
        /// </summary>
        protected DataBuilderOptions BuilderOptions
            => ServiceProvider.GetRequiredService<IOptions<DataBuilderOptions>>().Value;


        /// <summary>
        /// 当前租户。
        /// </summary>
        /// <value>返回 <see cref="ITenant"/>。</value>
        public virtual ITenant CurrentTenant
            => ServiceProvider.GetRequiredService<ITenantService>().GetTenantAsync(BaseTenants).Result;


        /// <summary>
        /// 开始模型创建。
        /// </summary>
        /// <param name="modelBuilder">给定的 <see cref="ModelBuilder"/>。</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureBaseEntities(BuilderOptions);
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
            // 改变为写入数据库（支持读写分离）
            if (BuilderOptions.TenantEnabled)
                ChangeDbConnectionAsync(tenant => tenant.WriteConnectionString).Wait();

            if (BuilderOptions.AuditEnabled)
                AuditSaveChangesAsync().Wait();

            var count = base.SaveChanges(acceptAllChangesOnSuccess);

            // 尝试还原改变的数据库连接
            if (BuilderOptions.TenantEnabled)
                ChangeDbConnectionAsync(tenant => tenant.DefaultConnectionString).Wait();

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
            // 改变为写入数据库（支持读写分离）
            if (BuilderOptions.TenantEnabled)
                await ChangeDbConnectionAsync(tenant => tenant.WriteConnectionString);

            if (BuilderOptions.AuditEnabled)
                await AuditSaveChangesAsync(cancellationToken);

            var count = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

            // 尝试还原改变的数据库连接
            if (BuilderOptions.TenantEnabled)
                await ChangeDbConnectionAsync(tenant => tenant.DefaultConnectionString);

            return count;
        }

        /// <summary>
        /// 异步审计保存更改。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        protected virtual async Task AuditSaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // 默认仅拦截实体的增删改操作
            var entityStates = new EntityState[] { EntityState.Added, EntityState.Modified, EntityState.Deleted };

            // 得到变化的实体集合
            var entityEntries = ChangeTracker.Entries()
                .Where(m => m.Entity.IsNotNull() && entityStates.Contains(m.State)).ToList();

            // 获取注册的实体入口处理器集合
            var auditService = ServiceProvider.GetService<IAuditService>();
            var audits = await auditService.GetAuditsAsync(entityEntries, cancellationToken);

            await Set<BaseAudit>().AddRangeAsync(audits, cancellationToken);

            // 通知审计实体列表
            var mediator = ServiceProvider.GetService<IMediator>();
            await mediator?.Publish(new AuditNotification { Audits = audits }, cancellationToken);
        }


        /// <summary>
        /// 改变数据库连接。
        /// </summary>
        /// <param name="connectionStringFactory">给定改变数据库连接的工厂方法。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回是否切换的布尔值。</returns>
        public virtual Task ChangeDbConnectionAsync(Func<ITenant, string> connectionStringFactory,
            CancellationToken cancellationToken = default)
        {
            if (CurrentTenant.IsNull() || connectionStringFactory.IsNull())
                return Task.CompletedTask;

            if (!CurrentTenant.WriteConnectionSeparation)
            {
                Logger?.LogInformation($"The tenant({CurrentTenant.Name}:{CurrentTenant.Host}) connection write separation is disable");
                return Task.CompletedTask;
            }

            var connectionString = connectionStringFactory.Invoke(CurrentTenant);
            var connection = Database.GetDbConnection();
            if (connection.ConnectionString == connectionString)
            {
                Logger?.LogInformation($"The tenant({CurrentTenant.Name}:{CurrentTenant.Host}) same as the current connection string");
                return Task.CompletedTask;
            }

            try
            {
                lock (_locker)
                {
                    return ChangeDbConnectionCoreAsync();
                }
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, ex.AsInnerMessage());
                return Task.CompletedTask;
            }

            // ChangeDbConnectionCoreAsync
            Task ChangeDbConnectionCoreAsync()
            {
                return cancellationToken.RunActionOrCancellationAsync(async () =>
                {
                    if (connection.State != ConnectionState.Closed)
                    {
                        connection.Close();
                        Logger?.LogInformation("Close connection");
                    }

                    // MYSql: System.InvalidOperationException
                    //  HResult = 0x80131509
                    //  Message = Cannot change connection string on a connection that has already been opened.
                    //  Source = MySqlConnector
                    // connection.ChangeDatabase(connectionString);

                    connection.ConnectionString = connectionString;
                    Logger?.LogInformation($"The tenant({CurrentTenant.Name}:{CurrentTenant.Host}) change connection string: {connectionString}");

                    if (connection.State != ConnectionState.Open)
                    {
                        await Database.EnsureCreatedAsync();

                        await connection.OpenAsync();
                        Logger?.LogInformation("Open connection");
                    }
                });
            }
        }

    }
}
