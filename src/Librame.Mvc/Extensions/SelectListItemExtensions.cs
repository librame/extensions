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
using System.Linq;

namespace System.Web.Mvc
{
    /// <summary>
    /// <see cref="SelectListItem"/> 静态扩展。
    /// </summary>
    public static class SelectListItemExtensions
    {
        /// <summary>
        /// 将序列中的每个元素投射到选择项列表中。
        /// </summary>
        /// <typeparam name="TSource">指定的源类型。</typeparam>
        /// <param name="sources">给定的源集合。</param>
        /// <param name="valueSelector">给定的值选择器。</param>
        /// <param name="textSelector">给定的文本选择器。</param>
        /// <param name="selectedFactory">给定的默认选中项方法。</param>
        /// <param name="defaultResults">给定的默认结果集合。</param>
        /// <param name="addDefaultListItem">增加默认列表项。</param>
        /// <returns>返回结果集合。</returns>
        public static IList<SelectListItem> AsSelectListItems<TSource>(this IEnumerable<TSource> sources,
            Func<TSource, string> valueSelector, Func<TSource, string> textSelector,
            Func<string, string, bool> selectedFactory, IList<SelectListItem> defaultResults = null,
            bool addDefaultListItem = true)
        {
            if (ReferenceEquals(sources, null) || sources.Count() < 1)
                return defaultResults;

            var list = sources.Select(s =>
            {
                var text = textSelector.Invoke(s);
                var value = valueSelector.Invoke(s);

                return new SelectListItem()
                {
                    Text = text,
                    Value = value,
                    Selected = selectedFactory.Invoke(value, text)
                };
            }).ToList();

            // 如果添加默认项
            if (addDefaultListItem)
            {
                list.Insert(0, new SelectListItem()
                {
                    Text = "默认",
                    Value = "0",
                });
            }

            return list;
        }

    }
}
