#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Logging;

namespace Librame.Extensions.Data
{
    using Core;

    /// <summary>
    /// 存储初始化器接口。
    /// </summary>
    public interface IStoreInitializer
    {
        /// <summary>
        /// 时钟服务。
        /// </summary>
        IClockService Clock { get; }

        /// <summary>
        /// 存储标识符。
        /// </summary>
        IStoreIdentifier Identifier { get; }

        /// <summary>
        /// 日志工厂。
        /// </summary>
        ILoggerFactory LoggerFactory { get; }


        /// <summary>
        /// 是否已完成初始化。
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// 需要保存变化。
        /// </summary>
        bool RequiredSaveChanges { get; }
    }
}
