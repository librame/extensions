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
    /// 抽象表名架构静态扩展。
    /// </summary>
    public static class AbstractionTableNameSchemaExtensions
    {
        /// <summary>
        /// 转换为表名架构。
        /// </summary>
        /// <param name="descriptor">给定的 <see cref="TableNameDescriptor"/>。</param>
        /// <param name="schema">给定的架构（可选）。</param>
        /// <returns>返回 <see cref="TableNameSchema"/>。</returns>
        public static TableNameSchema AsSchema(this TableNameDescriptor descriptor, string schema = null)
            => new TableNameSchema(descriptor, schema);

    }
}
