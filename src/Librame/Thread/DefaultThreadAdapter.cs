#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Amib.Threading;
using Amib.Threading.Internal;
using System;

namespace Librame.Thread
{
    /// <summary>
    /// 默认线程适配器。
    /// </summary>
    public class DefaultThreadAdapter : AbstractThreadAdapter, IThreadAdapter
    {
        private readonly SmartThreadPool _threads = null;

        /// <summary>
        /// 构造一个 <see cref="DefaultThreadAdapter"/> 实例。
        /// </summary>
        public DefaultThreadAdapter()
        {
            _threads = new SmartThreadPool();
        }


        /// <summary>
        /// 新增复制文件进程。
        /// </summary>
        /// <param name="sourceFileName">给定的源文件名。</param>
        /// <param name="destFileName">给定的目标文件名。</param>
        /// <returns>返回 <see cref="IWorkItemResult"/>。</returns>
        public virtual IWorkItemResult AddCopyFile(string sourceFileName, string destFileName)
        {
            // Queue an action (Fire and forget)
            return _threads.QueueWorkItem(System.IO.File.Copy, sourceFileName, destFileName);
        }


        /// <summary>
        /// 创建工作组。
        /// </summary>
        /// <param name="action">给定的操作方法。</param>
        public virtual void Build(Action<WorkItemsGroupBase> action)
        {
            action?.Invoke(_threads);
        }

    }
}
