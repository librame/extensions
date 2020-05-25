#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Network.Options
{
    /// <summary>
    /// 抓取器选项。
    /// </summary>
    public class CrawlerOptions
    {
        /// <summary>
        /// 图像文件扩展名集合（以英文逗号分隔）。
        /// </summary>
        public string ImageExtensions { get; set; }
            = ".jpg,.jpeg,.png,.bmp";

        /// <summary>
        /// 缓存过期秒数（默认 10 秒后过期）。
        /// </summary>
        public int CacheExpirationSeconds { get; set; }
            = 10;
    }
}
