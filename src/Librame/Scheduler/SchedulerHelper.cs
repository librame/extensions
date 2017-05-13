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
    using Utility;

    /// <summary>
    /// 调度程序助手。
    /// </summary>
    public class SchedulerHelper
    {
        /// <summary>
        /// 构建工作详情。
        /// </summary>
        /// <typeparam name="T">指定的工作类型。</typeparam>
        /// <param name="name">给定的工作名称。</param>
        /// <param name="group">给定的工作组。</param>
        /// <returns>返回 <see cref="IJobDetail"/>。</returns>
        public static IJobDetail BuildJob<T>(string name, string group)
            where T : IJob
        {
            return JobBuilder.Create<T>()
                .WithIdentity(name, group)
                .Build();
        }

        /// <summary>
        /// 构建工作详情。
        /// </summary>
        /// <typeparam name="T">指定的工作类型。</typeparam>
        /// <param name="key">给定的工作键。</param>
        /// <returns>返回 <see cref="IJobDetail"/>。</returns>
        public static IJobDetail BuildJob<T>(JobKey key)
            where T : IJob
        {
            return JobBuilder.Create<T>()
                .WithIdentity(key)
                .Build();
        }


        /// <summary>
        /// 构建触发器。
        /// </summary>
        /// <param name="name">给定的工作名称。</param>
        /// <param name="group">给定的工作组。</param>
        /// <param name="action">给定的操作方法。</param>
        /// <returns>返回 <see cref="ITrigger"/>。</returns>
        public static ITrigger BuildTrigger(string name, string group, Action<SimpleScheduleBuilder> action)
        {
            return TriggerBuilder.Create()
                .WithIdentity(name, group)
                .StartNow()
                .WithSimpleSchedule(action)
                .Build();
        }

        /// <summary>
        /// 构建触发器。
        /// </summary>
        /// <param name="key">给定的工作键。</param>
        /// <param name="action">给定的操作方法。</param>
        /// <returns>返回 <see cref="ITrigger"/>。</returns>
        public static ITrigger BuildTrigger(TriggerKey key, Action<SimpleScheduleBuilder> action)
        {
            return TriggerBuilder.Create()
                .WithIdentity(key)
                .StartNow()
                .WithSimpleSchedule(action)
                .Build();

            //x => x.WithIntervalInSeconds(40)
            //    .RepeatForever()
        }

    }
}
