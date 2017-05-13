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
using System.Linq;
using System.Reflection;

namespace Librame.Data
{
    using Utility;

    /// <summary>
    /// 数据助手。
    /// </summary>
    public class DataHelper : LibrameBase<DataHelper>
    {
        /// <summary>
        /// AppSetting 键名。
        /// </summary>
        internal const string APP_SETTING_KEY = "AppSettingKey=";
        /// <summary>
        /// ConnectionString 键名。
        /// </summary>
        internal const string CONNECTION_STRING_KEY = "ConnectionStringKey=";
        
        /// <summary>
        /// 程序集字符串集合分隔符。
        /// </summary>
        private const char ASSEMBLIES_SEPARATOR = ';';


        /// <summary>
        /// 获取要映射的实体程序集数组。
        /// </summary>
        /// <returns>返回程序集数组。</returns>
        public static Assembly[] GetMappingAssemblies(string assemblies)
        {
            try
            {
                var strs = assemblies.Trim(ASSEMBLIES_SEPARATOR).Split(ASSEMBLIES_SEPARATOR);
                return strs.Select(s => Assembly.Load(s)).ToArray();
            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerMessage(), ex);

                return null;
            }
        }


        private static readonly Type _entityAutomappingType = typeof(IEntityAutomapping);
        /// <summary>
        /// 是否应该自映射（默认会排除抽象类与接口）。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回布尔值。</returns>
        public static bool ShouldAutomapping(Type type)
        {
            return (!type.IsAbstract && !type.IsInterface && _entityAutomappingType.IsAssignableFrom(type));
        }


        /// <summary>
        /// 建立数据首选项键名。
        /// </summary>
        /// <typeparam name="TUsed">指定的使用类型。</typeparam>
        /// <param name="dataSettings">给定的数据首选项。</param>
        /// <returns>返回键名字符串。</returns>
        public static string BuildDataSettingsKey<TUsed>(DataSettings dataSettings)
        {
            return BuildDataSettingsKey(typeof(TUsed), dataSettings);
        }
        /// <summary>
        /// 建立数据首选项键名。
        /// </summary>
        /// <param name="usedType">给定的使用类型。</param>
        /// <param name="dataSettings">给定的数据首选项。</param>
        /// <returns>返回键名字符串。</returns>
        public static string BuildDataSettingsKey(Type usedType, DataSettings dataSettings)
        {
            // UsedFullName:DataSettingsName:Database
            return (usedType.FullName + ":" + typeof(DataSettings).FullName + ":" + dataSettings.Database);
        }


        /// <summary>
        /// 建立数据管道。
        /// </summary>
        /// <typeparam name="TSource">指定的源类型。</typeparam>
        /// <param name="dataSettings">给定的数据首选项。</param>
        /// <param name="typeStringFactory">给定的类型字符串工厂方法。</param>
        /// <returns>返回数据源实例。</returns>
        public static TSource BuildProvider<TSource>(DataSettings dataSettings, Func<DataSettings, string> typeStringFactory)
            where TSource : class
        {
            try
            {
                var typeString = typeStringFactory(dataSettings);
                var type = Type.GetType(typeString);

                return (TSource)Activator.CreateInstance(type, dataSettings);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        ///// <summary>
        ///// 复数化指定类型名称。
        ///// </summary>
        ///// <typeparam name="T">指定的类型。</typeparam>
        ///// <returns>返回单词字符串。</returns>
        //public static string PluralizeTypeName<T>()
        //{
        //    return PluralizeTypeName(typeof(T));
        //}
        ///// <summary>
        ///// 复数化指定类型名称。
        ///// </summary>
        ///// <param name="type">给定的类型。</param>
        ///// <returns>返回单词字符串。</returns>
        //public static string PluralizeTypeName(Type type)
        //{
        //    return PluralizeWord(type.Name);
        //}
        ///// <summary>
        ///// 复数化单词。
        ///// </summary>
        ///// <param name="word">给定的单词。</param>
        ///// <returns>返回单词字符串。</returns>
        //public static string PluralizeWord(string word)
        //{
        //    // BUG: Aphorism 不能正确复数化
        //    //var pluralizationService = DbConfiguration.DependencyResolver.GetService<IPluralizationService>();
        //    //return pluralizationService.Pluralize(word);

        //    return WordHelper.Pluralize(word);
        //}

    }
}
