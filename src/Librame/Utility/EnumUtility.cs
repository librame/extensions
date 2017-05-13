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
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Librame.Utility
{
    /// <summary>
    /// 枚举工具。
    /// </summary>
    public class EnumUtility
    {
        /// <summary>
        /// 解析指定名称的枚举对象。
        /// </summary>
        /// <typeparam name="TEnum">指定的枚举类型。</typeparam>
        /// <param name="name">给定的名称。</param>
        /// <returns>返回枚举对象。</returns>
        public static TEnum AsEnum<TEnum>(string name)
        {
            return (TEnum)Enum.Parse(typeof(TEnum), name);
        }

        /// <summary>
        /// 解析指定 32 位整数值的枚举对象。
        /// </summary>
        /// <typeparam name="TEnum">指定的枚举类型。</typeparam>
        /// <param name="value">给定的值。</param>
        /// <returns>返回枚举对象。</returns>
        public static TEnum AsEnum<TEnum>(int value)
        {
            return AsEnum<TEnum, int>(value, v => v.ToString());
        }
        /// <summary>
        /// 解析指定值的枚举对象。
        /// </summary>
        /// <typeparam name="TEnum">指定的枚举类型。</typeparam>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="value">给定的值。</param>
        /// <param name="valueConverter">给定的值转换器。</param>
        /// <returns>返回枚举对象。</returns>
        public static TEnum AsEnum<TEnum, TValue>(TValue value, Func<TValue, string> valueConverter)
        {
            return (TEnum)Enum.Parse(typeof(TEnum), valueConverter(value));
        }

        /// <summary>
        /// 解析枚举项的名称。
        /// </summary>
        /// <typeparam name="TEnum">指定的枚举类型。</typeparam>
        /// <param name="item">给定的枚举项。</param>
        /// <returns>返回字符串。</returns>
        public static string AsName<TEnum>(TEnum item)
        {
            return Enum.GetName(typeof(TEnum), item);
        }


        /// <summary>
        /// 将枚举类型转换为项列表集合。
        /// </summary>
        /// <typeparam name="TEnum">指定的枚举类型。</typeparam>
        /// <typeparam name="TItem">指定的项类型。</typeparam>
        /// <param name="itemFactory">给定创建项类型实例的方法。</param>
        /// <param name="valueFactory">给定取得值的方法（可选；默认返回常数值）。</param>
        /// <param name="textFactory">给定取得文本的方法（可选；默认返回描述属性说明）。</param>
        /// <returns>返回项列表。</returns>
        public static IList<TItem> AsList<TEnum, TItem>(Func<string, string, TItem> itemFactory,
            Func<FieldInfo, DescriptionAttribute, string> valueFactory = null,
            Func<FieldInfo, DescriptionAttribute, string> textFactory = null)
            where TItem : class
        {
            return AsList(typeof(TEnum), itemFactory, valueFactory, textFactory);
        }
        /// <summary>
        /// 将枚举类型转换为项列表集合。
        /// </summary>
        /// <typeparam name="TItem">指定的项类型。</typeparam>
        /// <param name="enumType">给定的枚举类型。</param>
        /// <param name="itemFactory">给定创建项类型实例的方法。</param>
        /// <param name="valueFactory">给定取得值的方法（可选；默认返回常数值）。</param>
        /// <param name="textFactory">给定取得文本的方法（可选；默认返回描述属性说明）。</param>
        /// <returns>返回项列表。</returns>
        public static IList<TItem> AsList<TItem>(Type enumType,
            Func<string, string, TItem> itemFactory,
            Func<FieldInfo, DescriptionAttribute, string> valueFactory = null,
            Func<FieldInfo, DescriptionAttribute, string> textFactory = null)
            where TItem : class
        {
            if (ReferenceEquals(enumType, null) || !enumType.IsEnum)
                return null;

            itemFactory.NotNull(nameof(itemFactory));

            // 默认以常数值为值
            if (ReferenceEquals(valueFactory, null))
                valueFactory = (fi, descr) => ((int)fi.GetValue(null)).ToString();

            // 默认以描述内容为文本
            if (ReferenceEquals(textFactory, null))
                textFactory = (fi, descr) => descr.Description;

            var list = new List<TItem>();

            var fields = enumType.GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (var f in fields)
            {
                var attrib = (DescriptionAttribute)Attribute.GetCustomAttribute(f,
                    typeof(DescriptionAttribute), false);

                var value = valueFactory.Invoke(f, attrib);
                var text = textFactory.Invoke(f, attrib);

                var result = itemFactory(value, text);
                list.Add(result);
            }

            return list;
        }


        /// <summary>
        /// 获取枚举描述。
        /// </summary>
        /// <typeparam name="TEnum">指定的枚举类型。</typeparam>
        /// <param name="item">给定的枚举项。</param>
        /// <returns>返回字符串。</returns>
        public static string GetDescription<TEnum>(TEnum item)
        {
            return GetDescription(typeof(TEnum), AsName(item));
        }
        /// <summary>
        /// 获取指定枚举项的描述属性内容。
        /// </summary>
        /// <param name="enumType">给定的枚举类型。</param>
        /// <param name="name">给定的枚举项名称。</param>
        /// <returns>返回字符串。</returns>
        public static string GetDescription(Type enumType, string name)
        {
            if (ReferenceEquals(enumType, null) || !enumType.IsEnum)
                return null;

            var fields = enumType.GetFields(BindingFlags.Static | BindingFlags.Public);
            var field = fields.SingleOrDefault(f => f.Name == name);

            if (ReferenceEquals(field, null))
                return string.Empty;

            var attrib = (DescriptionAttribute)Attribute.GetCustomAttribute(field,
                typeof(DescriptionAttribute), false);

            return attrib?.Description;
        }

    }


    /// <summary>
    /// <see cref="EnumUtility"/> 静态扩展。
    /// </summary>
    public static class EnumUtilityExtensions
    {
        /// <summary>
        /// 解析指定名称的枚举对象。
        /// </summary>
        /// <typeparam name="TEnum">指定的枚举类型。</typeparam>
        /// <param name="name">给定的名称。</param>
        /// <returns>返回枚举对象。</returns>
        public static TEnum AsEnum<TEnum>(this string name)
        {
            return EnumUtility.AsEnum<TEnum>(name);
        }


        /// <summary>
        /// 解析指定 32 位整数值的枚举对象。
        /// </summary>
        /// <typeparam name="TEnum">指定的枚举类型。</typeparam>
        /// <param name="value">给定的值。</param>
        /// <returns>返回枚举对象。</returns>
        public static TEnum AsEnum<TEnum>(this int value)
        {
            return AsEnum<TEnum, int>(value, v => v.ToString());
        }
        /// <summary>
        /// 解析指定值的枚举对象。
        /// </summary>
        /// <typeparam name="TEnum">指定的枚举类型。</typeparam>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="value">给定的值。</param>
        /// <param name="valueConverter">给定的值转换器。</param>
        /// <returns>返回枚举对象。</returns>
        public static TEnum AsEnum<TEnum, TValue>(this TValue value, Func<TValue, string> valueConverter)
        {
            return (TEnum)Enum.Parse(typeof(TEnum), valueConverter(value));
        }

        /// <summary>
        /// 解析枚举项的名称。
        /// </summary>
        /// <typeparam name="TEnum">指定的枚举类型。</typeparam>
        /// <param name="item">给定的枚举项。</param>
        /// <returns>返回字符串。</returns>
        public static string AsEnumName<TEnum>(this TEnum item)
        {
            return EnumUtility.AsName(item);
        }

    }
}
