#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Stores
{
    /// <summary>
    /// 创建时间周期数接口。
    /// </summary>
    /// <remarks>
    /// 主要用于解决日期与时间类型在不同数据库中 LINQ 查询的兼容性问题。
    /// </remarks>
    public interface ICreatedTimeTicks
    {
        /// <summary>
        /// 创建时间周期数。
        /// </summary>
        long CreatedTimeTicks { get; set; }
    }
}
