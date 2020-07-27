#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 迁移接口。
    /// </summary>
    public interface IMigration
    {
        /// <summary>
        /// 是从迁移调用。
        /// </summary>
        bool IsFromMigrateInvoke { get; }

        /// <summary>
        /// 包含初始化数据。
        /// </summary>
        bool ContainsInitializationData { get; }


        /// <summary>
        /// 数据库迁移。
        /// </summary>
        /// <returns>返回可能存在的受影响行数。</returns>
        int Migrate();

        /// <summary>
        /// 异步数据库迁移。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含可能存在受影响的行数的异步操作。</returns>
        Task<int> MigrateAsync(CancellationToken cancellationToken = default);
    }
}
