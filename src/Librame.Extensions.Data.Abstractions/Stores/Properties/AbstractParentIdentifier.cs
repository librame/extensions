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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Stores
{
    using Core.Identifiers;
    using Data.Resources;

    /// <summary>
    /// 抽象父标识。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    [NotMapped]
    public abstract class AbstractParentIdentifier<TId> : AbstractIdentifier<TId>, IParentIdentifier<TId>
        where TId : IEquatable<TId>
    {
        /// <summary>
        /// 父标识。
        /// </summary>
        [Display(Name = nameof(ParentId), GroupName = "GlobalGroup", ResourceType = typeof(AbstractEntityResource))]
        public virtual TId ParentId { get; set; }


        /// <summary>
        /// 获取父标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task{TParentId}"/>。</returns>
        public Task<TId> GetParentIdAsync(CancellationToken cancellationToken = default)
            => cancellationToken.RunFactoryOrCancellationAsync(() => ParentId);

        Task<object> IParentIdentifier.GetParentIdAsync(CancellationToken cancellationToken)
            => cancellationToken.RunFactoryOrCancellationAsync(() => (object)ParentId);


        /// <summary>
        /// 设置父标识。
        /// </summary>
        /// <param name="parentId">给定的 <typeparamref name="TId"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        public virtual Task SetParentIdAsync(TId parentId, CancellationToken cancellationToken = default)
            => cancellationToken.RunActionOrCancellationAsync(() => ParentId = parentId);

        /// <summary>
        /// 设置父标识。
        /// </summary>
        /// <param name="obj">给定的父标识对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        public virtual Task SetParentIdAsync(object obj, CancellationToken cancellationToken = default)
        {
            var parentId = obj.CastTo<object, TId>(nameof(obj));

            return cancellationToken.RunActionOrCancellationAsync(() => ParentId = parentId);
        }

    }
}
