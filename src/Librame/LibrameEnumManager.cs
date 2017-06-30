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
using System.Linq;

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
        /// <param name="predicate">给定的筛选方法（可选）。</param>
        /// <returns>返回项列表。</returns>
        public static IList<TItem> GetAccountStatusList(Func<string, string, TItem> itemFactory,
            Func<TItem, bool> predicate = null)
        {
            return ResolveList<Authorization.AccountStatus>(itemFactory, predicate);
        }


        /// <summary>
        /// 获取认证状态枚举项列表。
        /// </summary>
        /// <param name="itemFactory">给定创建项类型实例的方法。</param>
        /// <param name="predicate">给定的筛选方法（可选）。</param>
        /// <returns>返回项列表。</returns>
        public static IList<TItem> GetAuthenticateStatusList(Func<string, string, TItem> itemFactory,
            Func<TItem, bool> predicate = null)
        {
            return ResolveList<Authorization.AuthenticateStatus>(itemFactory, predicate);
        }

        #endregion


        #region Data

        /// <summary>
        /// 获取绑定标记枚举项列表。
        /// </summary>
        /// <param name="itemFactory">给定创建项类型实例的方法。</param>
        /// <param name="predicate">给定的筛选方法（可选）。</param>
        /// <returns>返回项列表。</returns>
        public static IList<TItem> GetBindingMarkupList(Func<string, string, TItem> itemFactory,
            Func<TItem, bool> predicate = null)
        {
            return ResolveList<Data.BindingMarkup>(itemFactory, predicate);
        }


        /// <summary>
        /// 获取数据状态枚举项列表。
        /// </summary>
        /// <param name="itemFactory">给定创建项类型实例的方法。</param>
        /// <param name="predicate">给定的筛选方法（可选）。</param>
        /// <returns>返回项列表。</returns>
        public static IList<TItem> GetDataStatusList(Func<string, string, TItem> itemFactory,
            Func<TItem, bool> predicate = null)
        {
            return ResolveList<Data.DataStatus>(itemFactory, predicate);
        }


        /// <summary>
        /// 获取定位状态枚举项列表。
        /// </summary>
        /// <param name="itemFactory">给定创建项类型实例的方法。</param>
        /// <param name="predicate">给定的筛选方法（可选）。</param>
        /// <returns>返回项列表。</returns>
        public static IList<TItem> GetPositionStatusList(Func<string, string, TItem> itemFactory,
            Func<TItem, bool> predicate = null)
        {
            return ResolveList<Data.PositionStatus>(itemFactory, predicate);
        }

        #endregion


        #region Forms

        /// <summary>
        /// 获取鼠标状态枚举项列表。
        /// </summary>
        /// <param name="itemFactory">给定创建项类型实例的方法。</param>
        /// <param name="predicate">给定的筛选方法（可选）。</param>
        /// <returns>返回项列表。</returns>
        public static IList<TItem> GetMouseStateList(Func<string, string, TItem> itemFactory,
            Func<TItem, bool> predicate = null)
        {
            return ResolveList<Forms.MouseState>(itemFactory, predicate);
        }

        #endregion


        #region MediaInfo

        /// <summary>
        /// 获取信息种类枚举项列表。
        /// </summary>
        /// <param name="itemFactory">给定创建项类型实例的方法。</param>
        /// <param name="predicate">给定的筛选方法（可选）。</param>
        /// <returns>返回项列表。</returns>
        public static IList<TItem> GetInfoKindList(Func<string, string, TItem> itemFactory,
            Func<TItem, bool> predicate = null)
        {
            return ResolveList<MediaInfo.InfoKind>(itemFactory, predicate);
        }


        /// <summary>
        /// 获取流种类枚举项列表。
        /// </summary>
        /// <param name="itemFactory">给定创建项类型实例的方法。</param>
        /// <param name="predicate">给定的筛选方法（可选）。</param>
        /// <returns>返回项列表。</returns>
        public static IList<TItem> GetStreamKindList(Func<string, string, TItem> itemFactory,
            Func<TItem, bool> predicate = null)
        {
            return ResolveList<MediaInfo.StreamKind>(itemFactory, predicate);
        }

        #endregion


        #region Utility

        /// <summary>
        /// 获取容量大小单位枚举项列表。
        /// </summary>
        /// <param name="itemFactory">给定创建项类型实例的方法。</param>
        /// <param name="predicate">给定的筛选方法（可选）。</param>
        /// <returns>返回项列表。</returns>
        public static IList<TItem> GetFileSizeUnitList(Func<string, string, TItem> itemFactory,
            Func<TItem, bool> predicate = null)
        {
            return ResolveList<FileSizeUnit>(itemFactory, predicate);
        }


        /// <summary>
        /// 获取查找方式枚举项列表。
        /// </summary>
        /// <param name="itemFactory">给定创建项类型实例的方法。</param>
        /// <param name="predicate">给定的筛选方法（可选）。</param>
        /// <returns>返回项列表。</returns>
        public static IList<TItem> GetSearchModeList(Func<string, string, TItem> itemFactory,
            Func<TItem, bool> predicate = null)
        {
            return ResolveList<SearchMode>(itemFactory, predicate);
        }

        #endregion


        /// <summary>
        /// 解析枚举列表。
        /// </summary>
        /// <typeparam name="TEnum">指定的枚举类型。</typeparam>
        /// <param name="itemFactory">给定创建项类型实例的方法。</param>
        /// <param name="predicate">给定的筛选方法（可选）。</param>
        /// <returns>返回项列表。</returns>
        protected static IList<TItem> ResolveList<TEnum>(Func<string, string, TItem> itemFactory,
            Func<TItem, bool> predicate = null)
        {
            var key = BuildKey<TEnum>();

            return Resolve(key, k =>
            {
                var list = EnumUtility.AsList<TEnum, TItem>(itemFactory);

                if (predicate == null)
                    return list;

                return list.Where(predicate).ToList();
            });
        }

    }
}
