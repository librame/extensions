#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Linq;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 表架构。
    /// </summary>
    public class TableSchema : AbstractSchema, ITableSchema
    {
        /// <summary>
        /// 构造一个 <see cref="TableSchema"/> 实例。
        /// </summary>
        /// <param name="name">给定的名称。</param>
        /// <param name="schema">给定的架构（可选）。</param>
        public TableSchema(string name, string schema = null)
            : base(schema)
        {
            Name = name;
        }


        /// <summary>
        /// 名称。
        /// </summary>
        public string Name { get; }


        /// <summary>
        /// 获取实体类型名称复数形式。
        /// </summary>
        /// <param name="entityType">给定的实体类型。</param>
        /// <returns>返回字符串。</returns>
        public static string GetEntityTypeNames(Type entityType)
        {
            if (entityType.IsNull())
                return string.Empty;

            if (entityType.IsGenericType)
            {
                var types = entityType.GetGenericArguments();
                if (types.IsNotNullOrEmpty())
                {
                    // 取出具体实类型名称（多个实类型则默认返回第一个）
                    return GetEntityTypeNames(types.Where(type => type.IsConcreteType()).FirstOrDefault());
                }

                return entityType.Name.SplitPair("`").Key?.AsPluralize();
            }

            return entityType.Name.AsPluralize();
        }

    }
}
