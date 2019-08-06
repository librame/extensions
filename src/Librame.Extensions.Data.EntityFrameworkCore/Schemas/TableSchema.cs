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

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 表架构。
    /// </summary>
    public class TableSchema : AbstractDataSchema, ITableSchema
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
        /// 转换为字符串。
        /// </summary>
        /// <returns>返回表名。</returns>
        public override string ToString()
        {
            if (Schema.IsNotNullOrEmpty())
                return $"{Schema}.{Name}";

            return Name;
        }


        /// <summary>
        /// 获取实体类型的复数名称。
        /// </summary>
        /// <typeparam name="TEntity">指定的实体类型。</typeparam>
        /// <returns>返回字符串。</returns>
        public static string GetEntityPluralName<TEntity>()
        {
            return GetEntityPluralName(typeof(TEntity));
        }

        /// <summary>
        /// 获取实体类型的复数名称。
        /// </summary>
        /// <param name="entityType">给定的实体类型。</param>
        /// <returns>返回字符串。</returns>
        public static string GetEntityPluralName(Type entityType)
        {
            return entityType.GetBodyName().AsPluralize();
        }

    }
}
