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
using System.Collections.Generic;
using System.Diagnostics;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 计时器实用工具。
    /// </summary>
    public static class StopwatchUtility
    {
        /// <summary>
        /// 运行方法集合。
        /// </summary>
        /// <param name="actions">给定的 <see cref="Action"/> 数组。</param>
        /// <returns>返回 <see cref="IEnumerable{TimeSpan}"/>。</returns>
        public static IEnumerable<TimeSpan> Run(params Action[] actions)
        {
            actions.NotNullOrEmpty(nameof(actions));

            var stopwatch = Stopwatch.StartNew();
            foreach (var action in actions)
            {
                stopwatch.Start();

                action.Invoke();

                stopwatch.Stop();

                yield return stopwatch.Elapsed;
            }
        }

    }
}
