﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Librame.Extensions.Data
{
    using Builders;
    using Services;

    /// <summary>
    /// 存储接口。
    /// </summary>
    /// <typeparam name="TBuilderOptions">指定的构建器选项类型。</typeparam>
    public interface IStore<TBuilderOptions> : IService<TBuilderOptions>
        where TBuilderOptions : class, IBuilderOptions, new()
    {
    }


    /// <summary>
    /// 存储接口。
    /// </summary>
    public interface IStore : IService
    {
        /// <summary>
        /// 异步获取审计。
        /// </summary>
        /// <param name="id">给定的标识。</param>
        /// <returns>返回一个包含 <see cref="Audit"/> 的异步操作。</returns>
        Task<Audit> GetAuditAsync(int id);

        /// <summary>
        /// 异步获取审计分页列表。
        /// </summary>
        /// <param name="index">给定的页索引。</param>
        /// <param name="size">给定的显示条数。</param>
        /// <returns>返回一个包含 <see cref="IPagingList{Audit}"/> 的异步操作。</returns>
        Task<IPagingList<Audit>> GetAuditsAsync(int index, int size);


        /// <summary>
        /// 异步获取租户。
        /// </summary>
        /// <param name="id">给定的标识。</param>
        /// <returns>返回一个包含 <see cref="Tenant"/> 的异步操作。</returns>
        Task<Tenant> GetTenantAsync(int id);

        /// <summary>
        /// 获取租户分页列表。
        /// </summary>
        /// <param name="index">给定的页索引。</param>
        /// <param name="size">给定的显示条数。</param>
        /// <returns>返回一个包含 <see cref="IPagingList{Tenant}"/> 的异步操作。</returns>
        Task<IPagingList<Tenant>> GetTenantsAsync(int index, int size);
    }

}
