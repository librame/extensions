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
    using Utility;

    /// <summary>
    /// 抽象轨道信息基类。
    /// </summary>
    /// <typeparam name="TTrackInfo">指定的轨道信息类型。</typeparam>
    public abstract class AbstarctTrackInfo<TTrackInfo>
        where TTrackInfo : class, ITrackInfo
    {
        /// <summary>
        /// 原始轨道集合。
        /// </summary>
        public IDictionary<string, string> RawTracks { get; private set; }

        /// <summary>
        /// 构造一个 <see cref="GeneralTrackInfo"/> 实例。
        /// </summary>
        /// <param name="tracks">给定的轨道集合。</param>
        public AbstarctTrackInfo(IDictionary<string, string> tracks)
        {
            RawTracks = tracks;

            if (!ReferenceEquals(RawTracks, null))
            {
                // 初始化
                Initialize();
            }
        }

        /// <summary>
        /// 初始化轨道信息。
        /// </summary>
        protected virtual void Initialize()
        {
            var pis = typeof(TTrackInfo).GetProperties();

            foreach (var pi in pis)
            {
                string value;
                if (RawTracks.TryGetValue(pi.Name, out value))
                {
                    // 设定当前属性值
                    pi.SetValue(this, value, null);
                }
            }
        }

        /// <summary>
        /// 获取或默认值。
        /// </summary>
        /// <param name="key">给定的键名。</param>
        /// <param name="defaultValue">给定的默认值。</param>
        /// <returns>返回字符串。</returns>
        protected virtual string GetOrDefault(string key, string defaultValue = "")
        {
            string value;
            if (RawTracks.TryGetValue(key, out value))
            {
                return value;
            }
            else
            {
                return defaultValue;
            }
        }


        /// <summary>
        /// 将名称和值的集合转换为字符串形式。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
        {
            return RawTracks.JoinStrings(f => f.Key + "=" + f.Value);
        }

    }
}
