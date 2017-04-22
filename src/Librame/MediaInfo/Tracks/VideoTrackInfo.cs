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
    /// 视频轨道信息。
    /// </summary>
    public class VideoTrackInfo : AbstarctTrackInfo<VideoTrackInfo>, ITrackInfo
    {
        /// <summary>
        /// 构造一个 <see cref="VideoTrackInfo"/> 实例。
        /// </summary>
        /// <param name="baseTrack">给定的轨道字典集合。</param>
        public VideoTrackInfo(IDictionary<string, string> baseTrack)
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
            FormatSettings = GetOrDefault("Format_Settings");
            
            CodecString = GetOrDefault("Codec/String");
            CodecFamily = GetOrDefault("Codec/Family");
            CodecInfo = GetOrDefault("Codec/Info");
            CodecUrl = GetOrDefault("Codec/Url");
            CodecProfile = GetOrDefault("Codec_Profile");
            CodecSettings = GetOrDefault("Codec_Settings");
            
            DurationFullString = GetOrDefault("Duration/String1");
            DurationTime = GetOrDefault("Duration/String3");

            BitRateMode = GetOrDefault("BitRate_Mode");
            BitRateModeString = GetOrDefault("BitRate_Mode/String");
            BitRateString = GetOrDefault("BitRate/String");
            BitRateMinimum = GetOrDefault("BitRate_Minimum");
            BitRateMinimumString = GetOrDefault("BitRate_Minimum/String");
            BitRateNominal = GetOrDefault("BitRate_Nominal");
            BitRateNominalString = GetOrDefault("BitRate_Nominal/String");
            BitRateMaximum = GetOrDefault("BitRate_Maximum");
            BitRateMaximumString = GetOrDefault("BitRate_Maximum/String");
            
            WidthString = GetOrDefault("Width/String");
            HeightString = GetOrDefault("Height/String");
            PixelAspectRatio = GetOrDefault("PixelAspectRatio");
            DisplayAspectRatio = GetOrDefault("DisplayAspectRatio");
            DisplayAspectRatioString = GetOrDefault("DisplayAspectRatio/String");

            string[] ratios = (string.IsNullOrEmpty(DisplayAspectRatioString) ? null :
                DisplayAspectRatioString.Split(':'));
            DisplayAspectRatioWidth = (ratios != null ? ratios[0] : string.Empty);
            DisplayAspectRatioHeight = (ratios != null ? ratios[ratios.Length - 1] : string.Empty);

            FrameRateMode = GetOrDefault("FrameRate_Mode");
            FrameRateModeString = GetOrDefault("FrameRate_Mode/String");
            FrameRateString = GetOrDefault("FrameRate/String");
            FrameRateMinimum = GetOrDefault("FrameRate_Minimum");
            FrameRateMinimumString = GetOrDefault("FrameRate_Minimum/String");
            FrameRateMaximum = GetOrDefault("FrameRate_Maximum");
            FrameRateMaximumString = GetOrDefault("FrameRate_Maximum/String");
            
            ResolutionString = GetOrDefault("Resolution/String");
            BitDepthString = GetOrDefault("BitDepth/String");
            
            ScanTypeString = GetOrDefault("ScanType/String");
            
            InterlacementString = GetOrDefault("Interlacement/String");
            
            Bits = GetOrDefault("Bits-(Pixel*Frame)");
            
            StreamSizeFullString = GetOrDefault("StreamSize/String4");
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
        /// 唯一编号
        /// </summary>
        public string UniqueID { get; private set; }
        /// <summary>
        /// 唯一编号
        /// </summary>
        public string UniqueIDString { get; private set; }

        /// <summary>
        /// 格式（AVC）
        /// </summary>
        public string Format { get; private set; }
        /// <summary>
        /// 格式信息（Advanced Video Codec）
        /// </summary>
        public string FormatInfo { get; private set; }
        /// <summary>
        /// 格式 URL（http://developers.videolan.org/x264.html）
        /// </summary>
        public string FormatUrl { get; private set; }
        /// <summary>
        /// 格式版本
        /// </summary>
        public string FormatVersion { get; private set; }
        /// <summary>
        /// 格式规范（High@L3.2）
        /// </summary>
        public string FormatProfile { get; private set; }
        /// <summary>
        /// 格式设定（CABAC / 16 Ref Frames）
        /// </summary>
        public string FormatSettings { get; private set; }

        /// <summary>
        /// 互联网媒体类型（video/H264）
        /// </summary>
        public string InternetMediaType { get; private set; }

        /// <summary>
        /// 编解码器（V_MPEG4/ISO/AVC）
        /// </summary>
        public string Codec { get; private set; }
        /// <summary>
        /// 编解码器（AVC）
        /// </summary>
        public string CodecString { get; private set; }
        /// <summary>
        /// 编解码器家族（AVC）
        /// </summary>
        public string CodecFamily { get; private set; }
        /// <summary>
        /// 编解码器信息（Advanced Video Codec）
        /// </summary>
        public string CodecInfo { get; private set; }
        /// <summary>
        /// 编解码器 URL（http://ffdshow-tryout.sourceforge.net/）
        /// </summary>
        public string CodecUrl { get; private set; }
        /// <summary>
        /// 编解码器规范（High@L3.2）
        /// </summary>
        public string CodecProfile { get; private set; }
        /// <summary>
        /// 编解码器设定（CABAC / 16 Ref Frames）
        /// </summary>
        public string CodecSettings { get; private set; }

        /// <summary>
        /// 文件时长（7125405）
        /// </summary>
        public string Duration { get; private set; }
        /// <summary>
        /// 文件时长（1h 58mn 45s 405ms）
        /// </summary>
        public string DurationFullString { get; private set; }
        /// <summary>
        /// 文件时长（01:58:45.405）
        /// </summary>
        public string DurationTime { get; private set; }

        /// <summary>
        /// 位率模式（Variable）
        /// </summary>
        public string BitRateMode { get; private set; }
        /// <summary>
        /// 位率模式（Variable）
        /// </summary>
        public string BitRateModeString { get; private set; }
        /// <summary>
        /// 位率（2 956 000）
        /// </summary>
        public string BitRate { get; private set; }
        /// <summary>
        /// 位率（2 956 Kbps）
        /// </summary>
        public string BitRateString { get; private set; }
        /// <summary>
        /// 最小位率（3 000 000）
        /// </summary>
        public string BitRateMinimum { get; private set; }
        /// <summary>
        /// 最小位率（3 000 Kbps）
        /// </summary>
        public string BitRateMinimumString { get; private set; }
        /// <summary>
        /// 额定位率（2 700 000）
        /// </summary>
        public string BitRateNominal { get; private set; }
        /// <summary>
        /// 额定位率（2 700 Kbps）
        /// </summary>
        public string BitRateNominalString { get; private set; }
        /// <summary>
        /// 最大位率（3 000 000）
        /// </summary>
        public string BitRateMaximum { get; private set; }
        /// <summary>
        /// 最大位率（3 000 Kbps）
        /// </summary>
        public string BitRateMaximumString { get; private set; }

        /// <summary>
        /// 画面宽（640）
        /// </summary>
        public string Width { get; private set; }
        /// <summary>
        /// 画面宽（640 pixels）
        /// </summary>
        public string WidthString { get; private set; }
        /// <summary>
        /// 画面高（480）
        /// </summary>
        public string Height { get; private set; }
        /// <summary>
        /// 画面高（480 pixels）
        /// </summary>
        public string HeightString { get; private set; }
        /// <summary>
        /// 像素纵横比（1.000）
        /// </summary>
        public string PixelAspectRatio { get; private set; }
        /// <summary>
        /// 显示纵横比（1.333）
        /// </summary>
        public string DisplayAspectRatio { get; private set; }
        /// <summary>
        /// 显示纵横比（4:3）
        /// </summary>
        public string DisplayAspectRatioString { get; private set; }
        /// <summary>
        /// 显示纵横比宽（4）
        /// </summary>
        public string DisplayAspectRatioWidth { get; private set; }
        /// <summary>
        /// 显示纵横比高（3）
        /// </summary>
        public string DisplayAspectRatioHeight { get; private set; }

        /// <summary>
        /// 帧率模式（Variable）
        /// </summary>
        public string FrameRateMode { get; private set; }
        /// <summary>
        /// 帧率模式（Variable）
        /// </summary>
        public string FrameRateModeString { get; private set; }
        /// <summary>
        /// 帧率（28.426）
        /// </summary>
        public string FrameRate { get; private set; }
        /// <summary>
        /// 帧率（28.426 fps）
        /// </summary>
        public string FrameRateString { get; private set; }
        /// <summary>
        /// 最小帧率（14.925）
        /// </summary>
        public string FrameRateMinimum { get; private set; }
        /// <summary>
        /// 最小帧率（14.925 fps）
        /// </summary>
        public string FrameRateMinimumString { get; private set; }
        /// <summary>
        /// 最大帧率（61.224）
        /// </summary>
        public string FrameRateMaximum { get; private set; }
        /// <summary>
        /// 最大帧率（61.224 fps）
        /// </summary>
        public string FrameRateMaximumString { get; private set; }
        /// <summary>
        /// 帧数
        /// </summary>
        public string FrameCount { get; private set; }

        /// <summary>
        /// 色彩空间（YUV）
        /// </summary>
        public string ColorSpace { get; private set; }
        /// <summary>
        /// 色度抽样（4:2:0）
        /// </summary>
        public string ChromaSubsampling { get; private set; }

        /// <summary>
        /// 解析位深（8）
        /// </summary>
        public string Resolution { get; private set; }
        /// <summary>
        /// 解析位深（8 bits）
        /// </summary>
        public string ResolutionString { get; private set; }
        /// <summary>
        /// 色彩位深（8）
        /// </summary>
        public string BitDepth { get; private set; }
        /// <summary>
        /// 色彩位深（8 bits）
        /// </summary>
        public string BitDepthString { get; private set; }

        /// <summary>
        /// 扫描类型（Progressive）
        /// </summary>
        public string ScanType { get; private set; }
        /// <summary>
        /// 扫描类型（Progressive）
        /// </summary>
        public string ScanTypeString { get; private set; }

        /// <summary>
        /// 交错（PPF）
        /// </summary>
        public string Interlacement { get; private set; }
        /// <summary>
        /// 交错（Progressive）
        /// </summary>
        public string InterlacementString { get; private set; }

        /// <summary>
        /// 压缩模式（Lossy）
        /// </summary>
        public string CompressionMode { get; private set; }
        /// <summary>
        /// 压缩比率（0.339）
        /// </summary>
        public string Bits { get; private set; }

        /// <summary>
        /// 流大小（20350217）
        /// </summary>
        public string StreamSize { get; private set; }
        /// <summary>
        /// 流大小（19.41 MiB）
        /// </summary>
        public string StreamSizeFullString { get; private set; }
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
        /// 缓冲大小
        /// </summary>
        public string BufferSize { get; private set; }


        /// <summary>
        /// 验证与指定的 <see cref="VideoTrackInfo"/> 是否相等。
        /// </summary>
        /// <param name="obj">给定的 <see cref="VideoTrackInfo"/>。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null) || !(obj is VideoTrackInfo))
                return false;

            var info = (obj as VideoTrackInfo);
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
