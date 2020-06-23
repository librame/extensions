#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Stores
{
    using Core.Services;

    /// <summary>
    /// 存储初始化器接口。
    /// </summary>
    public interface IStoreInitializer : IService
    {
        /// <summary>
        /// 验证器。
        /// </summary>
        /// <value>返回 <see cref="IStoreInitializationValidator"/>。</value>
        IStoreInitializationValidator Validator { get; }

        /// <summary>
        /// 标识符生成器。
        /// </summary>
        IStoreIdentifierGenerator IdentifierGenerator { get; }


        /// <summary>
        /// 需要保存变化。
        /// </summary>
        bool RequiredSaveChanges { get; }


        /// <summary>
        /// 初始化存储。
        /// </summary>
        /// <param name="stores">给定的 <see cref="IStoreHub"/>。</param>
        void Initialize(IStoreHub stores);
    }
}
