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
    /// <typeparam name="T">指定的类型。</typeparam>
    public class TableSchema<T> : TableSchema
    {
        /// <summary>
        /// 构造一个 <see cref="TableSchema"/> 实例。
        /// </summary>
        public TableSchema()
            : base(GetTableName())
        {
        }


        private static string GetTableName()
        {
            var type = typeof(T);
            var name = type.Name;

            if (type.IsGenericType)
                name = type.Name.SplitPair("`").Key;

            return name.AsPluralize();
        }
    }


    /// <summary>
    /// 表架构。
    /// </summary>
    public class TableSchema : ITableSchema
    {
        /// <summary>
        /// 构造一个 <see cref="TableSchema"/> 实例。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// name is empty.
        /// </exception>
        /// <param name="name">给定的名称。</param>
        public TableSchema(string name)
        {
            Name = name.NotEmpty(nameof(name));
        }


        /// <summary>
        /// 名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 架构。
        /// </summary>
        public string Schema { get; set; }


        /// <summary>
        /// 尝试应用架构（如果架构不为空，否则直接返回）。
        /// </summary>
        /// <param name="schema">给定的架构。</param>
        /// <returns>返回 <see cref="ITableSchema"/>。</returns>
        public ITableSchema TryApplySchema(string schema)
        {
            if (schema.IsNotEmpty())
                Schema = schema;

            return this;
        }
    }

}
