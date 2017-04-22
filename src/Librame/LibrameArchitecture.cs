#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections.Concurrent;

namespace Librame
{
    using Utility;

    /// <summary>
    /// Librame 天平架构快捷方式。
    /// </summary>
    public static class Archit
    {
        /// <summary>
        /// 获取容器适配器。
        /// </summary>
        public static Container.IContainerAdapter Can
        {
            get { return LibrameArchitecture.ContainerAdapter; }
        }

        /// <summary>
        /// 获取日志适配器。
        /// </summary>
        public static Logging.ILoggingAdapter Log
        {
            get { return LibrameArchitecture.LoggingAdapter; }
        }

        /// <summary>
        /// 获取适配器集合。
        /// </summary>
        public static Adaptation.IAdapterManager Adapters
        {
            get { return LibrameArchitecture.AdapterManager; }
        }


        /// <summary>
        /// 设定适配器。
        /// </summary>
        /// <typeparam name="TAdapter">指定的适配器类型。</typeparam>
        /// <param name="adapter">给定的适配器实例。</param>
        /// <returns>返回适配器实例。</returns>
        public static TAdapter SetAdapter<TAdapter>(TAdapter adapter)
            where TAdapter : Adaptation.IAdapter
        {
            return LibrameArchitecture.SetAdapter(adapter);
        }

    }


    /// <summary>
    /// Librame 天平架构。
    /// </summary>
    public static class LibrameArchitecture
    {
        private static readonly string _containerKey = KeyBuilder.BuildKey<Container.IContainerAdapter>();
        private static readonly string _loggingKey = KeyBuilder.BuildKey<Logging.ILoggingAdapter>();
        private static readonly string _adapterManagerKey = KeyBuilder.BuildKey<Adaptation.IAdapterManager>();
        
        /// <summary>
        /// 获取容器适配器。
        /// </summary>
        public static Container.IContainerAdapter ContainerAdapter
        {
            get
            {
                return (Container.IContainerAdapter)_adapters.GetOrAdd(_containerKey,
                    key => new Container.DefaultContainerAdapter());
            }
        }
        
        /// <summary>
        /// 获取日志适配器。
        /// </summary>
        public static Logging.ILoggingAdapter LoggingAdapter
        {
            get
            {
                return (Logging.ILoggingAdapter)_adapters.GetOrAdd(_loggingKey,
                    key => new Logging.DefaultLoggingAdapter());
            }
        }
        
        /// <summary>
        /// 获取容器管理器。
        /// </summary>
        public static Adaptation.IAdapterManager AdapterManager
        {
            get
            {
                return (Adaptation.IAdapterManager)_adapters.GetOrAdd(_adapterManagerKey,
                    key =>
                    {
                        var container = Container.ContainerHelper.BuildDependencyOverride<Container.IContainerAdapter>(ContainerAdapter);
                        var logging = Container.ContainerHelper.BuildDependencyOverride<Logging.ILoggingAdapter>(LoggingAdapter);

                        return ContainerAdapter.Resolve<Adaptation.IAdapterManager>(container, logging);
                    });
            }
        }


        #region BaseAdapters

        private static readonly ConcurrentDictionary<string, object> _adapters =
            new ConcurrentDictionary<string, object>();
        
        /// <summary>
        /// 设定适配器。
        /// </summary>
        /// <typeparam name="TAdapter">指定的适配器类型。</typeparam>
        /// <param name="adapter">给定的适配器实例。</param>
        /// <returns>返回适配器实例。</returns>
        public static TAdapter SetAdapter<TAdapter>(TAdapter adapter)
            where TAdapter : Adaptation.IAdapter
        {
            string key = KeyBuilder.BuildKey<TAdapter>();

            return (TAdapter)_adapters.AddOrUpdate(key, adapter, (k, v) => adapter);
        }

        #endregion

    }

}
