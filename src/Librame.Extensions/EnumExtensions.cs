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
using System.Reflection;

namespace Librame.Extensions
{
    /// <summary>
    /// 枚举静态扩展。
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// 转换为枚举字段名称。
        /// </summary>
        /// <typeparam name="TEnum">指定的枚举类型。</typeparam>
        /// <param name="enumField">给定的枚举字段。</param>
        /// <returns>返回字符串。</returns>
        public static string AsEnumName<TEnum>(this TEnum enumField)
            where TEnum : struct
        {
            return Enum.GetName(typeof(TEnum), enumField);
        }


        #region AsEnum

        /// <summary>
        /// 转换为特定枚举。
        /// </summary>
        /// <typeparam name="TEnum">指定的枚举类型。</typeparam>
        /// <param name="name">给定的字段名称。</param>
        /// <returns>返回枚举对象。</returns>
        public static TEnum AsEnum<TEnum>(this string name)
            where TEnum : struct
        {
            return (TEnum)Enum.Parse(typeof(TEnum), name);
        }
        /// <summary>
        /// 转换为特定枚举。
        /// </summary>
        /// <typeparam name="TEnum">指定的枚举类型。</typeparam>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="value">给定的常数值。</param>
        /// <returns>返回枚举对象。</returns>
        public static TEnum AsEnum<TEnum, TValue>(this TValue value)
            where TEnum : struct
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value.ToString());
        }

        /// <summary>
        /// 将输入枚举字段转换为输出枚举字段（通过匹配枚举字段名称实现）。
        /// </summary>
        /// <typeparam name="TInputEnum">指定的输入枚举类型。</typeparam>
        /// <typeparam name="TOutputEnum">指定的输出枚举类型。</typeparam>
        /// <param name="input">给定的输入枚举字段。</param>
        /// <returns>返回输出枚举字段。</returns>
        public static TOutputEnum AsOutputEnumByName<TInputEnum, TOutputEnum>(this TInputEnum input)
            where TInputEnum : struct
            where TOutputEnum : struct
        {
            return input.AsEnumName().AsEnum<TOutputEnum>();
        }

        #endregion


        #region AsEnumFields

        /// <summary>
        /// 当作枚举字段集合。
        /// </summary>
        /// <param name="enumType">给定的枚举类型。</param>
        /// <returns>返回字段信息数组。</returns>
        public static FieldInfo[] AsEnumFields(this Type enumType)
        {
            return enumType.GetTypeInfo().GetFields(BindingFlags.Static | BindingFlags.Public);
        }

        #endregion


        #region AsEnumResults

        /// <summary>
        /// 当作枚举结果。
        /// </summary>
        /// <typeparam name="TEnum">指定的枚举类型。</typeparam>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="enumField">给定的枚举项。</param>
        /// <param name="converter">给定的结果转换方法。</param>
        /// <returns>返回结果。</returns>
        public static TResult AsEnumResult<TEnum, TResult>(this TEnum enumField,
            Func<FieldInfo, TResult> converter)
            where TEnum : struct
        {
            converter.NotNull(nameof(converter));

            var name = enumField.AsEnumName();
            var field = typeof(TEnum).AsEnumFields().SingleOrDefault(f => f.Name == name);

            return converter.Invoke(field);
        }

        /// <summary>
        /// 当作枚举结果集合。
        /// </summary>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="enumType">给定的枚举类型。</param>
        /// <param name="converter">给定的结果转换方法。</param>
        /// <returns>返回结果集合。</returns>
        public static IEnumerable<TResult> AsEnumResults<TResult>(this Type enumType,
            Func<FieldInfo, TResult> converter)
        {
            converter.NotNull(nameof(converter));

            var fields = enumType.AsEnumFields();
            if (fields.IsNullOrEmpty()) return Enumerable.Empty<TResult>();

            return fields.Select(field => converter.Invoke(field));
        }

        #endregion


        #region AsEnumDictionary

        /// <summary>
        /// 当作枚举字典。
        /// </summary>
        /// <param name="enumType">给定的枚举类型。</param>
        /// <returns>返回结果字典。</returns>
        public static IDictionary<string, int> AsEnumDictionary(this Type enumType)
        {
            return enumType.AsEnumDictionary<int>();
        }
        /// <summary>
        /// 当作枚举字典。
        /// </summary>
        /// <typeparam name="TConst">指定的常量类型。</typeparam>
        /// <param name="enumType">给定的枚举类型。</param>
        /// <returns>返回结果字典。</returns>
        public static IDictionary<string, TConst> AsEnumDictionary<TConst>(this Type enumType)
            where TConst : struct
        {
            return enumType.AsEnumDictionary(field => (TConst)field.GetValue(null));
        }

        /// <summary>
        /// 当作枚举结果字典。
        /// </summary>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="enumType">给定的枚举类型。</param>
        /// <param name="converter">给定的结果转换方法。</param>
        /// <returns>返回结果字典。</returns>
        public static IDictionary<string, TResult> AsEnumDictionary<TResult>(this Type enumType,
            Func<FieldInfo, TResult> converter)
        {
            converter.NotNull(nameof(converter));

            var dict = new Dictionary<string, TResult>();

            var fields = enumType.AsEnumFields();
            if (fields.IsNullOrEmpty()) return dict;

            foreach (var field in fields)
                dict.Add(field.Name, converter.Invoke(field));

            return dict;
        }

        #endregion

    }
}
