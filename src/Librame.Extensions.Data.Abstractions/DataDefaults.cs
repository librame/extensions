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

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 数据默认值集合。
    /// </summary>
    public static class DataDefaults
    {
        /// <summary>
        /// 默认排序。
        /// </summary>
        public static readonly float Rank
            = 10;

        /// <summary>
        /// 默认状态。
        /// </summary>
        public static readonly DataStatus Status
            = DataStatus.Public;

        /// <summary>
        /// 默认 UTC 当前时间偏移。
        /// </summary>
        public static readonly DateTimeOffset UtcNowOffset
            = DateTimeOffset.UtcNow;


        /// <summary>
        /// 空标识。
        /// </summary>
        public static readonly string EmptyCombGuid
            = Guid.Empty.AsCombGuid(UtcNowOffset).ToString();
    }
}
