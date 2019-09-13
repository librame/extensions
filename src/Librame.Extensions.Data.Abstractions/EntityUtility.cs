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
    /// 实体工具。
    /// </summary>
    public class EntityUtility
    {
        /// <summary>
        /// 默认排序。
        /// </summary>
        public static readonly float DefaultRank
            = 10;

        /// <summary>
        /// 默认状态。
        /// </summary>
        public static readonly DataStatus DefaultStatus
            = DataStatus.Public;

        /// <summary>
        /// 默认时间。
        /// </summary>
        public static readonly DateTimeOffset DefaultTime
            = DateTimeOffset.UtcNow;


        /// <summary>
        /// 空标识。
        /// </summary>
        public static readonly string EmptyCombGuid
            = Guid.Empty.AsCombGuid(DefaultTime).ToString();


        /// <summary>
        /// 新标识。
        /// </summary>
        /// <param name="timestamp">给定的时间戳。</param>
        /// <returns>返回字符串。</returns>
        public static string NewCombGuid(DateTimeOffset timestamp)
            => Guid.NewGuid().AsCombGuid(timestamp).ToString();
    }
}
