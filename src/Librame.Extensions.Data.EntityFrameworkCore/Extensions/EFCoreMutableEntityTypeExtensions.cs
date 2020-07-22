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
using Librame.Extensions.Data;
using Librame.Extensions.Data.Stores;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// <see cref="IMutableEntityType"/> 静态扩展。
    /// </summary>
    public static class EFCoreMutableEntityTypeExtensions
    {
        /// <summary>
        /// 尝试添加删除状态查询过滤器。
        /// </summary>
        /// <param name="entityType">给定的 <see cref="IMutableEntityType"/>。</param>
        /// <returns>返回布尔值。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static bool TryAddDeleteStatusQueryFilter(this IMutableEntityType entityType)
        {
            entityType.NotNull(nameof(entityType));

            if (!entityType.ClrType.IsImplementedInterfaceType<IState<DataStatus>>())
                return false;

            var method = typeof(EFCoreMutableEntityTypeExtensions)
                .GetMethod(nameof(GetDeleteStatusQueryFilter), BindingFlags.NonPublic | BindingFlags.Static)
                .MakeGenericMethod(entityType.ClrType);

            var queryFilter = method.Invoke(null, Array.Empty<object>());
            entityType.SetQueryFilter((LambdaExpression)queryFilter);

            return true;
        }

        private static LambdaExpression GetDeleteStatusQueryFilter<TEntity>()
            where TEntity : class, IState<DataStatus>
        {
            Expression<Func<TEntity, bool>> filter = p => p.Status != DataStatus.Delete;
            return filter;
        }

    }
}
