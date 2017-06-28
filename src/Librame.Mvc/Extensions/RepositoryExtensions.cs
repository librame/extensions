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
using System.Linq;
using System.Linq.Expressions;

namespace System.Web.Mvc
{
    /// <summary>
    /// 仓库静态扩展。
    /// </summary>
    public static class RepositoryExtensions
    {

        #region SelectList

        /// <summary>
        /// 获取选择列表。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="repository">给定的仓库。</param>
        /// <param name="textSelector">给定的文本选择器。</param>
        /// <param name="predicate">给定的查询表达式（可选；如果为空，则查询所有数据）。</param>
        /// <param name="addDefaultListItem">增加默认列表项。</param>
        /// <returns>返回选择列表。</returns>
        public static SelectList GetSelectList<T>(this IRepository<T> repository,
            Func<T, string> textSelector,
            Expression<Func<T, bool>> predicate = null,
            bool addDefaultListItem = true)
            where T : class, IIdDescriptor<int>
        {
            // 无选中项
            return repository.GetSelectList(textSelector,
                (value, text) => false,
                predicate,
                addDefaultListItem);
        }
        /// <summary>
        /// 获取选择列表。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="repository">给定的仓库。</param>
        /// <param name="textSelector">给定的文本选择器。</param>
        /// <param name="selectedIds">给定的选中编号集合（多个编号以英文逗号分隔）。</param>
        /// <param name="predicate">给定的查询表达式（可选；如果为空，则查询所有数据）。</param>
        /// <param name="addDefaultListItem">增加默认列表项。</param>
        /// <returns>返回选择列表。</returns>
        public static SelectList GetSelectList<T>(this IRepository<T> repository,
            Func<T, string> textSelector,
            string selectedIds,
            Expression<Func<T, bool>> predicate = null,
            bool addDefaultListItem = true)
            where T : class, IIdDescriptor<int>
        {
            // 默认选中项
            var selectedValues = new string[] { "0" };

            if (!string.IsNullOrEmpty(selectedIds))
                selectedValues = selectedIds.Split(',');

            return repository.GetSelectList(textSelector,
                (value, text) => selectedValues.Contains(value),
                predicate,
                addDefaultListItem);
        }
        /// <summary>
        /// 获取选择列表。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="repository">给定的仓库。</param>
        /// <param name="textSelector">给定的文本选择器。</param>
        /// <param name="selectedId">给定的选中编号。</param>
        /// <param name="predicate">给定的查询表达式（可选；如果为空，则查询所有数据）。</param>
        /// <param name="addDefaultListItem">增加默认列表项。</param>
        /// <returns>返回选择列表。</returns>
        public static SelectList GetSelectList<T>(this IRepository<T> repository,
            Func<T, string> textSelector,
            int selectedId,
            Expression<Func<T, bool>> predicate = null,
            bool addDefaultListItem = true)
            where T : class, IIdDescriptor<int>
        {
            var selectedValue = selectedId.ToString();

            return repository.GetSelectList(textSelector,
                (value, text) => selectedValue == value,
                predicate,
                addDefaultListItem);
        }
        /// <summary>
        /// 获取选择列表。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="repository">给定的仓库。</param>
        /// <param name="textSelector">给定的文本选择器。</param>
        /// <param name="selectedFactory">给定的选中方法。</param>
        /// <param name="predicate">给定的查询表达式（可选；如果为空，则查询所有数据）。</param>
        /// <param name="addDefaultListItem">增加默认列表项。</param>
        /// <returns>返回选择列表。</returns>
        public static SelectList GetSelectList<T>(this IRepository<T> repository,
            Func<T, string> textSelector,
            Func<string, string, bool> selectedFactory,
            Expression<Func<T, bool>> predicate = null,
            bool addDefaultListItem = true)
            where T : class, IIdDescriptor<int>
        {
            var items = repository.GetMany(predicate);

            var selectListItems = items.AsSelectListItems(v => v.Id.ToString(),
                textSelector,
                selectedFactory,
                null /* defaultSelectListItems */,
                addDefaultListItem);

            return new SelectList(selectListItems, "Value", "Text");
        }

