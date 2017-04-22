#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using HtmlAgilityPack;
using System;

namespace Librame.Html
{
    /// <summary>
    /// HTML 适配器接口。
    /// </summary>
    public interface IHtmlAdapter
    {
        /// <summary>
        /// 解析 HTML 文件。
        /// </summary>
        /// <param name="path">给定的文件路径。</param>
        /// <param name="action">给定的操作方法。</param>
        void ParseFile(string path, Action<HtmlDocument> action);

        /// <summary>
        /// 解析网站。
        /// </summary>
        /// <param name="url">给定的 URL。</param>
        /// <param name="action">给定的操作方法。</param>
        void ParseWeb(string url, Action<HtmlDocument> action);
    }
}
