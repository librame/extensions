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

        #region AsEnumField

        /// <summary>
        /// 转换为特定枚举字段。
        /// </summary>
        /// <typeparam name="TEnum">指定的枚举类型。</typeparam>
        /// <param name="name">给定的字段名称。</param>
        /// <returns>返回枚举对象。</returns>
        public static TEnum AsEnumField<TEnum>(this string name)
            where TEnum : struct
        {
            return (TEnum)Enum.Parse(typeof(TEnum), name);
        }

        /// <summary>
        /// 转换为特定枚举字段。
        /// </summary>
        /// <typeparam name="TEnum">指定的枚举类型。</typeparam>
        /// <param name="value">给定的 64 位有符号整数值。</param>
        /// <returns>返回枚举对象。</returns>
        public static TEnum AsEnumField<TEnum>(this long value)
            where TEnum : struct
        {
            return value.AsEnumField<TEnum, long>(v => v.ToString());
        }
        /// <summary>
        /// 转换为特定枚举字段。
        /// </summary>
        /// <typeparam name="TEnum">指定的枚举类型。</typeparam>
        /// <param name="value">给定的 32 位有符号整数值。</param>
        /// <returns>返回枚举对象。</returns>
        public static TEnum AsEnumField<TEnum>(this int value)
            where TEnum : struct
        {
            return value.AsEnumField<TEnum, int>(v => v.ToString());
        }
        /// <summary>
        /// 转换为特定枚举字段。
        /// </summary>
        /// <typeparam name="TEnum">指定的枚举类型。</typeparam>
        /// <param name="value">给定的 16 位有符号整数值。</param>
        /// <returns>返回枚举对象。</returns>
        public static TEnum AsEnumField<TEnum>(this short value)
            where TEnum : struct
        {
            return value.AsEnumField<TEnum, short>(v => v.ToString());
        }
        /// <summary>
        /// 转换为特定枚举字段。
        /// </summary>
        /// <typeparam name="TEnum">指定的枚举类型。</typeparam>
        /// <param name="value">给定的 8 位有符号整数值。</param>
        /// <returns>返回枚举对象。</returns>
        public static TEnum AsEnumField<TEnum>(this sbyte value)
            where TEnum : struct
        {
            return value.AsEnumField<TEnum, sbyte>(v => v.ToString());
        }

        /// <summary>
        /// 转换为特定枚举字段。
        /// </summary>
        /// <typeparam name="TEnum">指定的枚举类型。</typeparam>
        /// <param name="value">给定的 64 位无符号整数值。</param>
        /// <returns>返回枚举对象。</returns>
        public static TEnum AsEnumField<TEnum>(this ulong value)
            where TEnum : struct
        {
            return value.AsEnumField<TEnum, ulong>(v => v.ToString());
        }
        /// <summary>
        /// 转换为特定枚举字段。
        /// </summary>
        /// <typeparam name="TEnum">指定的枚举类型。</typeparam>
        /// <param name="value">给定的 32 位无符号整数值。</param>
        /// <returns>返回枚举对象。</returns>
        public static TEnum AsEnumField<TEnum>(this uint value)
            where TEnum : struct
        {
            return value.AsEnumField<TEnum, uint>(v => v.ToString());
        }
        /// <summary>
        /// 转换为特定枚举字段。
        /// </summary>
        /// <typeparam name="TEnum">指定的枚举类型。</typeparam>
        /// <param name="value">给定的 16 位无符号整数值。</param>
        /// <returns>返回枚举对象。</returns>
        public static TEnum AsEnumField<TEnum>(this ushort value)
            where TEnum : struct
        {
            return value.AsEnumField<TEnum, ushort>(v => v.ToString());
        }
        /// <summary>
        /// 转换为特定枚举字段。
        /// </summary>
        /// <typeparam name="TEnum">指定的枚举类型。</typeparam>
        /// <param name="value">给定的 8 位无符号整数值。</param>
        /// <returns>返回枚举对象。</returns>
        public static TEnum AsEnumField<TEnum>(this byte value)
            where TEnum : struct
        {
            return value.AsEnumField<TEnum, byte>(v => v.ToString());
        }

        /// <summary>
        /// 转换为特定枚举字段。
        /// </summary>
        /// <typeparam name="TEnum">指定的枚举类型。</typeparam>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="value">给定的值。</param>
        /// <param name="valueConverter">给定的值转换器。</param>
        /// <returns>返回枚举对象。</returns>
        private static TEnum AsEnumField<TEnum, TValue>(this TValue value,
            Func<TValue, string> valueConverter)
            where TEnum : struct
        {
            return (TEnum)Enum.Parse(typeof(TEnum), valueConverter?.Invoke(value));
        }

        #endregion


        #region AsOutputEnumField

        /// <summary>
        /// 将输入枚举字段转换为输出枚举字段（通过匹配枚举字段名称实现）。
        /// </summary>
        /// <typeparam name="TInputEnum">指定的输入枚举类型。</typeparam>
        /// <typeparam name="TOutputEnum">指定的输出枚举类型。</typeparam>
        /// <param name="inputEnumField">给定的输入枚举字段。</param>
        /// <returns>返回输出枚举字段。</returns>
        public static TOutputEnum AsOutputEnumFieldByName<TInputEnum, TOutputEnum>(this TInputEnum inputEnumField)
            where TInputEnum : struct
            where TOutputEnum : struct
        {
            return inputEnumField.AsEnumName().AsEnumField<TOutputEnum>();
        }

        #endregion


        #region AsEnumValue

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


        /// <summary>
        /// 当作枚举值。
        /// </summary>
        /// <typeparam name="TEnum">指定的枚举类型。</typeparam>
        /// <typeparam name="TAttribute">指定的特性类型。</typeparam>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="enumField">给定的枚举字段。</param>
        /// <param name="converter">给定的值转换方法。</param>
        /// <returns>返回值。</returns>
        public static TValue AsEnumValueWithAttribute<TEnum, TAttribute, TValue>(this TEnum enumField,
            Func<FieldInfo, TAttribute, TValue> converter)
            where TEnum : struct
            where TAttribute : Attribute
        {
            var name = enumField.AsEnumName();

            return typeof(TEnum).AsEnumValueWithAttribute(name, converter);
        }
        /// <summary>
        /// 当作枚举值。
        /// </summary>
        /// <typeparam name="TAttribute">指定的特性类型。</typeparam>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="enumType">给定的枚举类型。</param>
        /// <param name="name">给定的枚举字段名称。</param>
        /// <param name="converter">给定的值转换方法。</param>
        /// <returns>返回值。</returns>
        public static TValue AsEnumValueWithAttribute<TAttribute, TValue>(this Type enumType,
            string name, Func<FieldInfo, TAttribute, TValue> converter)
            where TAttribute : Attribute
        {
            name.NotEmpty(nameof(name));
            converter.NotDefault(nameof(converter));

            var field = enumType.AsEnumFields().SingleOrDefault(f => f.Name == name);
            if (field.IsDefault()) return default;

            var attrib = field.AsAttribute<TAttribute>();
            return converter.Invoke(field, attrib);
        }

        #endregion


        #region AsEnumFields

        /// <summary>
        /// 当作枚举字段集合。
        /// </summary>
        /// <typeparam name="TEnum">指定的枚举类型。</typeparam>
        /// <returns>返回字段信息数组。</returns>
        public static FieldInfo[] AsEnumFields<TEnum>()
            where TEnum : struct
        {
            return typeof(TEnum).AsEnumFields();
        }
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
        /// 当作枚举结果集合。
        /// </summary>
        /// <typeparam name="TEnum">指定的枚举类型。</typeparam>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="converter">给定的结果转换方法。</param>
        /// <returns>返回结果集合。</returns>
        public static IEnumerable<TResult> AsEnumResults<TEnum, TResult>(
            Func<FieldInfo, object, TResult> converter)
            where TEnum : struct
        {
            return typeof(TEnum).AsEnumResults(converter);
        }
        /// <summary>
        /// 当作枚举结果集合。
        /// </summary>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="enumType">给定的枚举类型。</param>
        /// <param name="converter">给定的结果转换方法。</param>
        /// <returns>返回结果集合。</returns>
        public static IEnumerable<TResult> AsEnumResults<TResult>(this Type enumType,
            Func<FieldInfo, object, TResult> converter)
        {
            converter.NotDefault(nameof(converter));

            var fields = enumType.AsEnumFields();

            return fields.Select(f =>
            {
                var number = f.GetValue(null);
                return converter.Invoke(f, number);
            });
        }


        /// <summary>
        /// 当作枚举结果集合。
        /// </summary>
        /// <typeparam name="TEnum">指定的枚举类型。</typeparam>
        /// <typeparam name="TAttribute">指定的特性类型。</typeparam>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="converter">给定的结果转换方法。</param>
        /// <returns>返回结果集合。</returns>
        public static IEnumerable<TResult> AsEnumResultsWithAttribute<TEnum, TAttribute, TResult>(
            Func<FieldInfo, TAttribute, object, TResult> converter)
            where TEnum : struct
            where TAttribute : Attribute
        {
            return typeof(TEnum).AsEnumResultsWithAttribute(converter);
        }
        /// <summary>
        /// 当作枚举结果集合。
        /// </summary>
        /// <typeparam name="TAttribute">指定的特性类型。</typeparam>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="enumType">给定的枚举类型。</param>
        /// <param name="converter">给定的结果转换方法。</param>
        /// <returns>返回结果集合。</returns>
        public static IEnumerable<TResult> AsEnumResultsWithAttribute<TAttribute, TResult>(this Type enumType,
            Func<FieldInfo, TAttribute, object, TResult> converter)
            where TAttribute : Attribute
        {
            converter.NotDefault(nameof(converter));
            
            var fields = enumType.AsEnumFields();

            return fields.Select(f =>
            {
                var attrib = f.AsAttribute<TAttribute>();
                var number = f.GetValue(null);

                return converter.Invoke(f, attrib, number);
            });
        }

        #endregion


        #region AsEnumTextValues

        /// <summary>
        /// 当作枚举文本、值集合。
        /// </summary>
        /// <typeparam name="TEnum">指定的枚举类型。</typeparam>
        /// <typeparam name="TText">指定的文本类型。</typeparam>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <typeparam name="TResult">指定的项类型。</typeparam>
        /// <param name="textConverter">给定的文本转换方法。</param>
        /// <param name="valueConverter">给定的值转换方法。</param>
        /// <param name="converter">给定的结果转换方法。</param>
        /// <returns>返回结果集合。</returns>
        public static IEnumerable<TResult> AsEnumTextValues<TEnum, TText, TValue, TResult>(
            Func<FieldInfo, object, TText> textConverter,
            Func<FieldInfo, object, TValue> valueConverter,
            Func<TText, TValue, TResult> converter)
            where TEnum : struct
        {
            return typeof(TEnum).AsEnumTextValues(textConverter, valueConverter, converter);
        }
        /// <summary>
        /// 当作枚举文本、值集合。
        /// </summary>
        /// <typeparam name="TText">指定的文本类型。</typeparam>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <typeparam name="TResult">指定的项类型。</typeparam>
        /// <param name="enumType">给定的枚举类型。</param>
        /// <param name="textConverter">给定的文本转换方法。</param>
        /// <param name="valueConverter">给定的值转换方法。</param>
        /// <param name="converter">给定的结果转换方法。</param>
        /// <returns>返回结果集合。</returns>
        public static IEnumerable<TResult> AsEnumTextValues<TText, TValue, TResult>(this Type enumType,
            Func<FieldInfo, object, TText> textConverter,
            Func<FieldInfo, object, TValue> valueConverter,
            Func<TText, TValue, TResult> converter)
        {
            textConverter.NotDefault(nameof(textConverter));
            valueConverter.NotDefault(nameof(valueConverter));
            converter.NotDefault(nameof(converter));

            var fields = enumType.AsEnumFields();

            return fields.Select(f =>
            {
                var number = f.GetValue(null);

                var text = textConverter.Invoke(f, number);
                var value = valueConverter.Invoke(f, number);

                return converter.Invoke(text, value);
            });
        }


        /// <summary>
        /// 当作枚举文本、值集合。
        /// </summary>
        /// <typeparam name="TEnum">指定的枚举类型。</typeparam>
        /// <typeparam name="TAttribute">指定的特性类型。</typeparam>
        /// <typeparam name="TText">指定的文本类型。</typeparam>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <typeparam name="TResult">指定的项类型。</typeparam>
        /// <param name="textConverter">给定的文本转换方法。</param>
        /// <param name="valueConverter">给定的值转换方法。</param>
        /// <param name="converter">给定的结果转换方法。</param>
        /// <returns>返回结果集合。</returns>
        public static IEnumerable<TResult> AsEnumTextValuesWithAttribute<TEnum, TAttribute, TText, TValue, TResult>(
            Func<FieldInfo, TAttribute, object, TText> textConverter,
            Func<FieldInfo, TAttribute, object, TValue> valueConverter,
            Func<TText, TValue, TResult> converter)
            where TEnum : struct
            where TAttribute : Attribute
        {
            return typeof(TEnum).AsEnumTextValuesWithAttribute(textConverter, valueConverter, converter);
        }
        /// <summary>
        /// 当作枚举文本、值集合。
        /// </summary>
        /// <typeparam name="TAttribute">指定的特性类型。</typeparam>
        /// <typeparam name="TText">指定的文本类型。</typeparam>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <typeparam name="TResult">指定的项类型。</typeparam>
        /// <param name="enumType">给定的枚举类型。</param>
        /// <param name="textConverter">给定的文本转换方法。</param>
        /// <param name="valueConverter">给定的值转换方法。</param>
        /// <param name="converter">给定的结果转换方法。</param>
        /// <returns>返回结果集合。</returns>
        public static IEnumerable<TResult> AsEnumTextValuesWithAttribute<TAttribute, TText, TValue, TResult>(this Type enumType,
            Func<FieldInfo, TAttribute, object, TText> textConverter,
            Func<FieldInfo, TAttribute, object, TValue> valueConverter,
            Func<TText, TValue, TResult> converter)
            where TAttribute : Attribute
        {
            textConverter.NotDefault(nameof(textConverter));
            valueConverter.NotDefault(nameof(valueConverter));
            converter.NotDefault(nameof(converter));

            var fields = enumType.AsEnumFields();

            return fields.Select(f =>
            {
                var attrib = f.AsAttribute<TAttribute>();
                var number = f.GetValue(null);

                var text = textConverter.Invoke(f, attrib, number);
                var value = valueConverter.Invoke(f, attrib, number);

                return converter.Invoke(text, value);
            });
        }

        #endregion

    }
}