        /// <summary>
        /// 获取选择列表。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <typeparam name="TId">指定的主键类型。</typeparam>
        /// <param name="repository">给定的仓库。</param>
        /// <param name="textSelector">给定的文本选择器。</param>
        /// <param name="predicate">给定的查询表达式（可选；如果为空，则查询所有数据）。</param>
        /// <param name="addDefaultListItem">增加默认列表项。</param>
        /// <returns>返回选择列表。</returns>
        public static SelectList GetSelectList<T, TId>(this IRepository<T> repository,
            Func<T, string> textSelector,
            Expression<Func<T, bool>> predicate = null,
            bool addDefaultListItem = true)
            where T : class, IIdDescriptor<TId>
            where TId : struct
        {
            // 无选中项
            return repository.GetSelectList<T, TId>(textSelector,
                (value, text) => false,
                predicate,
                addDefaultListItem);
        }
        /// <summary>
        /// 获取选择列表。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <typeparam name="TId">指定的主键类型。</typeparam>
        /// <param name="repository">给定的仓库。</param>
        /// <param name="textSelector">给定的文本选择器。</param>
        /// <param name="selectedIds">给定的选中编号集合（多个编号以英文逗号分隔）。</param>
        /// <param name="predicate">给定的查询表达式（可选；如果为空，则查询所有数据）。</param>
        /// <param name="addDefaultListItem">增加默认列表项。</param>
        /// <returns>返回选择列表。</returns>
        public static SelectList GetSelectList<T, TId>(this IRepository<T> repository,
            Func<T, string> textSelector,
            string selectedIds,
            Expression<Func<T, bool>> predicate = null,
            bool addDefaultListItem = true)
            where T : class, IIdDescriptor<TId>
            where TId : struct
        {
            // 默认选中项
            var selectedValues = new string[] { "0" };

            if (!string.IsNullOrEmpty(selectedIds))
                selectedValues = selectedIds.Split(',');

            return repository.GetSelectList<T, TId>(textSelector,
                (value, text) => selectedValues.Contains(value),
                predicate,
                addDefaultListItem);
        }
        /// <summary>
        /// 获取选择列表。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <typeparam name="TId">指定的主键类型。</typeparam>
        /// <param name="repository">给定的仓库。</param>
        /// <param name="textSelector">给定的文本选择器。</param>
        /// <param name="selectedId">给定的选中编号。</param>
        /// <param name="predicate">给定的查询表达式（可选；如果为空，则查询所有数据）。</param>
        /// <param name="addDefaultListItem">增加默认列表项。</param>
        /// <returns>返回选择列表。</returns>
        public static SelectList GetSelectList<T, TId>(this IRepository<T> repository,
            Func<T, string> textSelector,
            TId selectedId,
            Expression<Func<T, bool>> predicate = null,
            bool addDefaultListItem = true)
            where T : class, IIdDescriptor<TId>
            where TId : struct
        {
            var selectedValue = selectedId.ToString();

            return repository.GetSelectList<T, TId>(textSelector,
                (value, text) => selectedValue == value,
                predicate,
                addDefaultListItem);
        }
        /// <summary>
        /// 获取选择列表。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <typeparam name="TId">指定的主键类型。</typeparam>
        /// <param name="repository">给定的仓库。</param>
        /// <param name="textSelector">给定的文本选择器。</param>
        /// <param name="selectedFactory">给定的选中方法。</param>
        /// <param name="predicate">给定的查询表达式（可选；如果为空，则查询所有数据）。</param>
        /// <param name="addDefaultListItem">增加默认列表项。</param>
        /// <returns>返回选择列表。</returns>
        public static SelectList GetSelectList<T, TId>(this IRepository<T> repository,
            Func<T, string> textSelector,
            Func<string, string, bool> selectedFactory,
            Expression<Func<T, bool>> predicate = null,
            bool addDefaultListItem = true)
            where T : class, IIdDescriptor<TId>
            where TId : struct
        {
            var items = repository.GetMany(predicate);

            var selectListItems = items.AsSelectListItems(v => v.Id.ToString(),
                textSelector,
                selectedFactory,
                null /* defaultSelectListItems */,
                addDefaultListItem);

            return new SelectList(selectListItems, "Value", "Text");
        }


