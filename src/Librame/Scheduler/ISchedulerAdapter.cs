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
using System;

namespace Librame.Scheduler
{
    /// <summary>
    /// 调度程序适配器接口。
    /// </summary>
    public interface ISchedulerAdapter : Adaptation.IAdapter
    {
        /// <summary>
        /// 构建调度程序。
        /// </summary>
        /// <param name="delay">给定的延迟时间间隔（可选；默认立即启动）。</param>
        /// <returns>返回 <see cref="IScheduler"/>。</returns>
        IScheduler BuildScheduler(TimeSpan delay = default(TimeSpan));


        /// <summary>
        /// 新增作业。
        /// </summary>
        /// <param name="trigger">给定的触发器（可使用 <see cref="SchedulerHelper.BuildTrigger(TriggerKey, Action{SimpleScheduleBuilder})"/> 快速构建）。</param>
        /// <returns>返回 <see cref="DateTimeOffset"/>。</returns>
        DateTimeOffset AddJob(ITrigger trigger);

        /// <summary>
        /// 新增作业。
        /// </summary>
        /// <param name="job">给定的工作详情（可使用 <see cref="SchedulerHelper.BuildJob{T}(JobKey)"/> 快速构建）。</param>
        /// <param name="trigger">给定的触发器（可使用 <see cref="SchedulerHelper.BuildTrigger(TriggerKey, Action{SimpleScheduleBuilder})"/> 快速构建）。</param>
        /// <returns>返回 <see cref="DateTimeOffset"/>。</returns>
        DateTimeOffset AddJob(IJobDetail job, ITrigger trigger);
    }
}
