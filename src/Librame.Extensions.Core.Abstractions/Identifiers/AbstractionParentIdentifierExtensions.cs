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
    /// 父标识符静态扩展。
    /// </summary>
    public static class AbstractionParentIdentifierExtensions
    {
        /// <summary>
        /// 异步设置父标识。
        /// </summary>
        /// <typeparam name="TParentId">指定的父标识类型（兼容各种引用与值类型标识）。</typeparam>
        /// <param name="parentIdentifier">给定的 <see cref="IParentIdentifier{TId}"/>。</param>
        /// <param name="newParentIdFactory">给定的新 <typeparamref name="TParentId"/> 工厂方法。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TParentId"/> （兼容各种引用与值类型标识）的异步操作。</returns>
        public static ValueTask<TParentId> SetParentIdAsync<TParentId>(this IParentIdentifier<TParentId> parentIdentifier,
            Func<TParentId, TParentId> newParentIdFactory, CancellationToken cancellationToken = default)
            where TParentId : IEquatable<TParentId>
        {
            parentIdentifier.NotNull(nameof(parentIdentifier));
            newParentIdFactory.NotNull(nameof(newParentIdFactory));

            return cancellationToken.RunOrCancelValueAsync(()
                => parentIdentifier.ParentId = newParentIdFactory.Invoke(parentIdentifier.ParentId));
        }


        /// <summary>
        /// 异步设置对象父标识。
        /// </summary>
        /// <param name="parentIdentifier">给定的 <see cref="IObjectParentIdentifier"/>。</param>
        /// <param name="newParentIdFactory">给定的新对象父标识工厂方法。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含父标识（兼容各种引用与值类型标识）的异步操作。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static async ValueTask<object> SetObjectParentIdAsync(this IObjectParentIdentifier parentIdentifier,
            Func<object, object> newParentIdFactory, CancellationToken cancellationToken = default)
        {
            parentIdentifier.NotNull(nameof(parentIdentifier));
            newParentIdFactory.NotNull(nameof(newParentIdFactory));

            var newParentId = await parentIdentifier.GetObjectParentIdAsync(cancellationToken).ConfigureAwait();
            return await parentIdentifier.SetObjectParentIdAsync(newParentIdFactory.Invoke(newParentId), cancellationToken)
                .ConfigureAwait();
        }

    }
}
