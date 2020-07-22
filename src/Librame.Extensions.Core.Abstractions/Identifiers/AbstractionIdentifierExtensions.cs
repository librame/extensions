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

namespace Librame.Extensions.Core.Identifiers
{
    /// <summary>
    /// 标识符静态扩展。
    /// </summary>
    public static class AbstractionIdentifierExtensions
    {
        /// <summary>
        /// 异步设置标识。
        /// </summary>
        /// <typeparam name="TId">指定的标识类型（兼容各种引用与值类型标识）。</typeparam>
        /// <param name="identifier">给定的 <see cref="IIdentifier{TId}"/>。</param>
        /// <param name="newIdFactory">给定的新 <typeparamref name="TId"/> 工厂方法。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TId"/> （兼容各种引用与值类型标识）的异步操作。</returns>
        public static ValueTask<TId> SetIdAsync<TId>(this IIdentifier<TId> identifier,
            Func<TId, TId> newIdFactory, CancellationToken cancellationToken = default)
            where TId : IEquatable<TId>
        {
            identifier.NotNull(nameof(identifier));
            newIdFactory.NotNull(nameof(newIdFactory));

            return cancellationToken.RunOrCancelValueAsync(()
                => identifier.Id = newIdFactory.Invoke(identifier.Id));
        }


        /// <summary>
        /// 异步设置对象标识。
        /// </summary>
        /// <param name="identifier">给定的 <see cref="IObjectIdentifier"/>。</param>
        /// <param name="newIdFactory">给定的新对象标识工厂方法。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含标识（兼容各种引用与值类型标识）的异步操作。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static async ValueTask<object> SetObjectIdAsync(this IObjectIdentifier identifier,
            Func<object, object> newIdFactory, CancellationToken cancellationToken = default)
        {
            identifier.NotNull(nameof(identifier));
            newIdFactory.NotNull(nameof(newIdFactory));

            var newId = await identifier.GetObjectIdAsync(cancellationToken).ConfigureAwait();
            return await identifier.SetObjectIdAsync(newIdFactory.Invoke(newId), cancellationToken)
                .ConfigureAwait();
        }

    }
}
