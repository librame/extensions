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
    /// 图像轨道信息。
    /// </summary>
    public class ImageTrackInfo : AbstarctTrackInfo<ImageTrackInfo>, ITrackInfo
    {
        /// <summary>
        /// 构造一个 <see cref="ImageTrackInfo"/> 实例。
        /// </summary>
        /// <param name="baseTrack">给定的轨道字典集合。</param>
        public ImageTrackInfo(IDictionary<string, string> baseTrack)
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
            
            CodecString = GetOrDefault("Codec/String");
            
            WidthString = GetOrDefault("Width/String");
            HeightString = GetOrDefault("Height/String");
            DisplayAspectRatioString = GetOrDefault("DisplayAspectRatio/String");
            
            ResolutionString = GetOrDefault("Resolution/String");
            BitDepthString = GetOrDefault("BitDepth/String");
            
            ScanTypeString = GetOrDefault("ScanType/String");
            
            InterlacementString = GetOrDefault("Interlacement/String");
            
            StreamSizeFullString = GetOrDefault("StreamSize/String4");
            StreamSizeProportion = GetOrDefault("StreamSize_Proportion");
            
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
        /// 格式（JPEG）
        /// </summary>
        public string Format { get; private set; }
        /// <summary>
        /// 格式信息
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
        /// 格式规范
        /// </summary>
        public string FormatProfile { get; private set; }

        /// <summary>
        /// 互联网媒体类型（image/jpeg）
        /// </summary>
        public string InternetMediaType { get; private set; }

        /// <summary>
        /// 编解码器（JPEG）
        /// </summary>
        public string Codec { get; private set; }
        /// <summary>
        /// 编解码器（JPEG）
        /// </summary>
        public string CodecString { get; private set; }

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
        /// 验证与指定的 <see cref="ImageTrackInfo"/> 是否相等。
        /// </summary>
        /// <param name="obj">给定的 <see cref="ImageTrackInfo"/>。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null) || !(obj is ImageTrackInfo))
                return false;

            var info = (obj as ImageTrackInfo);
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
