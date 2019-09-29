#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Librame.Extensions
{
    /// <summary>
    /// 任务静态扩展。
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        /// 异步配置并等待。
        /// </summary>
        /// <param name="task">给定的 <see cref="Task"/>。</param>
        /// <returns>返回 <see cref="ConfiguredTaskAwaitable"/>。</returns>
        public static ConfiguredTaskAwaitable ConfigureAndWaitAsync(this Task task)
        {
            if (task.IsNull()) return default;
            return task.ConfigureAwait(false);
        }

        /// <summary>
        /// 异步配置并返回结果。
        /// </summary>
        /// <typeparam name="TResult">指定的类型。</typeparam>
        /// <param name="task">给定的 <see cref="Task{TResult}"/>。</param>
        /// <returns>返回 <see cref="ConfiguredTaskAwaitable{TResult}"/>。</returns>
        public static ConfiguredTaskAwaitable<TResult> ConfigureAndResultAsync<TResult>(this Task<TResult> task)
        {
            if (task.IsNull()) return default;
            return task.ConfigureAwait(true);
        }


        /// <summary>
        /// 配置并等待。
        /// </summary>
        /// <param name="task">给定的 <see cref="Task"/>。</param>
        public static void ConfigureAndWait(this Task task)
            => task?.ConfigureAwait(false).GetAwaiter().GetResult();

        /// <summary>
        /// 配置并返回结果。
        /// </summary>
        /// <typeparam name="TResult">指定的类型。</typeparam>
        /// <param name="task">给定的 <see cref="Task{TResult}"/>。</param>
        /// <returns>返回 <typeparamref name="TResult"/>。</returns>
        public static TResult ConfigureAndResult<TResult>(this Task<TResult> task)
        {
            if (task.IsNull()) return default;
            return task.ConfigureAwait(true).GetAwaiter().GetResult();
        }


        #region ValueTask

        /// <summary>
        /// 异步配置并等待。
        /// </summary>
        /// <param name="valueTask">给定的 <see cref="ValueTask"/>。</param>
        /// <returns>返回 <see cref="ConfiguredValueTaskAwaitable"/>。</returns>
        public static ConfiguredValueTaskAwaitable ConfigureAndWaitAsync(this ValueTask valueTask)
        {
            if (valueTask.IsNull()) return default;
            return valueTask.ConfigureAwait(false);
        }

        /// <summary>
        /// 异步配置并返回结果。
        /// </summary>
        /// <typeparam name="TResult">指定的类型。</typeparam>
        /// <param name="valueTask">给定的 <see cref="ValueTask{TResult}"/>。</param>
        /// <returns>返回 <see cref="ConfiguredValueTaskAwaitable{TResult}"/>。</returns>
        public static ConfiguredValueTaskAwaitable<TResult> ConfigureAndResultAsync<TResult>(this ValueTask<TResult> valueTask)
        {
            if (valueTask.IsNull()) return default;
            return valueTask.ConfigureAwait(true);
        }


        /// <summary>
        /// 配置并等待。
        /// </summary>
        /// <param name="valueTask">给定的 <see cref="ValueTask"/>。</param>
        public static void ConfigureAndWait(this ValueTask valueTask)
        {
            if (valueTask.IsNull()) return;
            valueTask.ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 配置并返回结果。
        /// </summary>
        /// <typeparam name="TResult">指定的类型。</typeparam>
        /// <param name="valueTask">给定的 <see cref="ValueTask{TResult}"/>。</param>
        /// <returns>返回 <typeparamref name="TResult"/>。</returns>
        public static TResult ConfigureAndResult<TResult>(this ValueTask<TResult> valueTask)
        {
            if (valueTask.IsNull()) return default;
            return valueTask.ConfigureAwait(true).GetAwaiter().GetResult();
        }

        #endregion

    }
}
