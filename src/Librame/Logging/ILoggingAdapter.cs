#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Common.Logging;
using System;

namespace Librame.Logging
{
    /// <summary>
    /// 日志适配器接口。
    /// </summary>
    public interface ILoggingAdapter : Adaptation.IAdapter
    {
        /// <summary>
        /// 获取通用日志接口。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <returns>返回 <see cref="ILog"/>。</returns>
        ILog GetLogger<T>();

        /// <summary>
        /// 获取通用日志接口。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回 <see cref="ILog"/>。</returns>
        ILog GetLogger(Type type);

        /// <summary>
        /// 获取通用日志接口。
        /// </summary>
        /// <param name="key">给定的配置键名。</param>
        /// <returns>返回 <see cref="ILog"/>。</returns>
        ILog GetLogger(string key);
    }
}
