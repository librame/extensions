#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Quartz;
using Quartz.Impl;
using System;

namespace Librame.Scheduler
{
    /// <summary>
    /// 默认调度程序适配器。
    /// </summary>
    public class DefaultSchedulerAdapter : AbstractScheduleAdapter, ISchedulerAdapter
    {
        private readonly IScheduler _scheduler = null;

        /// <summary>
        /// 构造一个 <see cref="DefaultSchedulerAdapter"/> 实例。
        /// </summary>
        public DefaultSchedulerAdapter()
        {
            InitializeAdapter();

            _scheduler = BuildScheduler();
        }

        /// <summary>
        /// 初始化适配器。
        /// </summary>
        protected virtual void InitializeAdapter()
        {
            // 导出配置文件
            ExportConfigDirectory("job_scheduling_data_2_0.xsd");
        }


        /// <summary>
        /// 构建调度程序。
        /// </summary>
        /// <param name="delay">给定的延迟时间间隔（可选；默认立即启动）。</param>
        /// <returns>返回 <see cref="IScheduler"/>。</returns>
        public virtual IScheduler BuildScheduler(TimeSpan delay = default(TimeSpan))
        {
            var scheduler = CreateScheduler();

            if (ReferenceEquals(delay, default(TimeSpan)))
                scheduler.Start();
            else
                scheduler.StartDelayed(delay);

            return scheduler;
        }


        /// <summary>
        /// 创建调度程序。
        /// </summary>
        /// <returns>返回 <see cref="IScheduler"/>。</returns>
        protected virtual IScheduler CreateScheduler()
        {
            ISchedulerFactory factory = new StdSchedulerFactory();

            return factory.GetScheduler();
        }


        /// <summary>
        /// 新增作业。
        /// </summary>
        /// <param name="trigger">给定的触发器（可使用 <see cref="SchedulerHelper.BuildTrigger(TriggerKey, Action{SimpleScheduleBuilder})"/> 快速构建）。</param>
        /// <returns>返回 <see cref="DateTimeOffset"/>。</returns>
        public virtual DateTimeOffset AddJob(ITrigger trigger)
        {
            return _scheduler.ScheduleJob(trigger);
        }

        /// <summary>
        /// 新增作业。
        /// </summary>
        /// <param name="job">给定的工作详情（可使用 <see cref="SchedulerHelper.BuildJob{T}(JobKey)"/> 快速构建）。</param>
        /// <param name="trigger">给定的触发器（可使用 <see cref="SchedulerHelper.BuildTrigger(TriggerKey, Action{SimpleScheduleBuilder})"/> 快速构建）。</param>
        /// <returns>返回 <see cref="DateTimeOffset"/>。</returns>
        public virtual DateTimeOffset AddJob(IJobDetail job, ITrigger trigger)
        {
            return _scheduler.ScheduleJob(job, trigger);
        }

    }
}
