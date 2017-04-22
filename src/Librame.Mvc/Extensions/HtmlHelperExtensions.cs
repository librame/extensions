#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Authorization;
using Librame.Data;
using Librame.Utility;

namespace System.Web.Mvc
{
    /// <summary>
    /// <see cref="HtmlHelper"/> 静态扩展。
    /// </summary>
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// 获取数据状态枚举的带样式描述内容。
        /// </summary>
        /// <param name="htmlHelper">给定的 <see cref="HtmlHelper"/>。</param>
        /// <param name="status">给定的数据状态。</param>
        /// <returns>返回 <see cref="MvcHtmlString"/>。</returns>
        public static MvcHtmlString DescriptionWithStyle(this HtmlHelper htmlHelper, DataStatus status)
        {
            var descrWithStyle = EnumStyleUtility.GetDescriptionWithStyle(status);

            return new MvcHtmlString(descrWithStyle);
        }


        /// <summary>
        /// 获取数据状态枚举的带样式描述内容。
        /// </summary>
        /// <param name="htmlHelper">给定的 <see cref="HtmlHelper"/>。</param>
        /// <param name="status">给定的数据状态。</param>
        /// <returns>返回 <see cref="MvcHtmlString"/>。</returns>
        public static MvcHtmlString DescriptionWithStyle(this HtmlHelper htmlHelper, AccountStatus status)
        {
            var descrWithStyle = EnumStyleUtility.GetDescriptionWithStyle(status);

            return new MvcHtmlString(descrWithStyle);
        }

    }
}
