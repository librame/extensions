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
        /// 标识类型。
        /// </summary>
        [NotMapped]
        public virtual Type IdType
            => typeof(TId);


        /// <summary>
        /// 异步获取对象标识。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含标识（兼容各种引用与值类型标识）的异步操作。</returns>
        public virtual ValueTask<object> GetObjectIdAsync(CancellationToken cancellationToken)
            => cancellationToken.RunOrCancelValueAsync(() => (object)Id);

        /// <summary>
        /// 异步设置对象标识。
        /// </summary>
        /// <param name="newId">给定的新对象标识。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含标识（兼容各种引用与值类型标识）的异步操作。</returns>
        public virtual ValueTask<object> SetObjectIdAsync(object newId, CancellationToken cancellationToken = default)
        {
            var realNewId = newId.CastTo<object, TId>(nameof(newId));

            return cancellationToken.RunOrCancelValueAsync(() =>
            {
                Id = realNewId;
                return newId;
            });
        }


        /// <summary>
        /// 转换为字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
            => $"{nameof(Id)}={Id}";

    }
}
