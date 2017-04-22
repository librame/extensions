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
    /// 默认 HTML 适配器。
    /// </summary>
    public class DefaultHtmlAdapter : AbstractHtmlAdapter, IHtmlAdapter
    {
        /// <summary>
        /// 解析 HTML 文件。
        /// </summary>
        /// <param name="path">给定的文件路径。</param>
        /// <param name="action">给定的操作方法。</param>
        public virtual void ParseFile(string path, Action<HtmlDocument> action)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.Load(path);

            action?.Invoke(doc);

            //foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href"])
            //{
            //    HtmlAttribute att = link["href"];
            //    att.Value = FixLink(att);
            //}

            //doc.Save(path);
        }


        /// <summary>
        /// 解析网站。
        /// </summary>
        /// <param name="url">给定的 URL。</param>
        /// <param name="action">给定的操作方法。</param>
        public virtual void ParseWeb(string url, Action<HtmlDocument> action)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);

            action?.Invoke(doc);

            //foreach (HtmlNode link in doc.DocumentNode.SelectNodes("/html/body/div[2]/form/div"))
            //{
            //    HtmlAttribute att = link.Attributes["id"];
            //    System.Console.Write(att.Value);
            //}

            //System.Console.ReadKey();
        }

    }
}
