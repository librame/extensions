#region License

/* **************************************************************************************
 * Copyright (c) zwbwl All rights reserved.
 * 
 * http://51zwb.com
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Utility;
using System.ComponentModel;

namespace System.Web.Mvc
{
    /// <summary>
    /// 动作助手。
    /// </summary>
    public class ActionHelper
    {
        /// <summary>
        /// 返回列表。
        /// </summary>
        public const string RETURN_INDEX = "返回列表";


        /// <summary>
        /// 列表名称。
        /// </summary>
        public const string NAME_INDEX = "Index";
        /// <summary>
        /// 详情名称。
        /// </summary>
        public const string NAME_DETAILS = "Details";
        /// <summary>
        /// 新增名称。
        /// </summary>
        public const string NAME_CREATE = "Create";
        /// <summary>
        /// 修改名称。
        /// </summary>
        public const string NAME_EDIT = "Edit";
        /// <summary>
        /// 删除名称。
        /// </summary>
        public const string NAME_DELETE = "Delete";


        /// <summary>
        /// 列表标题。
        /// </summary>
        public const string TITLE_INDEX = "列表";
        /// <summary>
        /// 详情标题。
        /// </summary>
        public const string TITLE_DETAILS = "详情";
        /// <summary>
        /// 新增标题。
        /// </summary>
        public const string TITLE_CREATE = "新增";
        /// <summary>
        /// 修改标题。
        /// </summary>
        public const string TITLE_EDIT = "修改";
        /// <summary>
        /// 删除标题。
        /// </summary>
        public const string TITLE_DELETE = "删除";


        /// <summary>
        /// 解析动作标题。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="status">给定的动作状态。</param>
        /// <param name="customFactory">给定的自定义解析方法。</param>
        /// <returns>返回标题字符串。</returns>
        public static string ParseTitle<T>(ActionStatus status, Func<string, string> customFactory = null)
        {
            var title = string.Empty;
            var attrib = TypeUtility.GetClassAttribute<T, DescriptionAttribute>();

            if (!ReferenceEquals(attrib, null))
                title = attrib.Description;

            switch (status)
            {
                case ActionStatus.Index:
                    return title + TITLE_INDEX;

                case ActionStatus.Details:
                    return title + TITLE_DETAILS;

                case ActionStatus.Create:
                    return TITLE_CREATE + title;

                case ActionStatus.Edit:
                    return TITLE_EDIT + title;

                case ActionStatus.Delete:
                    return TITLE_DELETE + title;

                default:
                    {
                        if (!ReferenceEquals(customFactory, null))
                            return customFactory?.Invoke(title);

                        return title;
                    }
            }
        }


        /// <summary>
        /// 获取解析动作的标题。
        /// </summary>
        /// <param name="parseTitle">给定的解析动作标题。</param>
        /// <returns>返回标题字符串。</returns>
        public static string GetTitle(string parseTitle)
        {
            var keys = new string[] { TITLE_INDEX, TITLE_DETAILS, TITLE_CREATE, TITLE_EDIT, TITLE_DELETE };

            foreach (var k in keys)
                parseTitle = parseTitle.Replace(k, string.Empty);

            if (parseTitle.EndsWith("管理"))
                return parseTitle;

            return (parseTitle + "管理");
        }

    }
}
