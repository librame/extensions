#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.IO;
using System.Text;

namespace Librame.MediaInfo
{
    using Tracks;
    using Utility;

    /// <summary>
    /// 媒体信息助手。
    /// </summary>
    public class MediaInfoHelper
    {
        /// <summary>
        /// 导出为文本文件。
        /// </summary>
        /// <param name="info">给定的媒体轨道信息。</param>
        /// <param name="fileName">给定要导出的文件名。</param>
        /// <param name="encoding">给定的字符编码。</param>
        public static void Export(MediaTrackInfo info, string fileName, Encoding encoding)
        {
            using (var fs = new FileStream(fileName, FileMode.Create))
            {
                using (var sw = new StreamWriter(fs, encoding))
                {
                    // 概览（单轨）
                    WriteGeneral(sw, info.General);

                    // 视频（多轨）
                    WriteVideos(sw, info.Videos);

                    // 音频（多轨）
                    WriteAudios(sw, info.Audios);

                    // 文本（多轨）
                    WriteTexts(sw, info.Texts);

                    // 其它（多轨）
                    WriteOthers(sw, info.Others);

                    // 图像（多轨）
                    WriteImages(sw, info.Images);

                    // 菜单（多轨）
                    WriteMenus(sw, info.Menus);
                }
            }
        }

        private static void WriteGeneral(StreamWriter sw, GeneralTrackInfo general)
        {
            if (general == null)
                return;

            sw.WriteLine("General");

            general.RawTracks.Invoke(pair =>
            {
                sw.WriteLine(string.Format("{0}: {1}", pair.Key, pair.Value));
            });
        }

        private static void WriteVideos(StreamWriter sw, VideoTrackInfo[] videos)
        {
            if (videos == null)
                return;

            sw.WriteLine("Videos");

            videos.Invoke((v, i) =>
            {
                sw.WriteLine("Video" + i);

                if (v != null)
                {
                    v.RawTracks.Invoke(pair =>
                    {
                        sw.WriteLine(string.Format("{0}: {1}", pair.Key, pair.Value));
                    });
                }
            });
        }

        private static void WriteAudios(StreamWriter sw, AudioTrackInfo[] audios)
        {
            if (audios == null)
                return;

            sw.WriteLine("Audios");

            audios.Invoke((v, i) =>
            {
                sw.WriteLine("Audio" + i);

                if (v != null)
                {
                    v.RawTracks.Invoke(pair =>
                    {
                        sw.WriteLine(string.Format("{0}: {1}", pair.Key, pair.Value));
                    });
                }
            });
        }

        private static void WriteTexts(StreamWriter sw, TextTrackInfo[] texts)
        {
            if (texts == null)
                return;

            sw.WriteLine("Texts");

            texts.Invoke((v, i) =>
            {
                sw.WriteLine("Text" + i);

                if (v != null)
                {
                    v.RawTracks.Invoke(pair =>
                    {
                        sw.WriteLine(string.Format("{0}: {1}", pair.Key, pair.Value));
                    });
                }
            });
        }

        private static void WriteOthers(StreamWriter sw, OtherTrackInfo[] others)
        {
            if (others == null)
                return;

            sw.WriteLine("Others");

            others.Invoke((v, i) =>
            {
                sw.WriteLine("Other" + i);

                if (v != null)
                {
                    v.RawTracks.Invoke(pair =>
                    {
                        sw.WriteLine(string.Format("{0}: {1}", pair.Key, pair.Value));
                    });
                }
            });
        }

        private static void WriteImages(StreamWriter sw, ImageTrackInfo[] images)
        {
            if (images == null)
                return;

            sw.WriteLine("Images");

            images.Invoke((v, i) =>
            {
                sw.WriteLine("Image" + i);

                if (v != null)
                {
                    v.RawTracks.Invoke(pair =>
                    {
                        sw.WriteLine(string.Format("{0}: {1}", pair.Key, pair.Value));
                    });
                }
            });
        }

        private static void WriteMenus(StreamWriter sw, MenuTrackInfo[] menus)
        {
            if (menus == null)
                return;

            sw.WriteLine("Menus");

            menus.Invoke((v, i) =>
            {
                sw.WriteLine("Menu" + i);

                if (v != null)
                {
                    v.RawTracks.Invoke(pair =>
                    {
                        sw.WriteLine(string.Format("{0}: {1}", pair.Key, pair.Value));
                    });
                }
            });
        }

    }
}
