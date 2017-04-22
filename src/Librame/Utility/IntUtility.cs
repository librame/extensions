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

namespace Librame.Utility
{
    /// <summary>
    /// 整数实用工具。
    /// </summary>
    public class IntUtility
    {
        /// <summary>
        /// 将数值格式化为 2 位长度的字符串。
        /// </summary>
        /// <param name="number">给定的数值。</param>
        /// <returns>返回字符串。</returns>
        public static string AsString(int number)
        {
            return AsString(number, 2);
        }
        /// <summary>
        /// 将数值格式化为指定长度的字符串。
        /// </summary>
        /// <param name="number">给定的数值。</param>
        /// <param name="length">指定的长度。</param>
        /// <returns>返回字符串。</returns>
        public static string AsString(int number, int length)
        {
            string numberString = number.ToString();

            if (numberString.Length >= length)
                return numberString;

            string format = "0:";
            for (int i = 0; i < length; i++)
            {
                format += "0";
            }
            format = "{" + format + "}";

            return String.Format(format, number);
        }


        /// <summary>
        /// 格式化存储容量大小为友好的字符串形式。
        /// </summary>
        /// <param name="size">给定的存储容量大小。</param>
        /// <param name="sizeUnit">给定的显示大小单位（可选；默认单位为兆字节）。</param>
        /// <returns>返回字符串。</returns>
        public static string FormatFileSizeUnit(int size,
            FileSizeUnit sizeUnit = FileSizeUnit.MiB)
        {
            if ((long)sizeUnit >= (long)FileSizeUnit.TiB)
                throw new ArgumentException("32 位整数值不支持 TB 及以上的单位");

            return FormatFileSizeUnit((long)size, sizeUnit);
        }
        /// <summary>
        /// 格式化存储容量大小为友好的字符串形式。
        /// </summary>
        /// <param name="size">给定的存储容量大小。</param>
        /// <param name="sizeUnit">给定的显示大小单位（可选；默认单位为兆字节）。</param>
        /// <returns>返回字符串。</returns>
        public static string FormatFileSizeUnit(long size,
            FileSizeUnit sizeUnit = FileSizeUnit.MiB)
        {
            if (size < 1)
                return string.Format("{0} byte", size);

            if (sizeUnit == FileSizeUnit.Byte)
                return string.Format("{0} bytes", size);

            var unitSize = (long)sizeUnit;
            var unitString = EnumUtility.AsName(sizeUnit);
            return string.Format("{0:0,0.00} " + unitString, ((double)size) / unitSize);
        }


        /// <summary>
        /// 获取不大于最大值的整数。
        /// </summary>
        /// <param name="integer">给定的整数。</param>
        /// <param name="max">给定的最大值。</param>
        /// <param name="rtnMaxFactory">返回最大值的工厂方法（可选；默认为不大于最大值）。</param>
        /// <returns>返回整数。</returns>
        public static int Max(int integer, int max, Func<int, int, bool> rtnMaxFactory = null)
        {
            if (ReferenceEquals(rtnMaxFactory, null))
                rtnMaxFactory = (i, m) => i > max;

            if (rtnMaxFactory.Invoke(integer, max))
                return max;

            return integer;
        }
        /// <summary>
        /// 获取不小于最小值的整数。
        /// </summary>
        /// <param name="integer">给定的整数。</param>
        /// <param name="min">给定的最小值。</param>
        /// <param name="rtnMinFactory">返回最小值的工厂方法（可选；默认为不小于最小值）。</param>
        /// <returns>返回整数。</returns>
        public static int Min(int integer, int min, Func<int, int, bool> rtnMinFactory = null)
        {
            if (ReferenceEquals(rtnMinFactory, null))
                rtnMinFactory = (i, m) => i < min;

            if (rtnMinFactory.Invoke(integer, min))
                return min;

            return integer;
        }
        /// <summary>
        /// 获取既不小于最小值，也不大于最大值的整数。
        /// </summary>
        /// <exception cref="ArgumentException">
        /// 最小值不能大于等于最大值。
        /// </exception>
        /// <param name="integer">给定的整数。</param>
        /// <param name="min">给定的最小值。</param>
        /// <param name="max">给定的最大值。</param>
        /// <param name="rtnMinFactory">返回最小值的工厂方法（可选；默认为不小于最小值）。</param>
        /// <param name="rtnMaxFactory">返回最大值的工厂方法（可选；默认为不大于最大值）。</param>
        /// <returns>返回整数。</returns>
        public static int Range(int integer, int min, int max,
            Func<int, int, bool> rtnMinFactory = null, Func<int, int, bool> rtnMaxFactory = null)
        {
            if (min >= max)
            {
                var format = string.Format("The minimum value ({0}) is not greater than the maximum value {1}.",
                    min.ToString(), max.ToString());

                throw new ArgumentException(format);
            }

            integer = Min(integer, min, rtnMinFactory);

            return Max(integer, max, rtnMaxFactory);
        }

    }

