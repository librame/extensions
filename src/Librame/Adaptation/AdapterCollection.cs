#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Adaptation
{
    using Utility;

    /// <summary>
    /// 适配器集合。
    /// </summary>
    public class AdapterCollection : IAdapterCollection
    {
        /// <summary>
        /// 获取容器适配器。
        /// </summary>
        public Container.IContainerAdapter Container { get; }
        
        /// <summary>
        /// 获取日志适配器。
        /// </summary>
        public Logging.ILoggingAdapter Logging { get; }


        /// <summary>
        /// 构造一个 <see cref="AdapterCollection"/> 实例。
        /// </summary>
        /// <exception cref="System.ArgumentNullException">
        /// containerAdapter 或 loggingAdapter 为空。
        /// </exception>
        /// <param name="container">给定的容器适配器。</param>
        /// <param name="logging">给定的日志适配器。</param>
        public AdapterCollection(Container.IContainerAdapter container, Logging.ILoggingAdapter logging)
        {
            Container = container.NotNull(nameof(container));
            Logging = logging.NotNull(nameof(logging));
        }


        #region Container Settings

        /// <summary>
        /// 获取首选项。
        /// </summary>
        public AdapterSettings Settings
        {
            get { return Container.Resolve<AdapterSettings>(); }
        }

        /// <summary>
        /// 算法首选项。
        /// </summary>
        public Algorithm.AlgorithmSettings AlgorithmSettings
        {
            get { return Container.Resolve<Algorithm.AlgorithmSettings>(); }
        }

        /// <summary>
        /// 认证首选项。
        /// </summary>
        public Authorization.AuthorizeSettings AuthorizeSettings
        {
            get { return Container.Resolve<Authorization.AuthorizeSettings>(); }
        }

        /// <summary>
        /// 数据首选项。
        /// </summary>
        public Data.DataSettings DataSettings
        {
            get { return Container.Resolve<Data.DataSettings>(); }
        }

        /// <summary>
        /// 窗体首选项。
        /// </summary>
        public Forms.FormsSettings FormsSettings
        {
            get { return Container.Resolve<Forms.FormsSettings>(); }
        }

        #endregion


        #region Container Adapters
        
        /// <summary>
        /// 获取算法适配器。
        /// </summary>
        public Algorithm.IAlgorithmAdapter Algorithm
        {
            get { return Container.Resolve<Algorithm.IAlgorithmAdapter>(); }
        }

        /// <summary>
        /// 获取认证适配器。
        /// </summary>
        public Authorization.IAuthorizeAdapter Authorization
        {
            get { return Container.Resolve<Authorization.IAuthorizeAdapter>(); }
        }

        /// <summary>
        /// 获取压缩适配器。
        /// </summary>
        public Compression.ICompressionAdapter Compression
        {
            get { return Container.Resolve<Compression.ICompressionAdapter>(); }
        }

        /// <summary>
        /// 获取数据适配器。
        /// </summary>
        public Data.IDataAdapter Data
        {
            get { return Container.Resolve<Data.IDataAdapter>(); }
        }

        /// <summary>
        /// 获取窗体适配器。
        /// </summary>
        public Forms.IFormsAdapter Forms
        {
            get { return Container.Resolve<Forms.IFormsAdapter>(); }
        }

        /// <summary>
        /// 获取全文检索适配器。
        /// </summary>
        public FulltextSearch.IFulltextSearchAdapter FulltextSearch
        {
            get { return Container.Resolve<FulltextSearch.IFulltextSearchAdapter>(); }
        }

        /// <summary>
        /// 获取 HTML 适配器。
        /// </summary>
        public Html.IHtmlAdapter Html
        {
            get { return Container.Resolve<Html.IHtmlAdapter>(); }
        }

        /// <summary>
        /// 获取 HTTP 适配器。
        /// </summary>
        public Http.IHttpAdapter Http
        {
            get { return Container.Resolve<Http.IHttpAdapter>(); }
        }

        /// <summary>
        /// 获取媒体信息适配器。
        /// </summary>
        public MediaInfo.IMediaInfoAdapter MediaInfo
        {
            get { return Container.Resolve<MediaInfo.IMediaInfoAdapter>(); }
        }

        /// <summary>
        /// 获取办公适配器。
        /// </summary>
        public Office.IOfficeAdapter Office
        {
            get { return Container.Resolve<Office.IOfficeAdapter>(); }
        }

        /// <summary>
        /// 获取拼音适配器。
        /// </summary>
        public Pinyin.IPinyinAdapter Pinyin
        {
            get { return Container.Resolve<Pinyin.IPinyinAdapter>(); }
        }

        /// <summary>
        /// 获取资源适配器。
        /// </summary>
        public Resource.IResourceAdapter Resource
        {
            get { return Container.Resolve<Resource.IResourceAdapter>(); }
        }
        
        /// <summary>
        /// 获取调度程序适配器。
        /// </summary>
        public Scheduler.ISchedulerAdapter Scheduler
        {
            get { return Container.Resolve<Scheduler.ISchedulerAdapter>(); }
        }

        /// <summary>
        /// 获取敏感词适配器。
        /// </summary>
        public SensitiveWord.ISensitiveWordAdapter SensitiveWord
        {
            get { return Container.Resolve<SensitiveWord.ISensitiveWordAdapter>(); }
        }

        /// <summary>
        /// 获取套接字适配器。
        /// </summary>
        public Socket.ISocketAdapter Socket
        {
            get { return Container.Resolve<Socket.ISocketAdapter>(); }
        }

        /// <summary>
        /// 获取线程适配器。
        /// </summary>
        public Thread.IThreadAdapter Thread
        {
            get { return Container.Resolve<Thread.IThreadAdapter>(); }
        }

        /// <summary>
        /// 获取 Windows 服务适配器。
        /// </summary>
        public WinService.IWinServiceAdapter WinService
        {
            get { return Container.Resolve<WinService.IWinServiceAdapter>(); }
        }

        #endregion

    }
}
