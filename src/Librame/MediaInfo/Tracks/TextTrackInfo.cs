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
    /// 文本轨道信息。
    /// </summary>
    public class TextTrackInfo : AbstarctTrackInfo<TextTrackInfo>, ITrackInfo
    {
        /// <summary>
        /// 构造一个 <see cref="TextTrackInfo"/> 实例。
        /// </summary>
        /// <param name="baseTrack">给定的轨道字典集合。</param>
        public TextTrackInfo(IDictionary<string, string> baseTrack)
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
            CodecInfo = GetOrDefault("Codec/Info");
            CodecUrl = GetOrDefault("Codec/Url");

            CompressionMode = GetOrDefault("Compression_Mode");
            CompressionModeString = GetOrDefault("Compression_Mode/String");
            
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
        /// 唯一编号（3598764698）
        /// </summary>
        public string UniqueID { get; private set; }
        /// <summary>
        /// 唯一编号
        /// </summary>
        public string UniqueIDString { get; private set; }

        /// <summary>
        /// 格式（SSA）
        /// </summary>
        public string Format { get; private set; }
        /// <summary>
        /// 格式信息
        /// </summary>
        public string FormatInfo { get; private set; }
        /// <summary>
        /// 格式 URL（http://ffdshow.sourceforge.net/tikiwiki/tiki-index.php?page=Getting+ffdshow）
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
        /// 互联网媒体类型
        /// </summary>
        public string InternetMediaType { get; private set; }

        /// <summary>
        /// 编解码器（S_TEXT/SSA）
        /// </summary>
        public string Codec { get; private set; }
        /// <summary>
        /// 编解码器（SSA）
        /// </summary>
        public string CodecString { get; private set; }
        /// <summary>
        /// 编解码器信息（Sub Station Alpha）
        /// </summary>
        public string CodecInfo { get; private set; }
        /// <summary>
        /// 编解码器 URL
        /// </summary>
        public string CodecUrl { get; private set; }

        /// <summary>
        /// 压缩模式（Lossless）
        /// </summary>
        public string CompressionMode { get; private set; }
        /// <summary>
        /// 压缩模式（Lossless）
        /// </summary>
        public string CompressionModeString { get; private set; }

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
        /// 验证与指定的 <see cref="TextTrackInfo"/> 是否相等。
        /// </summary>
        /// <param name="obj">给定的 <see cref="TextTrackInfo"/>。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null) || !(obj is TextTrackInfo))
                return false;

            var info = (obj as TextTrackInfo);
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