        /// <summary>
        /// 获取数据状态为开放的选择列表。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="repository">给定的仓库。</param>
        /// <param name="textSelector">给定的文本选择器。</param>
        /// <param name="addDefaultListItem">增加默认列表项。</param>
        /// <returns>返回选择列表。</returns>
        public static SelectList GetPublicSelectList<T>(this IRepository<T> repository,
            Func<T, string> textSelector,
            bool addDefaultListItem = true)
            where T : class, IDataIdDescriptor<int>
        {
            return repository.GetSelectList(textSelector,
                p => p.DataStatus == DataStatus.Public,
                addDefaultListItem);
        }
        /// <summary>
        /// 获取选择列表。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="repository">给定的仓库。</param>
        /// <param name="textSelector">给定的文本选择器。</param>
        /// <param name="selectedIds">给定的选中编号集合（多个编号以英文逗号分隔）。</param>
        /// <param name="addDefaultListItem">增加默认列表项。</param>
        /// <returns>返回选择列表。</returns>
        public static SelectList GetPublicSelectList<T>(this IRepository<T> repository,
            Func<T, string> textSelector,
            string selectedIds,
            bool addDefaultListItem = true)
            where T : class, IDataIdDescriptor<int>
        {
            return repository.GetSelectList(textSelector,
                selectedIds,
                p => p.DataStatus == DataStatus.Public,
                addDefaultListItem);
        }
        /// <summary>
        /// 获取数据状态为开放的选择列表。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="repository">给定的仓库。</param>
        /// <param name="textSelector">给定的文本选择器。</param>
        /// <param name="selectedId">给定的选中编号。</param>
        /// <param name="addDefaultListItem">增加默认列表项。</param>
        /// <returns>返回选择列表。</returns>
        public static SelectList GetPublicSelectList<T>(this IRepository<T> repository,
            Func<T, string> textSelector,
            int selectedId,
            bool addDefaultListItem = true)
            where T : class, IDataIdDescriptor<int>
        {
            return repository.GetSelectList(textSelector,
                selectedId,
                p => p.DataStatus == DataStatus.Public,
                addDefaultListItem);
        }
        /// <summary>
        /// 获取数据状态为开放的选择列表。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="repository">给定的仓库。</param>
        /// <param name="textSelector">给定的文本选择器。</param>
        /// <param name="selectedFactory">给定的选中方法。</param>
        /// <param name="addDefaultListItem">增加默认列表项。</param>
        /// <returns>返回选择列表。</returns>
        public static SelectList GetPublicSelectList<T>(this IRepository<T> repository,
            Func<T, string> textSelector,
            Func<string, string, bool> selectedFactory,
            bool addDefaultListItem = true)
            where T : class, IDataIdDescriptor<int>
        {
            return repository.GetSelectList(textSelector,
                selectedFactory,
                p => p.DataStatus == DataStatus.Public,
                addDefaultListItem);
        }

