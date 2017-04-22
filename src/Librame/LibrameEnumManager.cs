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

namespace Librame
{
    using Utility;

    /// <summary>
    /// Librame 枚举管理器。
    /// </summary>
    /// <typeparam name="TItem">指定的列表项类型。</typeparam>
    public class LibrameEnumManager<TItem> : SingletonManager
        where TItem : class
    {

        #region Authorization

        /// <summary>
        /// 获取帐户状态枚举项列表。
        /// </summary>
        /// <param name="itemFactory">给定创建项类型实例的方法。</param>
        /// <returns>返回项列表。</returns>
        public static IList<TItem> GetAccountStatusList(Func<string, string, TItem> itemFactory)
        {
            return ResolveList<Authorization.AccountStatus>(itemFactory);
        }


        /// <summary>
        /// 获取认证状态枚举项列表。
        /// </summary>
        /// <param name="itemFactory">给定创建项类型实例的方法。</param>
        /// <returns>返回项列表。</returns>
        public static IList<TItem> GetAuthenticateStatusList(Func<string, string, TItem> itemFactory)
        {
            return ResolveList<Authorization.AuthenticateStatus>(itemFactory);
        }

        #endregion


        #region Data
        
        /// <summary>
        /// 获取数据状态枚举项列表。
        /// </summary>
        /// <param name="itemFactory">给定创建项类型实例的方法。</param>
        /// <returns>返回项列表。</returns>
        public static IList<TItem> GetDataStatusList(Func<string, string, TItem> itemFactory)
        {
            return ResolveList<Data.DataStatus>(itemFactory);
        }


        /// <summary>
        /// 获取定位状态枚举项列表。
        /// </summary>
        /// <param name="itemFactory">给定创建项类型实例的方法。</param>
        /// <returns>返回项列表。</returns>
        public static IList<TItem> GetPositionStatusList(Func<string, string, TItem> itemFactory)
        {
            return ResolveList<Data.PositionStatus>(itemFactory);
        }

        #endregion


        #region Forms

        /// <summary>
        /// 获取鼠标状态枚举项列表。
        /// </summary>
        /// <param name="itemFactory">给定创建项类型实例的方法。</param>
        /// <returns>返回项列表。</returns>
        public static IList<TItem> GetMouseStateList(Func<string, string, TItem> itemFactory)
        {
            return ResolveList<Forms.MouseState>(itemFactory);
        }

        #endregion


        #region MediaInfo

        /// <summary>
        /// 获取信息种类枚举项列表。
        /// </summary>
        /// <param name="itemFactory">给定创建项类型实例的方法。</param>
        /// <returns>返回项列表。</returns>
        public static IList<TItem> GetInfoKindList(Func<string, string, TItem> itemFactory)
        {
            return ResolveList<MediaInfo.InfoKind>(itemFactory);
        }


        /// <summary>
        /// 获取流种类枚举项列表。
        /// </summary>
        /// <param name="itemFactory">给定创建项类型实例的方法。</param>
        /// <returns>返回项列表。</returns>
        public static IList<TItem> GetStreamKindList(Func<string, string, TItem> itemFactory)
        {
            return ResolveList<MediaInfo.StreamKind>(itemFactory);
        }

        #endregion


        #region Utility

        /// <summary>
        /// 获取容量大小单位枚举项列表。
        /// </summary>
        /// <param name="itemFactory">给定创建项类型实例的方法。</param>
        /// <returns>返回项列表。</returns>
        public static IList<TItem> GetFileSizeUnitList(Func<string, string, TItem> itemFactory)
        {
            return ResolveList<FileSizeUnit>(itemFactory);
        }


        /// <summary>
        /// 获取查找方式枚举项列表。
        /// </summary>
        /// <param name="itemFactory">给定创建项类型实例的方法。</param>
        /// <returns>返回项列表。</returns>
        public static IList<TItem> GetSearchModeList(Func<string, string, TItem> itemFactory)
        {
            return ResolveList<SearchMode>(itemFactory);
        }

        #endregion


        /// <summary>
        /// 解析枚举列表。
        /// </summary>
        /// <typeparam name="TEnum">指定的枚举类型。</typeparam>
        /// <param name="itemFactory">给定创建项类型实例的方法。</param>
        /// <returns>返回项列表。</returns>
        protected static IList<TItem> ResolveList<TEnum>(Func<string, string, TItem> itemFactory)
        {
            var key = BuildKey<TEnum>();

            return Resolve(key, k => EnumUtility.AsList<TEnum, TItem>(itemFactory));
        }

    }
}
