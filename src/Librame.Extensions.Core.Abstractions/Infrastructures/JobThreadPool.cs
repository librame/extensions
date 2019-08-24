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
    public class JobThreadPool : AbstractDisposable
    {
        private readonly List<Thread> _threads;
        private readonly ConcurrentQueue<JobDescriptor> _jobs;

        
        /// <summary>
        /// 构造一个 <see cref="JobThreadPool"/>。
        /// </summary>
        public JobThreadPool()
            : this(null)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="JobThreadPool"/>。
        /// </summary>
        /// <param name="jobs">给定的 <see cref="IEnumerable{JobDescriptor}"/>。</param>
        public JobThreadPool(IEnumerable<JobDescriptor> jobs)
            : this(new ConcurrentQueue<JobDescriptor>(jobs))
        {
        }

        /// <summary>
        /// 构造一个 <see cref="JobThreadPool"/>。
        /// </summary>
        /// <param name="jobs">给定的 <see cref="ConcurrentQueue{JobDescriptor}"/>。</param>
        public JobThreadPool(ConcurrentQueue<JobDescriptor> jobs)
            : this(jobs, Environment.ProcessorCount + 2)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="JobThreadPool"/>。
        /// </summary>
        /// <param name="jobs">给定的 <see cref="ConcurrentQueue{JobDescriptor}"/>。</param>
        /// <param name="threadsCount">给定的线程数量（推荐使用 CPU 核心数+2，）。</param>
        public JobThreadPool(ConcurrentQueue<JobDescriptor> jobs, int threadsCount)
            : base()
        {
            _jobs = jobs ?? new ConcurrentQueue<JobDescriptor>();

            _threads = new List<Thread>(threadsCount);
            for (int i = 0; i < threadsCount; i++)
            {
                _threads.Add(new Thread(ExecuteJobsAction));
            }
        }


        /// <summary>
        /// 队列为空。
        /// </summary>
        public bool IsEmpty
            => _jobs.IsEmpty;

        /// <summary>
        /// 队列数量。
        /// </summary>
        public int Count
            => _jobs.Count;


        /// <summary>
        /// 增加工作。
        /// </summary>
        /// <param name="descriptor">给定的 <see cref="JobDescriptor"/>。</param>
        public void Add(JobDescriptor descriptor)
        {
            _jobs.Enqueue(descriptor);
        }


        /// <summary>
        /// 执行工作。
        /// </summary>
        public void Execute()
        {
            _threads.ForEach(thread => thread.Start());
        }

        private void ExecuteJobsAction()
        {
            while (_jobs.TryPeek(out JobDescriptor job) && job.Enabled)
            {
                if (_jobs.TryDequeue(out job))
                {
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
        /// 释放。
        /// </summary>
        public override void Dispose()
        {
            StopThreads();
            base.Dispose();
        }

        /// <summary>
        /// 停止线程池工作。
        /// </summary>
        private void StopThreads()
        {
            _threads.ForEach(thread =>
            {
                if (thread.ThreadState == ThreadState.Running)
                    thread.Abort();
            });
        }

    }
}
