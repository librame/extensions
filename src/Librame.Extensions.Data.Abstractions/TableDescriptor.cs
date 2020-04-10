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
    /// 表描述符。
    /// </summary>
    public class TableDescriptor : IEquatable<TableDescriptor>
    {
        /// <summary>
        /// 默认界定符。
        /// </summary>
        public const string DefaultDelimiter = ".";

        /// <summary>
        /// 默认名称连接符。
        /// </summary>
        public const string DefautlNameConnector = "_";


        /// <summary>
        /// 构造一个 <see cref="TableDescriptor"/>。
        /// </summary>
        /// <param name="name">给定的名称。</param>
        /// <param name="schema">给定的架构（可选）。</param>
        public TableDescriptor(string name, string schema = null)
        {
            Name = name.NotEmpty(nameof(name));
            Schema = schema;
        }


        /// <summary>
        /// 名称。
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 架构。
        /// </summary>
        public string Schema { get; set; }


        /// <summary>
        /// 界定符（用于界定架构与名称；默认为“.”）。
        /// </summary>
        public string Delimiter { get; set; }
            = DefaultDelimiter;

        /// <summary>
        /// 名称连接符（用于名称间多个部分的连接；默认为“_”）。
        /// </summary>
        public string NameConnector { get; set; }
            = DefautlNameConnector;


        /// <summary>
        /// 改变名称。
        /// </summary>
        /// <param name="newNameFactory">给定的新名称工厂方法。</param>
        /// <returns>返回 <see cref="TableDescriptor"/>。</returns>
        public TableDescriptor ChangeName(Func<string, string> newNameFactory)
            => ChangeName(newNameFactory?.Invoke(Name));

        /// <summary>
        /// 改变名称。
        /// </summary>
        /// <param name="newName">给定的新名称。</param>
        /// <returns>返回 <see cref="TableDescriptor"/>。</returns>
        public TableDescriptor ChangeName(string newName)
        {
            Name = newName.NotEmpty(nameof(newName));
            return this;
        }


        /// <summary>
        /// 配置表描述符。
        /// </summary>
        /// <param name="action">给定的动作（可选）。</param>
        /// <returns>返回 <see cref="TableDescriptor"/>。</returns>
        public TableDescriptor Configure(Action<TableDescriptor> action = null)
        {
            action?.Invoke(this);
            return this;
        }


        /// <summary>
        /// 复制实例。
        /// </summary>
        /// <returns>返回 <see cref="TableDescriptor"/>。</returns>
        public TableDescriptor Copy()
        {
            var table = new TableDescriptor(Name, Schema);
            table.Delimiter = Delimiter;
            table.NameConnector = NameConnector;

            return table;
        }


        /// <summary>
        /// 带有新名称。
        /// </summary>
        /// <param name="newNameFactory">给定的新名称工厂方法。</param>
        /// <returns>返回 <see cref="TableDescriptor"/>。</returns>
        public TableDescriptor WithName(Func<string, string> newNameFactory)
            => WithName(newNameFactory?.Invoke(Name));

        /// <summary>
        /// 带有新名称。
        /// </summary>
        /// <param name="newName">给定的新名称。</param>
        /// <returns>返回 <see cref="TableDescriptor"/>。</returns>
        public TableDescriptor WithName(string newName)
        {
            var table = new TableDescriptor(newName, Schema);
            table.Delimiter = Delimiter;
            table.NameConnector = NameConnector;

            return table;
        }


        /// <summary>
        /// 带有配置表描述符。
        /// </summary>
        /// <param name="action">给定的动作（可选）。</param>
        /// <returns>返回 <see cref="TableDescriptor"/>。</returns>
        public TableDescriptor WithConfigure(Action<TableDescriptor> action = null)
        {
            var table = Copy();
            action?.Invoke(table);

            return table;
        }


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="other">给定的其他 <see cref="TableDescriptor"/>。</param>
        /// <returns>返回布尔值。</returns>
        public bool Equals(TableDescriptor other)
            => ToString() == other?.ToString();

        /// <summary>
        /// 重写是否相等。
        /// </summary>
        /// <param name="obj">给定要比较的对象。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
            => (obj is TableDescriptor other) ? Equals(other) : false;


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回 32 位整数。</returns>
        public override int GetHashCode()
            => ToString().CompatibleGetHashCode();


        /// <summary>
        /// 转换为字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
        {
            if (Schema.IsNotEmpty())
                return $"{Schema}{Delimiter}{Name}";

            return Name;
        }


        /// <summary>
        /// 比较相等。
        /// </summary>
        /// <param name="a">给定的 <see cref="TableDescriptor"/>。</param>
        /// <param name="b">给定的 <see cref="TableDescriptor"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator ==(TableDescriptor a, TableDescriptor b)
            => (a?.Equals(b)).Value;

        /// <summary>
        /// 比较不等。
        /// </summary>
        /// <param name="a">给定的 <see cref="TableDescriptor"/>。</param>
        /// <param name="b">给定的 <see cref="TableDescriptor"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator !=(TableDescriptor a, TableDescriptor b)
            => !(a?.Equals(b)).Value;


        /// <summary>
        /// 隐式转换为字符串形式。
        /// </summary>
        /// <param name="descriptor">给定的 <see cref="TableDescriptor"/>。</param>
        public static implicit operator string(TableDescriptor descriptor)
            => descriptor?.ToString();


        /// <summary>
        /// 通过实体类型创建表描述符。
        /// </summary>
        /// <typeparam name="T">指定的实体类型。</typeparam>
        /// <param name="schema">给定的架构（可选）。</param>
        /// <returns>返回 <see cref="TableDescriptor"/>。</returns>
        public static TableDescriptor Create<T>(string schema = null)
            where T : class
            => Create(typeof(T), schema);

        /// <summary>
        /// 通过实体类型创建表描述符。
        /// </summary>
        /// <param name="entityType">给定的实体类型。</param>
        /// <param name="schema">给定的架构（可选）。</param>
        /// <returns>返回 <see cref="TableDescriptor"/>。</returns>
        public static TableDescriptor Create(Type entityType, string schema = null)
        {
            var name = entityType.GetGenericBodyName().AsPluralize();
            return new TableDescriptor(name, schema);
        }
    }
}
