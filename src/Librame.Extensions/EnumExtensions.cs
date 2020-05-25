#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
        /// 转换为特定枚举。
        /// </summary>
        /// <typeparam name="TEnum">指定的枚举类型。</typeparam>
        /// <param name="name">给定的字段名称。</param>
        /// <returns>返回枚举对象。</returns>
        public static TEnum AsEnum<TEnum>(this string name)
            where TEnum : Enum
            => (TEnum)Enum.Parse(typeof(TEnum), name);

        /// <summary>
        /// 转换为特定枚举。
        /// </summary>
        /// <typeparam name="TEnum">指定的枚举类型。</typeparam>
        /// <typeparam name="TValue">指定的值类型（如：32 位整数类型、64 位整数类型等）。</typeparam>
        /// <param name="value">给定的常数值（支持名称或数值）。</param>
        /// <returns>返回枚举对象。</returns>
        public static TEnum AsEnum<TEnum, TValue>(this TValue value)
            where TEnum : Enum
            where TValue : struct
            => (TEnum)Enum.Parse(typeof(TEnum), value.ToString());

        /// <summary>
        /// 转换为枚举字段名称。
        /// </summary>
        /// <typeparam name="TEnum">指定的枚举类型。</typeparam>
        /// <param name="enumField">给定的枚举字段。</param>
        /// <returns>返回字符串。</returns>
        public static string AsEnumName<TEnum>(this TEnum enumField)
            where TEnum : Enum
            => Enum.GetName(typeof(TEnum), enumField);

        /// <summary>
        /// 转换为枚举字段常量值。
        /// </summary>
        /// <typeparam name="TEnum">指定的枚举类型。</typeparam>
        /// <param name="enumField">给定的枚举字段。</param>
        /// <returns>返回字符串。</returns>
        public static object AsEnumValue<TEnum>(this TEnum enumField)
            where TEnum : Enum
            => typeof(TEnum).GetField(enumField.AsEnumName()).GetValue(null);

        /// <summary>
        /// 当作枚举字段集合。
        /// </summary>
        /// <param name="enumType">给定的枚举类型。</param>
        /// <returns>返回字段信息数组。</returns>
        public static FieldInfo[] AsEnumFields(this Type enumType)
            => enumType?.GetFields(BindingFlags.Static | BindingFlags.Public);


        #region AsEnumResults

        /// <summary>
        /// 当作枚举结果。
        /// </summary>
        /// <typeparam name="TEnum">指定的枚举类型。</typeparam>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="enumField">给定的枚举项。</param>
        /// <param name="converter">给定的结果转换方法。</param>
        /// <returns>返回结果。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static TResult AsEnumResult<TEnum, TResult>(this TEnum enumField,
            Func<FieldInfo, TResult> converter)
            where TEnum : Enum
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
            if (fields.IsEmpty())
                return Enumerable.Empty<TResult>();

            return fields.Select(field => converter.Invoke(field));
        }

        #endregion


        #region AsEnumDictionary

        /// <summary>
        /// 当作枚举名称、特性集合字典。
        /// </summary>
        /// <param name="enumType">给定的枚举类型。</param>
        /// <returns>返回名称、特性集合字典。</returns>
        public static ConcurrentDictionary<string, IEnumerable<Attribute>> AsEnumAttributesDictionary(this Type enumType)
            => enumType.AsEnumDictionary(field => field.GetCustomAttributes());


        /// <summary>
        /// 当作枚举名称、常量值字典。
        /// </summary>
        /// <param name="enumType">给定的枚举类型。</param>
        /// <returns>返回名称、常量值字典。</returns>
        public static ConcurrentDictionary<string, int> AsEnumValuesDictionary(this Type enumType)
            => enumType.AsEnumValuesDictionary<int>();

        /// <summary>
        /// 当作枚举名称、常量值字典。
        /// </summary>
        /// <typeparam name="TValue">指定的值类型（如：32 位整数类型、64 位整数类型等）。</typeparam>
        /// <param name="enumType">给定的枚举类型。</param>
        /// <returns>返回名称、常量值字典。</returns>
        public static ConcurrentDictionary<string, TValue> AsEnumValuesDictionary<TValue>(this Type enumType)
            where TValue : struct
            => enumType.AsEnumDictionary(field => (TValue)field.GetValue(null));


        /// <summary>
        /// 当作枚举结果字典。
        /// </summary>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="enumType">给定的枚举类型。</param>
        /// <param name="converter">给定的结果转换方法。</param>
        /// <returns>返回结果字典。</returns>
        public static ConcurrentDictionary<string, TResult> AsEnumDictionary<TResult>(this Type enumType,
            Func<FieldInfo, TResult> converter)
        {
            converter.NotNull(nameof(converter));

            var dict = new ConcurrentDictionary<string, TResult>();

            enumType.AsEnumFields().ForEach(field =>
            {
                var result = converter.Invoke(field);
                dict.AddOrUpdate(field.Name, result, (key, value) => result);
            });

            return dict;
        }

        #endregion


        #region MatchEnum

        /// <summary>
        /// 匹配与输入枚举字段名称相同的输出枚举字段。
        /// </summary>
        /// <typeparam name="TInput">指定的输入枚举类型。</typeparam>
        /// <typeparam name="TOutput">指定的输出枚举类型。</typeparam>
        /// <param name="input">给定的输入枚举字段。</param>
        /// <returns>返回输出枚举字段。</returns>
        public static TOutput MatchEnum<TInput, TOutput>(this TInput input)
            where TInput : Enum
            where TOutput : Enum
            => input.AsEnumName().AsEnum<TOutput>();

        /// <summary>
        /// 匹配与输入枚举字段常数值相同的输出枚举字段。
        /// </summary>
        /// <typeparam name="TInput">指定的输入枚举类型。</typeparam>
        /// <typeparam name="TOutput">指定的输出枚举类型。</typeparam>
        /// <typeparam name="TValue">指定的值类型（如：32 位整数类型、64 位整数类型等）。</typeparam>
        /// <param name="input">给定的输入枚举字段。</param>
        /// <returns>返回输出枚举字段。</returns>
        public static TOutput MatchEnum<TInput, TOutput, TValue>(this TInput input)
            where TInput : Enum
            where TOutput : Enum
            where TValue : struct
            => ((TValue)input.AsEnumValue()).AsEnum<TOutput, TValue>();

        #endregion

    }
}
