#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.MediaInfo
{
    /// <summary>
    /// 媒体信息适配器接口。
    /// </summary>
    public interface IMediaInfoAdapter : Adaptation.IAdapter
    {
        /// <summary>
        /// 获取媒体信息引擎。
        /// </summary>
        Engines.IMediaInfoEngine Engine { get; }


        /// <summary>
        /// 分析媒体文件。
        /// </summary>
        /// <param name="mediaFilename">给定的媒体文件。</param>
        /// <returns>返回媒体轨道信息。</returns>
        Tracks.MediaTrackInfo Analyze(string mediaFilename);
    }
}