        /// <summary>
        /// 获取数据状态为开放的选择列表。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <typeparam name="TId">指定的主键类型。</typeparam>
        /// <param name="repository">给定的仓库。</param>
        /// <param name="textSelector">给定的文本选择器。</param>
        /// <param name="addDefaultListItem">增加默认列表项。</param>
        /// <returns>返回选择列表。</returns>
        public static SelectList GetPublicSelectList<T, TId>(this IRepository<T> repository,
            Func<T, string> textSelector,
            bool addDefaultListItem = true)
            where T : class, IDataIdDescriptor<TId>
            where TId : struct
        {
            return repository.GetSelectList<T, TId>(textSelector,
                p => p.DataStatus == DataStatus.Public,
                addDefaultListItem);
        }
        /// <summary>
        /// 获取选择列表。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <typeparam name="TId">指定的主键类型。</typeparam>
        /// <param name="repository">给定的仓库。</param>
        /// <param name="textSelector">给定的文本选择器。</param>
        /// <param name="selectedIds">给定的选中编号集合（多个编号以英文逗号分隔）。</param>
        /// <param name="addDefaultListItem">增加默认列表项。</param>
        /// <returns>返回选择列表。</returns>
        public static SelectList GetPublicSelectList<T, TId>(this IRepository<T> repository,
            Func<T, string> textSelector,
            string selectedIds,
            bool addDefaultListItem = true)
            where T : class, IDataIdDescriptor<TId>
            where TId : struct
        {
            return repository.GetSelectList<T, TId>(textSelector,
                selectedIds,
                p => p.DataStatus == DataStatus.Public,
                addDefaultListItem);
        }
        /// <summary>
        /// 获取数据状态为开放的选择列表。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <typeparam name="TId">指定的主键类型。</typeparam>
        /// <param name="repository">给定的仓库。</param>
        /// <param name="textSelector">给定的文本选择器。</param>
        /// <param name="selectedId">给定的选中编号。</param>
        /// <param name="addDefaultListItem">增加默认列表项。</param>
        /// <returns>返回选择列表。</returns>
        public static SelectList GetPublicSelectList<T, TId>(this IRepository<T> repository,
            Func<T, string> textSelector,
            TId selectedId,
            bool addDefaultListItem = true)
            where T : class, IDataIdDescriptor<TId>
            where TId : struct
        {
            return repository.GetSelectList<T, TId>(textSelector,
                selectedId,
                p => p.DataStatus == DataStatus.Public,
                addDefaultListItem);
        }
        /// <summary>
        /// 获取数据状态为开放的选择列表。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <typeparam name="TId">指定的主键类型。</typeparam>
        /// <param name="repository">给定的仓库。</param>
        /// <param name="textSelector">给定的文本选择器。</param>
        /// <param name="selectedFactory">给定的选中方法。</param>
        /// <param name="addDefaultListItem">增加默认列表项。</param>
        /// <returns>返回选择列表。</returns>
        public static SelectList GetPublicSelectList<T, TId>(this IRepository<T> repository,
            Func<T, string> textSelector,
            Func<string, string, bool> selectedFactory,
            bool addDefaultListItem = true)
            where T : class, IDataIdDescriptor<TId>
            where TId : struct
        {
            return repository.GetSelectList<T, TId>(textSelector,
                selectedFactory,
                p => p.DataStatus == DataStatus.Public,
                addDefaultListItem);
        }

        #endregion


        #region TreeSelectList

