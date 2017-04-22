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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;

namespace Librame.Logging
{
    /// <summary>
    /// 默认日志适配器。
    /// </summary>
    public class DefaultLoggingAdapter : AbstractLoggingAdapter, ILoggingAdapter
    {
        /// <summary>
        /// 构造一个 <see cref="DefaultLoggingAdapter"/> 实例。
        /// </summary>
        public DefaultLoggingAdapter()
        {
            // Initialize
            LogHelper.InitConfigFiles(this);

            //var configurationSource = ConfigurationSourceFactory.Create();
            //var logWriterFactory = new LogWriterFactory(configurationSource);
            //Logger.SetLogWriter(logWriterFactory.Create());
        }


        /// <summary>
        /// 获取通用日志接口。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <returns>返回 <see cref="ILog"/>。</returns>
        public virtual ILog GetLogger<T>()
        {
            return LogManager.GetLogger<T>();
        }

        /// <summary>
        /// 获取通用日志接口。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回 <see cref="ILog"/>。</returns>
        public virtual ILog GetLogger(Type type)
        {
            return LogManager.GetLogger(type);
        }

        /// <summary>
        /// 获取通用日志接口。
        /// </summary>
        /// <param name="key">给定的配置键名。</param>
        /// <returns>返回 <see cref="ILog"/>。</returns>
        public virtual ILog GetLogger(string key)
        {
            return LogManager.GetLogger(key);
        }

    }
}
