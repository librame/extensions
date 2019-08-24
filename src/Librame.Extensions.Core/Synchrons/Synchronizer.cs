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
using System.Threading;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 同步程序。
    /// </summary>
    /// <typeparam name="TManager">指定的同步管理器类型。</typeparam>
    /// <typeparam name="TReader">指定的同步读取器类型。</typeparam>
    /// <typeparam name="TWriter">指定的同步写入器类型。</typeparam>
    public class Synchronizer<TManager, TReader, TWriter> : ISynchronizer<TManager, TReader, TWriter>
        where TManager : class, ISyncManager, TReader, TWriter
        where TReader : ISyncReader
        where TWriter : ISyncWriter
    {
        private ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        private TManager _manager;


        /// <summary>
        /// 构造一个 <see cref="Synchronizer{TManager, TReader, TWriter}"/>。
        /// </summary>
        /// <param name="manager">给定的同步管理器。</param>
        public Synchronizer(TManager manager)
        {
            _manager = manager.NotNull(nameof(manager));
        }

        /// <summary>
        /// 同步读取。
        /// </summary>
        /// <param name="action">给定的 <see cref="Action{TReader}"/>。</param>
        public void Read(Action<TReader> action)
        {
            _lock.EnterReadLock();

            try
            {
                action?.Invoke(_manager);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        /// <summary>
        /// 同步可升级读取。
        /// </summary>
        /// <param name="action">给定的 <see cref="Action{TManager}"/>。</param>
        public void EnterUpgradeableReadLock(Action<TManager> action)
        {
            _lock.EnterUpgradeableReadLock();

            try
            {
                action?.Invoke(_manager);
            }
            finally
            {
                _lock.ExitUpgradeableReadLock();
            }
        }

        /// <summary>
        /// 同步写入。
        /// </summary>
        /// <param name="action">给定的 <see cref="Action{TWriter}"/>。</param>
        public void Write(Action<TWriter> action)
        {
            _lock.EnterWriteLock();

            try
            {
                action?.Invoke(_manager);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

    }
}
