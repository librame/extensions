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

namespace Librame.MediaInfo
{
    using Engines;
    using Tracks;
    using Utility;

    /// <summary>
    /// 默认媒体信息适配器。
    /// </summary>
    public class DefaultMediaInfoAdapter : AbstarctMediaInfoAdapter, IMediaInfoAdapter
    {
        private readonly IMediaInfoEngine _engine = null;
        private readonly IMediaInfoListEngine _listEngine = null;

        /// <summary>
        /// 构造一个 <see cref="DefaultMediaInfoAdapter"/> 默认实例。
        /// </summary>
        public DefaultMediaInfoAdapter()
        {
            if (ReferenceEquals(_engine, null))
            {
                if (Environment.Is64BitProcess)
                {
                    _engine = new X64MediaInfoEngine();
                    _listEngine = new X64MediaInfoListEngine();
                }
                else
                {
                    _engine = new X86MediaInfoEngine();
                    _listEngine = new X86MediaInfoListEngine();
                }
            }
        }


        /// <summary>
        /// 获取媒体信息引擎。
        /// </summary>
        public IMediaInfoEngine Engine
        {
            get { return _engine; }
        }

        /// <summary>
        /// 获取媒体信息列表引擎。
        /// </summary>
        public IMediaInfoListEngine ListEngine
        {
            get { return _listEngine; }
        }

        
        /// <summary>
        /// 分析媒体文件。
        /// </summary>
        /// <param name="mediaFileName">给定的媒体文件。</param>
        /// <returns>返回媒体轨道信息。</returns>
        public virtual MediaTrackInfo Analyze(string mediaFileName)
        {
            // 读取媒体文件
            Engine.Open(mediaFileName.FileExists());

            // 概览信息（单轨）
            var general = Engine.GetTrack(StreamKind.General, 0)
                .AsOrDefault(c => new GeneralTrackInfo(c));

            // 多轨信息
            var videos = Engine.GetTracks(StreamKind.Video)
                .AsArrayOrDefault(c => new VideoTrackInfo(c));

            var audios = Engine.GetTracks(StreamKind.Audio)
                .AsArrayOrDefault(c => new AudioTrackInfo(c));

            var texts = Engine.GetTracks(StreamKind.Text)
                .AsArrayOrDefault(c => new TextTrackInfo(c));

            var others = Engine.GetTracks(StreamKind.Other)
                .AsArrayOrDefault(c => new OtherTrackInfo(c));

            var images = Engine.GetTracks(StreamKind.Image)
                .AsArrayOrDefault(c => new ImageTrackInfo(c));
            
            var menus = Engine.GetTracks(StreamKind.Menu)
                .AsArrayOrDefault(c => new MenuTrackInfo(c));

            return new MediaTrackInfo(mediaFileName, general,
                videos, audios, texts, others, images, menus);
        }

    }
}
