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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 工作线程池。
    /// </summary>
    public class JobThreadPool : AbstractDispose<JobThreadPool>, IJobThreadPool
    {
        private List<Thread> _threads;
        private ConcurrentQueue<JobDescriptor> _jobs;

        
        /// <summary>
        /// 构造一个 <see cref="JobThreadPool"/> 实例。
        /// </summary>
        public JobThreadPool()
            : this(null)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="JobThreadPool"/> 实例。
        /// </summary>
        /// <param name="jobs">给定的 <see cref="ConcurrentQueue{JobDescriptor}"/>。</param>
        public JobThreadPool(ConcurrentQueue<JobDescriptor> jobs)
            : this(jobs, Environment.ProcessorCount + 2)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="JobThreadPool"/> 实例。
        /// </summary>
        /// <param name="threadsCount">给定的线程数量（推荐使用 CPU 核心数+2，）。</param>
        public JobThreadPool(int threadsCount)
            : this(null, threadsCount)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="JobThreadPool"/> 实例。
        /// </summary>
        /// <param name="jobs">给定的 <see cref="ConcurrentQueue{JobDescriptor}"/>。</param>
        /// <param name="threadsCount">给定的线程数量（推荐使用 CPU 核心数+2，）。</param>
        public JobThreadPool(ConcurrentQueue<JobDescriptor> jobs, int threadsCount)
            : base()
        {
            Enabled = true;

            _threads = new List<Thread>(threadsCount);
            _jobs = jobs ?? new ConcurrentQueue<JobDescriptor>();

            for (int i = 0; i < threadsCount; i++)
            {
                var t = new Thread(ExecuteJobs);
                _threads.Add(t);
                t.Start();
            }
        }


        /// <summary>
        /// 是否已启用。
        /// </summary>
        public bool Enabled { get; private set; }

        /// <summary>
        /// 工作队列为空。
        /// </summary>
        public bool JobsIsEmpty => _jobs.IsEmpty;

        /// <summary>
        /// 工作队列数量。
        /// </summary>
        public int JobsCount => _jobs.Count;

        /// <summary>
        /// 线程集合数量。
        /// </summary>
        public int ThreadsCount => _threads.Count;


        /// <summary>
        /// 如果已释放则抛出异常。
        /// </summary>
        protected override void ThrowIfDisposed()
        {
            if (Enabled)
                base.ThrowIfDisposed();
        }


        private void ExecuteJobs()
        {
            while (true && _jobs.IsNotNull() && Enabled)
            {
                JobDescriptor job = null;

                if ((_jobs?.TryDequeue(out job)).Value)
                {
                    if (job.IsNull())
                    {
                        Thread.Sleep(10);
                        continue;
                    }

                    var thread = Thread.CurrentThread;

                    try
                    {
                        job.Execution?.Invoke(thread, job.Parameters);
                    }
                    catch (Exception error)
                    {
                        job.ErrorCallback?.Invoke(thread, job.Parameters, error);
                    }
                    finally
                    {
                        job.FinishCallback?.Invoke(thread, job.Parameters);
                    }
                }
            }
        }


        /// <summary>
        /// 增加一个工作。
        /// </summary>
        /// <param name="descriptor">给定的 <see cref="JobDescriptor"/>。</param>
        public void AddJob(JobDescriptor descriptor)
        {
            ThrowIfDisposed();

            _jobs?.Enqueue(descriptor);
        }


        /// <summary>
        /// 释放。
        /// </summary>
        public override void Dispose()
        {
            ThrowIfDisposed();

            Clear();
            base.Dispose();
        }


        /// <summary>
        /// 清除。
        /// </summary>
        private void Clear()
        {
            Enabled = false;

            _jobs = null;

            if (_threads.IsNotNull())
            {
                foreach (var t in _threads)
                {
                    // 强制线程退出会抛异常
                    t.Join(); //t.Abort();
                }

                _threads = null;
            }
        }
    }
}
