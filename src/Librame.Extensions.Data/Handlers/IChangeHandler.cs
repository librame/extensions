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
    using Builders;

    /// <summary>
    /// 变化处理程序接口。
    /// </summary>
    public interface IChangeHandler
    {
        /// <summary>
        /// 处理实体。
        /// </summary>
        /// <param name="entry">给定的 <see cref="EntityEntry"/>。</param>
        /// <param name="builderOptions">给定的 <see cref="DataBuilderOptions"/>。</param>
        void Process(EntityEntry entry, DataBuilderOptions builderOptions);
    }
}
