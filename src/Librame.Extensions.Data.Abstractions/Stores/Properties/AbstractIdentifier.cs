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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Stores
{
    using Core.Identifiers;
    using Data.Resources;

    /// <summary>
    /// 抽象标识。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    [NotMapped]
    public abstract class AbstractIdentifier<TId> : IIdentifier<TId>
        where TId : IEquatable<TId>
    {
        /// <summary>
        /// 标识。
        /// </summary>
        [Display(Name = nameof(Id), GroupName = "GlobalGroup", ResourceType = typeof(AbstractEntityResource))]
        public virtual TId Id { get; set; }


        /// <summary>
        /// 获取标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task{TId}"/>。</returns>
        public Task<TId> GetIdAsync(CancellationToken cancellationToken = default)
            => cancellationToken.RunFactoryOrCancellationAsync(() => Id);

        Task<object> IIdentifier.GetIdAsync(CancellationToken cancellationToken)
            => cancellationToken.RunFactoryOrCancellationAsync(() => (object)Id);


        /// <summary>
        /// 设置标识。
        /// </summary>
        /// <param name="id">给定的 <typeparamref name="TId"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        public virtual Task SetIdAsync(TId id, CancellationToken cancellationToken = default)
            => cancellationToken.RunActionOrCancellationAsync(() => Id = id);

        /// <summary>
        /// 设置标识。
        /// </summary>
        /// <param name="obj">给定的标识对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        public virtual Task SetIdAsync(object obj, CancellationToken cancellationToken = default)
        {
            var id = obj.CastTo<object, TId>(nameof(obj));
            
            return cancellationToken.RunActionOrCancellationAsync(() => Id = id);
        }

    }
}
