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
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data
{
    using Core.Identifiers;
    using Data.Stores;

    /// <summary>
    /// 数据库集管理器。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    public class DbSetManager<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// 构造一个 <see cref="DbSetManager{TEntity}"/>。
        /// </summary>
        /// <param name="dbSet">给定的 <see cref="DbSet{TEntity}"/>。</param>
        public DbSetManager(DbSet<TEntity> dbSet)
        {
            DbSet = dbSet.NotNull(nameof(dbSet));
        }


        /// <summary>
        /// 数据库集。
        /// </summary>
        /// <value>返回 <see cref="DbSet{TEntity}"/>。</value>
        public DbSet<TEntity> DbSet { get; }

        /// <summary>
        /// 实体类型。
        /// </summary>
        public Type EntityType { get; }
            = typeof(TEntity);


        #region AddOrUpdate

        /// <summary>
        /// 尝试添加实体集合。
        /// </summary>
        /// <param name="predicate">给定是否存在的谓语表达式。</param>
        /// <param name="addRangeFunc">给定的添加集合方法。</param>
        /// <param name="addedPostAction">给定已添加的后置动作（可选）。</param>
        /// <returns>返回是否已添加集合的布尔值。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public virtual bool TryAddRange(Expression<Func<TEntity, bool>> predicate,
            Func<IEnumerable<TEntity>> addRangeFunc, Action<IEnumerable<TEntity>> addedPostAction = null)
        {
            // From Database
            if (DbSet.Any(predicate))
                return false;

            var addRange = addRangeFunc.Invoke();

            // From Local Cache
            if (DbSet.Local.Any(predicate.Compile()))
            {
                // 默认使用附加，保存更改核心会自行根据情况重置状态
                DbSet.AttachRange(addRange);
            }
            else
            {
                DbSet.AddRange(addRange);
            }

            addedPostAction?.Invoke(addRange);
            return true;
        }

        /// <summary>
        /// 异步尝试添加实体集合。
        /// </summary>
        /// <param name="predicate">给定是否存在的谓语表达式。</param>
        /// <param name="addRangeFunc">给定的添加集合方法。</param>
        /// <param name="addedPostAction">给定已添加的后置动作（可选）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含是否已添加集合的布尔值的异步操作。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public virtual async Task<bool> TryAddRangeAsync(Expression<Func<TEntity, bool>> predicate,
            Func<IEnumerable<TEntity>> addRangeFunc, Action<IEnumerable<TEntity>> addedPostAction = null,
            CancellationToken cancellationToken = default)
        {
            // From Database
            if (await DbSet.AnyAsync(predicate, cancellationToken).ConfigureAwait())
                return false;

            var addRange = addRangeFunc.Invoke();

            // From Local Cache
            if (DbSet.Local.Any(predicate.Compile()))
            {
                // 默认使用附加，保存更改核心会自行根据情况重置状态
                DbSet.AttachRange(addRange);
            }
            else
            {
                await DbSet.AddRangeAsync(addRange, cancellationToken).ConfigureAwait();
            }

            addedPostAction?.Invoke(addRange);
            return true;
        }

        /// <summary>
        /// 异步尝试添加异步实体集合。
        /// </summary>
        /// <param name="predicate">给定是否存在的谓语表达式。</param>
        /// <param name="addRangeFunc">给定的异步添加集合方法。</param>
        /// <param name="addedPostAction">给定已添加的后置动作（可选）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含是否已添加集合的布尔值的异步操作。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public virtual async Task<bool> TryAddRangeAsync(Expression<Func<TEntity, bool>> predicate,
            Func<Task<IEnumerable<TEntity>>> addRangeFunc,
            Action<IEnumerable<TEntity>> addedPostAction = null,
            CancellationToken cancellationToken = default)
        {
            // From Database
            if (await DbSet.AnyAsync(predicate, cancellationToken).ConfigureAwait())
                return false;

            var adds = await addRangeFunc.Invoke().ConfigureAwait();

            // From Local Cache
            if (DbSet.Local.Any(predicate.Compile()))
            {
                // 默认使用附加，保存更改核心会自行根据情况重置状态
                DbSet.AttachRange(adds);
            }
            else
            {
                await DbSet.AddRangeAsync(adds, cancellationToken).ConfigureAwait();
            }

            addedPostAction?.Invoke(adds);
            return true;
        }


        /// <summary>
        /// 尝试添加实体。
        /// </summary>
        /// <param name="predicate">给定是否存在的谓语表达式。</param>
        /// <param name="addFunc">给定的添加方法。</param>
        /// <param name="addedPostAction">给定已添加的后置动作（可选）。</param>
        /// <returns>返回是否已添加的布尔值。</returns>
        public virtual bool TryAdd(Expression<Func<TEntity, bool>> predicate,
            Func<TEntity> addFunc, Action<EntityEntry<TEntity>> addedPostAction = null)
            => TryAddOrUpdate(predicate, addFunc, isUpdatedFunc: null, addedPostAction);

        /// <summary>
        /// 异步尝试添加实体。
        /// </summary>
        /// <param name="predicate">给定是否存在的谓语表达式。</param>
        /// <param name="addFunc">给定的添加方法。</param>
        /// <param name="addedPostAction">给定已添加的后置动作（可选）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含是否已添加或更新的布尔值的异步操作。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public virtual Task<bool> TryAddAsync(Expression<Func<TEntity, bool>> predicate,
            Func<TEntity> addFunc, Action<EntityEntry<TEntity>> addedPostAction = null,
            CancellationToken cancellationToken = default)
            => TryAddOrUpdateAsync(predicate, addFunc, isUpdatedFunc: null, addedPostAction,
                updatedPostAction: null, addedOrUpdatedPostAction: null, cancellationToken);

        /// <summary>
        /// 异步尝试异步添加实体。
        /// </summary>
        /// <param name="predicate">给定是否存在的谓语表达式。</param>
        /// <param name="addFunc">给定的异步添加方法。</param>
        /// <param name="addedPostAction">给定已添加的后置动作（可选）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含是否已添加或更新的布尔值的异步操作。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public virtual Task<bool> TryAddAsync(Expression<Func<TEntity, bool>> predicate,
            Func<Task<TEntity>> addFunc,
            Action<EntityEntry<TEntity>> addedPostAction = null,
            CancellationToken cancellationToken = default)
            => TryAddOrUpdateAsync(predicate, addFunc, isUpdatedFunc: null, addedPostAction,
                updatedPostAction: null, addedOrUpdatedPostAction: null, cancellationToken);


        /// <summary>
        /// 尝试添加或更新实体。
        /// </summary>
        /// <param name="predicate">给定是否存在的谓语表达式。</param>
        /// <param name="addFunc">给定的添加方法。</param>
        /// <param name="isUpdatedFunc">给定是否已更新的方法（可选）。</param>
        /// <param name="addedPostAction">给定已添加的后置动作（可选）。</param>
        /// <param name="updatedPostAction">给定已更新的后置动作（可选）。</param>
        /// <param name="addedOrUpdatedPostAction">给定已添加或更新的后置动作（可选）。</param>
        /// <returns>返回是否已添加或更新的布尔值。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public virtual bool TryAddOrUpdate(Expression<Func<TEntity, bool>> predicate,
            Func<TEntity> addFunc, Func<TEntity, bool> isUpdatedFunc = null,
            Action<EntityEntry<TEntity>> addedPostAction = null,
            Action<EntityEntry<TEntity>> updatedPostAction = null,
            Action<EntityEntry<TEntity>> addedOrUpdatedPostAction = null)
        {
            // From Database
            var entity = DbSet.FirstOrDefault(predicate);
            if (entity.IsNull())
            {
                EntityEntry<TEntity> added;

                // From Local Cache
                entity = DbSet.Local.FirstOrDefault(predicate.Compile());
                if (entity.IsNull())
                {
                    entity = addFunc.Invoke();
                    added = DbSet.Add(entity);
                }
                else
                {
                    // 默认使用附加，保存更改核心会自行根据情况重置状态
                    added = DbSet.Attach(entity);
                }

                addedPostAction?.Invoke(added);
                addedOrUpdatedPostAction?.Invoke(added);

                return true;
            }

            if (isUpdatedFunc.IsNotNull() && isUpdatedFunc.Invoke(entity))
            {
                var updated = DbSet.Update(entity);

                updatedPostAction?.Invoke(updated);
                addedOrUpdatedPostAction?.Invoke(updated);

                return true;
            }

            return false;
        }

        /// <summary>
        /// 异步尝试添加或更新实体。
        /// </summary>
        /// <param name="predicate">给定是否存在的谓语表达式。</param>
        /// <param name="addFunc">给定的添加方法。</param>
        /// <param name="isUpdatedFunc">给定是否已更新的方法（可选）。</param>
        /// <param name="addedPostAction">给定已添加的后置动作（可选）。</param>
        /// <param name="updatedPostAction">给定已更新的后置动作（可选）。</param>
        /// <param name="addedOrUpdatedPostAction">给定已添加或更新的后置动作（可选）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含是否已添加或更新的布尔值的异步操作。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public virtual async Task<bool> TryAddOrUpdateAsync(Expression<Func<TEntity, bool>> predicate,
            Func<TEntity> addFunc, Func<TEntity, bool> isUpdatedFunc = null,
            Action<EntityEntry<TEntity>> addedPostAction = null,
            Action<EntityEntry<TEntity>> updatedPostAction = null,
            Action<EntityEntry<TEntity>> addedOrUpdatedPostAction = null,
            CancellationToken cancellationToken = default)
        {
            // From Database
            var entity = await DbSet.FirstOrDefaultAsync(predicate, cancellationToken).ConfigureAwait();
            if (entity.IsNull())
            {
                EntityEntry<TEntity> added;

                // From Local Cache
                entity = DbSet.Local.FirstOrDefault(predicate.Compile());
                if (entity.IsNull())
                {
                    entity = addFunc.Invoke();
                    added = await DbSet.AddAsync(entity, cancellationToken).ConfigureAwait();
                }
                else
                {
                    // 默认使用附加，保存更改核心会自行根据情况重置状态
                    added = DbSet.Attach(entity);
                }

                addedPostAction?.Invoke(added);
                addedOrUpdatedPostAction?.Invoke(added);

                return true;
            }

            if (isUpdatedFunc.IsNotNull() && isUpdatedFunc.Invoke(entity))
            {
                var updated = DbSet.Update(entity);

                updatedPostAction?.Invoke(updated);
                addedOrUpdatedPostAction?.Invoke(updated);

                return true;
            }

            return false;
        }

        /// <summary>
        /// 异步尝试异步添加或更新实体。
        /// </summary>
        /// <param name="predicate">给定是否存在的谓语表达式。</param>
        /// <param name="addFunc">给定的异步添加方法。</param>
        /// <param name="isUpdatedFunc">给定是否已更新的异步方法（可选）。</param>
        /// <param name="addedPostAction">给定已添加的后置动作（可选）。</param>
        /// <param name="updatedPostAction">给定已更新的后置动作（可选）。</param>
        /// <param name="addedOrUpdatedPostAction">给定已添加或更新的后置动作（可选）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含是否已添加或更新的布尔值的异步操作。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public virtual async Task<bool> TryAddOrUpdateAsync(Expression<Func<TEntity, bool>> predicate,
            Func<Task<TEntity>> addFunc, Func<TEntity, Task<bool>> isUpdatedFunc = null,
            Action<EntityEntry<TEntity>> addedPostAction = null,
            Action<EntityEntry<TEntity>> updatedPostAction = null,
            Action<EntityEntry<TEntity>> addedOrUpdatedPostAction = null,
            CancellationToken cancellationToken = default)
        {
            // From Database
            var entity = await DbSet.FirstOrDefaultAsync(predicate, cancellationToken).ConfigureAwait();
            if (entity.IsNull())
            {
                EntityEntry<TEntity> added;

                // From Local Cache
                entity = DbSet.Local.FirstOrDefault(predicate.Compile());
                if (entity.IsNull())
                {
                    entity = await addFunc.Invoke().ConfigureAwait();
                    added = await DbSet.AddAsync(entity, cancellationToken).ConfigureAwait();
                }
                else
                {
                    // 默认使用附加，保存更改核心会自行根据情况重置状态
                    added = DbSet.Attach(entity);
                }

                addedPostAction?.Invoke(added);
                addedOrUpdatedPostAction?.Invoke(added);

                return true;
            }

            if (isUpdatedFunc.IsNotNull() && await isUpdatedFunc.Invoke(entity).ConfigureAwait())
            {
                var updated = DbSet.Update(entity);

                updatedPostAction?.Invoke(updated);
                addedOrUpdatedPostAction?.Invoke(updated);

                return true;
            }

            return false;
        }

        #endregion


        #region Initialize

        /// <summary>
        /// 尝试初始化实体集合。
        /// </summary>
        /// <param name="initializeFunc">给定的初始化实体集合方法。</param>
        /// <param name="initializePostAction">给定的初始化后置动作（可选）。</param>
        /// <returns>返回是否已初始化的布尔值。</returns>
        public virtual bool TryInitializeRange(Func<IEnumerable<TEntity>> initializeFunc,
            Action<IEnumerable<TEntity>> initializePostAction = null)
        {
            if (DbSet.Any())
                return false;

            IEnumerable<TEntity> addRange = null;

            if (!DbSet.Local.Any())
            {
                addRange = initializeFunc.NotNull(nameof(initializeFunc)).Invoke();
                DbSet.AddRange(addRange);
            }
            else
            {
                addRange = DbSet.Local;
            }

            initializePostAction?.Invoke(addRange);
            return true;
        }

        /// <summary>
        /// 异步尝试初始化实体集合。
        /// </summary>
        /// <param name="initializeFunc">给定的初始化实体集合方法。</param>
        /// <param name="initializePostAction">给定的初始化后置动作（可选）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含是否已初始化的布尔值的异步操作。</returns>
        public virtual async Task<bool> TryInitializeRangeAsync(Func<IEnumerable<TEntity>> initializeFunc,
            Action<IEnumerable<TEntity>> initializePostAction = null, CancellationToken cancellationToken = default)
        {
            if (await DbSet.AnyAsync(cancellationToken).ConfigureAwait())
                return false;

            IEnumerable<TEntity> addRange = null;

            if (!DbSet.Local.Any())
            {
                addRange = initializeFunc.NotNull(nameof(initializeFunc)).Invoke();
                await DbSet.AddRangeAsync(addRange, cancellationToken).ConfigureAwait();
            }
            else
            {
                addRange = DbSet.Local;
            }

            initializePostAction?.Invoke(addRange);
            return true;
        }

        /// <summary>
        /// 异步尝试异步初始化实体集合。
        /// </summary>
        /// <param name="initializeFunc">给定的异步初始化实体集合方法。</param>
        /// <param name="initializePostAction">给定的初始化后置动作（可选）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含是否已初始化的布尔值的异步操作。</returns>
        public virtual async Task<bool> TryInitializeRangeAsync(Func<Task<IEnumerable<TEntity>>> initializeFunc,
            Action<IEnumerable<TEntity>> initializePostAction = null, CancellationToken cancellationToken = default)
        {
            if (await DbSet.AnyAsync(cancellationToken).ConfigureAwait())
                return false;

            IEnumerable<TEntity> addRange = null;

            if (!DbSet.Local.Any())
            {
                addRange = await initializeFunc.NotNull(nameof(initializeFunc)).Invoke().ConfigureAwait();
                await DbSet.AddRangeAsync(addRange, cancellationToken).ConfigureAwait();
            }
            else
            {
                addRange = DbSet.Local;
            }

            initializePostAction?.Invoke(addRange);
            return true;
        }

        #endregion


        #region Remove

        /// <summary>
        /// 尝试移除实体标识集合。
        /// </summary>
        /// <typeparam name="TId">指定的标识类型。</typeparam>
        /// <param name="ids">给定的标识集合。</param>
        /// <param name="postAction">给定的移除后置动作（可选）。</param>
        /// <returns>返回是否已移除的布尔值。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public virtual bool TryRemoveIds<TId>(IEnumerable<TId> ids,
            Action<IEnumerable<TId>> postAction = null)
            where TId : IEquatable<TId>
        {
            ids.NotNull(nameof(ids));

            if (!EntityType.IsImplementedInterfaceType<IIdentifier<TId>>())
                throw new InvalidOperationException($"Unsupported remove entity type '{EntityType}'. The entity type needs to implement the identifier interface '{typeof(IIdentifier<TId>)}'.");

            foreach (var id in ids)
            {
                var entity = ObjectExtensions.EnsureCreate<TEntity>();

                if (entity is IIdentifier<TId> identifier)
                {
                    identifier.Id = id;
                    DbSet.Remove(entity);
                }
            }

            postAction?.Invoke(ids);
            return true;
        }


        /// <summary>
        /// 尝试逻辑移除实体集合。
        /// </summary>
        /// <param name="entities">给定实现 <see cref="IState{DataStatus}"/> 接口的实体集合。</param>
        /// <param name="postAction">给定的逻辑移除后置动作（可选）。</param>
        /// <returns>返回是否已逻辑移除的布尔值。</returns>
        public virtual bool TryLogicRemoveRange(IEnumerable<TEntity> entities,
            Action<IEnumerable<TEntity>> postAction = null)
            => TryLogicRemoveRange(entities, DataStatus.Delete, postAction);

        /// <summary>
        /// 尝试逻辑移除实体集合。
        /// </summary>
        /// <typeparam name="TStatus">指定的状态类型。</typeparam>
        /// <param name="entities">给定实现 <see cref="IState{TStatus}"/> 接口的实体集合。</param>
        /// <param name="deleteStatus">给定的删除状态。</param>
        /// <param name="postAction">给定的逻辑移除后置动作（可选）。</param>
        /// <returns>返回是否已逻辑移除的布尔值。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public virtual bool TryLogicRemoveRange<TStatus>(IEnumerable<TEntity> entities,
            TStatus deleteStatus, Action<IEnumerable<TEntity>> postAction = null)
            where TStatus : struct
        {
            entities.NotNull(nameof(entities));

            if (!EntityType.IsImplementedInterfaceType<IState<TStatus>>())
                throw new InvalidOperationException($"Unsupported logic remove entity type '{EntityType}'. The entity type needs to implement the state interface '{typeof(IState<TStatus>)}'.");

            foreach (var entity in entities)
            {
                if (entity is IState<TStatus> state)
                {
                    state.Status = deleteStatus;
                    DbSet.Update(entity);
                }
            }

            postAction?.Invoke(entities);
            return true;
        }

        #endregion

    }
}
