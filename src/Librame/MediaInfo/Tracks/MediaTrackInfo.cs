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
using System.Linq;

namespace Librame.MediaInfo.Tracks
{
    using Utility;

    /// <summary>
    /// 媒体轨道信息。
    /// </summary>
    public class MediaTrackInfo
    {
        /// <summary>
        /// 构造一个媒体信息对象。
        /// </summary>
        /// <param name="mediaFile">给定的媒体文件。</param>
        /// <param name="general">给定的概览轨道信息。</param>
        /// <param name="videos">给定的视频轨道信息数组。</param>
        /// <param name="audios">给定的音频轨道信息数组。</param>
        /// <param name="texts">给定的文本轨道信息数组。</param>
        /// <param name="others">给定的其它轨道信息数组。</param>
        /// <param name="images">给定的图像轨道信息数组。</param>
        /// <param name="menus">给定的菜单轨道信息数组。</param>
        public MediaTrackInfo(string mediaFile,
            GeneralTrackInfo general,
            VideoTrackInfo[] videos,
            AudioTrackInfo[] audios,
            TextTrackInfo[] texts,
            OtherTrackInfo[] others,
            ImageTrackInfo[] images,
            MenuTrackInfo[] menus)
        {
            MediaFile = mediaFile;
            General = general;
            Videos = videos;
            Audios = audios;
            Images = images;
            Texts = texts;
            Menus = menus;

            Initialize();
        }

        /// <summary>
        /// 初始化媒体信息。
        /// </summary>
        protected virtual void Initialize()
        {
            Name = Path.GetFileName(MediaFile);

            if (!ReferenceEquals(General, null))
            {
                FileSize = General.FileSize.AsOrDefault(s => Convert.ToInt64(s), 0);
                VideoCount = General.VideoCount.AsOrDefault(0);
                AudioCount = General.AudioCount.AsOrDefault(0);
                TextCount = General.TextCount.AsOrDefault(0);
                OtherCount = General.OtherCount.AsOrDefault(0);
                ImageCount = General.ImageCount.AsOrDefault(0);
                MenuCount = General.MenuCount.AsOrDefault(0);
            }
            
            Video = Videos?.FirstOrDefault();
            Audio = Audios?.FirstOrDefault();
            Text = Texts?.FirstOrDefault();
            Other = Others?.FirstOrDefault();
            Image = Images?.FirstOrDefault();
            Menu = Menus?.FirstOrDefault();
        }

        /// <summary>
        /// 媒体文件。
        /// </summary>
        public string MediaFile { get; private set; }
        /// <summary>
        /// 文件名。
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 文件大小。
        /// </summary>
        public long FileSize { get; private set; }

        /// <summary>
        /// 视轨数。
        /// </summary>
        public int VideoCount { get; private set; }
        /// <summary>
        /// 音轨数。
        /// </summary>
        public int AudioCount { get; private set; }
        /// <summary>
        /// 其它数。
        /// </summary>
        public int OtherCount { get; private set; }
        /// <summary>
        /// 文本数。
        /// </summary>
        public int TextCount { get; private set; }
        /// <summary>
        /// 图像数。
        /// </summary>
        public int ImageCount { get; private set; }
        /// <summary>
        /// 菜单数。
        /// </summary>
        public int MenuCount { get; private set; }


        /// <summary>
        /// 概览轨道信息。
        /// </summary>
        public GeneralTrackInfo General { get; private set; }

        /// <summary>
        /// 视频轨道信息数组。
        /// </summary>
        public VideoTrackInfo[] Videos { get; private set; }
        /// <summary>
        /// 默认的轨道信息数组。
        /// </summary>
        public VideoTrackInfo Video { get; private set; }

        /// <summary>
        /// 音频轨道信息数组。
        /// </summary>
        public AudioTrackInfo[] Audios { get; private set; }
        /// <summary>
        /// 默认的轨道信息数组。
        /// </summary>
        public AudioTrackInfo Audio { get; private set; }

        /// <summary>
        /// 其它轨道信息数组。
        /// </summary>
        public OtherTrackInfo[] Others { get; private set; }
        /// <summary>
        /// 默认的其它轨道信息。
        /// </summary>
        public OtherTrackInfo Other { get; private set; }

        /// <summary>
        /// 文本轨道信息数组。
        /// </summary>
        public TextTrackInfo[] Texts { get; private set; }
        /// <summary>
        /// 默认的文本轨道信息。
        /// </summary>
        public TextTrackInfo Text { get; private set; }

        /// <summary>
        /// 图像轨道信息数组。
        /// </summary>
        public ImageTrackInfo[] Images { get; private set; }
        /// <summary>
        /// 默认的图像轨道信息。
        /// </summary>
        public ImageTrackInfo Image { get; private set; }

        /// <summary>
        /// 菜单轨道信息数组。
        /// </summary>
        public MenuTrackInfo[] Menus { get; private set; }
        /// <summary>
        /// 默认的菜单轨道信息。
        /// </summary>
        public MenuTrackInfo Menu { get; private set; }

    }
}
