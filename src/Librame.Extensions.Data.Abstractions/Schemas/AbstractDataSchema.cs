#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 抽象数据架构。
    /// </summary>
    public abstract class AbstractDataSchema : IDataSchema
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractDataSchema"/>。
        /// </summary>
        /// <param name="schema">给定的架构（可选）。</param>
        public AbstractDataSchema(string schema = null)
        {
            Schema = schema;
        }


        /// <summary>
        /// 架构。
        /// </summary>
        public string Schema { get; set; }


        /// <summary>
        /// 转换为字符串。
        /// </summary>
        /// <returns>返回架构。</returns>
        public override string ToString()
        {
            return Schema;
        }

    }
}
