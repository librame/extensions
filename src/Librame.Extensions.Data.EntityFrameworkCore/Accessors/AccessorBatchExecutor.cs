// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Librame.Extensions;
using Librame.Extensions.Data.Accessors;
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
    ///     <para>
    ///         This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    ///         the same compatibility standards as public APIs. It may be changed or removed without notice in
    ///         any release. You should only use it directly in your code with extreme caution and knowing that
    ///         doing so can result in application failures when updating to a new Entity Framework Core release.
    ///     </para>
    ///     <para>
    ///         The service lifetime is <see cref="ServiceLifetime.Scoped" />. This means that each
    ///         <see cref="DbContext" /> instance will use its own instance of this service.
    ///         The implementation may depend on other services registered with any lifetime.
    ///         The implementation does not need to be thread-safe.
    ///     </para>
    /// </summary>
    public class AccessorBatchExecutor : IBatchExecutor
    {
        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        public AccessorBatchExecutor(ICurrentDbContext currentContext,
            IExecutionStrategyFactory executionStrategyFactory)
        {
            CurrentContext = currentContext.NotNull(nameof(currentContext));

#pragma warning disable 618
            ExecutionStrategyFactory = executionStrategyFactory.NotNull(nameof(executionStrategyFactory));
#pragma warning restore 618
        }


        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        public virtual ICurrentDbContext CurrentContext { get; }

        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        [Obsolete("This isn't used anymore")]
        protected virtual IExecutionStrategyFactory ExecutionStrategyFactory { get; }


        private DbContextAccessorBase DbContextAccessor
            => CurrentContext.Context is DbContextAccessorBase accessor ? accessor : null;


        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public virtual int Execute(
            IEnumerable<ModificationCommandBatch> commandBatches,
            IRelationalConnection connection)
        {
            commandBatches = VerifyLastWritingSaveChangesCommandBatches(commandBatches);
            if (commandBatches.IsNull())
                return 0; // 当调用 DbContext.SubmitSaveChangesSynchronization() 可能会存在无变化的情况

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

                if (DbContextAccessor?.RequiredIdentityInsertTableNames.IsNotNull() == true)
                    DbContextAccessor.RequiredIdentityInsertTableNames = null;
            }

            return rowsAffected;
        }


        /// <summary>
        ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
        ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
        ///     any release. You should only use it directly in your code with extreme caution and knowing that
        ///     doing so can result in application failures when updating to a new Entity Framework Core release.
        /// </summary>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public virtual async Task<int> ExecuteAsync(
            IEnumerable<ModificationCommandBatch> commandBatches,
            IRelationalConnection connection,
            CancellationToken cancellationToken = default)
        {
            commandBatches = VerifyLastWritingSaveChangesCommandBatches(commandBatches);
            if (commandBatches.IsNull())
                return 0; // 当调用 DbContext.SubmitSaveChangesSynchronizationAsync() 可能会存在无变化的情况

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

                if (DbContextAccessor?.RequiredIdentityInsertTableNames.IsNotNull() == true)
                    DbContextAccessor.RequiredIdentityInsertTableNames = null;
            }

            return rowsAffected;
        }


        private List<ModificationCommandBatch> _lastWritingBatches = null;
        private List<KeyValuePair<ModificationCommand, List<KeyValuePair<IUpdateEntry, EntityState>>>> _lastWritingCommands = null;

        [SuppressMessage("Usage", "EF1001:Internal EF Core API usage.", Justification = "<挂起>")]
        private IEnumerable<ModificationCommandBatch> VerifyLastWritingSaveChangesCommandBatches
            (IEnumerable<ModificationCommandBatch> commandBatches = null)
        {
            if (DbContextAccessor?.IsWritingConnectionString() == true && commandBatches.IsNotNull())
            {
                if (_lastWritingBatches.IsNull())
                    _lastWritingBatches = commandBatches.ToList();

                if (_lastWritingCommands.IsNull())
                    _lastWritingCommands = new List<KeyValuePair<ModificationCommand, List<KeyValuePair<IUpdateEntry, EntityState>>>>();

                foreach (var batch in _lastWritingBatches)
                {
                    foreach (var command in batch.ModificationCommands)
                    {
                        var value = command.Entries
                            .Select(s => new KeyValuePair<IUpdateEntry, EntityState>(s, s.EntityState))
                            .ToList();

                        var pair = new KeyValuePair<ModificationCommand,
                            List<KeyValuePair<IUpdateEntry, EntityState>>>(command, value);

                        _lastWritingCommands.Add(pair);
                    }
                }

                return commandBatches;
            }

            if (_lastWritingBatches.IsNotNull() && _lastWritingCommands.IsNotNull())
            {
                foreach (var batch in _lastWritingBatches)
                {
                    for (var i = 0; i < batch.ModificationCommands.Count; i++)
                    {
                        var indefinite = batch.ModificationCommands[i];
                        var original = _lastWritingCommands[i];

                        for (var j = 0; j < original.Value.Count; j++)
                        {
                            var indefiniteEntry = indefinite.Entries[j];
                            var originalEntry = original.Value[j];

                            if (indefiniteEntry is InternalEntityEntry entry)
                            {
                                // 重置原始实体状态
                                entry.SetEntityState(originalEntry.Value);

                                (var isGenerated, var isSet) = entry.IsKeySet;

                                if (entry.EntityState == EntityState.Added && isGenerated && isSet)
                                {
                                    var keyProperties = entry.EntityType.GetProperties()
                                        .Where(p => p.IsPrimaryKey());

                                    foreach (var key in keyProperties)
                                    {
                                        // 重置自生成主键默认值
                                        entry.Entity.SetProperty(key.Name, key.GetDefaultValue());
                                    }
                                }
                            }
                        }
                    }
                }

                commandBatches = _lastWritingBatches;

                // 清空缓存，确保仅用一次
                _lastWritingBatches = null;
                _lastWritingCommands = null;
            }
            
            return commandBatches;
        }

    }

}
