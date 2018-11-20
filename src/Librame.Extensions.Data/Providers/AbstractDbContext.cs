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
    /// <summary>
    /// 抽象数据库上下文。
    /// </summary>
    /// <typeparam name="TDbContext">指定的数据库上下文类型。</typeparam>
    /// <typeparam name="TBuilderOptions">指定的构建器选项类型。</typeparam>
    public abstract class AbstractDbContext<TDbContext, TBuilderOptions> : DbContext, IDbContext<TBuilderOptions>
        where TDbContext : DbContext, IDbContext<TBuilderOptions>
        where TBuilderOptions : DataBuilderOptions, new()
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractDbContext{TDbContext, TBuilderOptions}"/> 实例。
        /// </summary>
        /// <param name="trackerContext">给定的 <see cref="IChangeTrackerContext"/>。</param>
        /// <param name="builderOptions">给定的 <see cref="IOptions{TBuilderOptions}"/>。</param>
        /// <param name="logger">给定的 <see cref="ILogger{TDbContext}"/>。</param>
        /// <param name="dbContextOptions">给定的 <see cref="DbContextOptions{TDbContext}"/>。</param>
        public AbstractDbContext(IChangeTrackerContext trackerContext, IOptions<TBuilderOptions> builderOptions,
            ILogger<TDbContext> logger, DbContextOptions<TDbContext> dbContextOptions)
            : base(dbContextOptions)
        {
            Logger = logger.NotDefault(nameof(logger));
            TrackerContext = trackerContext.NotDefault(nameof(trackerContext));
            BuilderOptions = builderOptions.NotDefault(nameof(builderOptions)).Value;
            
            if (BuilderOptions.EnsureDbCreated)
                DatabaseCreated();
        }


        /// <summary>
        /// 记录器。
        /// </summary>
        protected ILogger Logger { get; }


        /// <summary>
        /// 变化跟踪器上下文。
        /// </summary>
        public IChangeTrackerContext TrackerContext { get; }

        /// <summary>
        /// 构建器选项。
        /// </summary>
        public TBuilderOptions BuilderOptions { get; }
        

        /// <summary>
        /// 审计数据集。
        /// </summary>
        public DbSet<Audit> Audits { get; set; }

        /// <summary>
        /// 审计属性数据集。
        /// </summary>
        public DbSet<AuditProperty> AuditProperties { get; set; }

        /// <summary>
        /// 租户数据集。
        /// </summary>
        public DbSet<Tenant> Tenants { get; set; }


        /// <summary>
        /// 提供程序名称。
        /// </summary>
        public string DbProviderName
        {
           get { return Database.ProviderName; }
        }

        /// <summary>
        /// 启用自动事务。
        /// </summary>
        public bool AutoTransactionsEnabled
        {
            get { return Database.AutoTransactionsEnabled; }
            set { Database.AutoTransactionsEnabled = value; }
        }


        /// <summary>
        /// 确保数据库已创建。
        /// </summary>
        /// <returns>返回布尔值。</returns>
        public virtual bool DatabaseCreated()
        {
            return Database.EnsureCreated();
        }

        /// <summary>
        /// 异步确保数据库已创建。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含布尔值的异步操作。</returns>
        public virtual Task<bool> DatabaseCreatedAsync(CancellationToken cancellationToken = default)
        {
            return Database.EnsureCreatedAsync(cancellationToken);
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
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="parameters">给定的参数集合。</param>
        /// <returns>返回一个包含受影响行数的异步操作。</returns>
        public virtual Task<int> ExecuteSqlCommandAsync(string sql, IEnumerable<object> parameters, CancellationToken cancellationToken = default)
        {
            return Database.ExecuteSqlCommandAsync(sql, parameters, cancellationToken);
        }


        /// <summary>
        /// 获取数据库连接。
        /// </summary>
        /// <returns>返回 <see cref="DbConnection"/>。</returns>
        public virtual DbConnection GetDbConnection()
        {
            return Database.GetDbConnection();
        }


        /// <summary>
        /// 异步保存变化。
        /// </summary>
        /// <returns>返回一个包含受影响行数的异步操作。</returns>
        public virtual Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        /// <summary>
        /// 保存变化。
        /// </summary>
        /// <returns>返回受影响的行数。</returns>
        public override int SaveChanges()
        {
            return ChangeTracking(() => base.SaveChanges());
        }
        /// <summary>
        /// 保存变化。
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">成功接受所有变化。</param>
        /// <returns>返回受影响的行数。</returns>
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return ChangeTracking(() => base.SaveChanges(acceptAllChangesOnSuccess));
        }

        /// <summary>
        /// 异步保存变化。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含受影响行数的异步操作。</returns>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() =>
            {
                return ChangeTracking(() => base.SaveChangesAsync(cancellationToken).Result);
            });
        }
        /// <summary>
        /// 异步保存变化。
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">成功接受所有变化。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含受影响行数的异步操作。</returns>
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() =>
            {
                return ChangeTracking(() => base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken).Result);
            });
        }


        /// <summary>
        /// 变化跟踪。
        /// </summary>
        /// <param name="saveChangesFactory">给定的保存变化工厂方法。</param>
        /// <returns>返回受影响的行数。</returns>
        protected virtual int ChangeTracking(Func<int> saveChangesFactory)
        {
            ChangeTracker.DetectChanges();
            TrackerContext.Process(ChangeTracker);

            Tenant tenant = null;
            IList<Audit> audits = null;

            foreach (var handler in TrackerContext.ChangeHandlers)
            {
                if (BuilderOptions.AuditEnabled && handler is AuditEntityChangeHandler auditHandler)
                {
                    audits = auditHandler.ChangeAudits;
                    Audits.AddRange(audits);
                }

                if (BuilderOptions.TenantEnabled && handler is TenantEntityChangeHandler tenantHandler)
                {
                    tenant = tenantHandler.Tenant;
                }
            }

            // Switch Write Connection
            if (tenant.WriteConnectionSeparation)
                TrySwitchConnection(tenant.WriteConnectionString);

            var count = saveChangesFactory.Invoke();

            // Restore Default Connection
            if (tenant.WriteConnectionSeparation)
                TrySwitchConnection(tenant.DefaultConnectionString);

            if (audits?.Count > 0)
                BuilderOptions.PublishAuditEvent?.Invoke(this, audits);

            return count;
        }


        /// <summary>
        /// 开始创建模型。
        /// </summary>
        /// <param name="modelBuilder">给定的 <see cref="ModelBuilder"/>。</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureAbstractDbContext(BuilderOptions);

            base.OnModelCreating(modelBuilder);
        }


        /// <summary>
        /// 尝试切换数据库连接。
        /// </summary>
        /// <param name="connectionStringFactory">给定的数据库连接字符串工厂方法。</param>
        /// <returns>返回是否切换的布尔值。</returns>
        public virtual bool TrySwitchConnection(Func<IConnectionStrings, string> connectionStringFactory)
        {
            return TrySwitchConnection(connectionStringFactory.Invoke(BuilderOptions.Connection));
        }
        /// <summary>
        /// 尝试切换数据库连接。
        /// </summary>
        /// <param name="connectionString">给定的数据库连接字符串。</param>
        /// <returns>返回是否切换的布尔值。</returns>
        public virtual bool TrySwitchConnection(string connectionString)
        {
            if (!BuilderOptions.Connection.WriteSeparation)
            {
                Logger.LogInformation("Connection write separation is disable");
                return false;
            }

            var connection = GetDbConnection();
            if (connection.ConnectionString == connectionString)
            {
                Logger.LogInformation("Same as the current connection string");
                return false;
            }

            switch (connection.State)
            {
                case ConnectionState.Closed:
                    {
                        try
                        {
                            //connection.ChangeDatabase(connectionString);
                            connection.ConnectionString = connectionString;
                            Logger.LogInformation($"Change connection string: {connectionString}");

                            // MySql Bug: System.InvalidOperationException
                            //  HResult = 0x80131509
                            //  Message = Cannot change connection string on a connection that has already been opened.
                            //  Source = MySqlConnector

                            DatabaseCreated();

                            if (connection.State != ConnectionState.Open)
                            {
                                connection.Open();
                                Logger.LogInformation("Open connection");
                            }

                            return true;
                        }
                        catch (Exception ex)
                        {
                            Logger.LogError(ex, ex.AsInnerMessage());

                            return false;
                        }
                    }

                case ConnectionState.Open:
                    {
                        try
                        {
                            connection.Close();

                            Logger.LogInformation("Close connection");
                        }
                        catch (Exception ex)
                        {
                            Logger.LogError(ex, ex.AsInnerMessage());

                            return false;
                        }
                    }
                    goto case ConnectionState.Closed;

                default:
                    goto case ConnectionState.Open;
            }
        }

    }
}
