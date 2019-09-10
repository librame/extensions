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
using System.Text;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 表名描述符。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    public class TableNameDescriptor<TEntity> : TableNameDescriptor
    {
        /// <summary>
        /// 构造一个 <see cref="TableNameDescriptor"/>。
        /// </summary>
        /// <param name="bodyNameFactory">给定的主体名称工厂方法（可选；默认为实体类型名称的复数形式）。</param>
        /// <param name="suffix">给定的后缀（可选）。</param>
        /// <param name="prefix">给定的前缀（可选）。</param>
        /// <param name="connector">给定的连接符（可选；默认为下划线）。</param>
        public TableNameDescriptor(Func<Type, string> bodyNameFactory = null,
            string suffix = null, string prefix = null, string connector = CONNECTOR)
            : base(typeof(TEntity), bodyNameFactory, suffix, prefix, connector)
        {
        }
    }


    /// <summary>
    /// 表名描述符。
    /// </summary>
    public class TableNameDescriptor : IEquatable<TableNameDescriptor>
    {
        /// <summary>
        /// 默认连接符。
        /// </summary>
        protected const string CONNECTOR
            = "_";

        private static readonly Func<Type, string> _defaultBodyNameFactory
            = type => type.GetBodyName().AsPluralize();


        /// <summary>
        /// 构造一个 <see cref="TableNameDescriptor"/>。
        /// </summary>
        /// <param name="entityType">给定的实体类型。</param>
        /// <param name="bodyNameFactory">给定的主体名称工厂方法（可选；默认为实体类型名称的复数形式）。</param>
        /// <param name="suffix">给定的后缀（可选）。</param>
        /// <param name="prefix">给定的前缀（可选）。</param>
        /// <param name="connector">给定的连接符（可选；默认为下划线）。</param>
        public TableNameDescriptor(Type entityType, Func<Type, string> bodyNameFactory = null,
            string suffix = null, string prefix = null, string connector = CONNECTOR)
        {
            EntityType = entityType.NotNull(nameof(entityType));
            Suffix = suffix;
            Prefix = prefix;
            Connector = connector;
            BodyName = (bodyNameFactory ?? _defaultBodyNameFactory).Invoke(entityType);
        }


        /// <summary>
        /// 实体类型。
        /// </summary>
        public Type EntityType { get; }

        /// <summary>
        /// 主体名称。
        /// </summary>
        public string BodyName { get; private set; }

        /// <summary>
        /// 前缀。
        /// </summary>
        public string Prefix { get; private set; }

        /// <summary>
        /// 后缀。
        /// </summary>
        public string Suffix { get; private set; }

        /// <summary>
        /// 连接符。
        /// </summary>
        public string Connector { get; private set; }


        #region Change

        /// <summary>
        /// 改变主体名称。
        /// </summary>
        /// <param name="newBodyNameFactory">给定的新主体名称工厂方法。</param>
        /// <returns>返回 <see cref="TableNameDescriptor"/>。</returns>
        public TableNameDescriptor ChangeBodyName(Func<string, string> newBodyNameFactory)
        {
            BodyName = newBodyNameFactory.NotNull(nameof(newBodyNameFactory)).Invoke(BodyName);
            return this;
        }


        /// <summary>
        /// 改变连接符。
        /// </summary>
        /// <param name="newConnector">给定的新连接符。</param>
        /// <returns>返回 <see cref="TableNameDescriptor"/>。</returns>
        public TableNameDescriptor ChangeConnector(string newConnector)
        {
            Connector = newConnector.NotNullOrEmpty(nameof(newConnector));
            return this;
        }


        /// <summary>
        /// 改变前缀。
        /// </summary>
        /// <param name="newPrefix">给定的新前缀。</param>
        /// <returns>返回 <see cref="TableNameDescriptor"/>。</returns>
        public TableNameDescriptor ChangePrefix(string newPrefix)
        {
            Prefix = newPrefix.NotNullOrEmpty(nameof(newPrefix));
            return this;
        }


        /// <summary>
        /// 改变日期后缀。
        /// </summary>
        /// <param name="newSuffixFactory">给定的新日期后缀工厂方法。</param>
        /// <returns>返回 <see cref="TableNameDescriptor"/>。</returns>
        public TableNameDescriptor ChangeDateSuffix(Func<DateTime, string> newSuffixFactory)
        {
            newSuffixFactory.NotNull(nameof(newSuffixFactory));
            return ChangeSuffix(newSuffixFactory.Invoke(DateTime.Now));
        }

        /// <summary>
        /// 改变日期后缀。
        /// </summary>
        /// <param name="newSuffixFactory">给定的新日期后缀工厂方法。</param>
        /// <returns>返回 <see cref="TableNameDescriptor"/>。</returns>
        public TableNameDescriptor ChangeDateOffsetSuffix(Func<DateTimeOffset, string> newSuffixFactory)
        {
            newSuffixFactory.NotNull(nameof(newSuffixFactory));
            return ChangeSuffix(newSuffixFactory.Invoke(DateTimeOffset.Now));
        }

        /// <summary>
        /// 改变后缀。
        /// </summary>
        /// <param name="newSuffix">给定的新后缀。</param>
        /// <returns>返回 <see cref="TableNameDescriptor"/>。</returns>
        public TableNameDescriptor ChangeSuffix(string newSuffix)
        {
            Suffix = newSuffix.NotNullOrEmpty(nameof(newSuffix));
            return this;
        }

        #endregion


        /// <summary>
        /// 重置到初始状态。
        /// </summary>
        /// <returns>返回 <see cref="TableNameDescriptor"/>。</returns>
        public TableNameDescriptor Reset()
        {
            BodyName = _defaultBodyNameFactory.Invoke(EntityType);
            Connector = CONNECTOR;

            Prefix = null;
            Suffix = null;

            return this;
        }


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="other">给定的其他 <see cref="TableNameDescriptor"/>。</param>
        /// <returns>返回布尔值。</returns>
        public bool Equals(TableNameDescriptor other)
        {
            if (other.IsNull())
                return false;

            return ToString() == other.ToString();
        }

        /// <summary>
        /// 重写是否相等。
        /// </summary>
        /// <param name="obj">给定要比较的对象。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
        {
            if (obj is TableNameDescriptor other)
                return Equals(other);

            return false;
        }


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回 32 位整数。</returns>
        public override int GetHashCode()
            => ToString().GetHashCode();


        /// <summary>
        /// 转换为字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            if (Prefix.IsNotNullOrEmpty())
            {
                sb.Append(Prefix);
                sb.Append(Connector);
            }

            sb.Append(BodyName);

            if (Suffix.IsNotNullOrEmpty())
            {
                sb.Append(Connector);
                sb.Append(Suffix);
            }

            return sb.ToString();
        }


        /// <summary>
        /// 比较相等。
        /// </summary>
        /// <param name="a">给定的 <see cref="TableNameDescriptor"/>。</param>
        /// <param name="b">给定的 <see cref="TableNameDescriptor"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator ==(TableNameDescriptor a, TableNameDescriptor b)
            => a.Equals(b);

        /// <summary>
        /// 比较不等。
        /// </summary>
        /// <param name="a">给定的 <see cref="TableNameDescriptor"/>。</param>
        /// <param name="b">给定的 <see cref="TableNameDescriptor"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator !=(TableNameDescriptor a, TableNameDescriptor b)
            => !a.Equals(b);


        /// <summary>
        /// 隐式转换为字符串形式。
        /// </summary>
        /// <param name="descriptor">给定的 <see cref="TableNameDescriptor"/>。</param>
        public static implicit operator string(TableNameDescriptor descriptor)
            => descriptor?.ToString();
    }
}
