#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data
{
    using Stores;

    /// <summary>
    /// 多租户接口。
    /// </summary>
    public interface IMultiTenancy
    {
        /// <summary>
        /// 当前租户。
        /// </summary>
        /// <value>返回 <see cref="ITenant"/>。</value>
        ITenant CurrentTenant { get; }

        /// <summary>
        /// 当前连接字符串。
        /// </summary>
        string CurrentConnectionString { get; }


        /// <summary>
        /// 获取当前反向连接字符串（如果当前为默认连接字符串则返回写入连接字符串，反之则返回默认连接字符串）。
        /// </summary>
        /// <returns>返回字符串。</returns>
        string GetCurrentReverseConnectionString();

        /// <summary>
        /// 获取当前数据连接描述（用于表示当前是 <see cref="ITenant.DefaultConnectionString"/> 还是 <see cref="ITenant.WritingConnectionString"/>）。
        /// </summary>
        /// <returns>返回字符串。</returns>
        string GetCurrentConnectionDescription();


        /// <summary>
        /// 是当前连接字符串。
        /// </summary>
        /// <param name="connectionString">给定的连接字符串。</param>
        /// <returns>返回布尔值。</returns>
        bool IsCurrentConnectionString(string connectionString);

        /// <summary>
        /// 是默认连接字符串（未启用读写分离也将被视为默认连接字符串）。
        /// </summary>
        /// <returns>返回布尔值。</returns>
        bool IsDefaultConnectionString();

        /// <summary>
        /// 是写入连接字符串（未启用读写分离也将被视为写入连接字符串）。
        /// </summary>
        /// <returns>返回布尔值。</returns>
        bool IsWritingConnectionString();


        /// <summary>
        /// 更改连接字符串。
        /// </summary>
        /// <param name="changeFunc">给定更改租户连接字符串的工厂方法。</param>
        /// <returns>返回已更改的布尔值。</returns>
        bool ChangeConnectionString(Func<ITenant, string> changeFunc);

        /// <summary>
        /// 异步更改连接字符串。
        /// </summary>
        /// <param name="changeFunc">给定更改租户连接字符串的工厂方法。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含已更改的布尔值的异步操作。</returns>
        Task<bool> ChangeConnectionStringAsync(Func<ITenant, string> changeFunc,
            CancellationToken cancellationToken = default);


        /// <summary>
        /// 尝试切换租户。
        /// </summary>
        /// <returns>返回是否已切换的布尔值。</returns>
        bool TrySwitchTenant();

        /// <summary>
        /// 异步尝试切换租户。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含是否已切换的布尔值的异步操作。</returns>
        Task<bool> TrySwitchTenantAsync(CancellationToken cancellationToken = default);
    }
}
