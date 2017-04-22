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

namespace Librame.MediaInfo
{
    using Engines;
    using Utility;

    /// <summary>
    /// 媒体信息助手。
    /// </summary>
    public class MediaInfoHelper
    {
        /// <summary>
        /// 获取单轨字典集合。
        /// </summary>
        /// <param name="engine">给定的 <see cref="IMediaInfoEngine"/>。</param>
        /// <param name="kind">给定的流种类。</param>
        /// <param name="index">给定的索引。</param>
        /// <returns>返回字典集合。</returns>
        public static IDictionary<string, string> GetSingleTrack(IMediaInfoEngine engine, StreamKind kind, int index)
        {
            var dictionary = new Dictionary<string, string>();

            int i = 0;
            while (true)
            {
                string key = engine.Get(kind, index, i++, InfoKind.Name);
                if (string.IsNullOrEmpty(key))
                    break;

                string value = engine.Get(kind, index, key);
                dictionary.Add(key, value);
            }

            return dictionary;
        }


        /// <summary>
        /// 获取多轨字典信息数组。
        /// </summary>
        /// <param name="engine">给定的 <see cref="IMediaInfoEngine"/>。</param>
        /// <param name="kind">给定的流种类。</param>
        /// <param name="general">给定的概览信息。</param>
        /// <returns>返回数组或 NULL。</returns>
        public static IDictionary<string, string>[] GetMultiTracks(IMediaInfoEngine engine, StreamKind kind, Tracks.GeneralTrackInfo general)
        {
            int count = 0;

            switch (kind)
            {
                case StreamKind.Video:
                    count = general.VideoCount.AsOrDefault(0);
                    break;

                case StreamKind.Audio:
                    count = general.AudioCount.AsOrDefault(0);
                    break;

                case StreamKind.Image:
                    count = general.ImageCount.AsOrDefault(0);
                    break;

                case StreamKind.Text:
                    count = general.TextCount.AsOrDefault(0);
                    break;

                case StreamKind.Chapters:
                    count = general.ChapterCount.AsOrDefault(0);
                    break;

                default:
                    break;
            }

            if (count < 1)
                return null;

            var list = new List<IDictionary<string, string>>();

            for (var i = 0; i < count; i++)
                list.Add(GetSingleTrack(engine, kind, i));

            return list.ToArray();
        }

    }
}
