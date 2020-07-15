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
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        protected DbContextAccessorBase(DbContextOptions options)
            : base(options)
        {
            InitializeAccessorBase(options);
        }


        private void InitializeAccessorBase(DbContextOptions options)
        {
            // Database.GetDbConnection().ConnectionString 的信息不一定完整（比如密码）
            CurrentConnectionString = Dependency.Options.OptionsExtensionConnectionStringFactory?.Invoke(options);
            CurrentConnectionString.NotEmpty("BuilderOptions.OptionsExtensionConnectionStringFactory");

            CurrentTenant = Dependency.Options.DefaultTenant.NotNull("Dependency.Options.DefaultTenant");
            
            if (Dependency.Options.SupportsCreateDatabase)
                EnsureDatabaseCreated();
        }


        /// <summary>
        /// 构建器依赖。
        /// </summary>
        public DataBuilderDependency Dependency
            => GetService<DataBuilderDependency>();


        #region EnsureDatabaseCreated and EnsureDatabaseChanged

        /// <summary>
        /// 确保已创建数据库。
        /// </summary>
        /// <returns>返回是否已创建的布尔值。</returns>
        protected virtual bool EnsureDatabaseCreated()
        {
            if (CreationValidator.IsCreated(this))
                return true;

            if (Database.EnsureCreated())
            {
                CreationValidator.SetCreated(this);

                Dependency.Options.PostDatabaseCreatedAction?.Invoke(this);

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

            if (IsCurrentConnectionString(connectionString))
            {
                //Logger.LogTrace(InternalResource.ChangeConnectionStringIsEmpty);
                //Logger.LogTrace(InternalResource.ChangeConnectionStringSameAsCurrent);

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

            if (IsCurrentConnectionString(connectionString))
            {
                //Logger.LogTrace(InternalResource.ChangeConnectionStringIsEmpty);
                //Logger.LogTrace(InternalResource.ChangeConnectionStringSameAsCurrent);

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


        #region IAccessor

        /// <summary>
        /// 时钟服务。
        /// </summary>
        /// <value>返回 <see cref="IClockService"/>。</value>
        public IClockService Clock
            => GetService<IClockService>();

        /// <summary>
        /// 数据库创建验证器。
        /// </summary>
        /// <value>返回 <see cref="IDatabaseCreationValidator"/>。</value>
        public IDatabaseCreationValidator CreationValidator
            => GetService<IDatabaseCreationValidator>();


        /// <summary>
        /// 当前时间戳。
        /// </summary>
        public DateTimeOffset CurrentTimestamp
            => Clock.GetNowOffsetAsync().ConfigureAwaitCompleted();

        /// <summary>
        /// 当前类型。
        /// </summary>
        public Type CurrentType
            => GetType();


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
            count = WritingSeparationOperation(() => MigrateCore());

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
                count = await MigrateCoreAsync(cancellationToken).ConfigureAwait();
                return count;
            }

            // 标识迁移调用，防止死循环
            IsFromMigrateInvoke = true;

            // 外部直接调用迁移，需引用写入分离操作
            count = await WritingSeparationOperationAsync(() =>
            {
                return MigrateCoreAsync(cancellationToken);
            },
            cancellationToken).ConfigureAwait();

            IsFromMigrateInvoke = false;

            return count;
        }


        /// <summary>
        /// 迁移核心（不区分读写）。
        /// </summary>
        /// <returns>返回受影响的行数。</returns>
        protected virtual int MigrateCore()
        {
            var migration = GetService<IMigrationAccessorService>();
            migration.Migrate(this);

            Dependency.Options.PostMigratedAction?.Invoke(this);

            if (Dependency.Options.UseInitializer)
            {
                var initializer = GetService<IStoreInitializer>();
                initializer.Initialize(this);
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
            var migration = GetService<IMigrationAccessorService>();
            await migration.MigrateAsync(this, cancellationToken).ConfigureAwait();

            Dependency.Options.PostMigratedAction?.Invoke(this);

            if (Dependency.Options.UseInitializer)
            {
                var initializer = GetService<IStoreInitializer>();
                await initializer.InitializeAsync(this, cancellationToken).ConfigureAwait();
            }

            var count = 0;

            if (RequiredSaveChanges)
                count = await SaveChangesCoreAsync(true, cancellationToken).ConfigureAwait();

            return count;
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

            var count = WritingSeparationOperation(() => SaveChangesCore(acceptAllChangesOnSuccess));

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
            var aspects = GetService<IServicesManager<ISaveChangesAccessorAspect>>();
            aspects.ForEach(aspect =>
            {
                if (aspect.Enabled)
                    aspect.PreProcess(this); // 前置处理保存变化
            });

            // 验证上次写入连接的实体状态集合
            //（解决在数据同步时，写入连接保存更改后会更新实体状态导致默认连接不能正常保存）
            VerifyLastWritingSaveChangesEntityStates();

            // 保存更改
            var count = this.SaveChangesNow(acceptAllChangesOnSuccess);

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
            var aspects = GetService<IServicesManager<ISaveChangesAccessorAspect>>();
            aspects.ForEach(async aspect =>
            {
                if (aspect.Enabled)
                    await aspect.PreProcessAsync(this, cancellationToken).ConfigureAwait(); // 异步前置处理保存变化
            });

            // 验证上次写入连接的实体状态集合
            //（解决在数据同步时，写入连接保存更改后会更新实体状态导致默认连接不能正常保存）
            VerifyLastWritingSaveChangesEntityStates();

            // 异步保存更改
            var count = await this.SaveChangesNowAsync(acceptAllChangesOnSuccess, cancellationToken)
                .ConfigureAwait();

            Dependency.Options.PostSaveChangesAction?.Invoke(this);

            aspects.ForEach(async aspect =>
            {
                if (aspect.Enabled)
                    await aspect.PostProcessAsync(this, cancellationToken).ConfigureAwait(); // 异步后置处理保存变化
            });

            return count;
        }


        private IList<EntityState> _lastWritingSaveChangesEntityStates = null;

        [SuppressMessage("Usage", "EF1001:Internal EF Core API usage.", Justification = "<挂起>")]
        private void VerifyLastWritingSaveChangesEntityStates()
        {
            var stateManager = GetService<IStateManager>();

            // 缓存写入连接字符串的实体状态集合
            if (IsWritingConnectionString())
            {
                if (stateManager.Count > 0 && stateManager.Entries.Any(entry
                    => entry.EntityState != EntityState.Unchanged))
                {
                    // 因同实体类型可能有不同状态，所以缓存所有列表以进行比较
                    _lastWritingSaveChangesEntityStates = stateManager.Entries
                        .Select(s => s.EntityState).ToList();
                }

                return;
            }

            if (_lastWritingSaveChangesEntityStates.IsNotEmpty())
            {
                if (stateManager.Count == _lastWritingSaveChangesEntityStates.Count)
                {
                    var index = 0;
                    foreach (var entry in stateManager.Entries)
                    {
                        entry.SetEntityState(_lastWritingSaveChangesEntityStates[index]);
                    }
                }

                // 仅使用一次，即使对比不匹配也直接清除
                _lastWritingSaveChangesEntityStates = null;
            }
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
            var tenantService = GetService<IMultiTenantAccessorService>();

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
            var tenantService = GetService<IMultiTenantAccessorService>();

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
        /// <param name="operationFunc">给定的操作工厂方法。</param>
        /// <returns>返回 <typeparamref name="TResult"/>。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        protected virtual TResult WritingSeparationOperation<TResult>(Func<TResult> operationFunc)
        {
            operationFunc.NotNull(nameof(operationFunc));

            // 暂存当前租户与连接字符串
            var lastTenant = CurrentTenant;
            var lastConnectionString = CurrentConnectionString;

            // 保存变化到写入连接字符串
            ChangeConnectionString(tenant => tenant.WritingConnectionString);

            var result = operationFunc.Invoke();

            if (CurrentTenant.DataSynchronization)
            {
                // 数据同步到默认连接字符串
                ChangeConnectionString(tenant => tenant.DefaultConnectionString);

                operationFunc.Invoke();
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
        /// <param name="operationFunc">给定的异步操作工厂方法。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TResult"/> 的异步操作。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        protected virtual async Task<TResult> WritingSeparationOperationAsync<TResult>
            (Func<Task<TResult>> operationFunc, CancellationToken cancellationToken = default)
        {
            operationFunc.NotNull(nameof(operationFunc));

            // 暂存当前租户与连接字符串
            var lastTenant = CurrentTenant;
            var lastConnectionString = CurrentConnectionString;

            // 保存变化到写入连接字符串
            await ChangeConnectionStringAsync(tenant => tenant.WritingConnectionString,
                cancellationToken).ConfigureAwait();

            var value = await operationFunc.Invoke().ConfigureAwait();

            if (CurrentTenant.DataSynchronization)
            {
                // 数据同步到默认连接字符串
                await ChangeConnectionStringAsync(tenant => tenant.DefaultConnectionString,
                    cancellationToken).ConfigureAwait();

                await operationFunc.Invoke().ConfigureAwait();
            }

            // 还原租户与连接字符串
            if (!CurrentTenant.Equals(lastTenant))
                EnsureTenantSwitched(lastTenant);

            if (!IsCurrentConnectionString(lastConnectionString))
                EnsureDatabaseChanged(lastConnectionString);

            return value;
        }

        #endregion


        #region IInfrastructureService

        /// <summary>
        /// 日志工厂。
        /// </summary>
        public ILoggerFactory LoggerFactory
            => GetService<ILoggerFactory>();

        /// <summary>
        /// 日志。
        /// </summary>
        protected ILogger Logger
            => LoggerFactory.CreateLogger(GetType());


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

        #endregion

    }
}
