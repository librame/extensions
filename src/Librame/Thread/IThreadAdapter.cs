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
    /// 线程插件接口。
    /// </summary>
    public interface IThreadAdapter : Adaptation.IAdapter
    {
        /// <summary>
        /// 新增复制文件进程。
        /// </summary>
        /// <param name="sourceFileName">给定的源文件名。</param>
        /// <param name="destFileName">给定的目标文件名。</param>
        /// <returns>返回 <see cref="IWorkItemResult"/>。</returns>
        IWorkItemResult AddCopyFile(string sourceFileName, string destFileName);


        /// <summary>
        /// 创建工作组。
        /// </summary>
        /// <param name="action">给定的操作方法。</param>
        void Build(Action<WorkItemsGroupBase> action);
    }
}
