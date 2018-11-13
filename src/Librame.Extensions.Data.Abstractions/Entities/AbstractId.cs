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
