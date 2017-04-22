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
using System.IO;

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

        /// <summary>
        /// 构造一个 <see cref="DefaultMediaInfoAdapter"/> 默认实例。
        /// </summary>
        public DefaultMediaInfoAdapter()
        {
            if (ReferenceEquals(_engine, null))
            {
                if (Environment.Is64BitProcess)
                    _engine = new X64MediaInfoEngine();
                else
                    _engine = new X86MediaInfoEngine();
            }
        }


        /// <summary>
        /// 获取媒体信息引擎。
        /// </summary>
        public virtual IMediaInfoEngine Engine
        {
            get { return _engine; }
        }

        
        /// <summary>
        /// 分析媒体文件。
        /// </summary>
        /// <param name="mediaFilename">给定的媒体文件。</param>
        /// <returns>返回媒体轨道信息。</returns>
        public virtual MediaTrackInfo Analyze(string mediaFilename)
        {
            mediaFilename.GuardNullOrEmpty(nameof(mediaFilename));
            if (!File.Exists(mediaFilename))
                throw new FileNotFoundException("file not found", mediaFilename);

            // 读取媒体文件
            Engine.Open(mediaFilename);

            // 获取概览信息（单轨）
            var general = MediaInfoHelper.GetSingleTrack(Engine, StreamKind.General, 0)
                .AsOrDefault(t => new GeneralTrackInfo(t));

            // 多轨信息
            var videos = MediaInfoHelper.GetMultiTracks(Engine, StreamKind.Video, general)
                .AsArrayOrDefault(c => new VideoTrackInfo(c));

            var audios = MediaInfoHelper.GetMultiTracks(Engine, StreamKind.Audio, general)
                .AsArrayOrDefault(c => new AudioTrackInfo(c));

            var images = MediaInfoHelper.GetMultiTracks(Engine, StreamKind.Image, general)
                .AsArrayOrDefault(c => new ImageTrackInfo(c));

            var texts = MediaInfoHelper.GetMultiTracks(Engine, StreamKind.Text, general)
                .AsArrayOrDefault(c => new TextTrackInfo(c));

            var chapters = MediaInfoHelper.GetMultiTracks(Engine, StreamKind.Chapters, general)
                .AsArrayOrDefault(c => new ChapterTrackInfo(c));

            return new MediaTrackInfo(mediaFilename, general, videos, audios, images, texts, chapters);
        }

    }
}
