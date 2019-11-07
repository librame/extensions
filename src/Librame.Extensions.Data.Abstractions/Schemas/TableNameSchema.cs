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
    /// 表名架构。
    /// </summary>
    public class TableNameSchema : AbstractTableSchema, IEquatable<TableNameSchema>
    {
        /// <summary>
        /// 构造一个 <see cref="TableNameSchema"/>。
        /// </summary>
        /// <param name="descriptor">给定的 <see cref="TableNameDescriptor"/>。</param>
        /// <param name="schema">给定的架构（可选）。</param>
        public TableNameSchema(TableNameDescriptor descriptor, string schema = null)
            : base(schema)
        {
            NameDescriptor = descriptor.NotNull(nameof(descriptor));
        }

        /// <summary>
        /// 构造一个 <see cref="TableNameSchema"/>。
        /// </summary>
        /// <param name="name">给定的表名。</param>
        /// <param name="schema">给定的架构（可选）。</param>
        public TableNameSchema(string name, string schema = null)
            : base(schema)
        {
            Name = name.NotEmpty(nameof(name));
        }


        /// <summary>
        /// 表名描述符。
        /// </summary>
        public TableNameDescriptor NameDescriptor { get; }

        /// <summary>
        /// 表名。
        /// </summary>
        public string Name { get; }


        /// <summary>
        /// 表名或描述符字符串。
        /// </summary>
        public string NameOrDescriptorString
            => NameDescriptor.IsNotNull() ? NameDescriptor : Name;


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="other">给定的其他 <see cref="TableNameSchema"/>。</param>
        /// <returns>返回布尔值。</returns>
        public bool Equals(TableNameSchema other)
            => ToString() == other?.ToString();

        /// <summary>
        /// 重写是否相等。
        /// </summary>
        /// <param name="obj">给定要比较的对象。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
            => (obj is TableNameSchema other) ? Equals(other) : false;


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回 32 位整数。</returns>
        public override int GetHashCode()
            => ToString().CompatibleGetHashCode();


        /// <summary>
        /// 转换为字符串。
        /// </summary>
        /// <returns>返回表名。</returns>
        public override string ToString()
            => $"{base.ToString()}{NameOrDescriptorString}";


        /// <summary>
        /// 比较相等。
        /// </summary>
        /// <param name="a">给定的 <see cref="TableNameSchema"/>。</param>
        /// <param name="b">给定的 <see cref="TableNameSchema"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator ==(TableNameSchema a, TableNameSchema b)
            => (a?.Equals(b)).Value;

        /// <summary>
        /// 比较不等。
        /// </summary>
        /// <param name="a">给定的 <see cref="TableNameDescriptor"/>。</param>
        /// <param name="b">给定的 <see cref="TableNameDescriptor"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator !=(TableNameSchema a, TableNameSchema b)
            => !(a?.Equals(b)).Value;


        /// <summary>
        /// 隐式转换为字符串形式。
        /// </summary>
        /// <param name="schema">给定的 <see cref="TableNameSchema"/>。</param>
        public static implicit operator string(TableNameSchema schema)
            => schema?.ToString();
    }
}
