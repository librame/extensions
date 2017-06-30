#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Web.Mvc;

namespace Librame
{
    using Utility;

    /// <summary>
    /// Librame 枚举管理器。
    /// </summary>
    public class LibrameEnumManager : LibrameEnumManager<SelectListItem>
    {

        #region Authorization

        /// <summary>
        /// 获取帐户状态枚举项列表。
        /// </summary>
        /// <param name="status">给定的默认选中项。</param>
        /// <param name="predicate">给定的筛选方法（可选）。</param>
        /// <returns>返回项列表。</returns>
        public static SelectList GetAccountStatusList(Authorization.AccountStatus status,
            Func<SelectListItem, bool> predicate = null)
        {
            string selectedValue = ((int)status).ToString();

            return GetAccountStatusList((value, text) => selectedValue == value, predicate);
        }
        /// <summary>
        /// 获取帐户状态枚举项列表。
        /// </summary>
        /// <param name="selectedFactory">给定的默认选中项方法。</param>
        /// <param name="predicate">给定的筛选方法（可选）。</param>
        /// <returns>返回项列表。</returns>
        public static SelectList GetAccountStatusList(Func<string, string, bool> selectedFactory,
            Func<SelectListItem, bool> predicate = null)
        {
            return ResolveSelectList<Authorization.AccountStatus>(selectedFactory, predicate);
        }


        /// <summary>
        /// 获取认证状态枚举项列表。
        /// </summary>
        /// <param name="status">给定的默认选中项。</param>
        /// <param name="predicate">给定的筛选方法（可选）。</param>
        /// <returns>返回项列表。</returns>
        public static SelectList GetAuthenticateStatusList(Authorization.AuthenticateStatus status,
            Func<SelectListItem, bool> predicate = null)
        {
            string selectedValue = ((int)status).ToString();

            return GetAuthenticateStatusList((value, text) => selectedValue == value, predicate);
        }
        /// <summary>
        /// 获取认证状态枚举项列表。
        /// </summary>
        /// <param name="selectedFactory">给定的默认选中项方法。</param>
        /// <param name="predicate">给定的筛选方法（可选）。</param>
        /// <returns>返回项列表。</returns>
        public static SelectList GetAuthenticateStatusList(Func<string, string, bool> selectedFactory,
            Func<SelectListItem, bool> predicate = null)
        {
            return ResolveSelectList<Authorization.AuthenticateStatus>(selectedFactory, predicate);
        }

        #endregion


        #region Data

        /// <summary>
        /// 获取绑定标记枚举项列表。
        /// </summary>
        /// <param name="markup">给定的默认选中项。</param>
        /// <param name="predicate">给定的筛选方法（可选）。</param>
        /// <returns>返回项列表。</returns>
        public static SelectList GetBindingMarkupList(Data.BindingMarkup markup,
            Func<SelectListItem, bool> predicate = null)
        {
            string selectedValue = ((int)markup).ToString();

            return GetBindingMarkupList((value, text) => selectedValue == value, predicate);
        }
        /// <summary>
        /// 获取绑定标记枚举项列表。
        /// </summary>
        /// <param name="selectedFactory">给定的默认选中项方法。</param>
        /// <param name="predicate">给定的筛选方法（可选）。</param>
        /// <returns>返回项列表。</returns>
        public static SelectList GetBindingMarkupList(Func<string, string, bool> selectedFactory,
            Func<SelectListItem, bool> predicate = null)
        {
            return ResolveSelectList<Data.BindingMarkup>(selectedFactory, predicate);
        }


        /// <summary>
        /// 获取数据状态枚举项列表。
        /// </summary>
        /// <param name="status">给定的默认选中项。</param>
        /// <param name="predicate">给定的筛选方法（可选）。</param>
        /// <returns>返回项列表。</returns>
        public static SelectList GetDataStatusList(Data.DataStatus status,
            Func<SelectListItem, bool> predicate = null)
        {
            string selectedValue = ((int)status).ToString();

            return GetDataStatusList((value, text) => selectedValue == value, predicate);
        }
        /// <summary>
        /// 获取数据状态枚举项列表。
        /// </summary>
        /// <param name="selectedFactory">给定的默认选中项方法。</param>
        /// <param name="predicate">给定的筛选方法（可选）。</param>
        /// <returns>返回项列表。</returns>
        public static SelectList GetDataStatusList(Func<string, string, bool> selectedFactory,
            Func<SelectListItem, bool> predicate = null)
        {
            return ResolveSelectList<Data.DataStatus>(selectedFactory, predicate);
        }


