#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 存储初始化器接口。
    /// </summary>
    /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
    /// <typeparam name="TIdentifier">指定的标识符类型。</typeparam>
    public interface IStoreInitializer<TAccessor, TIdentifier> : IStoreInitializer<TAccessor>
        where TAccessor : IAccessor
        where TIdentifier : IStoreIdentifier
    {
        /// <summary>
        /// 标识符。
        /// </summary>
        new TIdentifier Identifier { get; }
    }


    /// <summary>
    /// 存储初始化器接口。
    /// </summary>
    /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
    public interface IStoreInitializer<TAccessor> : IStoreInitializer
        where TAccessor : IAccessor
    {
        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="stores">给定的 <see cref="IStoreHub{TAccessor}"/>。</param>
        void Initialize(IStoreHub<TAccessor> stores);
    }


    /// <summary>
    /// 存储初始化器接口。
    /// </summary>
    public interface IStoreInitializer
    {
        /// <summary>
        /// 标识符。
        /// </summary>
        IStoreIdentifier Identifier { get; }

        /// <summary>
        /// 是否已完成初始化。
        /// </summary>
        bool IsInitialized { get; }
    }
}
