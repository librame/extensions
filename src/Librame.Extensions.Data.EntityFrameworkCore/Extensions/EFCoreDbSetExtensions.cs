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
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// <see cref="DbSet{TEntity}"/> 静态扩展。
    /// </summary>
    public static class EFCoreDbSetExtensions
    {
        /// <summary>
        /// 存在指定的实体工厂方法表达式。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="dbSet">给定的 <see cref="DbSet{TEntity}"/>。</param>
        /// <param name="lookupLocal">同时查找本地缓存（可选；默认查找）。</param>
        /// <returns>返回布尔值。</returns>
        public static bool Exists<TEntity>(this DbSet<TEntity> dbSet, bool lookupLocal = true)
            where TEntity : class
        {
            if (lookupLocal && dbSet.Local.Any())
                return true;

            return dbSet.Any();
        }

        /// <summary>
        /// 存在指定的实体工厂方法表达式。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="dbSet">给定的 <see cref="DbSet{TEntity}"/>。</param>
        /// <param name="predicate">给定断定实体存在的工厂方法表达式。</param>
        /// <param name="lookupLocal">同时查找本地缓存（可选；默认查找）。</param>
        /// <returns>返回布尔值。</returns>
        public static bool Exists<TEntity>(this DbSet<TEntity> dbSet, Expression<Func<TEntity, bool>> predicate,
            bool lookupLocal = true)
            where TEntity : class
        {
            predicate.NotNull(nameof(predicate));

            if (lookupLocal && dbSet.Local.Any(predicate.Compile()))
                return true;

            return dbSet.Any(predicate);
        }


        /// <summary>
        /// 异步存在指定的实体工厂方法表达式。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="dbSet">给定的 <see cref="DbSet{TEntity}"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="lookupLocal">同时查找本地缓存（可选；默认查找）。</param>
        /// <returns>返回一个包含布尔值的异步操作。</returns>
        public static Task<bool> ExistsAsync<TEntity>(this DbSet<TEntity> dbSet,
            CancellationToken cancellationToken = default, bool lookupLocal = true)
            where TEntity : class
        {
            if (lookupLocal && dbSet.Local.Any())
                return Task.FromResult(true);

            return dbSet.AnyAsync(cancellationToken);
        }

        /// <summary>
        /// 异步存在指定的实体工厂方法表达式。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="dbSet">给定的 <see cref="DbSet{TEntity}"/>。</param>
        /// <param name="predicate">给定断定实体存在的工厂方法表达式。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="lookupLocal">同时查找本地缓存（可选；默认查找）。</param>
        /// <returns>返回一个包含布尔值的异步操作。</returns>
        public static Task<bool> ExistsAsync<TEntity>(this DbSet<TEntity> dbSet, Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken = default, bool lookupLocal = true)
            where TEntity : class
        {
            predicate.NotNull(nameof(predicate));

            if (lookupLocal && dbSet.Local.Any(predicate.Compile()))
                return Task.FromResult(true);

            return dbSet.AnyAsync(predicate, cancellationToken);
        }


        /// <summary>
        /// 通过指定的结果选择器表达式查找符合最大结果的第一或默认实体。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="dbSet">给定的 <see cref="DbSet{TEntity}"/>。</param>
        /// <param name="resultSelector">给定的结果选择器表达式。</param>
        /// <param name="tableName">给定的 <see cref="TableNameSchema"/>。</param>
        /// <returns>返回实体。</returns>
        public static TEntity FirstOrDefaultByMax<TEntity, TResult>(this DbSet<TEntity> dbSet, Expression<Func<TEntity, TResult>> resultSelector, TableNameSchema tableName)
            where TEntity : class
            where TResult : IEquatable<TResult>
        {
            dbSet.NotNull(nameof(dbSet));
            resultSelector.NotNull(nameof(resultSelector));

            var resultFactory = resultSelector.Compile();

            try
            {
                // 涉及到不同数据库时可能会抛出 The LINQ expression 'Max<,>()' could not be translated.could not be translated. 的异常
                return dbSet.FirstOrDefault(p => resultFactory.Invoke(p).Equals(dbSet.Max(resultSelector)));
            }
            catch (InvalidOperationException)
            {
                tableName.NotNull(nameof(tableName));

                try
                {
                    //return dbSet.FromSqlRaw($"SELECT TOP 1 * FROM {tableName}").OrderByDescending(resultSelector).FirstOrDefault();
                    return dbSet.FromSqlRaw($"SELECT TOP 1 * FROM {tableName} ORDER BY {resultSelector.AsPropertyName()} DESC").FirstOrDefault();
                }
                catch (Exception)
                {
                    return dbSet.FromSqlRaw($"SELECT * FROM {tableName} WHERE {resultSelector.AsPropertyName()}=(SELECT MAX({resultSelector.AsPropertyName()}) FROM {tableName})").FirstOrDefault();
                }
            }
        }

        /// <summary>
        /// 通过指定的结果选择器表达式查找符合最小结果的第一或默认实体。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="dbSet">给定的 <see cref="DbSet{TEntity}"/>。</param>
        /// <param name="resultSelector">给定的结果选择器表达式。</param>
        /// <param name="tableName">给定的 <see cref="TableNameSchema"/>。</param>
        /// <returns>返回实体。</returns>
        public static TEntity FirstOrDefaultByMin<TEntity, TResult>(this DbSet<TEntity> dbSet, Expression<Func<TEntity, TResult>> resultSelector, TableNameSchema tableName)
            where TEntity : class
            where TResult : IEquatable<TResult>
        {
            dbSet.NotNull(nameof(dbSet));
            resultSelector.NotNull(nameof(resultSelector));

            var resultFactory = resultSelector.Compile();

            try
            {
                // 涉及到不同数据库时可能会抛出 The LINQ expression 'Min<,>()' could not be translated.could not be translated. 的异常
                return dbSet.FirstOrDefault(p => resultFactory.Invoke(p).Equals(dbSet.Min(resultSelector)));
            }
            catch (InvalidOperationException)
            {
                tableName.NotNull(nameof(tableName));

                try
                {
                    //return dbSet.FromSqlRaw($"SELECT TOP 1 * FROM {tableName}").OrderBy(resultSelector).FirstOrDefault();
                    return dbSet.FromSqlRaw($"SELECT TOP 1 * FROM {tableName} ORDER BY {resultSelector.AsPropertyName()}").FirstOrDefault();
                }
                catch (Exception)
                {
                    return dbSet.FromSqlRaw($"SELECT * FROM {tableName} WHERE {resultSelector.AsPropertyName()}=(SELECT MIN({resultSelector.AsPropertyName()}) FROM {tableName})").FirstOrDefault();
                }
            }
        }


        /// <summary>
        /// 异步尝试创建集合。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="dbSet">给定的 <see cref="DbSet{TEntity}"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="entities">给定要增加的实体集合。</param>
        /// <returns>返回一个包含 <see cref="OperationResult"/> 的异步操作。</returns>
        public static Task<OperationResult> TryCreateAsync<TEntity>(this DbSet<TEntity> dbSet,
            CancellationToken cancellationToken, params TEntity[] entities)
            where TEntity : class
            => OperationResult.TryRunFactoryAsync(() => dbSet.AddRangeAsync(entities, cancellationToken));

        /// <summary>
        /// 尝试创建集合。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="dbSet">给定的 <see cref="DbSet{TEntity}"/>。</param>
        /// <param name="entities">给定要增加的实体集合。</param>
        /// <returns>返回一个包含 <see cref="OperationResult"/> 的异步操作。</returns>
        public static OperationResult TryCreate<TEntity>(this DbSet<TEntity> dbSet, params TEntity[] entities)
            where TEntity : class
            => OperationResult.TryRunAction(() => dbSet.AddRange(entities));


        /// <summary>
        /// 尝试更新集合。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="dbSet">给定的 <see cref="DbSet{TEntity}"/>。</param>
        /// <param name="entities">给定要更新的实体集合。</param>
        /// <returns>返回 <see cref="OperationResult"/>。</returns>
        public static OperationResult TryUpdate<TEntity>(this DbSet<TEntity> dbSet, params TEntity[] entities)
            where TEntity : class
            => OperationResult.TryRunAction(() => dbSet.UpdateRange(entities));


        /// <summary>
        /// 尝试删除集合。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="dbSet">给定的 <see cref="DbSet{TEntity}"/>。</param>
        /// <param name="entities">给定要删除的实体集合。</param>
        /// <returns>返回 <see cref="OperationResult"/>。</returns>
        public static OperationResult TryDelete<TEntity>(this DbSet<TEntity> dbSet, params TEntity[] entities)
            where TEntity : class
            => OperationResult.TryRunAction(() => dbSet.RemoveRange(entities));

        /// <summary>
        /// 尝试逻辑删除集合。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="dbSet">给定的 <see cref="DbSet{TEntity}"/>。</param>
        /// <param name="entities">给定要删除的实体集合。</param>
        /// <returns>返回 <see cref="OperationResult"/>。</returns>
        public static OperationResult TryLogicDelete<TEntity>(this DbSet<TEntity> dbSet, params TEntity[] entities)
            where TEntity : class, IStatus<DataStatus>
        {
            entities.ForEach(entity => entity.Status = DataStatus.Delete);
            return dbSet.TryUpdate(entities);
        }

    }
}
