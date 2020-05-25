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
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

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

    }
}
