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

namespace Librame.Extensions.Data.Services
{
    using Data.Accessors;

    /// <summary>
    /// 迁移访问器服务接口。
    /// </summary>
    public interface IMigrationAccessorService : IAccessorService
    {
        /// <summary>
        /// 迁移数据。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        void Migrate(IAccessor accessor);

        /// <summary>
        /// 异步数据。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个异步操作。</returns>
        Task MigrateAsync(IAccessor accessor, CancellationToken cancellationToken = default);
    }
}
