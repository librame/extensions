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
using System.Collections.Generic;
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
        /// <returns>返回项列表。</returns>
        public static IList<SelectListItem> GetAccountStatusList(Authorization.AccountStatus status)
        {
            string selectedValue = ((int)status).ToString();

            return GetAccountStatusList((value, text) => selectedValue == value);
        }
        /// <summary>
        /// 获取帐户状态枚举项列表。
        /// </summary>
        /// <param name="selectedFactory">给定的默认选中项方法。</param>
        /// <returns>返回项列表。</returns>
        public static IList<SelectListItem> GetAccountStatusList(Func<string, string, bool> selectedFactory)
        {
            return ResolveList<Authorization.AccountStatus>(selectedFactory);
        }


        /// <summary>
        /// 获取认证状态枚举项列表。
        /// </summary>
        /// <param name="status">给定的默认选中项。</param>
        /// <returns>返回项列表。</returns>
        public static IList<SelectListItem> GetAuthenticateStatusList(Authorization.AuthenticateStatus status)
        {
            string selectedValue = ((int)status).ToString();

            return GetAuthenticateStatusList((value, text) => selectedValue == value);
        }
        /// <summary>
        /// 获取认证状态枚举项列表。
        /// </summary>
        /// <param name="selectedFactory">给定的默认选中项方法。</param>
        /// <returns>返回项列表。</returns>
        public static IList<SelectListItem> GetAuthenticateStatusList(Func<string, string, bool> selectedFactory)
        {
            return ResolveList<Authorization.AuthenticateStatus>(selectedFactory);
        }

        #endregion


        #region Data

        /// <summary>
        /// 获取数据状态枚举项列表。
        /// </summary>
        /// <param name="status">给定的默认选中项。</param>
        /// <returns>返回项列表。</returns>
        public static IList<SelectListItem> GetDataStatusList(Data.DataStatus status)
        {
            string selectedValue = ((int)status).ToString();

            return GetDataStatusList((value, text) => selectedValue == value);
        }
        /// <summary>
        /// 获取数据状态枚举项列表。
        /// </summary>
        /// <param name="selectedFactory">给定的默认选中项方法。</param>
        /// <returns>返回项列表。</returns>
        public static IList<SelectListItem> GetDataStatusList(Func<string, string, bool> selectedFactory)
        {
            return ResolveList<Data.DataStatus>(selectedFactory);
        }


        /// <summary>
        /// 获取定位状态枚举项列表。
        /// </summary>
        /// <param name="status">给定的默认选中项。</param>
        /// <returns>返回项列表。</returns>
        public static IList<SelectListItem> GetPositionStatusList(Data.PositionStatus status)
        {
            string selectedValue = ((int)status).ToString();

            return GetPositionStatusList((value, text) => selectedValue == value);
        }
        /// <summary>
        /// 获取定位状态枚举项列表。
        /// </summary>
        /// <param name="selectedFactory">给定的默认选中项方法。</param>
        /// <returns>返回项列表。</returns>
        public static IList<SelectListItem> GetPositionStatusList(Func<string, string, bool> selectedFactory)
        {
            return ResolveList<Data.PositionStatus>(selectedFactory);
        }

        #endregion


        #region Forms

        /// <summary>
        /// 获取鼠标状态枚举项列表。
        /// </summary>
        /// <param name="state">给定的默认选中项。</param>
        /// <returns>返回项列表。</returns>
        public static IList<SelectListItem> GetMouseStateList(Forms.MouseState state)
        {
            string selectedValue = ((int)state).ToString();

            return GetMouseStateList((value, text) => selectedValue == value);
        }
        /// <summary>
        /// 获取鼠标状态枚举项列表。
        /// </summary>
        /// <param name="selectedFactory">给定的默认选中项方法。</param>
        /// <returns>返回项列表。</returns>
        public static IList<SelectListItem> GetMouseStateList(Func<string, string, bool> selectedFactory)
        {
            return ResolveList<Forms.MouseState>(selectedFactory);
        }

        #endregion


        #region MediaInfo

        /// <summary>
        /// 获取信息种类枚举项列表。
        /// </summary>
        /// <param name="kind">给定的默认选中项。</param>
        /// <returns>返回项列表。</returns>
        public static IList<SelectListItem> GetInfoKindList(MediaInfo.InfoKind kind)
        {
            string selectedValue = ((int)kind).ToString();

            return GetInfoKindList((value, text) => selectedValue == value);
        }
        /// <summary>
        /// 获取信息种类枚举项列表。
        /// </summary>
        /// <param name="selectedFactory">给定的默认选中项方法。</param>
        /// <returns>返回项列表。</returns>
        public static IList<SelectListItem> GetInfoKindList(Func<string, string, bool> selectedFactory)
        {
            return ResolveList<MediaInfo.InfoKind>(selectedFactory);
        }


        /// <summary>
        /// 获取流种类枚举项列表。
        /// </summary>
        /// <param name="kind">给定的默认选中项。</param>
        /// <returns>返回项列表。</returns>
        public static IList<SelectListItem> GetStreamKindList(MediaInfo.StreamKind kind)
        {
            string selectedValue = ((int)kind).ToString();

            return GetStreamKindList((value, text) => selectedValue == value);
        }
        /// <summary>
        /// 获取流种类枚举项列表。
        /// </summary>
        /// <param name="selectedFactory">给定的默认选中项方法。</param>
        /// <returns>返回项列表。</returns>
        public static IList<SelectListItem> GetStreamKindList(Func<string, string, bool> selectedFactory)
        {
            return ResolveList<MediaInfo.StreamKind>(selectedFactory);
        }

        #endregion


        #region Utility

        /// <summary>
        /// 获取容量大小单位枚举项列表。
        /// </summary>
        /// <param name="unit">给定的默认选中项。</param>
        /// <returns>返回项列表。</returns>
        public static IList<SelectListItem> GetCapacitySizeUnitList(FileSizeUnit unit)
        {
            string selectedValue = ((int)unit).ToString();

            return GetCapacitySizeUnitList((value, text) => selectedValue == value);
        }
        /// <summary>
        /// 获取容量大小单位枚举项列表。
        /// </summary>
        /// <param name="selectedFactory">给定的默认选中项方法。</param>
        /// <returns>返回项列表。</returns>
        public static IList<SelectListItem> GetCapacitySizeUnitList(Func<string, string, bool> selectedFactory)
        {
            return ResolveList<FileSizeUnit>(selectedFactory);
        }


        /// <summary>
        /// 获取查找方式枚举项列表。
        /// </summary>
        /// <param name="mode">给定的默认选中项。</param>
        /// <returns>返回项列表。</returns>
        public static IList<SelectListItem> GetSearchModeList(SearchMode mode)
        {
            string selectedValue = ((int)mode).ToString();

            return GetSearchModeList((value, text) => selectedValue == value);
        }
        /// <summary>
        /// 获取查找方式枚举项列表。
        /// </summary>
        /// <param name="selectedFactory">给定的默认选中项方法。</param>
        /// <returns>返回项列表。</returns>
        public static IList<SelectListItem> GetSearchModeList(Func<string, string, bool> selectedFactory)
        {
            return ResolveList<SearchMode>(selectedFactory);
        }

        #endregion


        #region MVC

        /// <summary>
        /// 获取注意级别枚举项列表。
        /// </summary>
        /// <param name="level">给定的默认选中项。</param>
        /// <returns>返回项列表。</returns>
        public static IList<SelectListItem> GetAttentionLevelList(AttentionLevel level)
        {
            string selectedValue = ((int)level).ToString();

            return GetAttentionLevelList((value, text) => selectedValue == value);
        }
        /// <summary>
        /// 获取注意级别枚举项列表。
        /// </summary>
        /// <param name="selectedFactory">给定的默认选中项方法。</param>
        /// <returns>返回项列表。</returns>
        public static IList<SelectListItem> GetAttentionLevelList(Func<string, string, bool> selectedFactory)
        {
            return ResolveList<AttentionLevel>(selectedFactory);
        }


        /// <summary>
        /// 获取动作状态枚举项列表。
        /// </summary>
        /// <param name="status">给定的默认选中项。</param>
        /// <returns>返回项列表。</returns>
        public static IList<SelectListItem> GetActionStatusList(ActionStatus status)
        {
            string selectedValue = ((int)status).ToString();

            return GetAttentionLevelList((value, text) => selectedValue == value);
        }
        /// <summary>
        /// 获取动作状态枚举项列表。
        /// </summary>
        /// <param name="selectedFactory">给定的默认选中项方法。</param>
        /// <returns>返回项列表。</returns>
        public static IList<SelectListItem> GetActionStatusList(Func<string, string, bool> selectedFactory)
        {
            return ResolveList<ActionStatus>(selectedFactory);
        }

        #endregion


        /// <summary>
        /// 解析枚举列表。
        /// </summary>
        /// <typeparam name="TEnum">指定的枚举类型。</typeparam>
        /// <param name="selectedFactory">给定的默认选中项方法。</param>
        /// <returns>返回项列表。</returns>
        protected static IList<SelectListItem> ResolveList<TEnum>(Func<string, string, bool> selectedFactory)
        {
            selectedFactory.GuardNull(nameof(selectedFactory));

            return ResolveList<TEnum>((value, text) => new SelectListItem()
            {
                Text = text,
                Value = value,
                Selected = selectedFactory.Invoke(value, text)
            });
        }

    }
}
