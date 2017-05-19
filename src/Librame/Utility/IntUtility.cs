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
    public static class IntUtility
    {
        /// <summary>
        /// 将数值格式化为 2 位长度的字符串。
        /// </summary>
        /// <param name="number">给定的数值。</param>
        /// <returns>返回字符串。</returns>
        public static string AsString(this int number)
        {
            return AsString(number, 2);
        }
        /// <summary>
        /// 将数值格式化为指定长度的字符串。
        /// </summary>
        /// <param name="number">给定的数值。</param>
        /// <param name="length">指定的长度。</param>
        /// <returns>返回字符串。</returns>
        public static string AsString(this int number, int length)
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
        public static string FormatFileSizeUnit(this int size,
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
        public static string FormatFileSizeUnit(this long size,
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
        /// <param name="returnMaxFactory">给定返回最大值的条件，反之返回当前整数（可选；默认为不大于最大值）。</param>
        /// <returns>返回当前整数或最大值。</returns>
        public static int Max(this int integer, int max, Func<int, int, bool> returnMaxFactory = null)
        {
            if (ReferenceEquals(returnMaxFactory, null))
                returnMaxFactory = (i, m) => i > max;

            if (returnMaxFactory.Invoke(integer, max))
                return max;

            return integer;
        }

        /// <summary>
        /// 获取不小于最小值的整数。
        /// </summary>
        /// <param name="integer">给定的整数。</param>
        /// <param name="min">给定的最小值。</param>
        /// <param name="returnMinFactory">给定返回最小值的条件，反之返回当前整数（可选；默认为不小于最小值）。</param>
        /// <returns>返回当前整数或最小值。</returns>
        public static int Min(this int integer, int min, Func<int, int, bool> returnMinFactory = null)
        {
            if (ReferenceEquals(returnMinFactory, null))
                returnMinFactory = (i, m) => i < min;

            if (returnMinFactory.Invoke(integer, min))
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
        /// <param name="returnMinFactory">返回最小值的工厂方法（可选；默认为不小于最小值）。</param>
        /// <param name="returnMaxFactory">返回最大值的工厂方法（可选；默认为不大于最大值）。</param>
        /// <returns>返回整数。</returns>
        public static int Range(this int integer, int min, int max,
            Func<int, int, bool> returnMinFactory = null, Func<int, int, bool> returnMaxFactory = null)
        {
            if (min >= max)
            {
                var format = string.Format("The minimum value ({0}) is not greater than the maximum value {1}.",
                    min.ToString(), max.ToString());

                throw new ArgumentException(format);
            }

            integer = Min(integer, min, returnMinFactory);

            return Max(integer, max, returnMaxFactory);
        }

    }
}