        /// <summary>
        /// 获取定位状态枚举项列表。
        /// </summary>
        /// <param name="status">给定的默认选中项。</param>
        /// <param name="predicate">给定的筛选方法（可选）。</param>
        /// <returns>返回项列表。</returns>
        public static SelectList GetPositionStatusList(Data.PositionStatus status,
            Func<SelectListItem, bool> predicate = null)
        {
            string selectedValue = ((int)status).ToString();

            return GetPositionStatusList((value, text) => selectedValue == value, predicate);
        }
        /// <summary>
        /// 获取定位状态枚举项列表。
        /// </summary>
        /// <param name="selectedFactory">给定的默认选中项方法。</param>
        /// <param name="predicate">给定的筛选方法（可选）。</param>
        /// <returns>返回项列表。</returns>
        public static SelectList GetPositionStatusList(Func<string, string, bool> selectedFactory,
            Func<SelectListItem, bool> predicate = null)
        {
            return ResolveSelectList<Data.PositionStatus>(selectedFactory, predicate);
        }

        #endregion


        #region Forms

        /// <summary>
        /// 获取鼠标状态枚举项列表。
        /// </summary>
        /// <param name="state">给定的默认选中项。</param>
        /// <param name="predicate">给定的筛选方法（可选）。</param>
        /// <returns>返回项列表。</returns>
        public static SelectList GetMouseStateList(Forms.MouseState state,
            Func<SelectListItem, bool> predicate = null)
        {
            string selectedValue = ((int)state).ToString();

            return GetMouseStateList((value, text) => selectedValue == value, predicate);
        }
        /// <summary>
        /// 获取鼠标状态枚举项列表。
        /// </summary>
        /// <param name="selectedFactory">给定的默认选中项方法。</param>
        /// <param name="predicate">给定的筛选方法（可选）。</param>
        /// <returns>返回项列表。</returns>
        public static SelectList GetMouseStateList(Func<string, string, bool> selectedFactory,
            Func<SelectListItem, bool> predicate = null)
        {
            return ResolveSelectList<Forms.MouseState>(selectedFactory, predicate);
        }

        #endregion


        #region MediaInfo

        /// <summary>
        /// 获取信息种类枚举项列表。
        /// </summary>
        /// <param name="kind">给定的默认选中项。</param>
        /// <param name="predicate">给定的筛选方法（可选）。</param>
        /// <returns>返回项列表。</returns>
        public static SelectList GetInfoKindList(MediaInfo.InfoKind kind,
            Func<SelectListItem, bool> predicate = null)
        {
            string selectedValue = ((int)kind).ToString();

            return GetInfoKindList((value, text) => selectedValue == value, predicate);
        }
        /// <summary>
        /// 获取信息种类枚举项列表。
        /// </summary>
        /// <param name="selectedFactory">给定的默认选中项方法。</param>
        /// <param name="predicate">给定的筛选方法（可选）。</param>
        /// <returns>返回项列表。</returns>
        public static SelectList GetInfoKindList(Func<string, string, bool> selectedFactory,
            Func<SelectListItem, bool> predicate = null)
        {
            return ResolveSelectList<MediaInfo.InfoKind>(selectedFactory, predicate);
        }


        /// <summary>
        /// 获取流种类枚举项列表。
        /// </summary>
        /// <param name="kind">给定的默认选中项。</param>
        /// <param name="predicate">给定的筛选方法（可选）。</param>
        /// <returns>返回项列表。</returns>
        public static SelectList GetStreamKindList(MediaInfo.StreamKind kind,
            Func<SelectListItem, bool> predicate = null)
        {
            string selectedValue = ((int)kind).ToString();

            return GetStreamKindList((value, text) => selectedValue == value, predicate);
        }
        /// <summary>
        /// 获取流种类枚举项列表。
        /// </summary>
        /// <param name="selectedFactory">给定的默认选中项方法。</param>
        /// <param name="predicate">给定的筛选方法（可选）。</param>
        /// <returns>返回项列表。</returns>
        public static SelectList GetStreamKindList(Func<string, string, bool> selectedFactory,
            Func<SelectListItem, bool> predicate = null)
        {
            return ResolveSelectList<MediaInfo.StreamKind>(selectedFactory, predicate);
        }

        #endregion


        #region Utility

