#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using System.Diagnostics;

namespace Librame.Logging
{
    using Utility;

    /// <summary>
    /// 日志助手。
    /// </summary>
    public class LogHelper
    {
        /// <summary>
        /// 默认日志类别。
        /// </summary>
        internal const string DEFAULT_CATEGORY = LibrameAssemblyConstants.TRADEMARK + "Logging";


        ///// <summary>
        ///// 获取日志写入器。
        ///// </summary>
        ///// <returns>返回 <see cref="LogWriter"/></returns>
        //public static LogWriter GetLogWriter()
        //{
        //    var logger = LibrameArchitecture.AdapterManager.LoggingAdapter.GetLogger<LoggingHelper>();

        //    if (logger is EntLib.EntLibLogger)
        //        return (logger as EntLib.EntLibLogger).LogWriter;

        //    return Logger.Writer;
        //}


        /// <summary>
        /// 填充日志入口。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="message">给定的消息。</param>
        /// <param name="severity">给定的跟踪事件类型。</param>
        /// <param name="eventId">给定的事件编号。</param>
        /// <param name="priority">给定的优先级。</param>
        /// <returns>返回 <see cref="LogEntry"/>。</returns>
        public static LogEntry PopulateLogEntry<T>(string message, TraceEventType severity = TraceEventType.Information, int eventId = 1, int priority = 1)
        {
            return PopulateLogEntry(typeof(T), message, severity, eventId, priority);
        }
        /// <summary>
        /// 填充日志入口。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <param name="message">给定的消息。</param>
        /// <param name="severity">给定的跟踪事件类型。</param>
        /// <param name="eventId">给定的事件编号。</param>
        /// <param name="priority">给定的优先级。</param>
        /// <returns>返回 <see cref="LogEntry"/>。</returns>
        public static LogEntry PopulateLogEntry(Type type, string message, TraceEventType severity = TraceEventType.Information, int eventId = 1, int priority = 1)
        {
            type.NotNull(nameof(type));

            var log = new LogEntry();

            log.Severity = severity;
            log.TimeStamp = DateTime.Now;
            log.EventId = eventId;
            log.Priority = priority;
            log.Title = type.AssemblyQualifiedName;
            log.Message = message;
            
            // 配置的默认分类
            log.Categories.Clear();
            log.Categories.Add(DEFAULT_CATEGORY);

            return log;
        }

    }
}
