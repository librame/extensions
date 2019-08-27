﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Threading.Tasks;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 人性化服务接口。
    /// </summary>
    public interface IHumanizationService : IService
    {
        /// <summary>
        /// 异步人性化。
        /// </summary>
        /// <param name="dateTime">给定的 <see cref="DateTime"/>。</param>
        /// <returns>返回一个包含字符串的异步操作。</returns>
        Task<string> HumanizeAsync(DateTime dateTime);

        /// <summary>
        /// 异步人性化。
        /// </summary>
        /// <param name="dateTimeOffset">给定的 <see cref="DateTimeOffset"/>。</param>
        /// <returns>返回一个包含字符串的异步操作。</returns>
        Task<string> HumanizeAsync(DateTimeOffset dateTimeOffset);
    }
}