        /// <summary>
        /// 获取树形选择列表。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="repository">给定的仓库。</param>
        /// <param name="textSelector">给定的文本选择器。</param>
        /// <param name="predicate">给定的查询表达式（可选；如果为空，则查询所有数据）。</param>
        /// <param name="textPrefixFactory">给定的文本前缀方法（可选；如自定义深度等级文本）。</param>
        /// <param name="addDefaultListItem">增加默认列表项。</param>
        /// <returns>返回树形选择列表。</returns>
        public static SelectList GetTreeSelectList<T>(this IRepository<T> repository,
            Func<T, string> textSelector,
            Expression<Func<T, bool>> predicate = null,
            Func<TreeingNode<T, int>, string> textPrefixFactory = null,
            bool addDefaultListItem = true)
            where T : class, IParentIdDescriptor<int>
        {
            // 无选中项
            return repository.GetTreeSelectList(textSelector,
                (value, text) => false,
                predicate,
                textPrefixFactory,
                addDefaultListItem);
        }
        /// <summary>
        /// 获取树形选择列表。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="repository">给定的仓库。</param>
        /// <param name="textSelector">给定的文本选择器。</param>
        /// <param name="selectedIds">给定的选中编号集合（多个编号以英文逗号分隔）。</param>
        /// <param name="predicate">给定的查询表达式（可选；如果为空，则查询所有数据）。</param>
        /// <param name="textPrefixFactory">给定的文本前缀方法（可选；如自定义深度等级文本）。</param>
        /// <param name="addDefaultListItem">增加默认列表项。</param>
        /// <returns>返回树形选择列表。</returns>
        public static SelectList GetTreeSelectList<T>(this IRepository<T> repository,
            Func<T, string> textSelector,
            string selectedIds,
            Expression<Func<T, bool>> predicate = null,
            Func<TreeingNode<T, int>, string> textPrefixFactory = null,
            bool addDefaultListItem = true)
            where T : class, IParentIdDescriptor<int>
        {
            // 默认选中项
            var selectedValues = new string[] { "0" };

            if (!string.IsNullOrEmpty(selectedIds))
                selectedValues = selectedIds.Split(',');

            return repository.GetTreeSelectList(textSelector,
                (value, text) => selectedValues.Contains(value),
                predicate,
                textPrefixFactory,
                addDefaultListItem);
        }
        /// <summary>
        /// 获取树形选择列表。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="repository">给定的仓库。</param>
        /// <param name="textSelector">给定的文本选择器。</param>
        /// <param name="selectedId">给定的选中编号。</param>
        /// <param name="predicate">给定的查询表达式（可选；如果为空，则查询所有数据）。</param>
        /// <param name="textPrefixFactory">给定的文本前缀方法（可选；如自定义深度等级文本）。</param>
        /// <param name="addDefaultListItem">增加默认列表项。</param>
        /// <returns>返回树形选择列表。</returns>
        public static SelectList GetTreeSelectList<T>(this IRepository<T> repository,
            Func<T, string> textSelector,
            int selectedId,
            Expression<Func<T, bool>> predicate = null,
            Func<TreeingNode<T, int>, string> textPrefixFactory = null,
            bool addDefaultListItem = true)
            where T : class, IParentIdDescriptor<int>
        {
            var selectedValue = selectedId.ToString();

            return repository.GetTreeSelectList(textSelector,
                (value, text) => selectedValue == value,
                predicate,
                textPrefixFactory,
                addDefaultListItem);
        }
        /// <summary>
        /// 获取树形选择列表。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="repository">给定的仓库。</param>
        /// <param name="textSelector">给定的文本选择器。</param>
        /// <param name="selectedFactory">给定的选中方法。</param>
        /// <param name="predicate">给定的查询表达式（可选；如果为空，则查询所有数据）。</param>
        /// <param name="textPrefixFactory">给定的文本前缀方法（可选；如自定义深度等级文本）。</param>
        /// <param name="addDefaultListItem">增加默认列表项。</param>
        /// <returns>返回树形选择列表。</returns>
        public static SelectList GetTreeSelectList<T>(this IRepository<T> repository,
            Func<T, string> textSelector,
            Func<string, string, bool> selectedFactory,
            Expression<Func<T, bool>> predicate = null,
            Func<TreeingNode<T, int>, string> textPrefixFactory = null,
            bool addDefaultListItem = true)
            where T : class, IParentIdDescriptor<int>
        {
            var items = repository.GetMany(predicate);

            var selectListItems = items.AsTreeSelectListItems(v => v.Id.ToString(),
                textSelector,
                selectedFactory,
                textPrefixFactory,
                null /* defaultSelectListItems */,
                addDefaultListItem);

            return new SelectList(selectListItems, "Value", "Text");
        }

