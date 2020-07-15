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

namespace Librame.Extensions.Data.Stores
{
    using Core.Services;
    using Data.Accessors;
    using Data.Validators;

    /// <summary>
    /// 存储初始化器接口。
    /// </summary>
    public interface IStoreInitializer : IService
    {
        /// <summary>
        /// 初始化验证器。
        /// </summary>
        /// <value>返回 <see cref="IDataInitializationValidator"/>。</value>
        IDataInitializationValidator Validator { get; }

        /// <summary>
        /// 标识符生成器。
        /// </summary>
        /// <value>返回 <see cref="IStoreIdentifierGenerator"/>。</value>
        IStoreIdentifierGenerator IdentifierGenerator { get; }

        /// <summary>
        /// 时钟。
        /// </summary>
        /// <value>返回 <see cref="IClockService"/>。</value>
        IClockService Clock { get; }


        /// <summary>
        /// 初始化访问器。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        void Initialize(IAccessor accessor);

        /// <summary>
        /// 异步初始化访问器。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        Task InitializeAsync(IAccessor accessor, CancellationToken cancellationToken = default);
    }
}
