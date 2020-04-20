#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

//using Microsoft.EntityFrameworkCore;
//using System;

//namespace Librame.Extensions.Data.Accessors
//{
//    /// <summary>
//    /// 数据库上下文访问器模型缓存键。
//    /// </summary>
//    public class DbContextAccessorModelCacheKey : IEquatable<DbContextAccessorModelCacheKey>
//    {
//        private readonly Type _dbContextType;
//        private readonly DateTimeOffset _dateTimeOffset;


//        /// <summary>
//        /// 构造一个 <see cref="DbContextAccessorModelCacheKey"/>。
//        /// </summary>
//        /// <param name="context">给定的 <see cref="DbContext"/>。</param>
//        /// <param name="dateTimeOffset">给定的时间戳。</param>
//        public DbContextAccessorModelCacheKey(DbContext context, DateTimeOffset dateTimeOffset)
//        {
//            _dbContextType = context.NotNull(nameof(context)).GetType();
//            _dateTimeOffset = dateTimeOffset;
//        }


//        /// <summary>
//        /// 是否相等。
//        /// </summary>
//        /// <param name="other">给定的 <see cref="DbContextAccessorModelCacheKey"/>。</param>
//        /// <returns>返回布尔值。</returns>
//        public bool Equals(DbContextAccessorModelCacheKey other)
//            => _dbContextType == other?._dbContextType && _dateTimeOffset == other._dateTimeOffset;

//        /// <summary>
//        /// 是否相等。
//        /// </summary>
//        /// <param name="obj">给定的对象。</param>
//        /// <returns>返回布尔值。</returns>
//        public override bool Equals(object obj)
//            => obj is DbContextAccessorModelCacheKey other && Equals(other);


//        /// <summary>
//        /// 获取哈希码。
//        /// </summary>
//        /// <returns>返回整数。</returns>
//        public override int GetHashCode()
//            => _dbContextType.GetHashCode() ^ _dateTimeOffset.GetHashCode();
//    }
//}
