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
    /// 概览轨道信息。
    /// </summary>
    public class GeneralTrackInfo : AbstarctTrackInfo<GeneralTrackInfo>, ITrackInfo
    {
        /// <summary>
        /// 构造一个 <see cref="GeneralTrackInfo"/> 实例。
        /// </summary>
        /// <param name="baseTrack">给定的轨道字典集合。</param>
        public GeneralTrackInfo(IDictionary<string, string> baseTrack)
            : base(baseTrack)
        {
        }

        /// <summary>
        /// 初始化轨道信息。
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            UniqueIDString = GetOrDefault("UniqueID/String");

            VideoFormatList = GetOrDefault("Video_Format_List");
            VideoCodecList = GetOrDefault("Video_Codec_List");
            VideoLanguageList = GetOrDefault("Video_Language_List");

            AudioFormatList = GetOrDefault("Audio_Format_List");
            AudioCodecList = GetOrDefault("Audio_Codec_List");
            AudioLanguageList = GetOrDefault("Audio_Language_List");

            TextFormatList = GetOrDefault("Text_Format_List");
            TextCodecList = GetOrDefault("Text_Codec_List");
            TextLanguageList = GetOrDefault("Text_Language_List");

            OtherFormatList = GetOrDefault("Other_Format_List");
            OtherCodecList = GetOrDefault("Other_Codec_List");
            OtherLanguageList = GetOrDefault("Other_Language_List");

            ImageFormatList = GetOrDefault("Image_Format_List");
            ImageCodecList = GetOrDefault("Image_Codec_List");
            ImageLanguageList = GetOrDefault("Image_Language_List");

            MenuFormatList = GetOrDefault("Menu_Format_List");
            MenuCodecList = GetOrDefault("Menu_Codec_List");
            MenuLanguageList = GetOrDefault("Menu_Language_List");

            FormatString = GetOrDefault("Format/String");
            FormatInfo = GetOrDefault("Format/Info");
            FormatUrl = GetOrDefault("Format/Url");
            FormatExtensions = GetOrDefault("Format/Extensions");
            FormatVersion = GetOrDefault("Format_Version");

            DurationString = GetOrDefault("Duration/String1");
            DurationTime = GetOrDefault("Duration/String3");

            OverallBitRateString = GetOrDefault("OverallBitRate/String");

            StreamSizeString = GetOrDefault("StreamSize/String4");
            StreamSizeProportion = GetOrDefault("StreamSize_Proportion");

            EncodedDate = GetOrDefault("Encoded_Date");
            EncodedApplication = GetOrDefault("Encoded_Application");
            EncodedLibrary = GetOrDefault("Encoded_Library");
            EncodedLibraryString = GetOrDefault("Encoded_Library/String");
        }
        

        /// <summary>
        /// 视轨数。
        /// </summary>
        public string VideoCount { get; private set; }
        /// <summary>
        /// 音轨数。
        /// </summary>
        public string AudioCount { get; private set; }
        /// <summary>
        /// 字幕数。
        /// </summary>
        public string TextCount { get; private set; }
        /// <summary>
        /// 其它数。
        /// </summary>
        public string OtherCount { get; private set; }
        /// <summary>
        /// 图像数。
        /// </summary>
        public string ImageCount { get; private set; }
        /// <summary>
        /// 菜单数。
        /// </summary>
        public string MenuCount { get; private set; }


        /// <summary>
        /// 唯一编号（201998698691846446268886151526523723250）
        /// </summary>
        public string UniqueID { get; private set; }
        /// <summary>
        /// 唯一编号（201998698691846446268886151526523723250 (0x97F788C4DB340374B1151BDA290DDDF2)）
        /// </summary>
        public string UniqueIDString { get; private set; }

        /// <summary>
        /// 完整名称。
        /// </summary>
        public string CompleteName { get; private set; }
        /// <summary>
        /// 目录名称。
        /// </summary>
        public string FolderName { get; private set; }
        /// <summary>
        /// 文件名称。
        /// </summary>
        public string FileName { get; private set; }
        /// <summary>
        /// 文件扩展名
        /// </summary>
        public string FileExtension { get; private set; }

        /// <summary>
        /// 视频格式列表（AVC）
        /// </summary>
        public string VideoFormatList { get; private set; }
        /// <summary>
        /// 视频编码器列表（AVC）
        /// </summary>
        public string VideoCodecList { get; private set; }
        /// <summary>
        /// 视频语言列表（English）
        /// </summary>
        public string VideoLanguageList { get; private set; }

        /// <summary>
        /// 音频格式列表（AAC / AAC）
        /// </summary>
        public string AudioFormatList { get; private set; }
        /// <summary>
        /// 音频编码器列表（AAC LC-SBR-PS / AAC LC-SBR-PS）
        /// </summary>
        public string AudioCodecList { get; private set; }
        /// <summary>
        /// 音频格式列表（Chinese / English）
        /// </summary>
        public string AudioLanguageList { get; private set; }

        /// <summary>
        /// 字幕格式列表（SSA / SSA / UTF-8 / UTF-8 / UTF-8 / UTF-8）
        /// </summary>
        public string TextFormatList { get; private set; }
        /// <summary>
        /// 字幕编码器列表（SSA / SSA / UTF-8 / UTF-8 / UTF-8 / UTF-8）
        /// </summary>
        public string TextCodecList { get; private set; }
        /// <summary>
        /// 字幕格式列表（Chinese / English / Chinese / Chinese / English / Chinese）
        /// </summary>
        public string TextLanguageList { get; private set; }

        /// <summary>
        /// 其它格式列表
        /// </summary>
        public string OtherFormatList { get; private set; }
        /// <summary>
        /// 其它编码器列表
        /// </summary>
        public string OtherCodecList { get; private set; }
        /// <summary>
        /// 其它格式列表
        /// </summary>
        public string OtherLanguageList { get; private set; }

        /// <summary>
        /// 图像格式列表
        /// </summary>
        public string ImageFormatList { get; private set; }
        /// <summary>
        /// 图像编码器列表
        /// </summary>
        public string ImageCodecList { get; private set; }
        /// <summary>
        /// 图像格式列表
        /// </summary>
        public string ImageLanguageList { get; private set; }

        /// <summary>
        /// 菜单格式列表
        /// </summary>
        public string MenuFormatList { get; private set; }
        /// <summary>
        /// 菜单编码器列表
        /// </summary>
        public string MenuCodecList { get; private set; }
        /// <summary>
        /// 菜单格式列表
        /// </summary>
        public string MenuLanguageList { get; private set; }

        /// <summary>
        /// 容器格式（Matroska）
        /// </summary>
        public string Format { get; private set; }
        /// <summary>
        /// 容器格式（Matroska）
        /// </summary>
        public string FormatString { get; private set; }
        /// <summary>
        /// 容器格式信息
        /// </summary>
        public string FormatInfo { get; private set; }
        /// <summary>
        /// 容器格式 URL（http://packs.matroska.org/）
        /// </summary>
        public string FormatUrl { get; private set; }
        /// <summary>
        /// 容器格式扩展名（mkv mk3d mka mks）
        /// </summary>
        public string FormatExtensions { get; private set; }
        /// <summary>
        /// 容器格式版本（Version 1）
        /// </summary>
        public string FormatVersion { get; private set; }

        /// <summary>
        /// 互联网媒体类型（application/vnd.rn-realmedia）
        /// </summary>
        public string InternetMediaType { get; private set; }

        /// <summary>
        /// 文件大小（808682860）
        /// </summary>
        public string FileSize { get; private set; }
        /// <summary>
        /// 文件大小（771.2 MiB）
        /// </summary>
        public string FileSizeString { get; private set; }

        /// <summary>
        /// 文件时长（7125405）
        /// </summary>
        public string Duration { get; private set; }
        /// <summary>
        /// 文件时长（1h 58mn 45s 405ms）
        /// </summary>
        public string DurationString { get; private set; }
        /// <summary>
        /// 文件时长（01:58:45.405）
        /// </summary>
        public string DurationTime { get; private set; }

        /// <summary>
        /// 平均位率（885095）
        /// </summary>
        public string OverallBitRate { get; private set; }
        /// <summary>
        /// 平均位率（885 Kbps）
        /// </summary>
        public string OverallBitRateString { get; private set; }

        /// <summary>
        /// 流大小（20350217）
        /// </summary>
        public string StreamSize { get; private set; }
        /// <summary>
        /// 流大小（19.41 MiB）
        /// </summary>
        public string StreamSizeString { get; private set; }
        /// <summary>
        /// 流大小比率（0.02516）
        /// </summary>
        public string StreamSizeProportion { get; private set; }

        /// <summary>
        /// 编码日期（UTC 2012-03-18 04:25:40）
        /// </summary>
        public string EncodedDate { get; private set; }
        /// <summary>
        /// 编码程序（mkvmerge v2.2.0 ('Turn It On Again') built on Mar 4 2008 12:58:26）
        /// </summary>
        public string EncodedApplication { get; private set; }
        /// <summary>
        /// 编码库（libebml v0.7.7 + libmatroska v0.8.1）
        /// </summary>
        public string EncodedLibrary { get; private set; }
        /// <summary>
        /// 编码库（libebml v0.7.7 + libmatroska v0.8.1）
        /// </summary>
        public string EncodedLibraryString { get; private set; }


        /// <summary>
        /// 验证与指定的 <see cref="GeneralTrackInfo"/> 是否相等。
        /// </summary>
        /// <param name="obj">给定的 <see cref="GeneralTrackInfo"/>。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null) || !(obj is GeneralTrackInfo))
                return false;

            var info = (obj as GeneralTrackInfo);
            return (UniqueID == info.UniqueID);
        }

        /// <summary>
        /// 用作特定类型的哈希函数。
        /// </summary>
        /// <returns>返回整数值。</returns>
        public override int GetHashCode()
        {
            return UniqueID.GetHashCode();
        }

    }
}
