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

namespace Librame.Extensions.Core.Utilities
{
    /// <summary>
    /// <see cref="Stopwatch"/> 实用工具。
    /// </summary>
    public static class StopwatchUtility
    {
        private static readonly Lazy<Stopwatch> _stopwatch
            = new Lazy<Stopwatch>(() => Stopwatch.StartNew());


        /// <summary>
        /// 运行方法集合。
        /// </summary>
        /// <param name="actions">给定的 <see cref="Action"/> 数组。</param>
        /// <returns>返回 <see cref="IEnumerable{TimeSpan}"/>。</returns>
        public static IEnumerable<TimeSpan> Run(params Action[] actions)
        {
            actions.NotEmpty(nameof(actions));

            _stopwatch.Value.Reset();

            foreach (var action in actions)
            {
                _stopwatch.Value.Start();

                action.Invoke();

                _stopwatch.Value.Stop();

                yield return _stopwatch.Value.Elapsed;
            }
        }

    }
}
