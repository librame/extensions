#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Data.Builders
{
    /// <summary>
    /// <see cref="IDataBuilder"/> 静态扩展。
    /// </summary>
    public static class AbstractionDataBuilderMemoryCacheExtensions
    {
        /// <summary>
        /// 获取数据构建器。
        /// </summary>
        /// <param name="memoryCache">给定的 <see cref="IMemoryCache"/>。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static IDataBuilder GetDataBuilder(this IMemoryCache memoryCache)
        {
            memoryCache.NotNull(nameof(memoryCache));

            if (memoryCache.TryGetValue(nameof(IDataBuilder), out var value))
                return value as IDataBuilder;

            return null;
        }

        /// <summary>
        /// 设置数据构建器。
        /// </summary>
        /// <param name="memoryCache">给定的 <see cref="IMemoryCache"/>。</param>
        /// <param name="builder">给定的 <see cref="IDataBuilder"/>。</param>
        /// <returns>返回 <see cref="IMemoryCache"/>。</returns>
        public static IMemoryCache SetDataBuilder(this IMemoryCache memoryCache,
            IDataBuilder builder)
        {
            builder.NotNull(nameof(builder));
            memoryCache.NotNull(nameof(memoryCache));

            memoryCache.Set(nameof(IDataBuilder), builder);

            return memoryCache;
        }

    }
}
