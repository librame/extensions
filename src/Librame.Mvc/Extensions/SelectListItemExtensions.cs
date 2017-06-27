#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Data.Descriptors;
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


        /// <summary>
        /// 将序列中的每个元素投射到选择项列表中。
        /// </summary>
        /// <typeparam name="TSource">指定的源类型。</typeparam>
        /// <param name="sources">给定的源集合。</param>
        /// <param name="valueSelector">给定的值选择器。</param>
        /// <param name="textSelector">给定的文本选择器。</param>
        /// <param name="selectedFactory">给定的默认选中项方法。</param>
        /// <param name="textPrefixFactory">给定的文本前缀方法（如果自定义深度等级文本）。</param>
        /// <param name="defaultResults">给定的默认结果集合。</param>
        /// <param name="addDefaultListItem">增加默认列表项。</param>
        /// <returns>返回结果集合。</returns>
        public static IList<SelectListItem> AsTreeSelectListItems<TSource>(this IEnumerable<TSource> sources,
            Func<TSource, string> valueSelector, Func<TSource, string> textSelector,
            Func<string, string, bool> selectedFactory, Func<TreeingNode<TSource, int>, string> textPrefixFactory = null,
            IList<SelectListItem> defaultResults = null, bool addDefaultListItem = true)
            where TSource : class, IParentIdDescriptor<int>
        {
            return sources.AsTreeSelectListItems<TSource, int>(valueSelector, textSelector,
                selectedFactory, textPrefixFactory, defaultResults, addDefaultListItem);
        }
        /// <summary>
        /// 将序列中的每个元素投射到选择项列表中。
        /// </summary>
        /// <typeparam name="TSource">指定的源类型。</typeparam>
        /// <typeparam name="TId">指定的主键类型。</typeparam>
        /// <param name="sources">给定的源集合。</param>
        /// <param name="valueSelector">给定的值选择器。</param>
        /// <param name="textSelector">给定的文本选择器。</param>
        /// <param name="selectedFactory">给定的默认选中项方法。</param>
        /// <param name="textPrefixFactory">给定的文本前缀方法（如果自定义深度等级文本）。</param>
        /// <param name="defaultResults">给定的默认结果集合。</param>
        /// <param name="addDefaultListItem">增加默认列表项。</param>
        /// <returns>返回结果集合。</returns>
        public static IList<SelectListItem> AsTreeSelectListItems<TSource, TId>(this IEnumerable<TSource> sources,
            Func<TSource, string> valueSelector, Func<TSource, string> textSelector,
            Func<string, string, bool> selectedFactory, Func<TreeingNode<TSource, TId>, string> textPrefixFactory = null,
            IList<SelectListItem> defaultResults = null, bool addDefaultListItem = true)
            where TSource : class, IParentIdDescriptor<TId>
            where TId : struct
        {
            if (sources == null || sources.Count() < 1)
                return defaultResults;

            if (textPrefixFactory == null)
            {
                textPrefixFactory = node =>
                {
                    if (node.DepthLevel < 1)
                        return string.Empty;

                    return ("|" + new string('-', node.DepthLevel * 2));
                };
            }

            var treeSources = new TreeingList<TSource, TId>(sources);

            var list = treeSources.NonstratifiedNodes.Select(s =>
            {
                var textPrefix = textPrefixFactory.Invoke(s);
                var text = textPrefix + textSelector.Invoke(s.Item);
                var value = valueSelector.Invoke(s.Item);

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
