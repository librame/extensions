#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Accessors
{
    using Core.Services;
    using Data.Aspects;
    using Data.Builders;
    using Data.Resources;
    using Data.Services;
    using Data.Stores;
    using Data.Validators;

    /// <summary>
    /// 数据库上下文访问器基类。
    /// </summary>
    public class DbContextAccessorBase : DbContext, IAccessor
    {
        /// <summary>
        /// 构造一个数据库上下文访问器基类。
        /// </summary>
        /// <param name="options">给定的 <see cref="DbContextOptions"/>。</param>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        [SuppressMessage("Globalization", "CA1303:请不要将文本作为本地化参数传递", Justification = "<挂起>")]
        protected DbContextAccessorBase(DbContextOptions options)
            : base(options)
        {
            var extension = options.FindExtension<CoreOptionsExtension>();
            var relationalExtension = RelationalOptionsExtension.Extract(options);

            MemoryCache = extension.MemoryCache;

            var dataBuilder = MemoryCache.GetDataBuilder();
            if (dataBuilder.IsNull())
                throw new InvalidOperationException($"You need to register to builder.{nameof(AccessorDataBuilderExtensions.AddAccessor)}().");

            Dependency = dataBuilder.Dependency as DataBuilderDependency;

            if (Dependency.Options.DefaultTenant.IsNull())
                throw new InvalidOperationException($"The data builder dependency '{Dependency}' options default tenant is null.");

            // Database.GetDbConnection().ConnectionString 的信息不一定完整（比如密码）
            if (relationalExtension.ConnectionString.IsEmpty())
                throw new InvalidOperationException($"The relational options extension '{relationalExtension}' connection string is empty.");

            CurrentTenant = Dependency.Options.DefaultTenant;
            CurrentConnectionString = relationalExtension.ConnectionString;

            Dependency.Options.PostAccessorInitializedAction?.Invoke(this);
        }


        /// <summary>
        /// 构建器依赖。
        /// </summary>
        public DataBuilderDependency Dependency { get; }


        #region EnsureDatabase

        /// <summary>
        /// 确保已创建数据库。
        /// </summary>
        /// <returns>返回是否已创建的布尔值。</returns>
        internal protected virtual bool EnsureDatabaseCreated()
        {
            if (CreationValidator.IsCreated(this))
                return true;

            if (Database.EnsureCreated())
            {
                CreationValidator.SetCreated(this);

                Dependency.Options.PostDatabaseCreatedAction?.Invoke(this);

                // 初始化迁移
                if (!IsFromMigrateInvoke)
                    Migrate();

                return true;
            }

            return false;
        }

        /// <summary>
        /// 异步确保已创建数据库。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含是否已创建的布尔值的异步操作。</returns>
        protected virtual async Task<bool> EnsureDatabaseCreatedAsync
            (CancellationToken cancellationToken = default)
        {
            if (await CreationValidator.IsCreatedAsync(this, cancellationToken).ConfigureAwait())
                return true;

            if (await Database.EnsureCreatedAsync(cancellationToken).ConfigureAwait())
            {
                await CreationValidator.SetCreatedAsync(this, cancellationToken).ConfigureAwait();

                Dependency.Options.PostDatabaseCreatedAction?.Invoke(this);

                // 初始化迁移
                if (!IsFromMigrateInvoke)
                    await MigrateAsync(cancellationToken).ConfigureAwait();

                return true;
            }

            return false;
        }


        /// <summary>
        /// 确保已更改数据库。
        /// </summary>
        /// <param name="connectionString">给定的连接字符串。</param>
        /// <returns>返回是否已更改的布尔值。</returns>
        protected virtual bool EnsureDatabaseChanged(string connectionString)
        {
            connectionString.NotEmpty(connectionString);
            //Logger.LogTrace(InternalResource.ChangeConnectionStringIsEmpty);

            if (IsCurrentConnectionString(connectionString))
            {
                Logger.LogTrace(InternalResource.ChangeConnectionStringSameAsCurrent);
                return false;
            }

            // 手动关闭当前连接
            var connection = Database.GetDbConnection();
            if (connection.State != ConnectionState.Closed)
            {
                connection.Close();
                Logger.LogTrace(InternalResource.CloseConnectionFormat.Format(connection.ConnectionString));
            }

            connection.ConnectionString = CurrentConnectionString = connectionString;
            Logger.LogInformation(InternalResource.ChangeNewConnectionFormat.Format(CurrentTenant.ToString(),
                CurrentConnectionString));

            Dependency.Options.PostDatabaseChangedAction?.Invoke(this);

            // 先尝试创建数据库（如果数据库不存在，打开时会抛出异常）
            if (Dependency.Options.SupportsCreateDatabase)
                EnsureDatabaseCreated();

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
                Logger.LogInformation(InternalResource.OpenConnectionFormat.Format(CurrentConnectionString));
            }

            return true;
        }

        /// <summary>
        /// 异步确保已更改数据库。
        /// </summary>
        /// <param name="connectionString">给定的连接字符串。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含是否已更改的布尔值的异步操作。</returns>
        protected virtual async Task<bool> EnsureDatabaseChangedAsync(string connectionString,
            CancellationToken cancellationToken = default)
        {
            connectionString.NotEmpty(connectionString);
            //Logger.LogTrace(InternalResource.ChangeConnectionStringIsEmpty);

            if (IsCurrentConnectionString(connectionString))
            {
                Logger.LogTrace(InternalResource.ChangeConnectionStringSameAsCurrent);
                return false;
            }

            var connection = Database.GetDbConnection();
            if (connection.State != ConnectionState.Closed)
            {
                await connection.CloseAsync().ConfigureAwait();
                Logger.LogTrace(InternalResource.CloseConnectionFormat.Format(connection.ConnectionString));
            }

            connection.ConnectionString = CurrentConnectionString = connectionString;
            Logger?.LogInformation(InternalResource.ChangeNewConnectionFormat.Format(CurrentTenant.ToString(),
                CurrentConnectionString));

            Dependency.Options.PostDatabaseChangedAction?.Invoke(this);

            // 先尝试创建数据库（如果数据库不存在，打开时会抛出异常）
            if (Dependency.Options.SupportsCreateDatabase)
                await EnsureDatabaseCreatedAsync(cancellationToken).ConfigureAwait();

            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync(cancellationToken).ConfigureAwait();
                Logger?.LogInformation(InternalResource.OpenConnectionFormat.Format(CurrentConnectionString));
            }

            return true;
        }

        #endregion


        #region OnModelCreating

        /// <summary>
        /// 配置模型构建器（配置模型映射推荐重写 <see cref="OnModelCreatingCore(ModelBuilder)"/> 而不是本方法）。
        /// </summary>
        /// <param name="modelBuilder">给定的 <see cref="ModelBuilder"/>。</param>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreatingCore(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                entityType.TryAddDeleteStatusQueryFilter();
            }
        }

        /// <summary>
        /// 配置模型构建器核心（重写时注意区分 <see cref="OnModelCreating(ModelBuilder)"/> 与本方法，否则可能会导致访问冲突而退出进程）。
        /// </summary>
        /// <param name="modelBuilder">给定的 <see cref="ModelBuilder"/>。</param>
        protected virtual void OnModelCreatingCore(ModelBuilder modelBuilder)
        {
        }

        #endregion


        #region IAccessor

        /// <summary>
        /// 内存缓存。
        /// </summary>
        /// <value>返回 <see cref="IMemoryCache"/>。</value>
        public IMemoryCache MemoryCache { get; }

        /// <summary>
        /// 时钟服务。
        /// </summary>
        /// <value>返回 <see cref="IClockService"/>。</value>
        public IClockService Clock
            => this.GetService<IClockService>();

        /// <summary>
        /// 数据库创建验证器。
        /// </summary>
        /// <value>返回 <see cref="IDatabaseCreationValidator"/>。</value>
        public IDatabaseCreationValidator CreationValidator
            => this.GetService<IDatabaseCreationValidator>();


        /// <summary>
        /// 当前时间戳。
        /// </summary>
        public DateTimeOffset CurrentTimestamp
            => Clock.GetNowOffset();

        /// <summary>
        /// 当前类型。
        /// </summary>
        public Type CurrentType
            => GetType();


        /// <summary>
        /// 存在任何数据集集合（默认返回 FALSE）。
        /// </summary>
        /// <returns>返回布尔值。</returns>
        public virtual bool AnySets()
            => false;


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

        #endregion


        #region IMigration

        /// <summary>
        /// 是从迁移调用。
        /// </summary>
        public bool IsFromMigrateInvoke { get; protected set; }

        /// <summary>
        /// 包含初始化数据。
        /// </summary>
        public bool ContainsInitializationData { get; protected set; }


        /// <summary>
        /// 数据库迁移，默认配置在改变数据库连接的后置动作时被调用（如果选项已启用迁移，则调用迁移核心方法，反之则调用数据库默认迁移方法）。
        /// </summary>
        /// <returns>返回可能存在受影响的行数。</returns>
        public virtual int Migrate()
        {
            var count = 0;

            if (!Dependency.Options.Stores.UseDataMigration)
            {
                Database.Migrate();
                return count;
            }

            // 如果是从保存更改调用的写入分离操作
            if (IsFromSaveChangesInvoke)
            {
                // 已区分读写，直接调用迁移核心
                count = MigrateCore();
                return count;
            }

            // 标识迁移调用，防止死循环
            IsFromMigrateInvoke = true;

            // 外部直接调用迁移，需引用写入分离操作
            count = WritingSeparationOperation(() =>
            {
                return MigrateCore();
            },
            () =>
            {
                return MigrateSynchronization();
            });

            ContainsInitializationData = false;
            IsFromMigrateInvoke = false;

            return count;
        }

        /// <summary>
        /// 异步数据库迁移，默认配置在改变数据库连接的后置动作时被调用（如果选项已启用迁移，则调用迁移核心方法，反之则调用数据库默认迁移方法）。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含可能存在受影响的行数的异步操作。</returns>
        public virtual async Task<int> MigrateAsync(CancellationToken cancellationToken = default)
        {
            var count = 0;

            if (!Dependency.Options.Stores.UseDataMigration)
            {
                await Database.MigrateAsync(cancellationToken).ConfigureAwait();
                return count;
            }

            // 如果是从保存更改调用的写入分离操作
            if (IsFromSaveChangesInvoke)
            {
                // 已区分读写，直接调用迁移核心
                count = await MigrateCoreAsync(cancellationToken: cancellationToken).ConfigureAwait();
                return count;
            }

            // 标识迁移调用，防止死循环
            IsFromMigrateInvoke = true;

            // 外部直接调用迁移，需引用写入分离操作
            count = await WritingSeparationOperationAsync(() =>
            {
                return MigrateCoreAsync(cancellationToken: cancellationToken);
            },
            () =>
            {
                return MigrateSynchronizationAsync(cancellationToken);
            },
            cancellationToken).ConfigureAwait();

            ContainsInitializationData = false;
            IsFromMigrateInvoke = false;

            return count;
        }


        /// <summary>
        /// 迁移核心（不区分读写）。
        /// </summary>
        /// <returns>返回受影响的行数。</returns>
        protected virtual int MigrateCore()
        {
            var migration = this.GetService<IMigrationAccessorService>();
            migration.Migrate(this);

            Dependency.Options.PostMigratedAction?.Invoke(this);

            if (Dependency.Options.UseInitializer)
            {
                var initializer = this.GetService<IStoreInitializer>();
                initializer.Initialize(this);

                ContainsInitializationData = true;
            }

            var count = 0;

            if (RequiredSaveChanges)
                count = SaveChangesCore(true);

            return count;
        }

        /// <summary>
        /// 异步迁移核心（不区分读写）。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含受影响的行数的异步操作。</returns>
        protected virtual async Task<int> MigrateCoreAsync(CancellationToken cancellationToken = default)
        {
            var migration = this.GetService<IMigrationAccessorService>();
            await migration.MigrateAsync(this, cancellationToken).ConfigureAwait();

            Dependency.Options.PostMigratedAction?.Invoke(this);

            if (Dependency.Options.UseInitializer)
            {
                var initializer = this.GetService<IStoreInitializer>();
                await initializer.InitializeAsync(this, cancellationToken).ConfigureAwait();

                ContainsInitializationData = true;
            }

            var count = 0;

            if (RequiredSaveChanges)
                count = await SaveChangesCoreAsync(true, cancellationToken).ConfigureAwait();

            return count;
        }


        /// <summary>
        /// 迁移同步。
        /// </summary>
        /// <returns>返回受影响的行数。</returns>
        protected virtual int MigrateSynchronization()
        {
            // 可能存在结构迁移
            var migration = this.GetService<IMigrationAccessorService>();
            migration.Migrate(this);

            //Dependency.Options.PostMigratedAction?.Invoke(this);

            return this.SubmitSaveChangesSynchronization();
        }

        /// <summary>
        /// 异步迁移同步。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含受影响的行数的异步操作。</returns>
        protected virtual async Task<int> MigrateSynchronizationAsync(CancellationToken cancellationToken = default)
        {
            // 可能存在结构迁移
            var migration = this.GetService<IMigrationAccessorService>();
            await migration.MigrateAsync(this, cancellationToken).ConfigureAwait();

            //Dependency.Options.PostMigratedAction?.Invoke(this);

            return await this.SubmitSaveChangesSynchronizationAsync(cancellationToken).ConfigureAwait();
        }

        #endregion


        #region ISaveChanges

        /// <summary>
        /// 是从保存更改调用。
        /// </summary>
        public bool IsFromSaveChangesInvoke { get; protected set; }

        /// <summary>
        /// 所需的保存更改。
        /// </summary>
        public bool RequiredSaveChanges { get; set; }


        /// <summary>
        /// 重载保存更改。
        /// </summary>
        /// <returns>返回受影响的行数。</returns>
        public override int SaveChanges()
            => SaveChanges(acceptAllChangesOnSuccess: true);

        /// <summary>
        /// 重载保存更改。
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">指示是否在更改已成功发送到数据库之后调用。</param>
        /// <returns>返回受影响的行数。</returns>
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            // 标识保存更改调用，防止死循环
            IsFromSaveChangesInvoke = true;

            var count = WritingSeparationOperation(() =>
            {
                return SaveChangesCore(acceptAllChangesOnSuccess);
            },
            () =>
            {
                return this.SubmitSaveChangesSynchronization();
            });

            IsFromSaveChangesInvoke = false;

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
            // 标识保存更改调用，防止死循环
            IsFromSaveChangesInvoke = true;

            var count = await WritingSeparationOperationAsync(() =>
            {
                return SaveChangesCoreAsync(acceptAllChangesOnSuccess, cancellationToken);
            },
            () =>
            {
                return this.SubmitSaveChangesSynchronizationAsync(cancellationToken);
            },
            cancellationToken).ConfigureAwait();

            IsFromSaveChangesInvoke = false;

            return count;
        }

        /// <summary>
        /// 重载异步保存更改。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含受影响行数的异步操作。</returns>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => base.SaveChangesAsync(acceptAllChangesOnSuccess: true, cancellationToken);


        /// <summary>
        /// 保存更改核心（不区分读写，由调用方区分）。
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">指示是否在更改已成功发送到数据库之后调用。</param>
        /// <returns>返回受影响的行数。</returns>
        protected virtual int SaveChangesCore(bool acceptAllChangesOnSuccess)
        {
            var aspects = this.GetService<IServicesManager<ISaveChangesAccessorAspect>>();
            aspects.ForEach(aspect =>
            {
                if (aspect.Enabled)
                    aspect.PreProcess(this); // 前置处理保存变化
            });

            // 保存更改
            var count = this.SubmitSaveChanges(acceptAllChangesOnSuccess);

            Dependency.Options.PostSaveChangesAction?.Invoke(this);

            aspects.ForEach(aspect =>
            {
                if (aspect.Enabled)
                    aspect.PostProcess(this); // 后置处理保存变化
            });

            return count;
        }

        /// <summary>
        /// 异步保存更改核心（不区分读写，由调用方区分）。
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">指示是否在更改已成功发送到数据库之后调用。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含受影响行数的异步操作。</returns>
        protected virtual async Task<int> SaveChangesCoreAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            var aspects = this.GetService<IServicesManager<ISaveChangesAccessorAspect>>();
            aspects.ForEach(async aspect =>
            {
                if (aspect.Enabled)
                    await aspect.PreProcessAsync(this, cancellationToken).ConfigureAwait(); // 异步前置处理保存变化
            });

            // 异步保存更改
            var count = await this.SubmitSaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken)
                .ConfigureAwait();

            Dependency.Options.PostSaveChangesAction?.Invoke(this);

            aspects.ForEach(async aspect =>
            {
                if (aspect.Enabled)
                    await aspect.PostProcessAsync(this, cancellationToken).ConfigureAwait(); // 异步后置处理保存变化
            });

            return count;
        }

        #endregion


        #region IMultiTenancy

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
        /// 获取当前反向连接字符串（如果当前为默认连接字符串则返回写入连接字符串，反之则返回默认连接字符串）。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public virtual string GetCurrentReverseConnectionString()
        {
            if (IsCurrentConnectionString(CurrentTenant.DefaultConnectionString))
                return CurrentTenant.WritingConnectionString;

            return CurrentTenant.DefaultConnectionString;
        }

        /// <summary>
        /// 获取当前数据连接字符串描述。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public virtual string GetCurrentConnectionDescription()
        {
            if (CurrentTenant.WritingSeparation && IsCurrentConnectionString(CurrentTenant.WritingConnectionString))
                return nameof(ITenant.WritingConnectionString);

            return nameof(ITenant.DefaultConnectionString);
        }


        /// <summary>
        /// 是当前数据连接字符串。
        /// </summary>
        /// <param name="connectionString">给定的连接字符串。</param>
        /// <returns>返回布尔值。</returns>
        public virtual bool IsCurrentConnectionString(string connectionString)
            => CurrentConnectionString.Equals(connectionString, StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// 是默认连接字符串（未启用读写分离也将被视为默认连接字符串）。
        /// </summary>
        /// <returns>返回布尔值。</returns>
        public virtual bool IsDefaultConnectionString()
            => !CurrentTenant.WritingSeparation
            || (CurrentTenant.WritingSeparation && IsCurrentConnectionString(CurrentTenant.DefaultConnectionString));

        /// <summary>
        /// 是写入连接字符串（未启用读写分离也将被视为写入连接字符串）。
        /// </summary>
        /// <returns>返回布尔值。</returns>
        public virtual bool IsWritingConnectionString()
            => !CurrentTenant.WritingSeparation
            || (CurrentTenant.WritingSeparation && IsCurrentConnectionString(CurrentTenant.WritingConnectionString));


        /// <summary>
        /// 更改数据库连接字符串。
        /// </summary>
        /// <param name="changeFunc">给定更改租户连接字符串的工厂方法。</param>
        /// <returns>返回是否已更改的布尔值。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public virtual bool ChangeConnectionString(Func<ITenant, string> changeFunc)
        {
            changeFunc.NotNull(nameof(changeFunc));

            if (Dependency.Options.Stores.UseDataTenant)
                TrySwitchTenant();

            var connectionString = changeFunc.Invoke(CurrentTenant);
            var isChanged = EnsureDatabaseChanged(connectionString);

            // 实时迁移
            if (!IsFromMigrateInvoke)
                Migrate();

            return isChanged;
        }

        /// <summary>
        /// 异步更改数据库连接字符串。
        /// </summary>
        /// <param name="changeFunc">给定更改租户连接字符串的工厂方法。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含是否已更改布尔值的异步操作。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public virtual async Task<bool> ChangeConnectionStringAsync
            (Func<ITenant, string> changeFunc, CancellationToken cancellationToken = default)
        {
            changeFunc.NotNull(nameof(changeFunc));

            if (Dependency.Options.Stores.UseDataTenant)
                await TrySwitchTenantAsync(cancellationToken).ConfigureAwait();

            return await cancellationToken.RunOrCancelAsync(async () =>
            {
                var connectionString = changeFunc.Invoke(CurrentTenant);
                var isChanged = await EnsureDatabaseChangedAsync(connectionString,
                    cancellationToken).ConfigureAwait();

                // 实时迁移
                if (!IsFromMigrateInvoke)
                    await MigrateAsync(cancellationToken).ConfigureAwait();

                return isChanged;
            })
            .ConfigureAwait();
        }


        /// <summary>
        /// 尝试切换租户。
        /// </summary>
        /// <returns>返回是否已切换的布尔值。</returns>
        public virtual bool TrySwitchTenant()
        {
            var tenantService = this.GetService<IMultiTenancyAccessorService>();

            var tenant = tenantService.GetCurrentTenant(this);

            return EnsureTenantSwitched(tenant);
        }

        /// <summary>
        /// 异步尝试切换租户。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含是否已切换的布尔值的异步操作。</returns>
        public virtual async Task<bool> TrySwitchTenantAsync(CancellationToken cancellationToken = default)
        {
            var tenantService = this.GetService<IMultiTenancyAccessorService>();

            var tenant = await tenantService.GetCurrentTenantAsync(this, cancellationToken)
                .ConfigureAwait();

            return EnsureTenantSwitched(tenant);
        }

        /// <summary>
        /// 确保已切换租户。
        /// </summary>
        /// <param name="tenant">给定的 <see cref="ITenant"/>。</param>
        /// <returns>返回是否已切换的布尔值。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        protected bool EnsureTenantSwitched(ITenant tenant)
        {
            tenant.NotNull(nameof(tenant));

            if (CurrentTenant.Equals(tenant))
                return false;

            CurrentTenant = tenant;
            Logger.LogTrace(InternalResource.SwitchNewTenantFormat.Format(tenant.ToString()));

            Dependency.Options.PostTenantSwitchedAction?.Invoke(this);

            return true;
        }


        /// <summary>
        /// 写入分离操作。
        /// </summary>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="writingFunc">给定的写入操作工厂方法。</param>
        /// <param name="dataSynchronizationFunc">给定的数据同步操作工厂方法（可选）。</param>
        /// <returns>返回 <typeparamref name="TResult"/>。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        protected virtual TResult WritingSeparationOperation<TResult>(Func<TResult> writingFunc,
            Func<TResult> dataSynchronizationFunc = null)
        {
            writingFunc.NotNull(nameof(writingFunc));

            // 暂存当前租户与连接字符串
            var lastTenant = CurrentTenant;
            var lastConnectionString = CurrentConnectionString;

            // 保存变化到写入连接字符串
            ChangeConnectionString(tenant => tenant.WritingConnectionString);

            var result = writingFunc.Invoke();

            if (CurrentTenant.DataSynchronization)
            {
                dataSynchronizationFunc.NotNull(nameof(dataSynchronizationFunc));

                // 数据同步到默认连接字符串
                ChangeConnectionString(tenant => tenant.DefaultConnectionString);

                dataSynchronizationFunc.Invoke();
            }

            // 还原租户与连接字符串
            if (!CurrentTenant.Equals(lastTenant))
                EnsureTenantSwitched(lastTenant);

            if (!IsCurrentConnectionString(lastConnectionString))
                EnsureDatabaseChanged(lastConnectionString);

            return result;
        }

        /// <summary>
        /// 异步写入分离操作。
        /// </summary>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="writingFunc">给定的异步写入操作工厂方法。</param>
        /// <param name="dataSynchronizationFunc">给定的异步数据同步操作工厂方法（可选）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TResult"/> 的异步操作。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        protected virtual async Task<TResult> WritingSeparationOperationAsync<TResult>
            (Func<Task<TResult>> writingFunc, Func<Task<TResult>> dataSynchronizationFunc = null,
            CancellationToken cancellationToken = default)
        {
            writingFunc.NotNull(nameof(writingFunc));

            // 暂存当前租户与连接字符串
            var lastTenant = CurrentTenant;
            var lastConnectionString = CurrentConnectionString;

            // 保存变化到写入连接字符串
            await ChangeConnectionStringAsync(tenant => tenant.WritingConnectionString,
                cancellationToken).ConfigureAwait();

            var value = await writingFunc.Invoke().ConfigureAwait();

            if (CurrentTenant.DataSynchronization)
            {
                dataSynchronizationFunc.NotNull(nameof(dataSynchronizationFunc));

                // 数据同步到默认连接字符串
                await ChangeConnectionStringAsync(tenant => tenant.DefaultConnectionString,
                    cancellationToken).ConfigureAwait();

                await dataSynchronizationFunc.Invoke().ConfigureAwait();
            }

            // 还原租户与连接字符串
            if (!CurrentTenant.Equals(lastTenant))
                EnsureTenantSwitched(lastTenant);

            if (!IsCurrentConnectionString(lastConnectionString))
                EnsureDatabaseChanged(lastConnectionString);

            return value;
        }

        #endregion


        #region IService

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

        #endregion

    }
}
