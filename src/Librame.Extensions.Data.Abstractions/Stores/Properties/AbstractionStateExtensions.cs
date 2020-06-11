#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Stores
{
    /// <summary>
    /// 状态静态扩展。
    /// </summary>
    public static class AbstractionStateExtensions
    {
        /// <summary>
        /// 异步获取状态。
        /// </summary>
        /// <typeparam name="TStatus">指定的状态类型（兼容不支持枚举类型的实体框架）。</typeparam>
        /// <param name="state">给定的 <see cref="IState{TStatus}"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TStatus"/> （兼容整数、单双精度的排序字段）的异步操作。</returns>
        public static ValueTask<TStatus> GetStatusAsync<TStatus>(this IState<TStatus> state,
            CancellationToken cancellationToken = default)
            where TStatus : struct
        {
            state.NotNull(nameof(state));

            return cancellationToken.RunFactoryOrCancellationValueAsync(() => state.Status);
        }


        /// <summary>
        /// 异步设置状态。
        /// </summary>
        /// <typeparam name="TStatus">指定的状态类型（兼容不支持枚举类型的实体框架）。</typeparam>
        /// <param name="state">给定的 <see cref="IState{TStatus}"/>。</param>
        /// <param name="newStatusFactory">给定的新 <typeparamref name="TStatus"/> 工厂方法。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TStatus"/> （兼容整数、单双精度的排序字段）的异步操作。</returns>
        public static ValueTask<TStatus> SetStatusAsync<TStatus>(this IState<TStatus> state,
            Func<TStatus, TStatus> newStatusFactory, CancellationToken cancellationToken = default)
            where TStatus : struct
        {
            state.NotNull(nameof(state));
            newStatusFactory.NotNull(nameof(newStatusFactory));

            return cancellationToken.RunFactoryOrCancellationValueAsync(()
                => state.Status = newStatusFactory.Invoke(state.Status));
        }

        /// <summary>
        /// 异步设置状态。
        /// </summary>
        /// <typeparam name="TStatus">指定的状态类型（兼容不支持枚举类型的实体框架）。</typeparam>
        /// <param name="state">给定的 <see cref="IState{TStatus}"/>。</param>
        /// <param name="newStatus">给定的新 <typeparamref name="TStatus"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TStatus"/> （兼容整数、单双精度的排序字段）的异步操作。</returns>
        public static ValueTask<TStatus> SetStatusAsync<TStatus>(this IState<TStatus> state,
            TStatus newStatus, CancellationToken cancellationToken = default)
            where TStatus : struct
        {
            state.NotNull(nameof(state));

            return cancellationToken.RunFactoryOrCancellationValueAsync(()
                => state.Status = newStatus);
        }


        /// <summary>
        /// 异步设置对象状态。
        /// </summary>
        /// <param name="state">给定的 <see cref="IObjectState"/>。</param>
        /// <param name="newStatusFactory">给定的新对象状态工厂方法。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含状态（兼容不支持枚举类型的实体框架）的异步操作。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static async ValueTask<object> SetObjectStatusAsync(this IObjectState state,
            Func<object, object> newStatusFactory, CancellationToken cancellationToken = default)
        {
            state.NotNull(nameof(state));
            newStatusFactory.NotNull(nameof(newStatusFactory));

            var newStatus = await state.GetObjectStatusAsync(cancellationToken).ConfigureAndResultAsync();
            return await state.SetObjectStatusAsync(newStatusFactory.Invoke(newStatus), cancellationToken)
                .ConfigureAndResultAsync();
        }

    }
}
