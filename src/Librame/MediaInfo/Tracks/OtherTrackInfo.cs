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
    /// 其它轨道信息。
    /// </summary>
    public class OtherTrackInfo : AbstarctTrackInfo<MenuTrackInfo>, ITrackInfo
    {
        /// <summary>
        /// 构造一个 <see cref="OtherTrackInfo"/> 实例。
        /// </summary>
        /// <param name="baseTrack">给定的轨道字典集合。</param>
        public OtherTrackInfo(IDictionary<string, string> baseTrack)
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
        }


        ///// <summary>
        ///// 唯一编号（201998698691846446268886151526523723250）。
        ///// </summary>
        //public string UniqueID { get; private set; }
        /// <summary>
        /// 唯一编号（201998698691846446268886151526523723250 (0x97F788C4DB340374B1151BDA290DDDF2)）。
        /// </summary>
        public string UniqueIDString { get; private set; }


        /// <summary>
        /// 验证与指定的 <see cref="MenuTrackInfo"/> 是否相等。
        /// </summary>
        /// <param name="obj">给定的 <see cref="MenuTrackInfo"/>。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null) || !(obj is MenuTrackInfo))
                return false;

            var info = (obj as MenuTrackInfo);
            return (UniqueIDString == info.UniqueIDString);
        }

        /// <summary>
        /// 用作特定类型的哈希函数。
        /// </summary>
        /// <returns>返回整数值。</returns>
        public override int GetHashCode()
        {
            return UniqueIDString.GetHashCode();
        }

    }
}
