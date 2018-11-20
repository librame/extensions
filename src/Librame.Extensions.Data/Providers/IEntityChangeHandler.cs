#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 实体变化处理程序。
    /// </summary>
    public interface IEntityChangeHandler
    {
        /// <summary>
        /// 处理实体。
        /// </summary>
        /// <param name="entry">给定的 <see cref="EntityEntry"/>。</param>
        void Process(EntityEntry entry);
    }
}
