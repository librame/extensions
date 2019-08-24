#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 同步程序接口。
    /// </summary>
    /// <typeparam name="TManager">指定的同步管理器类型。</typeparam>
    /// <typeparam name="TReader">指定的同步读取器类型。</typeparam>
    /// <typeparam name="TWriter">指定的同步写入器类型。</typeparam>
    public interface ISynchronizer<TManager, TReader, TWriter> : ISynchronizer
        where TManager : ISyncManager, TReader, TWriter
        where TReader : ISyncReader
        where TWriter : ISyncWriter
    {
        /// <summary>
        /// 同步读取。
        /// </summary>
        /// <param name="action">给定的 <see cref="Action{TReader}"/>。</param>
        void Read(Action<TReader> action);

        /// <summary>
        /// 同步可升级读取。
        /// </summary>
        /// <param name="action">给定的 <see cref="Action{TManager}"/>。</param>
        void EnterUpgradeableReadLock(Action<TManager> action);

        /// <summary>
        /// 同步写入。
        /// </summary>
        /// <param name="action">给定的 <see cref="Action{TWriter}"/>。</param>
        void Write(Action<TWriter> action);
    }


    /// <summary>
    /// 同步程序接口。
    /// </summary>
    public interface ISynchronizer
    {
    }
}
