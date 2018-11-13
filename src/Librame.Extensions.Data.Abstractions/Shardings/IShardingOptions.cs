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

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 分表选项接口。
    /// </summary>
    public interface IShardingOptions : IOptions
    {
        /// <summary>
        /// 表名格式化器。
        /// </summary>
        string Formatter { get; set; }

        /// <summary>
        /// 格式化集合。
        /// </summary>
        IList<FormattingDescriptor> Formattings { get; set; }

        /// <summary>
        /// 派生自 <see cref="IShardingRule"/> 接口的分表规则类型。
        /// </summary>
        Type RuleType { get; set; }

        /// <summary>
        /// 自定义参数。
        /// </summary>
        object Data { get; set; }
    }
}
