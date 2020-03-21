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

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 可排序接口。
    /// </summary>
    public interface ISortable : IComparable<ISortable>
    {
        /// <summary>
        /// 排序优先级（数值越小越优先）。
        /// </summary>
        float Priority { get; set; }
    }
}
