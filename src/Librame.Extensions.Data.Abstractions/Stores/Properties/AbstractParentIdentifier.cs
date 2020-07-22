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
        /// 异步获取对象标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含标识（兼容各种引用与值类型标识）的异步操作。</returns>
        public virtual ValueTask<object> GetObjectParentIdAsync(CancellationToken cancellationToken)
            => cancellationToken.RunOrCancelValueAsync(() => (object)ParentId);

        /// <summary>
        /// 异步设置对象标识。
        /// </summary>
        /// <param name="newParentId">给定的新对象标识。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含标识（兼容各种引用与值类型标识）的异步操作。</returns>
        public virtual ValueTask<object> SetObjectParentIdAsync(object newParentId, CancellationToken cancellationToken = default)
        {
            var realNewParentId = newParentId.CastTo<object, TId>(nameof(newParentId));

            return cancellationToken.RunOrCancelValueAsync(() =>
            {
                ParentId = realNewParentId;
                return newParentId;
            });
        }


        /// <summary>
        /// 转换为标识键值对字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
            => $"{base.ToString()};{nameof(ParentId)}={ParentId}";

    }
}
