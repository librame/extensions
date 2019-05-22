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
    /// 工作线程池接口。
    /// </summary>
    public interface IJobThreadPool : IDisposable
    {
        /// <summary>
        /// 是否已启用。
        /// </summary>
        bool Enabled { get; }

        /// <summary>
        /// 工作队列为空。
        /// </summary>
        bool JobsIsEmpty { get; }

        /// <summary>
        /// 工作队列数量。
        /// </summary>
        int JobsCount { get; }

        /// <summary>
        /// 线程集合数量。
        /// </summary>
        int ThreadsCount { get; }


        /// <summary>
        /// 增加一个工作。
        /// </summary>
        /// <param name="descriptor">给定的 <see cref="JobDescriptor"/>。</param>
        void AddJob(JobDescriptor descriptor);
    }
}
