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
    /// 分表架构。
    /// </summary>
    public class ShardingSchema : IShardingSchema
    {
        /// <summary>
        /// 构造一个 <see cref="ShardingSchema"/> 实例。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// formatter is empty.
        /// </exception>
        /// <param name="formatter">给定的名称。</param>
        /// <param name="ruleType">给定的规则类型。</param>
        /// <param name="formattings">给定的格式化描述符集合（可选）。</param>
        public ShardingSchema(string formatter, Type ruleType, params FormattingDescriptor[] formattings)
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
        /// <returns>返回 <see cref="IShardingSchema"/>。</returns>
        public IShardingSchema TryApplySchema(string schema)
        {
            if (schema.IsNotEmpty())
                Schema = schema;

            return this;
        }
    }

}
