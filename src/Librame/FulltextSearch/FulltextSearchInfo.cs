#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.FulltextSearch
{
    /// <summary>
    /// 全文检索信息。
    /// </summary>
    public class FulltextSearchInfo
    {
        /// <summary>
        /// 构造一个 <see cref="FulltextSearchInfo"/> 实例。
        /// </summary>
        /// <param name="id">给定的标识。</param>
        /// <param name="title">给定的标题。</param>
        /// <param name="content">给定的内容。</param>
        /// <param name="flag">给定的标记。</param>
        /// <param name="imageUrl">给定的图片路径。</param>
        /// <param name="updateTime">给定的更新时间。</param>
        /// <param name="createTime">给定的创建时间。</param>
        public FulltextSearchInfo(string id, string title, string content, string flag,
            string imageUrl, string updateTime, string createTime)
        {
            Id = id;
            Title = title;
            Content = content;
            Flag = flag;
            ImageUrl = imageUrl;
            UpdateTime = updateTime;
            CreateTime = createTime;
        }

        /// <summary>
        /// 标识。
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// 标题。
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// 内容。
        /// </summary>
        public string Content { get; }

        /// <summary>
        /// 标记。
        /// </summary>
        public string Flag { get; }

        /// <summary>
        /// 图片路径。
        /// </summary>
        public string ImageUrl { get; }

        /// <summary>
        /// 更新时间。
        /// </summary>
        public string UpdateTime { get; }

        /// <summary>
        /// 创建时间。
        /// </summary>
        public string CreateTime { get; }
    }
}
