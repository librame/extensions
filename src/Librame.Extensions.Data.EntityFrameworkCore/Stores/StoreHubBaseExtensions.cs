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

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 存储中心基类静态扩展。
    /// </summary>
    public static class StoreHubBaseExtensions
    {
        /// <summary>
        /// 并发更新。
        /// </summary>
        /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <param name="stores">给定的 <see cref="IStore{TAccessor}"/>。</param>
        /// <param name="exception">给定的 <see cref="DbUpdateConcurrencyException"/>。</param>
        /// <param name="predicate">给定获取单条数据的断定方法。</param>
        /// <param name="updatePropertyName">给定要更新的属性名（可选；默认为空表示更新所有属性值）。</param>
        /// <returns>返回 <see cref="EntityResult"/>。</returns>
        public static EntityResult ConcurrentUpdates<TAccessor, TEntity>(this IStoreHub<TAccessor> stores,
            DbUpdateConcurrencyException exception, Func<TEntity, bool> predicate, string updatePropertyName = null)
            where TAccessor : DbContextAccessor
            where TEntity : class
        {
            try
            {
                foreach (var entry in exception.Entries)
                {
                    if (entry.Entity is TEntity)
                    {
                        foreach (var property in entry.Metadata.GetProperties())
                        {
                            var dbEntity = stores.Accessor.Set<TEntity>().AsNoTracking().Single(predicate);
                            var dbEntityEntry = stores.Accessor.Entry(dbEntity);

                            //var proposedValue = entry.Property(property.Name).CurrentValue;
                            //var originalValue = entry.Property(property.Name).OriginalValue;
                            //var databaseValue = dbEntityEntry.Property(property.Name).CurrentValue;

                            // 如果要更新的属性名不为空且等于当前属性，或要更新的属性名为空，则并发更新此属性
                            if ((updatePropertyName.IsNotNullOrEmpty() && property.Name == updatePropertyName)
                                || updatePropertyName.IsNullOrEmpty())
                            {
                                // Update original values to
                                entry.Property(property.Name).OriginalValue = dbEntityEntry.Property(property.Name).CurrentValue;

                                if (updatePropertyName.IsNotNullOrEmpty())
                                    break; // 如果只更新当前属性，则跳出属性遍历
                            }
                        }
                    }
                    else
                    {
                        throw new NotSupportedException("Don't know how to handle concurrency conflicts for " + entry.Metadata.Name);
                    }
                }

                stores.Accessor.SaveChanges();

                return EntityResult.Success;
            }
            catch (Exception ex)
            {
                return EntityResult.Failed(ex);
            }
        }

    }
}
