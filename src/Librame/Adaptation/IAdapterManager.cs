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
    /// <summary>
    /// 适配器管理器接口。
    /// </summary>
    public interface IAdapterManager
    {
        /// <summary>
        /// 获取容器适配器。
        /// </summary>
        Container.IContainerAdapter Container { get; }
        
        /// <summary>
        /// 获取日志适配器。
        /// </summary>
        Logging.ILoggingAdapter Logging { get; }


        #region Container Settings

        /// <summary>
        /// 获取首选项。
        /// </summary>
        AdapterSettings Settings { get; }

        /// <summary>
        /// 算法首选项。
        /// </summary>
        Algorithm.AlgorithmSettings AlgorithmSettings { get; }

        /// <summary>
        /// 认证首选项。
        /// </summary>
        Authorization.AuthorizeSettings AuthorizeSettings { get; }

        /// <summary>
        /// 数据首选项。
        /// </summary>
        Data.DataSettings DataSettings { get; }

        /// <summary>
        /// 窗体首选项。
        /// </summary>
        Forms.FormsSettings FormsSettings { get; }

        #endregion


        #region Container Adapters

        /// <summary>
        /// 获取算法适配器。
        /// </summary>
        Algorithm.IAlgorithmAdapter Algorithm { get; }

        /// <summary>
        /// 获取认证适配器。
        /// </summary>
        Authorization.IAuthorizeAdapter Authorization { get; }

        /// <summary>
        /// 获取压缩适配器。
        /// </summary>
        Compression.ICompressionAdapter Compression { get; }

        /// <summary>
        /// 获取数据适配器。
        /// </summary>
        Data.IDataAdapter Data { get; }

        /// <summary>
        /// 获取窗体适配器。
        /// </summary>
        Forms.IFormsAdapter Forms { get; }

        /// <summary>
        /// 获取全文检索适配器。
        /// </summary>
        FulltextSearch.IFulltextSearchAdapter FulltextSearch { get; }

        /// <summary>
        /// 获取 HTML 适配器。
        /// </summary>
        Html.IHtmlAdapter Html { get; }

        /// <summary>
        /// 获取 HTTP 适配器。
        /// </summary>
        Http.IHttpAdapter Http { get; }

        /// <summary>
        /// 获取媒体信息适配器。
        /// </summary>
        MediaInfo.IMediaInfoAdapter MediaInfo { get; }

        /// <summary>
        /// 获取办公适配器。
        /// </summary>
        Office.IOfficeAdapter Office { get; }

        /// <summary>
        /// 获取拼音适配器。
        /// </summary>
        Pinyin.IPinyinAdapter Pinyin { get; }

        /// <summary>
        /// 获取资源适配器。
        /// </summary>
        Resource.IResourceAdapter Resource { get; }

        /// <summary>
        /// 获取调度程序适配器。
        /// </summary>
        Scheduler.ISchedulerAdapter Scheduler { get; }

        /// <summary>
        /// 获取敏感词适配器。
        /// </summary>
        SensitiveWord.ISensitiveWordAdapter SensitiveWord { get; }

        /// <summary>
        /// 获取套接字适配器。
        /// </summary>
        Socket.ISocketAdapter Socket { get; }

        /// <summary>
        /// 获取线程适配器。
        /// </summary>
        Thread.IThreadAdapter Thread { get; }

        /// <summary>
        /// 获取 Windows 服务适配器。
        /// </summary>
        WinService.IWinServiceAdapter WinService { get; }

        #endregion

    }
}
