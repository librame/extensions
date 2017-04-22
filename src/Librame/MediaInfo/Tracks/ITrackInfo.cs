#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections.Generic;

namespace Librame.MediaInfo.Tracks
{
    /// <summary>
    /// 轨道信息接口。
    /// </summary>
    public interface ITrackInfo
    {
        /// <summary>
        /// 原始轨道集合。
        /// </summary>
        IDictionary<string, string> RawTracks { get; }
    }
}
