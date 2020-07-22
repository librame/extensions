#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// <see cref="TableDescriptor"/> 静态扩展。
    /// </summary>
    public static class EFCoreTableDescriptorExtensions
    {
        /// <summary>
        /// 获取表描述符。
        /// </summary>
        /// <param name="entityType">给定的 <see cref="IEntityType"/>。</param>
        /// <returns>返回 <see cref="TableDescriptor"/>。</returns>
        public static TableDescriptor GetTableDescriptor(this IEntityType entityType)
            => new TableDescriptor(entityType.GetTableName(), entityType.GetSchema());

    }
}