    /// <summary>
    /// <see cref="IntUtility"/> 静态扩展。
    /// </summary>
    public static class IntUtilityExtensions
    {
        /// <summary>
        /// 将数值转换为 2 位长度的字符串。
        /// </summary>
        /// <param name="number">给定的数值。</param>
        /// <returns>返回字符串。</returns>
        public static string AsString(this int number)
        {
            return IntUtility.AsString(number);
        }
        /// <summary>
        /// 将数值转换为指定长度的字符串。
        /// </summary>
        /// <param name="number">给定的数值。</param>
        /// <param name="length">指定的长度。</param>
        /// <returns>返回字符串。</returns>
        public static string AsString(this int number, int length)
        {
            return IntUtility.AsString(number, length);
        }


        /// <summary>
        /// 格式化存储容量大小为友好的字符串形式。
        /// </summary>
        /// <param name="size">给定的存储容量大小。</param>
        /// <param name="sizeUnit">给定的显示大小单位（可选；默认单位为兆字节）。</param>
        /// <returns>返回字符串。</returns>
        public static string FormatFileSizeUnit(this int size,
            FileSizeUnit sizeUnit = FileSizeUnit.MiB)
        {
            return IntUtility.FormatFileSizeUnit(size, sizeUnit);
        }
        /// <summary>
        /// 格式化存储容量大小为友好的字符串形式。
        /// </summary>
        /// <param name="size">给定的存储容量大小。</param>
        /// <param name="sizeUnit">给定的显示单位。</param>
        /// <returns>返回字符串。</returns>
        public static string FormatFileSizeUnit(this long size, FileSizeUnit sizeUnit)
        {
            return IntUtility.FormatFileSizeUnit(size, sizeUnit);
        }


        /// <summary>
        /// 获取不大于最大值的整数。
        /// </summary>
        /// <param name="integer">给定的整数。</param>
        /// <param name="max">给定的最大值。</param>
        /// <param name="rtnMaxFactory">返回最大值的工厂方法（可选；默认为不大于最大值）。</param>
        /// <returns>返回整数。</returns>
        public static int Max(this int integer, int max, Func<int, int, bool> rtnMaxFactory = null)
        {
            return IntUtility.Max(integer, max, rtnMaxFactory);
        }
        /// <summary>
        /// 获取不小于最小值的整数。
        /// </summary>
        /// <param name="integer">给定的整数。</param>
        /// <param name="min">给定的最小值。</param>
        /// <param name="rtnMinFactory">返回最小值的工厂方法（可选；默认为不小于最小值）。</param>
        /// <returns>返回整数。</returns>
        public static int Min(this int integer, int min, Func<int, int, bool> rtnMinFactory = null)
        {
            return IntUtility.Min(integer, min, rtnMinFactory);
        }
        /// <summary>
        /// 获取既不小于最小值，也不大于最大值的整数。
        /// </summary>
        /// <exception cref="ArgumentException">
        /// 最小值不能大于等于最大值。
        /// </exception>
        /// <param name="integer">给定的整数。</param>
        /// <param name="min">给定的最小值。</param>
        /// <param name="max">给定的最大值。</param>
        /// <param name="rtnMinFactory">返回最小值的工厂方法（可选；默认为不小于最小值）。</param>
        /// <param name="rtnMaxFactory">返回最大值的工厂方法（可选；默认为不大于最大值）。</param>
        /// <returns>返回整数。</returns>
        public static int Range(this int integer, int min, int max,
            Func<int, int, bool> rtnMinFactory = null, Func<int, int, bool> rtnMaxFactory = null)
        {
            return IntUtility.Range(integer, min, max, rtnMinFactory, rtnMaxFactory);
        }

    }
}
