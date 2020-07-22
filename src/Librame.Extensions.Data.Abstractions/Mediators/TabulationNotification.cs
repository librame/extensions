#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections.Generic;

namespace Librame.Extensions.Data.Mediators
{
    using Core.Mediators;

    /// <summary>
    /// 表格通知。
    /// </summary>
    /// <typeparam name="TTabulation">指定的表格类型。</typeparam>
    public class TabulationNotification<TTabulation> : INotificationIndication
        where TTabulation : class
    {
        /// <summary>
        /// 添加表格集合。
        /// </summary>
        public IReadOnlyList<TTabulation> Adds { get; set; }

        /// <summary>
        /// 更新表格集合。
        /// </summary>
        public IReadOnlyList<TTabulation> Updates { get; set; }

        /// <summary>
        /// 删除表格集合。
        /// </summary>
        public IReadOnlyList<TTabulation> Removes { get; set; }
    }
}
