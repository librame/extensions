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

namespace Librame.Extensions.Data.Validators
{
    using Core.Services;
    using Data.Accessors;

    /// <summary>
    /// 数据库初始化验证器接口（主要用于持久化验证数据库初始化操作）。
    /// </summary>
    public interface IDataInitializationValidator : IService
    {
        /// <summary>
        /// 是否已初始化。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        /// <returns>返回布尔值。</returns>
        bool IsInitialized(IAccessor accessor);

        /// <summary>
        /// 异步是否已初始化。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含布尔值的异步操作。</returns>
        Task<bool> IsInitializedAsync(IAccessor accessor, CancellationToken cancellationToken = default);


        /// <summary>
        /// 设置已初始化。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        void SetInitialized(IAccessor accessor);

        /// <summary>
        /// 异步设置已初始化。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        Task SetInitializedAsync(IAccessor accessor, CancellationToken cancellationToken = default);
    }
}
