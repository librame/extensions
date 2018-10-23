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
    /// 分表特性。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ShardingAttribute : Attribute, IShardingOptions
    {
        /// <summary>
        /// 构造一个 <see cref="ShardingAttribute"/> 实例。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// formatter is empty.
        /// </exception>
        /// <param name="formatter">给定的格式化器。</param>
        /// <param name="ruleType">给定的规则类型。</param>
        /// <param name="formattings">给定的格式化描述符集合（可选）。</param>
        public ShardingAttribute(string formatter, Type ruleType, params FormattingDescriptor[] formattings)
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
        /// <value>
        /// 返回 <see cref="IList{FormattingDescriptor}"/>。
        /// </value>
        public IList<FormattingDescriptor> Formattings { get; set; }
            = new List<FormattingDescriptor>();
        
        private Type _ruleType;
        /// <summary>
        /// 派生自 <see cref="IShardingRule"/> 接口的分表规则类型。
        /// </summary>
        public Type RuleType
        {
            get
            {
                return _ruleType.NotDefault(nameof(RuleType));
            }
            set
            {
                _ruleType = typeof(IShardingRule).CanAssignableFromType(value);
            }
        }

        /// <summary>
        /// 自定义参数。
        /// </summary>
        public object Data { get; set; }
    }
}
