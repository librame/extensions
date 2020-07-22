#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 保存更改接口。
    /// </summary>
    public interface ISaveChanges
    {
        /// <summary>
        /// 是从保存更改调用。
        /// </summary>
        bool IsFromSaveChangesInvoke { get; }

        /// <summary>
        /// 所需的保存更改。
        /// </summary>
        bool RequiredSaveChanges { get; set; }

        /// <summary>
        /// 所需的标识插入表名列表。
        /// </summary>
        IReadOnlyList<string> RequiredIdentityInsertTableNames { get; set; }


        /// <summary>
        /// 保存更改。
        /// </summary>
        /// <returns>返回受影响的行数。</returns>
        int SaveChanges();

        /// <summary>
        /// 保存更改。
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">指示是否在更改已成功发送到数据库之后调用。</param>
        /// <returns>返回受影响的行数。</returns>
        int SaveChanges(bool acceptAllChangesOnSuccess);


        /// <summary>
        /// 异步保存更改。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含受影响行数的异步操作。</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步保存更改。
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">指示是否在更改已成功发送到数据库之后调用。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含受影响行数的异步操作。</returns>
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
    }
}
