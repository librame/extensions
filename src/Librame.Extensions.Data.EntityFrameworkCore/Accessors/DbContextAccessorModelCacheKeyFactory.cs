#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Librame.Extensions.Data.Accessors
{
    /// <summary>
    /// 数据库上下文访问器模型缓存键工厂。
    /// </summary>
    public class DbContextAccessorModelCacheKeyFactory : IModelCacheKeyFactory
    {
        /// <summary>
        /// 创建缓存键。
        /// </summary>
        /// <param name="context">给定的 <see cref="DbContext"/>。</param>
        /// <returns>返回缓存键对象。</returns>
        public object Create(DbContext context)
        {
            context.NotNull(nameof(context));

            if (context is DbContextAccessorBase accessor)
                return new DbContextAccessorModelCacheKey(accessor, accessor.CurrentTimestamp);

            return new ModelCacheKey(context);
        }

    }
}
