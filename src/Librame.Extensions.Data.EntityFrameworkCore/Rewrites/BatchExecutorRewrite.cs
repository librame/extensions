#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions;
using Librame.Extensions.Data.Accessors;
using Librame.Extensions.Data.Stores;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Microsoft.EntityFrameworkCore.Update.Internal
{
    /// <summary>
    /// <see cref="BatchExecutor"/> 改写。
    /// </summary>
    public class BatchExecutorRewrite : IBatchExecutor
    {
        /// <summary>
        /// 构造一个 <see cref="BatchExecutorRewrite"/>。
        /// </summary>
        /// <param name="currentContext">给定的 <see cref="ICurrentDbContext"/>。</param>
        /// <param name="executionStrategyFactory">给定的 <see cref="IExecutionStrategyFactory"/>。</param>
        public BatchExecutorRewrite(ICurrentDbContext currentContext,
            IExecutionStrategyFactory executionStrategyFactory)
        {
            CurrentContext = currentContext.NotNull(nameof(currentContext));

#pragma warning disable 618
            ExecutionStrategyFactory = executionStrategyFactory.NotNull(nameof(executionStrategyFactory));
#pragma warning restore 618
        }


        /// <summary>
        /// 当前数据库上下文。
        /// </summary>
        public virtual ICurrentDbContext CurrentContext { get; }

        /// <summary>
        /// 执行策略工厂。
        /// </summary>
        [Obsolete("This isn't used anymore")]
        protected virtual IExecutionStrategyFactory ExecutionStrategyFactory { get; }

        /// <summary>
        /// <see cref="DbContextAccessorBase"/> 或 NULL。
        /// </summary>
        private DbContextAccessorBase DbContextAccessor
            => CurrentContext.Context is DbContextAccessorBase accessor ? accessor : null;


        /// <summary>
        /// 执行修改命令批处理集合。
        /// </summary>
        /// <param name="commandBatches">给定的 <see cref="IEnumerable{ModificationCommandBatch}"/>。</param>
        /// <param name="connection">给定的 <see cref="IRelationalConnection"/>。</param>
        /// <returns>返回受影响的行数。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public virtual int Execute(IEnumerable<ModificationCommandBatch> commandBatches,
            IRelationalConnection connection)
        {
            commandBatches = VerifyLastWritingSaveChangesCommandBatches(out var resetUnchangedState, commandBatches);
            if (commandBatches.IsNull())
                return 0; // 当直接通过 DbContext.SubmitSaveChangesSynchronization() 调用时可能会存在无变化的情况

            var rowsAffected = 0;
            IDbContextTransaction startedTransaction = null;
            try
            {
                if (connection.CurrentTransaction == null
                    && (connection as ITransactionEnlistmentManager)?.EnlistedTransaction == null
                    && Transaction.Current == null
                    && CurrentContext.Context.Database.AutoTransactionsEnabled)
                {
                    startedTransaction = connection.BeginTransaction();
                }
                else
                {
                    connection.Open();
                }

                foreach (var batch in commandBatches)
                {
                    batch.Execute(connection);
                    rowsAffected += batch.ModificationCommands.Count;
                }

                startedTransaction?.Commit();
            }
            finally
            {
                if (startedTransaction != null)
                {
                    startedTransaction.Dispose();
                }
                else
                {
                    connection.Close();
                }
            }

            if (resetUnchangedState)
            {
                // 重置为无变化的实体状态
                ResetEntityStates(commandBatches, i => EntityState.Unchanged);
            }

            if (DbContextAccessor.IsNotNull())
            {
                if (DbContextAccessor.Dependency.Options.UseInitializer)
                {
                    var initializer = DbContextAccessor.GetService<IStoreInitializer>();
                    initializer.Validator.SetInitialized(DbContextAccessor);
                }

                DbContextAccessor.RequiredSaveChanges = false;
            }

            return rowsAffected;
        }


        /// <summary>
        /// 异步执行修改命令批处理集合。
        /// </summary>
        /// <param name="commandBatches">给定的 <see cref="IEnumerable{ModificationCommandBatch}"/>。</param>
        /// <param name="connection">给定的 <see cref="IRelationalConnection"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含受影响的行数的异步操作。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public virtual async Task<int> ExecuteAsync(IEnumerable<ModificationCommandBatch> commandBatches,
            IRelationalConnection connection, CancellationToken cancellationToken = default)
        {
            commandBatches = VerifyLastWritingSaveChangesCommandBatches(out var resetUnchangedState, commandBatches);
            if (commandBatches.IsNull())
                return 0; // 当直接通过 DbContext.SubmitSaveChangesSynchronizationAsync() 调用时可能会存在无变化的情况

            var rowsAffected = 0;
            IDbContextTransaction startedTransaction = null;
            try
            {
                if (connection.CurrentTransaction == null
                    && (connection as ITransactionEnlistmentManager)?.EnlistedTransaction == null
                    && Transaction.Current == null
                    && CurrentContext.Context.Database.AutoTransactionsEnabled)
                {
                    startedTransaction = await connection.BeginTransactionAsync(cancellationToken).ConfigureAwait();
                }
                else
                {
                    await connection.OpenAsync(cancellationToken).ConfigureAwait();
                }

                foreach (var batch in commandBatches)
                {
                    await batch.ExecuteAsync(connection, cancellationToken).ConfigureAwait();
                    rowsAffected += batch.ModificationCommands.Count;
                }

                startedTransaction?.Commit();
            }
            finally
            {
                if (startedTransaction != null)
                {
                    await startedTransaction.DisposeAsync().ConfigureAwait();
                }
                else
                {
                    await connection.CloseAsync().ConfigureAwait();
                }
            }

            if (resetUnchangedState)
            {
                // 重置为无变化的实体状态
                ResetEntityStates(commandBatches, i => EntityState.Unchanged);
            }

            if (DbContextAccessor.IsNotNull())
            {
                if (DbContextAccessor.Dependency.Options.UseInitializer)
                {
                    var initializer = DbContextAccessor.GetService<IStoreInitializer>();
                    await initializer.Validator.SetInitializedAsync(DbContextAccessor,
                        cancellationToken).ConfigureAwait();
                }

                DbContextAccessor.RequiredSaveChanges = false;
            }

            return rowsAffected;
        }


        private List<ModificationCommandBatch> _lastWritingBatches;
        private List<EntityState> _lastWritingStates;

        [SuppressMessage("Usage", "EF1001:Internal EF Core API usage.", Justification = "<挂起>")]
        private IEnumerable<ModificationCommandBatch> VerifyLastWritingSaveChangesCommandBatches
            (out bool resetUnchangedState, IEnumerable<ModificationCommandBatch> commandBatches = null)
        {
            if (DbContextAccessor?.IsWritingConnectionString() == true)
            {
                commandBatches.NotNull(nameof(commandBatches));

                // 缓存实体写入状态
                if (_lastWritingStates.IsNull())
                    _lastWritingStates = new List<EntityState>();

                foreach (var batch in commandBatches)
                {
                    foreach (var command in batch.ModificationCommands)
                    {
                        var entries = command.Entries
                            .Select(entry => entry.EntityState);

                        _lastWritingStates.AddRange(entries);
                    }
                }

                _lastWritingBatches = commandBatches.ToList();
                resetUnchangedState = false;

                return commandBatches;
            }

            if (_lastWritingBatches.IsNotNull())
            {
                // 重置为上次写入的实体状态
                ResetEntityStates(_lastWritingBatches, i => _lastWritingStates[i]);

                commandBatches = _lastWritingBatches;

                // 清空缓存，确保仅用一次
                _lastWritingBatches = null;
                _lastWritingStates = null;
            }

            resetUnchangedState = true;
            return commandBatches;
        }

        [SuppressMessage("Usage", "EF1001:Internal EF Core API usage.", Justification = "<挂起>")]
        private static void ResetEntityStates(IEnumerable<ModificationCommandBatch> batches,
            Func<int, EntityState> stateFunc, bool resetPrimaryKey = false)
        {
            var entries = new List<IUpdateEntry>();

            foreach (var batch in batches)
            {
                foreach (var command in batch.ModificationCommands)
                {
                    entries.AddRange(command.Entries);
                }
            }

            for (var i = 0; i < entries.Count; i++)
            {
                var entry = entries[i] as InternalEntityEntry;

                // 设定原始实体状态
                entry.SetEntityState(stateFunc.Invoke(i));

                if (!resetPrimaryKey)
                    continue;

                //// 如果是自增长主键且已设置标识，则需要重置
                //(var isGenerated, var isSet) = entry.IsKeySet;
                //if (entry.EntityState == EntityState.Added && isGenerated && isSet)
                //{
                //    var keyProperties = entry.EntityType.GetProperties()
                //        .Where(p => p.IsPrimaryKey());

                //    foreach (var key in keyProperties)
                //    {
                //        // 重置自生成主键默认值
                //        entry.Entity.SetProperty(key.Name, key.GetDefaultValue());
                //    }
                //}
            }
        }

    }

}