        /// <summary>
        /// 获取树形选择列表。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <typeparam name="TId">指定的主键类型。</typeparam>
        /// <param name="repository">给定的仓库。</param>
        /// <param name="textSelector">给定的文本选择器。</param>
        /// <param name="predicate">给定的查询表达式（可选；如果为空，则查询所有数据）。</param>
        /// <param name="textPrefixFactory">给定的文本前缀方法（可选；如自定义深度等级文本）。</param>
        /// <param name="addDefaultListItem">增加默认列表项。</param>
        /// <returns>返回树形选择列表。</returns>
        public static SelectList GetTreeSelectList<T, TId>(this IRepository<T> repository,
            Func<T, string> textSelector,
            Expression<Func<T, bool>> predicate = null,
            Func<TreeingNode<T, TId>, string> textPrefixFactory = null,
            bool addDefaultListItem = true)
            where T : class, IParentIdDescriptor<TId>
            where TId : struct
        {
            // 无选中项
            return repository.GetTreeSelectList(textSelector,
                (value, text) => false,
                predicate,
                textPrefixFactory,
                addDefaultListItem);
        }
        /// <summary>
        /// 获取树形选择列表。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <typeparam name="TId">指定的主键类型。</typeparam>
        /// <param name="repository">给定的仓库。</param>
        /// <param name="textSelector">给定的文本选择器。</param>
        /// <param name="selectedIds">给定的选中编号集合（多个编号以英文逗号分隔）。</param>
        /// <param name="predicate">给定的查询表达式（可选；如果为空，则查询所有数据）。</param>
        /// <param name="textPrefixFactory">给定的文本前缀方法（可选；如自定义深度等级文本）。</param>
        /// <param name="addDefaultListItem">增加默认列表项。</param>
        /// <returns>返回树形选择列表。</returns>
        public static SelectList GetTreeSelectList<T, TId>(this IRepository<T> repository,
            Func<T, string> textSelector,
            string selectedIds,
            Expression<Func<T, bool>> predicate = null,
            Func<TreeingNode<T, TId>, string> textPrefixFactory = null,
            bool addDefaultListItem = true)
            where T : class, IParentIdDescriptor<TId>
            where TId : struct
        {
            // 默认选中项
            var selectedValues = new string[] { "0" };

            if (!string.IsNullOrEmpty(selectedIds))
                selectedValues = selectedIds.Split(',');

            return repository.GetTreeSelectList(textSelector,
                (value, text) => selectedValues.Contains(value),
                predicate,
                textPrefixFactory,
                addDefaultListItem);
        }
        /// <summary>
        /// 获取树形选择列表。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <typeparam name="TId">指定的主键类型。</typeparam>
        /// <param name="repository">给定的仓库。</param>
        /// <param name="textSelector">给定的文本选择器。</param>
        /// <param name="selectedId">给定的选中编号。</param>
        /// <param name="predicate">给定的查询表达式（可选；如果为空，则查询所有数据）。</param>
        /// <param name="textPrefixFactory">给定的文本前缀方法（可选；如自定义深度等级文本）。</param>
        /// <param name="addDefaultListItem">增加默认列表项。</param>
        /// <returns>返回树形选择列表。</returns>
        public static SelectList GetTreeSelectList<T, TId>(this IRepository<T> repository,
            Func<T, string> textSelector,
            TId selectedId,
            Expression<Func<T, bool>> predicate = null,
            Func<TreeingNode<T, TId>, string> textPrefixFactory = null,
            bool addDefaultListItem = true)
            where T : class, IParentIdDescriptor<TId>
            where TId : struct
        {
            var selectedValue = selectedId.ToString();

            return repository.GetTreeSelectList(textSelector,
                (value, text) => selectedValue == value,
                predicate,
                textPrefixFactory,
                addDefaultListItem);
        }
        /// <summary>
        /// 获取树形选择列表。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <typeparam name="TId">指定的主键类型。</typeparam>
        /// <param name="repository">给定的仓库。</param>
        /// <param name="textSelector">给定的文本选择器。</param>
        /// <param name="selectedFactory">给定的选中方法。</param>
        /// <param name="predicate">给定的查询表达式（可选；如果为空，则查询所有数据）。</param>
        /// <param name="textPrefixFactory">给定的文本前缀方法（可选；如自定义深度等级文本）。</param>
        /// <param name="addDefaultListItem">增加默认列表项。</param>
        /// <returns>返回树形选择列表。</returns>
        public static SelectList GetTreeSelectList<T, TId>(this IRepository<T> repository,
            Func<T, string> textSelector,
            Func<string, string, bool> selectedFactory,
            Expression<Func<T, bool>> predicate = null,
            Func<TreeingNode<T, TId>, string> textPrefixFactory = null,
            bool addDefaultListItem = true)
            where T : class, IParentIdDescriptor<TId>
            where TId : struct
        {
            var items = repository.GetMany(predicate);

            var selectListItems = items.AsTreeSelectListItems(v => v.Id.ToString(),
                textSelector,
                selectedFactory,
                textPrefixFactory,
                null /* defaultSelectListItems */,
                addDefaultListItem);

            return new SelectList(selectListItems, "Value", "Text");
        }


