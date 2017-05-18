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

namespace Librame.Utility
{
    /// <summary>
    /// <see cref="string"/> 实用工具。
    /// </summary>
    public static class StringUtility
    {
        /// <summary>
        /// 空字符串。
        /// </summary>
        public const string Empty = "";

        /// <summary>
        /// 英文逗号分隔符。
        /// </summary>
        public const string PUNCTUATION_ENGLISH_COMMA = ",";

        /// <summary>
        /// 英文分号分隔符。
        /// </summary>
        public const string PUNCTUATION_ENGLISH_SEMICOLON = ";";

        /// <summary>
        /// 英文句号分隔符。
        /// </summary>
        public const string PUNCTUATION_ENGLISH_PERIOD = ".";

        /// <summary>
        /// 等号分隔符。
        /// </summary>
        public const string SEPARATOR_EQUALS = "=";


        /// <summary>
        /// 附加字符串到当前字符串的末尾。
        /// </summary>
        /// <param name="str">给定的当前字符串。</param>
        /// <param name="append">要附加的字符串。</param>
        /// <returns>返回附加后的字符串。</returns>
        public static string Append(this string str, string append)
        {
            return (str + append);
        }


        #region AsOrDefault

        /// <summary>
        /// 转换字符串格式的布尔值或返回默认值。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="defaultValue">给定的默认值。</param>
        /// <returns>返回布尔值。</returns>
        public static bool AsOrDefault(this string str, bool defaultValue)
        {
            return AsOrDefault(str, s => Boolean.Parse(s), defaultValue);
        }
        /// <summary>
        /// 转换字符串格式的 32 位整数值或返回默认值。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="defaultValue">给定的默认值。</param>
        /// <returns>返回 32 位整数值。</returns>
        public static int AsOrDefault(this string str, int defaultValue)
        {
            return AsOrDefault(str, s => Int32.Parse(s), defaultValue);
        }
        /// <summary>
        /// 转换字符串格式的 64 位整数值或返回默认值。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="defaultValue">给定的默认值。</param>
        /// <returns>返回 64 位整数值。</returns>
        public static long AsOrDefault(this string str, long defaultValue)
        {
            return AsOrDefault(str, s => Int64.Parse(s), defaultValue);
        }
        /// <summary>
        /// 转换指定值类型的值。
        /// </summary>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="str">给定的字符串。</param>
        /// <param name="convertFactory">给定的转换方法。</param>
        /// <param name="defaultValue">给定的默认值。</param>
        /// <returns>返回值。</returns>
        public static TValue AsOrDefault<TValue>(this string str, Func<string, TValue> convertFactory, TValue defaultValue)
        {
            try
            {
                return convertFactory(str);
            }
            catch
            {
                return defaultValue;
            }
        }


        /// <summary>
        /// 转换对象或默认值。
        /// </summary>
        /// <param name="value">给定的值。</param>
        /// <param name="type">给定的转换类型。</param>
        /// <returns>返回对象。</returns>
        public static object AsOrDefault(this string value, Type type)
        {
            bool isDefault = string.IsNullOrEmpty(value);

