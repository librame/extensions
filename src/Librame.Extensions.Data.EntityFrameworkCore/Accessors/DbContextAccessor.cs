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
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// <see cref="DbContext"/> 访问器。
    /// </summary>
    public class DbContextAccessor : DbContext, IAccessor
    {
        private static object _locker = new object();


        /// <summary>
        /// 构造一个 <see cref="DbContextAccessor"/> 实例。
        /// </summary>
        /// <param name="options">给定的 <see cref="DbContextOptions"/>。</param>
        public DbContextAccessor(DbContextOptions options)
            : base(options)
        {
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
        public virtual ITenant CurrentTenant => BuilderOptions.LocalTenant;


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
        /// 重载保存更改。
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">指示是否在更改已成功发送到数据库之后调用。</param>
        /// <returns>返回受影响的行数。</returns>
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            // 改变为写入数据库（支持读写分离）
            ChangeDbConnection(tenant => tenant.WriteConnectionString).Wait();

            PreproccessSaveChangesAsync().Wait();

            var count = base.SaveChanges(acceptAllChangesOnSuccess);

            // 尝试还原改变的数据库连接
            ChangeDbConnection(tenant => tenant.DefaultConnectionString).Wait();

            return count;
        }

        /// <summary>
        /// 重载异步保存更改。
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">指示是否在更改已成功发送到数据库之后调用。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含受影响行数的异步操作。</returns>
        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            // 改变为写入数据库（支持读写分离）
            await ChangeDbConnection(tenant => tenant.WriteConnectionString);

            await PreproccessSaveChangesAsync();

            var count = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

            // 尝试还原改变的数据库连接
            await ChangeDbConnection(tenant => tenant.DefaultConnectionString);

            return count;
        }

        /// <summary>
        /// 异步预处理保存更改。
        /// </summary>
        /// <returns>返回 <see cref="Task"/>。</returns>
        protected virtual async Task PreproccessSaveChangesAsync()
        {
            // 默认仅拦截实体的增删改操作
            var entityStates = new EntityState[] { EntityState.Added, EntityState.Modified, EntityState.Deleted };

            // 得到变化的实体集合
            var entityEntries = ChangeTracker.Entries()
                .Where(m => m.Entity != null && entityStates.Contains(m.State)).ToList();

            // 获取注册的实体入口处理器集合
            var auditService = ServiceProvider.GetService<IAuditService>();
            var audits = await auditService.GetAuditsAsync(entityEntries);

            await BaseAudits.AddRangeAsync(audits);
        }


        /// <summary>
        /// 改变数据库连接。
        /// </summary>
        /// <param name="connectionStringFactory">给定改变数据库连接的工厂方法。</param>
        /// <returns>返回是否切换的布尔值。</returns>
        public virtual Task ChangeDbConnection(Func<ITenant, string> connectionStringFactory)
        {
            if (null == CurrentTenant || null == connectionStringFactory)
                return Task.CompletedTask;

            var connectionString = connectionStringFactory.Invoke(CurrentTenant);
            if (!CurrentTenant.WriteConnectionSeparation)
            {
                Logger?.LogInformation($"The tenant({CurrentTenant.Name}:{CurrentTenant.Host}) connection write separation is disable");
                return Task.CompletedTask;
            }

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
                    switch (connection.State)
                    {
                        case ConnectionState.Closed:
                            {
                                // MYSql: System.InvalidOperationException
                                //  HResult = 0x80131509
                                //  Message = Cannot change connection string on a connection that has already been opened.
                                //  Source = MySqlConnector

                                // connection.ChangeDatabase(connectionString);
                                connection.ConnectionString = connectionString;
                                Logger?.LogInformation($"The tenant({CurrentTenant.Name}:{CurrentTenant.Host}) change connection string: {connectionString}");

                                if (connection.State != ConnectionState.Open)
                                {
                                    Database.EnsureCreated();

                                    connection.Open();
                                    Logger?.LogInformation("Open connection");
                                }
                            }
                            break;

                        case ConnectionState.Open:
                            {
                                connection.Close();
                                Logger?.LogInformation("Close connection");
                            }
                            goto case ConnectionState.Closed;

                        default:
                            goto case ConnectionState.Open;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex, ex.AsInnerMessage());
            }

            return Task.CompletedTask;
        }
    }
}