        /// <summary>
        /// 获取数据状态为开放的树形选择列表。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="repository">给定的仓库。</param>
        /// <param name="textSelector">给定的文本选择器。</param>
        /// <param name="textPrefixFactory">给定的文本前缀方法（可选；如自定义深度等级文本）。</param>
        /// <param name="addDefaultListItem">增加默认列表项。</param>
        /// <returns>返回树形选择列表。</returns>
        public static SelectList GetPublicTreeSelectList<T>(this IRepository<T> repository,
            Func<T, string> textSelector,
            Func<TreeingNode<T, int>, string> textPrefixFactory = null,
            bool addDefaultListItem = true)
            where T : class, IDataIdDescriptor<int>, IParentIdDescriptor<int>
        {
            return repository.GetTreeSelectList(textSelector,
                p => p.DataStatus == DataStatus.Public,
                textPrefixFactory,
                addDefaultListItem);
        }
        /// <summary>
        /// 获取树形选择列表。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="repository">给定的仓库。</param>
        /// <param name="textSelector">给定的文本选择器。</param>
        /// <param name="selectedIds">给定的选中编号集合（多个编号以英文逗号分隔）。</param>
        /// <param name="textPrefixFactory">给定的文本前缀方法（可选；如自定义深度等级文本）。</param>
        /// <param name="addDefaultListItem">增加默认列表项。</param>
        /// <returns>返回树形选择列表。</returns>
        public static SelectList GetPublicTreeSelectList<T>(this IRepository<T> repository,
            Func<T, string> textSelector,
            string selectedIds,
            Func<TreeingNode<T, int>, string> textPrefixFactory = null,
            bool addDefaultListItem = true)
            where T : class, IDataIdDescriptor<int>, IParentIdDescriptor<int>
        {
            return repository.GetTreeSelectList(textSelector,
                selectedIds,
                p => p.DataStatus == DataStatus.Public,
                textPrefixFactory,
                addDefaultListItem);
        }
        /// <summary>
        /// 获取数据状态为开放的树形选择列表。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="repository">给定的仓库。</param>
        /// <param name="textSelector">给定的文本选择器。</param>
        /// <param name="selectedId">给定的选中编号。</param>
        /// <param name="textPrefixFactory">给定的文本前缀方法（可选；如自定义深度等级文本）。</param>
        /// <param name="addDefaultListItem">增加默认列表项。</param>
        /// <returns>返回树形选择列表。</returns>
        public static SelectList GetPublicTreeSelectList<T>(this IRepository<T> repository,
            Func<T, string> textSelector,
            int selectedId,
            Func<TreeingNode<T, int>, string> textPrefixFactory = null,
            bool addDefaultListItem = true)
            where T : class, IDataIdDescriptor<int>, IParentIdDescriptor<int>
        {
            return repository.GetTreeSelectList(textSelector,
                selectedId,
                p => p.DataStatus == DataStatus.Public,
                textPrefixFactory,
                addDefaultListItem);
        }
        /// <summary>
        /// 获取数据状态为开放的树形选择列表。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="repository">给定的仓库。</param>
        /// <param name="textSelector">给定的文本选择器。</param>
        /// <param name="selectedFactory">给定的选中方法。</param>
        /// <param name="textPrefixFactory">给定的文本前缀方法（可选；如自定义深度等级文本）。</param>
        /// <param name="addDefaultListItem">增加默认列表项。</param>
        /// <returns>返回树形选择列表。</returns>
        public static SelectList GetPublicTreeSelectList<T>(this IRepository<T> repository,
            Func<T, string> textSelector,
            Func<string, string, bool> selectedFactory,
            Func<TreeingNode<T, int>, string> textPrefixFactory = null,
            bool addDefaultListItem = true)
            where T : class, IDataIdDescriptor<int>, IParentIdDescriptor<int>
        {
            return repository.GetTreeSelectList(textSelector,
                selectedFactory,
                p => p.DataStatus == DataStatus.Public,
                textPrefixFactory,
                addDefaultListItem);
        }

