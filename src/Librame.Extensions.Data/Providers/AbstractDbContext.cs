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
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data
{
    using Builders;

    /// <summary>
    /// 抽象数据库上下文。
    /// </summary>
    /// <typeparam name="TDbContext">指定的数据库上下文类型。</typeparam>
    /// <typeparam name="TBuilderOptions">指定的构建器选项类型。</typeparam>
    public abstract class AbstractDbContext<TDbContext, TBuilderOptions> : AbstractDbContext<TDbContext, Tenant, TBuilderOptions>, IDbContext<TBuilderOptions>
        where TDbContext : DbContext, IDbContext
        where TBuilderOptions : DataBuilderOptions, new()
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractDbContext{TDbContext, TBuilderOptions}"/> 实例。
        /// </summary>
        /// <param name="builderOptions">给定的 <see cref="IOptions{TBuilderOptions}"/>。</param>
        /// <param name="dbContextOptions">给定的 <see cref="DbContextOptions{TDbContext}"/>。</param>
        public AbstractDbContext(IOptions<TBuilderOptions> builderOptions, DbContextOptions<TDbContext> dbContextOptions)
            : base(builderOptions, dbContextOptions)
        {
        }
    }
    /// <summary>
    /// 抽象数据库上下文。
    /// </summary>
    /// <typeparam name="TDbContext">指定的数据库上下文类型。</typeparam>
    /// <typeparam name="TTenant">指定的租户类型。</typeparam>
    /// <typeparam name="TBuilderOptions">指定的构建器选项类型。</typeparam>
    public abstract class AbstractDbContext<TDbContext, TTenant, TBuilderOptions> : AbstractDbContext<TDbContext>, IDbContext<TTenant, TBuilderOptions>
        where TDbContext : DbContext, IDbContext
        where TTenant : class, ITenant
        where TBuilderOptions : DataBuilderOptions, new()
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractDbContext{TDbContext, TTenant, TBuilderOptions}"/> 实例。
        /// </summary>
        /// <param name="builderOptions">给定的 <see cref="IOptions{TBuilderOptions}"/>。</param>
        /// <param name="dbContextOptions">给定的 <see cref="DbContextOptions{TDbContext}"/>。</param>
        public AbstractDbContext(IOptions<TBuilderOptions> builderOptions, DbContextOptions<TDbContext> dbContextOptions)
            : base(dbContextOptions)
        {
            BuilderOptions = builderOptions.NotDefault(nameof(builderOptions)).Value;
        }


        /// <summary>
        /// 构建器选项。
        /// </summary>
        public TBuilderOptions BuilderOptions { get; }

        /// <summary>
        /// 租户上下文。
        /// </summary>
        /// <value>
        /// 返回 <see cref="ITenantContext{TTenant}"/>。
        /// </value>
        public ITenantContext<TTenant> TenantContext => this.GetService<ITenantContext<TTenant>>();

        /// <summary>
        /// 当前租户。
        /// </summary>
        /// <value>
        /// 返回 <see cref="ITenant"/>。
        /// </value>
        public override ITenant CurrentTenant => TenantContext.GetTenant(Tenants, BuilderOptions);


        /// <summary>
        /// 租户数据集。
        /// </summary>
        /// <value>
        /// 返回 <see cref="DbSet{Tenant}"/>。
        /// </value>
        public DbSet<TTenant> Tenants { get; set; }

        
        /// <summary>
        /// 变化跟踪。
        /// </summary>
        /// <param name="saveChangesFactory">给定的保存变化工厂方法。</param>
        /// <returns>返回受影响的行数。</returns>
        protected override int ChangeTracking(Func<int> saveChangesFactory)
        {
            ChangeTracker.DetectChanges();

            // 尝试绑定当前租户
            if (TrackerContext.TryGetChangeHandler(out ITenantChangeHandler tenantChange))
                tenantChange.SetTenant = CurrentTenant;

            TrackerContext.Process(ChangeTracker, BuilderOptions);
            
            // 处理变化的审计列表
            IList<Audit> audits = null;

            foreach (var handler in TrackerContext.ChangeHandlers)
            {
                if (handler is IAuditChangeHandler auditHandler && BuilderOptions.AuditEnabled)
                {
                    audits = auditHandler.ChangeAudits;
                    Audits.AddRange(audits);
                }
            }

            var count = SwitchConnectionProcess(saveChangesFactory);
            
            if (audits?.Count > 0 && count > 0)
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
    }


    /// <summary>
    /// 抽象数据库上下文。
    /// </summary>
    /// <typeparam name="TDbContext">指定的数据库上下文类型。</typeparam>
    public abstract class AbstractDbContext<TDbContext> : DbContext, IDbContext
        where TDbContext : DbContext, IDbContext
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractDbContext{TDbContext}"/> 实例。
        /// </summary>
        /// <param name="dbContextOptions">给定的 <see cref="DbContextOptions{TDbContext}"/>。</param>
        public AbstractDbContext(DbContextOptions<TDbContext> dbContextOptions)
            : base(dbContextOptions)
        {
        }


        /// <summary>
        /// 记录器。
        /// </summary>
        /// <value>
        /// 返回 <see cref="ILogger"/>。
        /// </value>
        protected ILogger Logger => this.GetService<ILogger<TDbContext>>();


        /// <summary>
        /// 变化跟踪器上下文。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IChangeTrackerContext"/>。
        /// </value>
        public IChangeTrackerContext TrackerContext => this.GetService<IChangeTrackerContext>();


        /// <summary>
        /// 审计数据集。
        /// </summary>
        /// <value>
        /// 返回 <see cref="DbSet{Audit}"/>。
        /// </value>
        public DbSet<Audit> Audits { get; set; }

        /// <summary>
        /// 审计属性数据集。
        /// </summary>
        /// <value>
        /// 返回 <see cref="DbSet{AuditProperty}"/>。
        /// </value>
        public DbSet<AuditProperty> AuditProperties { get; set; }

        /// <summary>
        /// 迁移审计数据集。
        /// </summary>
        /// <value>
        /// 返回 <see cref="DbSet{MigrationAudit}"/>。
        /// </value>
        public DbSet<MigrationAudit> MigrationAudits { get; set; }


        /// <summary>
        /// 当前租户。
        /// </summary>
        /// <value>
        /// 返回 <see cref="Tenant"/>。
        /// </value>
        public abstract ITenant CurrentTenant { get; }


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
        /// 变化跟踪。
        /// </summary>
        /// <param name="saveChangesFactory">给定的保存变化工厂方法。</param>
        /// <returns>返回受影响的行数。</returns>
        protected abstract int ChangeTracking(Func<int> saveChangesFactory);


        /// <summary>
        /// 确保数据库已创建。
        /// </summary>
        /// <returns>返回布尔值。</returns>
        public virtual void DatabaseCreated()
        {
            Database.EnsureCreated();
        }

        /// <summary>
        /// 异步确保数据库已创建。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个异步操作。</returns>
        public virtual Task DatabaseCreatedAsync(CancellationToken cancellationToken = default)
        {
            return Database.EnsureCreatedAsync(cancellationToken);
        }


        /// <summary>
        /// 数据库更新表集合。
        /// </summary>
        public virtual void DatabaseUpdateTables()
        {
            var dependencies = this.GetService<RelationalDatabaseCreatorDependencies>();
            if (dependencies.IsDefault()) return;

            // 获取初始模型的所有操作命令集合
            var operations = dependencies.ModelDiffer.GetDifferences(null, dependencies.Model);
            var commands = dependencies.MigrationsSqlGenerator.Generate(operations, dependencies.Model);
            if (commands.IsEmpty()) return;

            var updateCommands = new List<MigrationCommand>();

            foreach (var cmd in commands)
            {
                // 查找此命令是否已执行
                var commandHash = cmd.CommandText.Sha256Base64String();
                var exists = MigrationAudits.FirstOrDefault(p => p.CommandHash == commandHash);
                if (exists.IsDefault())
                {
                    updateCommands.Add(cmd);

                    MigrationAudits.Add(new MigrationAudit
                    {
                        CommandHash = commandHash,
                        CommandText = cmd.CommandText
                    });
                }
            }
            
            if (updateCommands.IsNotEmpty() && MigrationAudits.IsNotEmpty())
            {
                // 初始已通过 DatabaseCreated 执行，不手动执行命令
                dependencies.MigrationCommandExecutor.ExecuteNonQuery(updateCommands, dependencies.Connection);
                SaveChanges();
            }
        }

        /// <summary>
        /// 数据库更新表集合。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个异步操作。</returns>
        public virtual Task DatabaseUpdateTablesAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            DatabaseUpdateTables();

            return Task.CompletedTask;
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
        /// 切换数据库连接执行。
        /// </summary>
        /// <param name="processAction">给定的处理动作。</param>
        public virtual void SwitchConnectionProcess(Action processAction)
        {
            // Switch Write Connection
            if (CurrentTenant.WriteConnectionSeparation)
                TrySwitchConnection(CurrentTenant.WriteConnectionString);

            processAction.Invoke();

            // Restore Default Connection
            if (CurrentTenant.WriteConnectionSeparation)
                TrySwitchConnection(CurrentTenant.DefaultConnectionString);
        }
        /// <summary>
        /// 切换数据库连接执行。
        /// </summary>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="processFactory">给定的处理工厂方法。</param>
        /// <returns>返回结果实例。</returns>
        public virtual TResult SwitchConnectionProcess<TResult>(Func<TResult> processFactory)
        {
            // Switch Write Connection
            if (CurrentTenant.WriteConnectionSeparation)
                TrySwitchConnection(CurrentTenant.WriteConnectionString);
            
            var output = processFactory.Invoke();

            // Restore Default Connection
            if (CurrentTenant.WriteConnectionSeparation)
                TrySwitchConnection(CurrentTenant.DefaultConnectionString);

            return output;
        }


        /// <summary>
        /// 尝试切换数据库连接。
        /// </summary>
        /// <param name="connectionStringFactory">给定的数据库连接字符串工厂方法。</param>
        /// <returns>返回是否切换的布尔值。</returns>
        public virtual bool TrySwitchConnection(Func<IConnectionStrings, string> connectionStringFactory)
        {
            return TrySwitchConnection(connectionStringFactory.Invoke(CurrentTenant));
        }
        /// <summary>
        /// 尝试切换数据库连接。
        /// </summary>
        /// <param name="connectionString">给定的数据库连接字符串。</param>
        /// <returns>返回是否切换的布尔值。</returns>
        public virtual bool TrySwitchConnection(string connectionString)
        {
            if (!CurrentTenant.WriteConnectionSeparation)
            {
                Logger.LogInformation($"The tenant({CurrentTenant.Name}:{CurrentTenant.Host}) connection write separation is disable");
                return false;
            }

            var connection = GetDbConnection();
            if (connection.ConnectionString == connectionString)
            {
                Logger.LogInformation($"The tenant({CurrentTenant.Name}:{CurrentTenant.Host}) same as the current connection string");
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
                            Logger.LogInformation($"The tenant({CurrentTenant.Name}:{CurrentTenant.Host}) change connection string: {connectionString}");

                            // MySql Bug: System.InvalidOperationException
                            //  HResult = 0x80131509
                            //  Message = Cannot change connection string on a connection that has already been opened.
                            //  Source = MySqlConnector

                            if (connection.State != ConnectionState.Open)
                            {
                                DatabaseCreated();

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
