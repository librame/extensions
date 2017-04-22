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
using Common.Logging.Configuration;
using Common.Logging.EntLib;
using Common.Logging.Factory;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;

namespace Librame.Logging.EntLib
{
    /// <summary>
    /// Enterprise Library 记录器工厂适配器。
    /// </summary>
    public class EntLibLoggerFactoryAdapter : AbstractCachingLoggerFactoryAdapter
    {
        private readonly EntLibLoggerSettings _settings;
        private LogWriter _logWriter;

        /// <summary>
        /// The default priority used to log events.
        /// </summary>
        /// <remarks>defaults to <see cref="EntLibLoggerSettings.DEFAULTPRIORITY"/></remarks>
        public int DefaultPriority
        {
            get { return _settings.priority; }
        }

        /// <summary>
        /// The format string used for formatting exceptions
        /// </summary>
        /// <remarks>
        /// defaults to <see cref="EntLibLoggerSettings.DEFAULTEXCEPTIONFORMAT"/>
        /// </remarks>
        public string ExceptionFormat
        {
            get { return _settings.exceptionFormat; }
        }

        /// <summary>
        /// the <see cref="_logWriter"/> to write log events to.
        /// </summary>
        /// <remarks>
        /// defaults to <see cref="Logger.Writer"/>.
        /// </remarks>
        public LogWriter LogWriter
        {
            get
            {
                if (_logWriter == null)
                {
                    lock (this)
                    {
                        if (_logWriter == null)
                        {
                            _logWriter = GetWriter();
                        }
                    }
                }
                return _logWriter;
            }
        }

        private LogWriter GetWriter()
        {
            //per http://growingtech.blogspot.it/2013/05/enterprise-library-60-logwriter-has-not.html 
            //  we have to explicitly set the writer for EntLib6 before we can return it
            var configurationSource = ConfigurationSourceFactory.Create();
            var logWriterFactory = new LogWriterFactory(configurationSource);
            Logger.SetLogWriter(logWriterFactory.Create());

            return Logger.Writer;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntLibLoggerFactoryAdapter"/> class.
        /// </summary>
        public EntLibLoggerFactoryAdapter()
            : this(EntLibLoggerSettings.DEFAULTPRIORITY, EntLibLoggerSettings.DEFAULTEXCEPTIONFORMAT, null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntLibLoggerFactoryAdapter"/> class
        /// with the specified configuration parameters.
        /// </summary>
        /// <param name="defaultPriority">defaults to <see cref="EntLibLoggerSettings.DEFAULTPRIORITY"/></param>
        /// <param name="exceptionFormat">defaults to <see cref="EntLibLoggerSettings.DEFAULTEXCEPTIONFORMAT"/></param>
        /// <param name="logWriter">a <see cref="LogWriter"/> instance to use</param>
        public EntLibLoggerFactoryAdapter(int defaultPriority, string exceptionFormat, LogWriter logWriter)
            : base(true)
        {
            if (exceptionFormat.Length == 0)
            {
                exceptionFormat = null;
            }
            _settings = new EntLibLoggerSettings(defaultPriority, exceptionFormat);
            _logWriter = logWriter;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntLibLoggerFactoryAdapter"/> class.
        /// </summary>
        /// <remarks>passed in values are not used, configuration is external to EntLib logging API</remarks>
        /// <param name="properties">The properties.</param>
        public EntLibLoggerFactoryAdapter(NameValueCollection properties)
            : this(ArgUtils.TryParse(EntLibLoggerSettings.DEFAULTPRIORITY, ArgUtils.GetValue(properties, "priority"))
                 , ArgUtils.Coalesce(ArgUtils.GetValue(properties, "exceptionFormat"), EntLibLoggerSettings.DEFAULTEXCEPTIONFORMAT)
                 , null
            )
        { }

        /// <summary>
        /// Creates a new <see cref="EntLibLogger"/> instance.
        /// </summary>
        protected override ILog CreateLogger(string name)
        {
            return CreateLogger(name, LogWriter, _settings);
        }

        /// <summary>
        /// Creates a new <see cref="EntLibLogger"/> instance.
        /// </summary>
        protected virtual ILog CreateLogger(string name, LogWriter logWriter, EntLibLoggerSettings settings)
        {
            try
            {
                // 如果是引用类型名
                var referType = Type.GetType(name);

                // 加载默认类别日志配置，同时分配类型名给日志标题
                return new EntLibLogger(LogHelper.DEFAULT_CATEGORY, referType, logWriter, settings);
            }
            catch
            {
                // 兼容常规日志（无引用类型）
                return new EntLibLogger(name, null, LogWriter, settings);
            }
        }

    }
}
