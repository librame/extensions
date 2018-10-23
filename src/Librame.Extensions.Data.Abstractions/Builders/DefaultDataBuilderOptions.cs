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
using System.Collections.Generic;
using System.Linq;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 默认数据构建器选项。
    /// </summary>
    public class DefaultDataBuilderOptions : IDataBuilderOptions
    {
        /// <summary>
        /// 默认架构。
        /// </summary>
        public string DefaultSchema { get; set; }

        /// <summary>
        /// 启用审计（默认已启用）。
        /// </summary>
        public bool AuditEnabled { get; set; } = true;

        /// <summary>
        /// 确保数据库已创建（默认已启用）。
        /// </summary>
        public bool EnsureDbCreated { get; set; } = true;

        /// <summary>
        /// 发布审计事件动作方法。
        /// </summary>
        public Action<IDbContext, IList<Audit>> PublishAuditEvent { get; set; }


        /// <summary>
        /// 连接选项。
        /// </summary>
        public ConnectionOptions Connection { get; set; } = new ConnectionOptions();

        /// <summary>
        /// 审计表选项。
        /// </summary>
        public ITableOptions AuditTable { get; set; }

        /// <summary>
        /// 审计属性表选项。
        /// </summary>
        public IShardingOptions AuditPropertyTable { get; set; }
    }
    
    
    /// <summary>
    /// 连接选项。
    /// </summary>
    public class ConnectionOptions
    {
        /// <summary>
        /// 默认连接字符串。
        /// </summary>
        public string DefaultString { get; set; } = "librame_default";

        /// <summary>
        /// 写入连接字符串。
        /// </summary>
        public string WriteString { get; set; } = "librame_writer";

        /// <summary>
        /// 写入分离（默认不启用）。
        /// </summary>
        public bool WriteSeparation { get; set; } = false;
    }


    /// <summary>
    /// 分表选项。
    /// </summary>
    public class ShardingOptions : IShardingOptions
    {
        /// <summary>
        /// 构造一个 <see cref="ShardingOptions"/> 实例。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// formatter is empty.
        /// </exception>
        /// <param name="formatter">给定的名称。</param>
        /// <param name="ruleType">给定的规则类型。</param>
        /// <param name="formattings">给定的格式化描述符集合（可选）。</param>
        public ShardingOptions(string formatter, Type ruleType, params FormattingDescriptor[] formattings)
        {
            Formatter = formatter.NotEmpty(nameof(formatter));
            RuleType = ruleType;
            Formattings = formattings.ToList();
        }


        /// <summary>
        /// 格式化器。
        /// </summary>
        public string Formatter { get; set; }

        /// <summary>
        /// 架构。
        /// </summary>
        public string Schema { get; set; }

        /// <summary>
        /// 格式化集合。
        /// </summary>
        public IList<FormattingDescriptor> Formattings { get; set; }

        /// <summary>
        /// 规则类型。
        /// </summary>
        public Type RuleType { get; set; }

        /// <summary>
        /// 自定义参数。
        /// </summary>
        public object Data { get; set; }


        /// <summary>
        /// 尝试应用架构（如果架构不为空，否则直接返回）。
        /// </summary>
        /// <param name="schema">给定的架构。</param>
        /// <returns>返回 <see cref="IShardingOptions"/>。</returns>
        public IShardingOptions TryApplySchema(string schema)
        {
            if (schema.IsNotEmpty())
                Schema = schema;

            return this;
        }
    }


    /// <summary>
    /// 表选项。
    /// </summary>
    public class TableOptions : ITableOptions
    {
        /// <summary>
        /// 构造一个 <see cref="TableOptions"/> 实例。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// name is empty.
        /// </exception>
        /// <param name="name">给定的名称。</param>
        public TableOptions(string name)
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
        /// <returns>返回 <see cref="ITableOptions"/>。</returns>
        public ITableOptions TryApplySchema(string schema)
        {
            if (schema.IsNotEmpty())
                Schema = schema;

            return this;
        }
    }

    /// <summary>
    /// 表选项。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    public class TableOptions<T> : TableOptions
    {
        /// <summary>
        /// 构造一个 <see cref="TableOptions"/> 实例。
        /// </summary>
        public TableOptions()
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

}
