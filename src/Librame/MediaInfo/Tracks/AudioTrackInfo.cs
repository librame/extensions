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
    /// 音频轨道信息。
    /// </summary>
    public class AudioTrackInfo : AbstarctTrackInfo<AudioTrackInfo>, ITrackInfo
    {
        /// <summary>
        /// 构造一个 <see cref="AudioTrackInfo"/> 实例。
        /// </summary>
        /// <param name="baseTrack">给定的轨道字典集合。</param>
        public AudioTrackInfo(IDictionary<string, string> baseTrack)
            : base(baseTrack)
        {
        }

        /// <summary>
        /// 初始化轨道信息。
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            
            IDString = GetOrDefault("ID/String");
            
            UniqueIDString = GetOrDefault("UniqueID/String");
            
            FormatInfo = GetOrDefault("Format/Info");
            FormatUrl = GetOrDefault("Format/Url");
            FormatVersion = GetOrDefault("Format_Version");
            FormatProfile = GetOrDefault("Format_Profile");
            FormatSettingsMode = GetOrDefault("Format_Settings_Mode");
            FormatSettingsModeExtension = GetOrDefault("Format_Settings_ModeExtension");
            
            CodecString = GetOrDefault("Codec/String");
            CodecFamily = GetOrDefault("Codec/Family");
            CodecInfo = GetOrDefault("Codec/Info");
            CodecUrl = GetOrDefault("Codec/Url");
            CodecProfile = GetOrDefault("Codec_Profile");
            CodecSettings = GetOrDefault("Codec_Settings");
            
            DurationString = GetOrDefault("Duration/String1");
            DurationTime = GetOrDefault("Duration/String3");

            BitRateMode = GetOrDefault("BitRate_Mode");
            BitRateModeString = GetOrDefault("BitRate_Mode/String");
            BitRateString = GetOrDefault("BitRate/String");

            Channels = GetOrDefault("Channel(s)");
            ChannelsString = GetOrDefault("Channel(s)/String");
            ChannelPositionsString = GetOrDefault("ChannelPositions/String2");
            
            SamplingRateString = GetOrDefault("SamplingRate/String");

            CompressionMode = GetOrDefault("Compression_Mode");
            CompressionModeString = GetOrDefault("Compression_Mode/String");
            
            StreamSizeString = GetOrDefault("StreamSize/String4");
            StreamSizeProportion = GetOrDefault("StreamSize_Proportion");
            
            EncodedApplication = GetOrDefault("Encoded_Application");
            EncodedLibrary = GetOrDefault("Encoded_Library");
            EncodedLibraryString = GetOrDefault("Encoded_Library/String");
            //EncodedLibraryName = GetOrDefault("Encoded_Library/Name");
            EncodedLibraryVersion = GetOrDefault("Encoded_Library/Version");
            EncodedLibrarySettings = GetOrDefault("Encoded_Library_Settings");
            
            LanguageString = GetOrDefault("Language/String");
            LanguageAbbreviation = GetOrDefault("Language/String3");
        }
        

        /// <summary>
        /// 编号
        /// </summary>
        public string ID { get; private set; }
        /// <summary>
        /// 编号
        /// </summary>
        public string IDString { get; private set; }

        /// <summary>
        /// 唯一编号（197058999）
        /// </summary>
        public string UniqueID { get; private set; }
        /// <summary>
        /// 唯一编号
        /// </summary>
        public string UniqueIDString { get; private set; }

        /// <summary>
        /// 格式（MPEG Audio）
        /// </summary>
        public string Format { get; private set; }
        /// <summary>
        /// 格式信息（Advanced Audio Codec）
        /// </summary>
        public string FormatInfo { get; private set; }
        /// <summary>
        /// 格式 URL
        /// </summary>
        public string FormatUrl { get; private set; }
        /// <summary>
        /// 格式版本
        /// </summary>
        public string FormatVersion { get; private set; }
        /// <summary>
        /// 格式规范（HE-AACv2 / HE-AAC / LC）
        /// </summary>
        public string FormatProfile { get; private set; }
        /// <summary>
        /// 格式设定调式（Joint stereo）
        /// </summary>
        public string FormatSettingsMode { get; private set; }
        /// <summary>
        /// 格式设定调式扩展（MS Stereo）
        /// </summary>
        public string FormatSettingsModeExtension { get; private set; }

        /// <summary>
        /// 互联网媒体类型（audio/mpeg）
        /// </summary>
        public string InternetMediaType { get; private set; }

        /// <summary>
        /// 编解码器（AAC LC-SBR-PS）
        /// </summary>
        public string Codec { get; private set; }
        /// <summary>
        /// 编解码器（AAC LC-SBR-PS）
        /// </summary>
        public string CodecString { get; private set; }
        /// <summary>
        /// 编解码器家族（AAC）
        /// </summary>
        public string CodecFamily { get; private set; }
        /// <summary>
        /// 编解码器信息
        /// </summary>
        public string CodecInfo { get; private set; }
        /// <summary>
        /// 编解码器 URL
        /// </summary>
        public string CodecUrl { get; private set; }
        /// <summary>
        /// 编解码器规范
        /// </summary>
        public string CodecProfile { get; private set; }
        /// <summary>
        /// 编解码器设定
        /// </summary>
        public string CodecSettings { get; private set; }

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
        /// 位率模式（CBR）
        /// </summary>
        public string BitRateMode { get; private set; }
        /// <summary>
        /// 位率模式（Constant）
        /// </summary>
        public string BitRateModeString { get; private set; }
        /// <summary>
        /// 位率（112000）
        /// </summary>
        public string BitRate { get; private set; }
        /// <summary>
        /// 位率（112 Kbps）
        /// </summary>
        public string BitRateString { get; private set; }

        /// <summary>
        /// 声道数（2 / 1 / 1）
        /// </summary>
        public string Channels { get; private set; }
        /// <summary>
        /// 声道数（2 channels / 1 channel / 1 channel）
        /// </summary>
        public string ChannelsString { get; private set; }
        /// <summary>
        /// 声道定位（Front: L R / Front: C / Front: C）
        /// </summary>
        public string ChannelPositions { get; private set; }
        /// <summary>
        /// 声道定位（1/0/0）
        /// </summary>
        public string ChannelPositionsString { get; private set; }

        /// <summary>
        /// 采样率（48000 / 48000 / 24000）
        /// </summary>
        public string SamplingRate { get; private set; }
        /// <summary>
        /// 采样率（48.0 KHz / 48.0 KHz / 24.0 KHz）
        /// </summary>
        public string SamplingRateString { get; private set; }
        /// <summary>
        /// 采样数（257323008）
        /// </summary>
        public string SamplingCount { get; private set; }

        /// <summary>
        /// 压缩模式（Lossy）
        /// </summary>
        public string CompressionMode { get; private set; }
        /// <summary>
        /// 压缩模式（Lossy）
        /// </summary>
        public string CompressionModeString { get; private set; }

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
        /// 标题
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// 编码程序
        /// </summary>
        public string EncodedApplication { get; private set; }
        /// <summary>
        /// 编码库（x264 - core 68 r1183M f21daff）
        /// </summary>
        public string EncodedLibrary { get; private set; }
        /// <summary>
        /// 编码库（x264 core 68 r1183M f21daff）
        /// </summary>
        public string EncodedLibraryString { get; private set; }
        ///// <summary>
        ///// 编码库名称（x264）
        ///// </summary>
        //public string EncodedLibraryName { get; private set; }
        /// <summary>
        /// 编码库版本（core 68 r1183M f21daff）
        /// </summary>
        public string EncodedLibraryVersion { get; private set; }
        /// <summary>
        /// 编码库设定
        /// </summary>
        public string EncodedLibrarySettings { get; private set; }

        /// <summary>
        /// 语言（en）
        /// </summary>
        public string Language { get; private set; }
        /// <summary>
        /// 语言（English）
        /// </summary>
        public string LanguageString { get; private set; }
        /// <summary>
        /// 语言缩写（eng）
        /// </summary>
        public string LanguageAbbreviation { get; private set; }

        /// <summary>
        /// 默认（Yes）
        /// </summary>
        public string Default { get; private set; }
        /// <summary>
        /// 强制（No）
        /// </summary>
        public string Forced { get; private set; }


        /// <summary>
        /// 验证与指定的 <see cref="AudioTrackInfo"/> 是否相等。
        /// </summary>
        /// <param name="obj">给定的 <see cref="AudioTrackInfo"/>。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null) || !(obj is AudioTrackInfo))
                return false;

            var info = (obj as AudioTrackInfo);
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