            switch (type.FullName)
            {
                case "System.Boolean":
                    return (isDefault ? false : bool.Parse(value));

                case "System.Decimal":
                    return (isDefault ? decimal.One : decimal.Parse(value));

                case "System.Double":
                    return (isDefault ? double.NaN : double.Parse(value));

                case "System.DateTime":
                    return (isDefault ? DateTime.Now : DateTime.Parse(value));

                case "System.Guid":
                    return (isDefault ? Guid.Empty : Guid.Parse(value));

                case "System.String":
                    return value;

                case "System.TimeSpan":
                    return (isDefault ? TimeSpan.Zero : TimeSpan.Parse(value));

                // Int
                case "System.Byte":
                    return (isDefault ? byte.MinValue : byte.Parse(value)); // byte

                case "System.Int16":
                    return (isDefault ? byte.MinValue : short.Parse(value)); // short

                case "System.Int32":
                    return (isDefault ? byte.MinValue : int.Parse(value)); // int

                case "System.Int64":
                    return (isDefault ? byte.MinValue : long.Parse(value)); // long

                case "System.SByte":
                    return (isDefault ? sbyte.MinValue : sbyte.Parse(value)); // sbyte

                case "System.UInt16":
                    return (isDefault ? byte.MinValue : ushort.Parse(value)); // ushort

                case "System.UInt32":
                    return (isDefault ? byte.MinValue : uint.Parse(value)); // uint

                case "System.UInt64":
                    return (isDefault ? byte.MinValue : ulong.Parse(value)); // ulong

                default:
                    {
                        if (type.IsGenericType)
                        {
                            try
                            {
                                var gts = type.GenericTypeArguments;
                                // 链式转换
                                var parameters = gts.Select(t => AsOrDefault(value, t)).ToArray();

                                var ci = type.GetConstructor(type.GenericTypeArguments);
                                return ci.Invoke(parameters);
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }

                        if (type.IsClass && !type.IsAbstract)
                            return Activator.CreateInstance(type);

                        return null;
                    }
            }
        }

        #endregion


        #region Trim

        /// <summary>
        /// 清除首尾英文逗号。
        /// </summary>
        /// <param name="str">指定的字符串。</param>
        /// <returns>返回清除后的字符串。</returns>
        public static string TrimComma(this string str)
        {
            return Trim(str, PUNCTUATION_ENGLISH_COMMA);
        }

        /// <summary>
        /// 清除首尾英文句号。
        /// </summary>
        /// <param name="str">指定的字符串。</param>
        /// <returns>返回清除后的字符串。</returns>
        public static string TrimPeriod(this string str)
        {
            return Trim(str, PUNCTUATION_ENGLISH_PERIOD);
        }

        /// <summary>
        /// 清除首尾英文分号。
        /// </summary>
        /// <param name="str">指定的字符串。</param>
        /// <returns>返回清除后的字符串。</returns>
        public static string TrimSemicolon(this string str)
        {
            return Trim(str, PUNCTUATION_ENGLISH_SEMICOLON);
        }


        /// <summary>
        /// 清除首尾指定字符串（忽略大小写）。
        /// </summary>
        /// <param name="str">指定的字符串。</param>
        /// <param name="trim">要清除的字符串。</param>
        /// <param name="loops">是否循环查找（可选；默认启用）。</param>
        /// <returns>返回清除后的字符串。</returns>
        public static string Trim(this string str, string trim, bool loops = true)
        {
            str = TrimStart(str, trim, loops);

            str = TrimEnd(str, trim, loops);

            return str;
        }

        /// <summary>
        /// 清除首部指定字符串（忽略大小写）。
        /// </summary>
        /// <param name="str">指定的字符串。</param>
        /// <param name="trim">要清除的字符串。</param>
        /// <param name="loops">是否循环查找（可选；默认启用）。</param>
        /// <returns>返回清除后的字符串。</returns>
        public static string TrimStart(this string str, string trim, bool loops = true)
        {
            if (str.StartsWith(trim, StringComparison.OrdinalIgnoreCase))
            {
                str = str.Substring(trim.Length);

                if (loops)
                {
                    str = TrimStart(str, trim);
                }
            }

            return str;
        }

        /// <summary>
        /// 清除尾部指定字符串（忽略大小写）。
        /// </summary>
        /// <param name="str">指定的字符串。</param>
        /// <param name="trim">要清除的字符串。</param>
        /// <param name="loops">是否循环查找（可选；默认启用）。</param>
        /// <returns>返回清除后的字符串。</returns>
        public static string TrimEnd(this string str, string trim, bool loops = true)
        {
            if (str.EndsWith(trim, StringComparison.OrdinalIgnoreCase))
            {
                str = str.Substring(0, (str.Length - trim.Length));

                if (loops)
                {
                    str = TrimEnd(str, trim);
                }
            }

            return str;
        }

        #endregion

    }
}