        /// <summary>
        /// 获取数据状态为开放的树形选择列表。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <typeparam name="TId">指定的主键类型。</typeparam>
        /// <param name="repository">给定的仓库。</param>
        /// <param name="textSelector">给定的文本选择器。</param>
        /// <param name="textPrefixFactory">给定的文本前缀方法（可选；如自定义深度等级文本）。</param>
        /// <param name="addDefaultListItem">增加默认列表项。</param>
        /// <returns>返回树形选择列表。</returns>
        public static SelectList GetPublicTreeSelectList<T, TId>(this IRepository<T> repository,
            Func<T, string> textSelector,
            Func<TreeingNode<T, TId>, string> textPrefixFactory = null,
            bool addDefaultListItem = true)
            where T : class, IDataIdDescriptor<TId>, IParentIdDescriptor<TId>
            where TId : struct
        {
            return repository.GetTreeSelectList(textSelector,
                p => p.DataStatus == DataStatus.Public,
                textPrefixFactory,
                addDefaultListItem);
        }
        /// <summary>
        /// 获取树形选择列表。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <typeparam name="TId">指定的主键类型。</typeparam>
        /// <param name="repository">给定的仓库。</param>
        /// <param name="textSelector">给定的文本选择器。</param>
        /// <param name="selectedIds">给定的选中编号集合（多个编号以英文逗号分隔）。</param>
        /// <param name="textPrefixFactory">给定的文本前缀方法（可选；如自定义深度等级文本）。</param>
        /// <param name="addDefaultListItem">增加默认列表项。</param>
        /// <returns>返回树形选择列表。</returns>
        public static SelectList GetPublicTreeSelectList<T, TId>(this IRepository<T> repository,
            Func<T, string> textSelector,
            string selectedIds,
            Func<TreeingNode<T, TId>, string> textPrefixFactory = null,
            bool addDefaultListItem = true)
            where T : class, IDataIdDescriptor<TId>, IParentIdDescriptor<TId>
            where TId : struct
        {
            return repository.GetTreeSelectList<T, TId>(textSelector,
                selectedIds,
                p => p.DataStatus == DataStatus.Public,
                textPrefixFactory,
                addDefaultListItem);
        }
        /// <summary>
        /// 获取数据状态为开放的树形选择列表。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <typeparam name="TId">指定的主键类型。</typeparam>
        /// <param name="repository">给定的仓库。</param>
        /// <param name="textSelector">给定的文本选择器。</param>
        /// <param name="selectedId">给定的选中编号。</param>
        /// <param name="textPrefixFactory">给定的文本前缀方法（可选；如自定义深度等级文本）。</param>
        /// <param name="addDefaultListItem">增加默认列表项。</param>
        /// <returns>返回树形选择列表。</returns>
        public static SelectList GetPublicTreeSelectList<T, TId>(this IRepository<T> repository,
            Func<T, string> textSelector,
            TId selectedId,
            Func<TreeingNode<T, TId>, string> textPrefixFactory = null,
            bool addDefaultListItem = true)
            where T : class, IDataIdDescriptor<TId>, IParentIdDescriptor<TId>
            where TId : struct
        {
            return repository.GetTreeSelectList<T, TId>(textSelector,
                selectedId,
                p => p.DataStatus == DataStatus.Public,
                textPrefixFactory,
                addDefaultListItem);
        }
        /// <summary>
        /// 获取数据状态为开放的树形选择列表。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <typeparam name="TId">指定的主键类型。</typeparam>
        /// <param name="repository">给定的仓库。</param>
        /// <param name="textSelector">给定的文本选择器。</param>
        /// <param name="selectedFactory">给定的选中方法。</param>
        /// <param name="textPrefixFactory">给定的文本前缀方法（可选；如自定义深度等级文本）。</param>
        /// <param name="addDefaultListItem">增加默认列表项。</param>
        /// <returns>返回树形选择列表。</returns>
        public static SelectList GetPublicTreeSelectList<T, TId>(this IRepository<T> repository,
            Func<T, string> textSelector,
            Func<string, string, bool> selectedFactory,
            Func<TreeingNode<T, TId>, string> textPrefixFactory = null,
            bool addDefaultListItem = true)
            where T : class, IDataIdDescriptor<TId>, IParentIdDescriptor<TId>
            where TId : struct
        {
            return repository.GetTreeSelectList<T, TId>(textSelector,
                selectedFactory,
                p => p.DataStatus == DataStatus.Public,
                textPrefixFactory,
                addDefaultListItem);
        }

        #endregion

    }
}
