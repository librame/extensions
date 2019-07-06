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

namespace Librame.Extensions.Data
{
    using Core;

    /// <summary>
    /// 抽象标识（默认以字符串为标识类型）。
    /// </summary>
    public abstract class AbstractId : AbstractId<string>, IId
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractId"/> 默认实例。
        /// </summary>
        public AbstractId()
        {
            // 默认使用空标识符，新增推荐使用服务注入
            Id = GuIdentifier.Empty;
        }
    }


    /// <summary>
    /// 抽象标识。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    public abstract class AbstractId<TId> : IId<TId>
        where TId : IEquatable<TId>
    {
        /// <summary>
        /// 标识。
        /// </summary>
        [Display(Name = nameof(Id), GroupName = "GlobalGroup", ResourceType = typeof(AbstractEntityResource))]
        public virtual TId Id { get; set; }
    }
}
