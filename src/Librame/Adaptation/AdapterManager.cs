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
    /// 适配器管理器。
    /// </summary>
    public class AdapterManager : IAdapterManager
    {
        /// <summary>
        /// 获取容器适配器。
        /// </summary>
        public Container.IContainerAdapter ContainerAdapter { get; }

        /// <summary>
        /// 获取日志适配器。
        /// </summary>
        public Logging.ILoggingAdapter LoggingAdapter { get; }


        /// <summary>
        /// 构造一个 <see cref="AdapterManager"/> 实例。
        /// </summary>
        /// <exception cref="System.ArgumentNullException">
        /// containerAdapter 或 loggingAdapter 为空。
        /// </exception>
        /// <param name="containerAdapter">给定的容器适配器。</param>
        /// <param name="loggingAdapter">给定的日志适配器。</param>
        public AdapterManager(Container.IContainerAdapter containerAdapter, Logging.ILoggingAdapter loggingAdapter)
        {
            ContainerAdapter = containerAdapter.NotNull(nameof(containerAdapter));
            LoggingAdapter = loggingAdapter.NotNull(nameof(loggingAdapter));
        }


        #region Container Settings

        /// <summary>
        /// 获取首选项。
        /// </summary>
        public AdapterSettings Settings
        {
            get { return ContainerAdapter.Resolve<AdapterSettings>(); }
        }

        /// <summary>
        /// 算法首选项。
        /// </summary>
        public Algorithm.AlgorithmSettings AlgorithmSettings
        {
            get { return ContainerAdapter.Resolve<Algorithm.AlgorithmSettings>(); }
        }

        /// <summary>
        /// 数据首选项。
        /// </summary>
        public Data.DataSettings DataSettings
        {
            get { return ContainerAdapter.Resolve<Data.DataSettings>(); }
        }

        /// <summary>
        /// 窗体首选项。
        /// </summary>
        public Forms.FormsSettings FormsSettings
        {
            get { return ContainerAdapter.Resolve<Forms.FormsSettings>(); }
        }

        #endregion


        #region Container Adapters
        
        /// <summary>
        /// 获取算法适配器。
        /// </summary>
        public Algorithm.IAlgorithmAdapter AlgorithmAdapter
        {
            get { return ContainerAdapter.Resolve<Algorithm.IAlgorithmAdapter>(); }
        }

        /// <summary>
        /// 获取认证适配器。
        /// </summary>
        public Authorization.IAuthorizeAdapter AuthorizationAdapter
        {
            get { return ContainerAdapter.Resolve<Authorization.IAuthorizeAdapter>(); }
        }

        /// <summary>
        /// 获取压缩适配器。
        /// </summary>
        public Compression.ICompressionAdapter CompressionAdapter
        {
            get { return ContainerAdapter.Resolve<Compression.ICompressionAdapter>(); }
        }

        /// <summary>
        /// 获取数据适配器。
        /// </summary>
        public Data.IDataAdapter DataAdapter
        {
            get { return ContainerAdapter.Resolve<Data.IDataAdapter>(); }
        }

        /// <summary>
        /// 获取窗体适配器。
        /// </summary>
        public Forms.IFormsAdapter FormsAdapter
        {
            get { return ContainerAdapter.Resolve<Forms.IFormsAdapter>(); }
        }

        /// <summary>
        /// 获取全文检索适配器。
        /// </summary>
        public FulltextSearch.IFulltextSearchAdapter FulltextSearchAdapter
        {
            get { return ContainerAdapter.Resolve<FulltextSearch.IFulltextSearchAdapter>(); }
        }

        /// <summary>
        /// 获取 HTML 适配器。
        /// </summary>
        public Html.IHtmlAdapter HtmlAdapter
        {
            get { return ContainerAdapter.Resolve<Html.IHtmlAdapter>(); }
        }

        /// <summary>
        /// 获取 HTTP 适配器。
        /// </summary>
        public Http.IHttpAdapter HttpAdapter
        {
            get { return ContainerAdapter.Resolve<Http.IHttpAdapter>(); }
        }

        /// <summary>
        /// 获取媒体信息适配器。
        /// </summary>
        public MediaInfo.IMediaInfoAdapter MediaInfoAdapter
        {
            get { return ContainerAdapter.Resolve<MediaInfo.IMediaInfoAdapter>(); }
        }

        /// <summary>
        /// 获取办公适配器。
        /// </summary>
        public Office.IOfficeAdapter OfficeAdapter
        {
            get { return ContainerAdapter.Resolve<Office.IOfficeAdapter>(); }
        }

        /// <summary>
        /// 获取拼音适配器。
        /// </summary>
        public Pinyin.IPinyinAdapter PinyinAdapter
        {
            get { return ContainerAdapter.Resolve<Pinyin.IPinyinAdapter>(); }
        }

        /// <summary>
        /// 获取资源适配器。
        /// </summary>
        public Resource.IResourceAdapter ResourceAdapter
        {
            get { return ContainerAdapter.Resolve<Resource.IResourceAdapter>(); }
        }
        
        /// <summary>
        /// 获取调度程序适配器。
        /// </summary>
        public Scheduler.ISchedulerAdapter SchedulerAdapter
        {
            get { return ContainerAdapter.Resolve<Scheduler.ISchedulerAdapter>(); }
        }

        /// <summary>
        /// 获取套接字适配器。
        /// </summary>
        public Socket.ISocketAdapter SocketAdapter
        {
            get { return ContainerAdapter.Resolve<Socket.ISocketAdapter>(); }
        }

        /// <summary>
        /// 获取线程适配器。
        /// </summary>
        public Thread.IThreadAdapter ThreadAdapter
        {
            get { return ContainerAdapter.Resolve<Thread.IThreadAdapter>(); }
        }

        /// <summary>
        /// 获取 Windows 服务适配器。
        /// </summary>
        public WinService.IWinServiceAdapter WinServiceAdapter
        {
            get { return ContainerAdapter.Resolve<WinService.IWinServiceAdapter>(); }
        }

        #endregion

    }
}
