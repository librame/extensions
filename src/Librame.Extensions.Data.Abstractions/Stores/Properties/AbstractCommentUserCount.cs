#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Stores
{
    using Resources;

    /// <summary>
    /// 抽象评论用户计数。
    /// </summary>
    /// <typeparam name="TValue">指定的数值类型。</typeparam>
    public abstract class AbstractCommentUserCount<TValue> : AbstractUserCount<TValue>, ICommentUserCount<TValue>
        where TValue : struct
    {
        /// <summary>
        /// 评论次数。
        /// </summary>
        [Display(Name = nameof(CommentCount), ResourceType = typeof(AbstractEntityResource))]
        public virtual TValue CommentCount { get; set; }

        /// <summary>
        /// 评论人数。
        /// </summary>
        [Display(Name = nameof(CommenterCount), ResourceType = typeof(AbstractEntityResource))]
        public virtual TValue CommenterCount { get; set; }


        /// <summary>
        /// 异步累加评论次数。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{TValue}"/>。</returns>
        public virtual async ValueTask<TValue> ProgressiveCommentCountAsync(CancellationToken cancellationToken = default)
        {
            CommentCount = await ProgressiveCountAsync(CommentCount, cancellationToken).ConfigureAndResultAsync();
            return CommentCount;
        }

        /// <summary>
        /// 异步累加评论人数。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{TValue}"/>。</returns>
        public virtual async ValueTask<TValue> ProgressiveCommenterCountAsync(CancellationToken cancellationToken = default)
        {
            CommenterCount = await ProgressiveCountAsync(CommenterCount, cancellationToken).ConfigureAndResultAsync();
            return CommenterCount;
        }

    }
}
