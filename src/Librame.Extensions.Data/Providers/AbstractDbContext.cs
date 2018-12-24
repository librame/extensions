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
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

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


        /// <summary>
        /// 初始化数据库。
        /// </summary>
        protected override void InitializeDatabase()
        {
            DatabaseCreatedActions.Add(() => AddDefaultTenant());

            base.InitializeDatabase();
        }


        /// <summary>
        /// 添加默认租户。
        /// </summary>
        protected virtual void AddDefaultTenant()
        {
            if (BuilderOptions.LocalTenant is Tenant tenant)
            {
                if (!Tenants.Any(p => p.Name == tenant.Name && p.Host == tenant.Host)
                    && !Tenants.Local.Any(p => p.Name == tenant.Name && p.Host == tenant.Host))
                {
                    Tenants.Add(tenant);
                }
            }
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

            InitializeDatabase();
        }


        /// <summary>
        /// 初始化数据库。
        /// </summary>
        protected virtual void InitializeDatabase()
        {
            if (BuilderOptions.DefaultSnapshotName.IsNotEmpty())
                SnapshotName = BuilderOptions.DefaultSnapshotName;

            if (BuilderOptions.EnsureDatabase)
                CreateDatabase();
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
            InitializeDbContext();
        }


        /// <summary>
        /// 初始化数据库上下文。
        /// </summary>
        protected virtual void InitializeDbContext()
        {
            DatabaseCreatedActions.Add(() => UpdateMigrationAudits());
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
        /// 数据库已创建动作列表。
        /// </summary>
        public IList<Action> DatabaseCreatedActions { get; set; } = new List<Action>();


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
        /// 快照名称。
        /// </summary>
        public string SnapshotName { get; set; } = "LibrameSnapshot";


        /// <summary>
        /// 变化跟踪。
        /// </summary>
        /// <param name="saveChangesFactory">给定的保存变化工厂方法。</param>
        /// <returns>返回受影响的行数。</returns>
        protected abstract int ChangeTracking(Func<int> saveChangesFactory);


        /// <summary>
        /// 更新迁移审计集合。
        /// </summary>
        /// <param name="operationsFactory">给定的迁移操作列表工厂方法（可选）。</param>
        protected virtual void UpdateMigrationAudits(Func<IReadOnlyList<MigrationOperation>, bool> operationsFactory = null)
        {
            MigrationAudit lastMigrationAudit = null;
            if (MigrationAudits.Any())
            {
                lastMigrationAudit = MigrationAudits.FirstOrDefault(p => p.CreateTime == MigrationAudits.Max(s => s.CreateTime));
            }
            else if (MigrationAudits.Local.Any())
            {
                lastMigrationAudit = MigrationAudits.Local.FirstOrDefault(p => p.CreateTime == MigrationAudits.Local.Max(s => s.CreateTime));
            }

            IModel lastModel = null;
            if (lastMigrationAudit.IsNotDefault())
            {
                var lastSnapshotCode = lastMigrationAudit.SnapshotCode.FromEncodingBase64String();
                lastModel = ModelSnapshotHelper.CreateSnapshot(this, lastSnapshotCode, SnapshotName)?.Model;
            }

            var differ = this.GetService<IMigrationsModelDiffer>();
            var operations = differ.GetDifferences(lastModel, Model);
            // 如果模型无变化则直接返回
            if (operations.IsEmpty()) return;

            var executeResult = operationsFactory?.Invoke(operations);
            // 如果执行结果存在且不成功则直接返回
            if (executeResult.HasValue && !executeResult.Value) return;

            var snapshotCode = ModelSnapshotHelper.GenerateSnapshotCode(this, SnapshotName);
            var snapshotBuffer = snapshotCode.AsEncodingBytes();
            var snapshotHash = snapshotBuffer.Sha1().AsBase64String();

            if (!MigrationAudits.Any(p => p.SnapshotHash == snapshotHash && p.SnapshotName == SnapshotName)
                && !MigrationAudits.Local.Any(p => p.SnapshotHash == snapshotHash && p.SnapshotName == SnapshotName))
            {
                MigrationAudits.Add(new MigrationAudit
                {
                    SnapshotCode = snapshotBuffer.AsBase64String(),
                    SnapshotHash = snapshotHash,
                    SnapshotName = SnapshotName
                });

                base.SaveChanges();
            }
        }


        /// <summary>
        /// 执行迁移命令集合。
        /// </summary>
        /// <param name="operations">给定的 <see cref="IReadOnlyList{MigrationOperation}"/>。</param>
        /// <returns>返回布尔值。</returns>
        protected virtual bool ExecuteMigrationCommands(IReadOnlyList<MigrationOperation> operations)
        {
            var executeResult = false;
            if (operations.IsEmpty()) return executeResult;

            var generator = this.GetService<IMigrationsSqlGenerator>();
            var commands = generator.Generate(operations, Model);

            using (new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled))
            {
                Database.OpenConnection();

                try
                {
                    foreach (var cmd in commands)
                    {
                        IDbContextTransaction transaction = null;

                        try
                        {
                            if (transaction == null
                                && !cmd.TransactionSuppressed)
                            {
                                transaction = Database.BeginTransaction();
                            }

                            if (transaction != null
                                && cmd.TransactionSuppressed)
                            {
                                transaction.Commit();
                                transaction.Dispose();
                                transaction = null;
                            }

                            Database.ExecuteSqlCommand(cmd.CommandText);
                            transaction?.Commit();

                            if (!executeResult)
                                executeResult = true;
                        }
                        catch (Exception ex)
                        {
                            transaction?.Rollback();
                            Logger.LogWarning(ex, ex.AsInnerMessage());
                        }
                        finally
                        {
                            transaction?.Dispose();
                        }
                    }
                }
                finally
                {
                    Database.CloseConnection();
                }
            }

            return executeResult;
        }


        /// <summary>
        /// 创建数据库。
        /// </summary>
        public virtual void CreateDatabase()
        {
            if (Database.EnsureCreated())
            {
                foreach (var action in DatabaseCreatedActions)
                    action?.Invoke();
            }
        }

        /// <summary>
        /// 创建数据库。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个异步操作。</returns>
        public virtual Task CreateDatabaseAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            CreateDatabase();

            return Task.CompletedTask;
        }


        /// <summary>
        /// 更新数据库。
        /// </summary>
        public virtual void UpdateDatabase()
        {
            UpdateMigrationAudits(ExecuteMigrationCommands);
        }

        /// <summary>
        /// 更新数据库。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个异步操作。</returns>
        public virtual Task UpdateDatabaseAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            UpdateDatabase();

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
        /// <param name="parameters">给定的参数集合。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含受影响行数的异步操作。</returns>
        public virtual Task<int> ExecuteSqlCommandAsync(string sql, IEnumerable<object> parameters, CancellationToken cancellationToken = default)
        {
            return Database.ExecuteSqlCommandAsync(sql, parameters, cancellationToken);
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

            var connection = Database.GetDbConnection();
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
                                CreateDatabase();

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
