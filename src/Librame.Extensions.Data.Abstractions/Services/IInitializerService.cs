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
    using Core;

    /// <summary>
    /// 初始化器服务。
    /// </summary>
    /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
    /// <typeparam name="TIdentifier">指定的标识符服务类型。</typeparam>
    public interface IInitializerService<in TAccessor, out TIdentifier> : IInitializerService<TAccessor>
        where TAccessor : IAccessor
        where TIdentifier : IIdentifierService
    {
        /// <summary>
        /// 标识符服务。
        /// </summary>
        /// <value>返回 <typeparamref name="TIdentifier"/>。</value>
        new TIdentifier Identifier { get; }
    }


    /// <summary>
    /// 初始化器服务。
    /// </summary>
    /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
    public interface IInitializerService<in TAccessor> : IService
        where TAccessor : IAccessor
    {
        /// <summary>
        /// 标识符服务。
        /// </summary>
        /// <value>返回 <see cref="IIdentifierService"/>。</value>
        IIdentifierService Identifier { get; }


        /// <summary>
        /// 初始化服务。
        /// </summary>
        /// <param name="stores">给定的 <see cref="IStoreHub{TAccessor}"/>。</param>
        void InitializeService(IStoreHub<TAccessor> stores);
    }
}
