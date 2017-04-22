#region License

/* **************************************************************************************
 * Copyright (c) zwbwl All rights reserved.
 * 
 * http://51zwb.com
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Data;
using Librame.Data.Descriptors;
using System.Collections.Generic;

namespace System.Web.Mvc
{
    /// <summary>
    /// 仓库静态扩展。
    /// </summary>
    public static class RepositoryExtensions
    {
        /// <summary>
        /// 获取选择列表。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="repository">给定的仓库。</param>
        /// <param name="textSelector">给定的文本选择器。</param>
        /// <param name="addDefaultListItem">增加默认列表项。</param>
        /// <returns>返回选择列表项集合。</returns>
        public static IList<SelectListItem> GetSelectList<T>(this IRepository<T> repository,
            Func<T, string> textSelector, bool addDefaultListItem = true)
            where T : IIdDescriptor<int>
        {
            // 无选中项
            return repository.GetSelectList(textSelector,
                (value, text) => false, addDefaultListItem);
        }
        /// <summary>
        /// 获取选择列表。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="repository">给定的仓库。</param>
        /// <param name="textSelector">给定的文本选择器。</param>
        /// <param name="selectedId">给定的选中编号。</param>
        /// <param name="addDefaultListItem">增加默认列表项。</param>
        /// <returns>返回选择列表项集合。</returns>
        public static IList<SelectListItem> GetSelectList<T>(this IRepository<T> repository,
            Func<T, string> textSelector, int selectedId, bool addDefaultListItem = true)
            where T : IIdDescriptor<int>
        {
            var selectedValue = selectedId.ToString();

            return repository.GetSelectList(textSelector,
                (value, text) => selectedValue == value, addDefaultListItem);
        }
        /// <summary>
        /// 获取选择列表。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="repository">给定的仓库。</param>
        /// <param name="textSelector">给定的文本选择器。</param>
        /// <param name="selectedFactory">给定的选中方法。</param>
        /// <param name="addDefaultListItem">增加默认列表项。</param>
        /// <returns>返回选择列表项集合。</returns>
        public static IList<SelectListItem> GetSelectList<T>(this IRepository<T> repository,
            Func<T, string> textSelector, Func<string, string, bool> selectedFactory,
            bool addDefaultListItem = true)
            where T : IIdDescriptor<int>
        {
            var items = repository.GetMany();

            return items.AsSelectListItems(v => v.Id.ToString(), textSelector,
                selectedFactory, null, addDefaultListItem);
        }


        /// <summary>
        /// 获取数据状态为开放的选择列表。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="repository">给定的仓库。</param>
        /// <param name="textSelector">给定的文本选择器。</param>
        /// <param name="addDefaultListItem">增加默认列表项。</param>
        /// <returns>返回选择列表项集合。</returns>
        public static IList<SelectListItem> GetDataStatusSelectList<T>(this IRepository<T> repository,
            Func<T, string> textSelector, bool addDefaultListItem = true)
            where T : IDataIdDescriptor<int>
        {
            // 无选中项
            return repository.GetDataStatusSelectList(textSelector,
                (value, text) => false, addDefaultListItem);
        }
        /// <summary>
        /// 获取数据状态为开放的选择列表。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="repository">给定的仓库。</param>
        /// <param name="textSelector">给定的文本选择器。</param>
        /// <param name="selectedId">给定的选中编号。</param>
        /// <param name="addDefaultListItem">增加默认列表项。</param>
        /// <returns>返回选择列表项集合。</returns>
        public static IList<SelectListItem> GetDataStatusSelectList<T>(this IRepository<T> repository,
            Func<T, string> textSelector, int selectedId, bool addDefaultListItem = true)
            where T : IDataIdDescriptor<int>
        {
            var selectedValue = selectedId.ToString();

            return repository.GetDataStatusSelectList(textSelector,
                (value, text) => selectedValue == value, addDefaultListItem);
        }
        /// <summary>
        /// 获取数据状态为开放的选择列表。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="repository">给定的仓库。</param>
        /// <param name="textSelector">给定的文本选择器。</param>
        /// <param name="selectedFactory">给定的选中方法。</param>
        /// <param name="addDefaultListItem">增加默认列表项。</param>
        /// <returns>返回选择列表项集合。</returns>
        public static IList<SelectListItem> GetDataStatusSelectList<T>(this IRepository<T> repository,
            Func<T, string> textSelector, Func<string, string, bool> selectedFactory,
            bool addDefaultListItem = true)
            where T : IDataIdDescriptor<int>
        {
            var items = repository.GetMany(p => p.DataStatus == DataStatus.Public);

            return items.AsSelectListItems(v => v.Id.ToString(), textSelector,
                selectedFactory, null, addDefaultListItem);
        }

    }
}
