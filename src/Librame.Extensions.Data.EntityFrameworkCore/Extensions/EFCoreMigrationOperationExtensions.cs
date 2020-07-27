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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Microsoft.EntityFrameworkCore.Migrations.Operations
{
    /// <summary>
    /// <see cref="MigrationOperation"/> 静态扩展。
    /// </summary>
    public static class EFCoreMigrationOperationExtensions
    {
        /// <summary>
        /// 按表操作为最高优先级排序。
        /// </summary>
        /// <param name="operations">给定的 <see cref="IReadOnlyList{MigrationOperation}"/>。</param>
        /// <returns>返回 <see cref="IReadOnlyList{MigrationOperation}"/>。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static IReadOnlyList<MigrationOperation> OrderByTableOperation
            (this IReadOnlyList<MigrationOperation> operations)
        {
            operations.NotNull(nameof(operations));

            if (!operations.Any(p => p is TableOperation) || operations.Count <= 1)
                return operations;

            // 因 MigrationOperation 未实现排序接口，所以采用手动排序
            var allOperations = new List<MigrationOperation>();

            List<MigrationOperation> tableOperations = null;
            List<MigrationOperation> otherOperations = null;

            foreach (var operation in operations)
            {
                if (operation is TableOperation)
                {
                    if (operation is CreateTableOperation)
                    {
                        // 创建表操作具有最高优先级
                        allOperations.Add(operation);
                    }
                    else
                    {
                        if (tableOperations.IsNull())
                            tableOperations = new List<MigrationOperation>();

                        tableOperations.Add(operation);
                    }
                }
                else
                {
                    if (otherOperations.IsNull())
                        otherOperations = new List<MigrationOperation>();

                    otherOperations.Add(operation);
                }
            }

            if (tableOperations.IsNotNull())
                allOperations.AddRange(tableOperations);

            if (otherOperations.IsNotNull())
                allOperations.AddRange(otherOperations);

            return allOperations;
        }

    }
}
