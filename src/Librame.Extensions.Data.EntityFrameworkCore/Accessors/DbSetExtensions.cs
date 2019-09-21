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
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// <see cref="DbSet{TEntity}"/> 静态扩展。
    /// </summary>
    public static class DbSetExtensions
    {
        /// <summary>
        /// 尝试异步创建集合。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="dbSet">给定的 <see cref="DbSet{TEntity}"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="entities">给定要增加的实体集合。</param>
        /// <returns>返回一个包含 <see cref="EntityResult"/> 的异步操作。</returns>
        public static async Task<EntityResult> TryCreateAsync<TEntity>(this DbSet<TEntity> dbSet,
            CancellationToken cancellationToken, params TEntity[] entities)
            where TEntity : class
        {
            try
            {
                await dbSet.AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);

                return EntityResult.Success;
            }
            catch (Exception ex)
            {
                return EntityResult.Failed(ex);
            }
        }


        /// <summary>
        /// 尝试更新集合。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="dbSet">给定的 <see cref="DbSet{TEntity}"/>。</param>
        /// <param name="entities">给定要更新的实体集合。</param>
        /// <returns>返回 <see cref="EntityResult"/>。</returns>
        public static EntityResult TryUpdate<TEntity>(this DbSet<TEntity> dbSet, params TEntity[] entities)
            where TEntity : class
        {
            try
            {
                dbSet.UpdateRange(entities);

                return EntityResult.Success;
            }
            catch (Exception ex)
            {
                return EntityResult.Failed(ex);
            }
        }


        /// <summary>
        /// 尝试删除集合。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="dbSet">给定的 <see cref="DbSet{TEntity}"/>。</param>
        /// <param name="entities">给定要删除的实体集合。</param>
        /// <returns>返回 <see cref="EntityResult"/>。</returns>
        public static EntityResult TryDelete<TEntity>(this DbSet<TEntity> dbSet, params TEntity[] entities)
            where TEntity : class
        {
            try
            {
                dbSet.RemoveRange(entities);

                return EntityResult.Success;
            }
            catch (Exception ex)
            {
                return EntityResult.Failed(ex);
            }
        }

        /// <summary>
        /// 尝试逻辑删除集合。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="dbSet">给定的 <see cref="DbSet{TEntity}"/>。</param>
        /// <param name="entities">给定要删除的实体集合。</param>
        /// <returns>返回 <see cref="EntityResult"/>。</returns>
        public static EntityResult TryLogicDelete<TEntity>(this DbSet<TEntity> dbSet, params TEntity[] entities)
            where TEntity : class, IStatus<DataStatus>
        {
            foreach (var entity in entities)
                entity.Status = DataStatus.Delete;

            return dbSet.TryUpdate(entities);
        }

    }
}
