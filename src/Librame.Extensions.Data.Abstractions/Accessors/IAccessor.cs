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
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Accessors
{
    using Core.Services;
    using Data.Validators;

    /// <summary>
    /// 访问器接口。
    /// </summary>
    public interface IAccessor : IDisposable, IMigration, IMultiTenancy, ISaveChanges, IService
    {
        /// <summary>
        /// 内存缓存。
        /// </summary>
        /// <value>返回 <see cref="IMemoryCache"/>。</value>
        IMemoryCache MemoryCache { get; }

        /// <summary>
        /// 时钟服务。
        /// </summary>
        /// <value>返回 <see cref="IClockService"/>。</value>
        IClockService Clock { get; }

        /// <summary>
        /// 数据库创建验证器。
        /// </summary>
        /// <value>返回 <see cref="IDatabaseCreationValidator"/>。</value>
        IDatabaseCreationValidator CreationValidator { get; }


        /// <summary>
        /// 当前时间戳。
        /// </summary>
        DateTimeOffset CurrentTimestamp { get; }

        /// <summary>
        /// 当前类型。
        /// </summary>
        Type CurrentType { get; }


        /// <summary>
        /// 存在任何数据集集合。
        /// </summary>
        /// <returns>返回布尔值。</returns>
        bool AnySets();


        /// <summary>
        /// 执行 SQL 命令。
        /// </summary>
        /// <param name="sql">给定的 SQL 语句。</param>
        /// <param name="parameters">给定的参数集合。</param>
        /// <returns>返回受影响的行数。</returns>
        int ExecuteSqlRaw(string sql, params object[] parameters);

        /// <summary>
        /// 异步执行 SQL 命令。
        /// </summary>
        /// <param name="sql">给定的 SQL 语句。</param>
        /// <param name="parameters">给定的参数集合。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含受影响行数的异步操作。</returns>
        Task<int> ExecuteSqlRawAsync(string sql, IEnumerable<object> parameters, CancellationToken cancellationToken = default);
    }
}
