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
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// <see cref="DbContext"/> 静态扩展。
    /// </summary>
    public static class EFCoreDbContextExtensions
    {
        /// <summary>
        /// 并发更新。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="dbContext">给定的 <see cref="DbContext"/>。</param>
        /// <param name="exception">给定的 <see cref="DbUpdateConcurrencyException"/>。</param>
        /// <param name="predicate">给定获取单条数据的断定方法。</param>
        /// <param name="updatePropertyName">给定要更新的属性名（可选；默认为空表示更新所有属性值）。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static void ConcurrentUpdates<TEntity>(this DbContext dbContext,
            DbUpdateConcurrencyException exception, Func<TEntity, bool> predicate, string updatePropertyName = null)
            where TEntity : class
        {
            dbContext.NotNull(nameof(dbContext));
            exception.NotNull(nameof(exception));

            foreach (var entry in exception.Entries)
            {
                if (entry.Entity is TEntity)
                {
                    foreach (var property in entry.Metadata.GetProperties())
                    {
                        var dbEntity = dbContext.Set<TEntity>().AsNoTracking().Single(predicate);
                        var dbEntityEntry = dbContext.Entry(dbEntity);

                        //var proposedValue = entry.Property(property.Name).CurrentValue;
                        //var originalValue = entry.Property(property.Name).OriginalValue;
                        //var databaseValue = dbEntityEntry.Property(property.Name).CurrentValue;

                        // 如果要更新的属性名不为空且等于当前属性，或要更新的属性名为空，则并发更新此属性
                        if ((updatePropertyName.IsNotEmpty() && property.Name == updatePropertyName)
                            || updatePropertyName.IsEmpty())
                        {
                            // Update original values to
                            entry.Property(property.Name).OriginalValue = dbEntityEntry.Property(property.Name).CurrentValue;

                            if (updatePropertyName.IsNotEmpty())
                                break; // 如果只更新当前属性，则跳出属性遍历
                        }
                    }
                }
                else
                {
                    throw new NotSupportedException("Don't know how to handle concurrency conflicts for " + entry.Metadata.Name);
                }
            }

            dbContext.SaveChanges();
        }


        #region SaveChanges

        /// <summary>
        /// 提交保存更改同步。
        /// </summary>
        /// <param name="dbContextAccessor">给定的 <see cref="DbContextAccessorBase"/>。</param>
        /// <returns>返回受影响的行数。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static int SubmitSaveChangesSynchronization(this DbContextAccessorBase dbContextAccessor)
        {
            var count = SubmitSaveChangesSynchronization((DbContext)dbContextAccessor);

            dbContextAccessor.RequiredSaveChanges = false;

            return count;
        }

        /// <summary>
        /// 异步提交保存更改同步。
        /// </summary>
        /// <param name="dbContextAccessor">给定的 <see cref="DbContextAccessorBase"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含受影响的行数的异步操作。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static async Task<int> SubmitSaveChangesSynchronizationAsync(this DbContextAccessorBase dbContextAccessor,
            CancellationToken cancellationToken = default)
        {
            var count = await SubmitSaveChangesSynchronizationAsync((DbContext)dbContextAccessor,
                cancellationToken).ConfigureAwait();

            dbContextAccessor.RequiredSaveChanges = false;

            return count;
        }


        /// <summary>
        /// 提交保存更改同步。
        /// </summary>
        /// <param name="dbContext">给定的 <see cref="DbContext"/>。</param>
        /// <returns>返回受影响的行数。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static int SubmitSaveChangesSynchronization(this DbContext dbContext)
        {
            var dependencies = dbContext.GetService<RelationalDatabaseDependencies>();
            var batchExecutor = dbContext.GetService<IBatchExecutor>();

            return batchExecutor.Execute(null, dependencies.Connection);
        }

        /// <summary>
        /// 异步提交保存更改同步。
        /// </summary>
        /// <param name="dbContext">给定的 <see cref="DbContext"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含受影响的行数的异步操作。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static Task<int> SubmitSaveChangesSynchronizationAsync(this DbContext dbContext,
            CancellationToken cancellationToken = default)
        {
            var dependencies = dbContext.GetService<RelationalDatabaseDependencies>();
            var batchExecutor = dbContext.GetService<IBatchExecutor>();

            return batchExecutor.ExecuteAsync(null, dependencies.Connection, cancellationToken);
        }


        /// <summary>
        /// 提交保存更改。
        /// </summary>
        /// <param name="dbContextAccessor">给定的 <see cref="DbContextAccessorBase"/>。</param>
        /// <param name="acceptAllChangesOnSuccess">指示是否在更改已成功发送到数据库之后调用。</param>
        /// <returns>返回受影响的行数。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static int SubmitSaveChanges(this DbContextAccessorBase dbContextAccessor,
            bool acceptAllChangesOnSuccess = true)
        {
            var count = SubmitSaveChanges((DbContext)dbContextAccessor, acceptAllChangesOnSuccess);

            dbContextAccessor.RequiredSaveChanges = false;

            return count;
        }

        /// <summary>
        /// 异步提交保存更改。
        /// </summary>
        /// <param name="dbContextAccessor">给定的 <see cref="DbContextAccessorBase"/>。</param>
        /// <param name="acceptAllChangesOnSuccess">指示是否在更改已成功发送到数据库之后调用。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含受影响的行数的异步操作。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static async Task<int> SubmitSaveChangesAsync(this DbContextAccessorBase dbContextAccessor,
            bool acceptAllChangesOnSuccess = true, CancellationToken cancellationToken = default)
        {
            var count = await SubmitSaveChangesAsync((DbContext)dbContextAccessor,
                acceptAllChangesOnSuccess, cancellationToken).ConfigureAwait();

            dbContextAccessor.RequiredSaveChanges = false;

            return count;
        }


        /// <summary>
        /// 提交保存更改。
        /// </summary>
        /// <param name="dbContext">给定的 <see cref="DbContext"/>。</param>
        /// <param name="acceptAllChangesOnSuccess">指示是否在更改已成功发送到数据库之后调用。</param>
        /// <returns>返回受影响的行数。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        [SuppressMessage("Usage", "EF1001:Internal EF Core API usage.", Justification = "<挂起>")]
        public static int SubmitSaveChanges(this DbContext dbContext,
            bool acceptAllChangesOnSuccess = true)
        {
            dbContext.NotNull(nameof(dbContext));

            CheckDisposed(dbContext);

            var dependencies = dbContext.GetService<IDbContextDependencies>();

            dependencies.UpdateLogger.SaveChangesStarting(dbContext);

            TryDetectChanges(dbContext);

            try
            {
                var entitiesSaved = dependencies.StateManager.SaveChanges(acceptAllChangesOnSuccess);

                dependencies.UpdateLogger.SaveChangesCompleted(dbContext, entitiesSaved);

                return entitiesSaved;
            }
            catch (DbUpdateConcurrencyException exception)
            {
                dependencies.UpdateLogger.OptimisticConcurrencyException(dbContext, exception);

                throw;
            }
            catch (Exception exception)
            {
                dependencies.UpdateLogger.SaveChangesFailed(dbContext, exception);

                throw;
            }
        }

        /// <summary>
        /// 异步提交保存更改。
        /// </summary>
        /// <param name="dbContext">给定的 <see cref="DbContext"/>。</param>
        /// <param name="acceptAllChangesOnSuccess">指示是否在更改已成功发送到数据库之后调用。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含受影响的行数的异步操作。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        [SuppressMessage("Usage", "EF1001:Internal EF Core API usage.", Justification = "<挂起>")]
        public static async Task<int> SubmitSaveChangesAsync(this DbContext dbContext,
            bool acceptAllChangesOnSuccess = true, CancellationToken cancellationToken = default)
        {
            dbContext.NotNull(nameof(dbContext));

            CheckDisposed(dbContext);

            var dependencies = dbContext.GetService<IDbContextDependencies>();

            dependencies.UpdateLogger.SaveChangesStarting(dbContext);

            TryDetectChanges(dbContext);

            try
            {
                var entitiesSaved = await dependencies.StateManager
                    .SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken)
                    .ConfigureAwait();

                dependencies.UpdateLogger.SaveChangesCompleted(dbContext, entitiesSaved);

                return entitiesSaved;
            }
            catch (DbUpdateConcurrencyException exception)
            {
                dependencies.UpdateLogger.OptimisticConcurrencyException(dbContext, exception);

                throw;
            }
            catch (Exception exception)
            {
                dependencies.UpdateLogger.SaveChangesFailed(dbContext, exception);

                throw;
            }
        }

        private static void CheckDisposed(DbContext dbContext)
        {
            var disposed = (bool)typeof(DbContext)
                .GetField("_disposed", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(dbContext);

            if (disposed)
            {
                throw new ObjectDisposedException(dbContext.GetType().ShortDisplayName(),
                    CoreStrings.ContextDisposed);
            }
        }

        private static void TryDetectChanges(DbContext dbContext)
        {
            if (dbContext.ChangeTracker.AutoDetectChangesEnabled)
            {
                dbContext.ChangeTracker.DetectChanges();
            }
        }

        #endregion

    }
}
