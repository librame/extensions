#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 数据排序接口。
    /// </summary>
    public interface IDataRank
    {
        /// <summary>
        /// 数据排序。
        /// </summary>
        int DataRank { get; set; }
    }
}
