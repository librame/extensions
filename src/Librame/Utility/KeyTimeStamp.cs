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

namespace Librame.Utility
{
    /// <summary>
    /// 键名时间戳。
    /// </summary>
    public class KeyTimeStamp
    {
        /// <summary>
        /// 构造一个以当前时间为起始时间的 <see cref="KeyTimeStamp"/> 实例。
        /// </summary>
        /// <param name="key">给定的键名。</param>
        public KeyTimeStamp(string key)
            : this(key, DateTime.Now)
        {
        }
        /// <summary>
        /// 构造一个 <see cref="KeyTimeStamp"/> 实例。
        /// </summary>
        /// <param name="key">给定的键名。</param>
        /// <param name="startTime">给定的开始时间。</param>
        public KeyTimeStamp(string key, DateTime startTime)
        {
            Key = key;
            StartTime = startTime;

            // 初始化过期时间
            InitializeExpireTime();
        }

        /// <summary>
        /// 初始化过期时间（默认 30 分钟后过期）。
        /// </summary>
        protected virtual void InitializeExpireTime()
        {
            // 默认 30 分钟过期
            ExpireTime = StartTime.AddMinutes(30);
        }


        /// <summary>
        /// 键名。
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// 开始时间。
        /// </summary>
        public DateTime StartTime { get; private set; }

        /// <summary>
        /// 过期时间。
        /// </summary>
        public DateTime ExpireTime { get; set; }


        /// <summary>
        /// 检测是否已过期。
        /// </summary>
        public bool IsExpired
        {
            get { return (DateTime.Now > ExpireTime); }
        }

    }
}