        /// <summary>
        /// 获取容量大小单位枚举项列表。
        /// </summary>
        /// <param name="unit">给定的默认选中项。</param>
        /// <param name="predicate">给定的筛选方法（可选）。</param>
        /// <returns>返回项列表。</returns>
        public static SelectList GetCapacitySizeUnitList(FileSizeUnit unit,
            Func<SelectListItem, bool> predicate = null)
        {
            string selectedValue = ((int)unit).ToString();

            return GetCapacitySizeUnitList((value, text) => selectedValue == value, predicate);
        }
        /// <summary>
        /// 获取容量大小单位枚举项列表。
        /// </summary>
        /// <param name="selectedFactory">给定的默认选中项方法。</param>
        /// <param name="predicate">给定的筛选方法（可选）。</param>
        /// <returns>返回项列表。</returns>
        public static SelectList GetCapacitySizeUnitList(Func<string, string, bool> selectedFactory,
            Func<SelectListItem, bool> predicate = null)
        {
            return ResolveSelectList<FileSizeUnit>(selectedFactory, predicate);
        }


        /// <summary>
        /// 获取查找方式枚举项列表。
        /// </summary>
        /// <param name="mode">给定的默认选中项。</param>
        /// <param name="predicate">给定的筛选方法（可选）。</param>
        /// <returns>返回项列表。</returns>
        public static SelectList GetSearchModeList(SearchMode mode,
            Func<SelectListItem, bool> predicate = null)
        {
            string selectedValue = ((int)mode).ToString();

            return GetSearchModeList((value, text) => selectedValue == value, predicate);
        }
        /// <summary>
        /// 获取查找方式枚举项列表。
        /// </summary>
        /// <param name="selectedFactory">给定的默认选中项方法。</param>
        /// <param name="predicate">给定的筛选方法（可选）。</param>
        /// <returns>返回项列表。</returns>
        public static SelectList GetSearchModeList(Func<string, string, bool> selectedFactory,
            Func<SelectListItem, bool> predicate = null)
        {
            return ResolveSelectList<SearchMode>(selectedFactory, predicate);
        }

        #endregion


        #region MVC

        /// <summary>
        /// 获取注意级别枚举项列表。
        /// </summary>
        /// <param name="level">给定的默认选中项。</param>
        /// <param name="predicate">给定的筛选方法（可选）。</param>
        /// <returns>返回项列表。</returns>
        public static SelectList GetAttentionLevelList(AttentionLevel level,
            Func<SelectListItem, bool> predicate = null)
        {
            string selectedValue = ((int)level).ToString();

            return GetAttentionLevelList((value, text) => selectedValue == value, predicate);
        }
        /// <summary>
        /// 获取注意级别枚举项列表。
        /// </summary>
        /// <param name="selectedFactory">给定的默认选中项方法。</param>
        /// <param name="predicate">给定的筛选方法（可选）。</param>
        /// <returns>返回项列表。</returns>
        public static SelectList GetAttentionLevelList(Func<string, string, bool> selectedFactory,
            Func<SelectListItem, bool> predicate = null)
        {
            return ResolveSelectList<AttentionLevel>(selectedFactory, predicate);
        }


        /// <summary>
        /// 获取动作状态枚举项列表。
        /// </summary>
        /// <param name="status">给定的默认选中项。</param>
        /// <param name="predicate">给定的筛选方法（可选）。</param>
        /// <returns>返回项列表。</returns>
        public static SelectList GetActionStatusList(ActionStatus status,
            Func<SelectListItem, bool> predicate = null)
        {
            string selectedValue = ((int)status).ToString();

            return GetAttentionLevelList((value, text) => selectedValue == value, predicate);
        }
        /// <summary>
        /// 获取动作状态枚举项列表。
        /// </summary>
        /// <param name="selectedFactory">给定的默认选中项方法。</param>
        /// <param name="predicate">给定的筛选方法（可选）。</param>
        /// <returns>返回项列表。</returns>
        public static SelectList GetActionStatusList(Func<string, string, bool> selectedFactory,
            Func<SelectListItem, bool> predicate = null)
        {
            return ResolveSelectList<ActionStatus>(selectedFactory, predicate);
        }

        #endregion


        /// <summary>
        /// 解析枚举列表。
        /// </summary>
        /// <typeparam name="TEnum">指定的枚举类型。</typeparam>
        /// <param name="selectedFactory">给定的默认选中项方法。</param>
        /// <param name="predicate">给定的筛选方法（可选）。</param>
        /// <returns>返回项列表。</returns>
        protected static SelectList ResolveSelectList<TEnum>(Func<string, string, bool> selectedFactory,
            Func<SelectListItem, bool> predicate = null)
        {
            selectedFactory.NotNull(nameof(selectedFactory));

            var selectListItems = ResolveList<TEnum>((value, text) => new SelectListItem()
            {
                Text = text,
                Value = value,
                Selected = selectedFactory.Invoke(value, text)
            },
            predicate);

            return new SelectList(selectListItems, "Value", "Text");
        }

    }
}
