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
using System.ComponentModel;
using System.Drawing;
using System.Globalization;

namespace Librame.Utility.TypeConverters
{
    /// <summary>
    /// <see cref="Color"/> 类型转换器。
    /// </summary>
    public class ColorTypeConverter : TypeConverter
    {
        /// <summary>
        /// 能否转换为字符串。
        /// </summary>
        /// <param name="context">给定的 <see cref="ITypeDescriptorContext"/>。</param>
        /// <param name="destinationType">给定的目标类型。</param>
        /// <returns>返回布尔值。</returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// 转换为字符串。
        /// </summary>
        /// <param name="context">给定的 <see cref="ITypeDescriptorContext"/>。</param>
        /// <param name="culture">给定的 <see cref="CultureInfo"/>。</param>
        /// <param name="value">给定的 <see cref="Color"/>。</param>
        /// <param name="destinationType">给定的目标类型。</param>
        /// <returns>返回字符串。</returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            // check that the value we got passed on is of type Color
            if (value != null)
            {
                if (!(value is Color))
                    throw new Exception(string.Format(Properties.Resources.TypeNoSupportConversionExceptionFormat, value.GetType()));
            }

            // convert to a string
            if (destinationType == typeof(string))
            {
                // no value so we return an empty string
                if (value == null)
                    return string.Empty;

                var color = (Color)value;
                return color.ToString();
            }

            // call the base converter
            return base.ConvertTo(context, culture, value, destinationType);
        }


        /// <summary>
        /// 能否还原为 <see cref="Color"/>。
        /// </summary>
        /// <param name="context">给定的 <see cref="ITypeDescriptorContext"/>。</param>
        /// <param name="sourceType">给定的来源类型。</param>
        /// <returns>返回布尔值。</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// 还原为 <see cref="Color"/>。
        /// </summary>
        /// <param name="context">给定的 <see cref="ITypeDescriptorContext"/>。</param>
        /// <param name="culture">给定的 <see cref="CultureInfo"/>。</param>
        /// <param name="value">给定的字符串值。</param>
        /// <returns>返回 <see cref="Color"/>。</returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var defaultColor = Color.Black;

            // no value so we return a new Color instance
            if (value == null)
                return defaultColor;

            // convert from a string
            if (value is string)
            {
                string str = (value as string);
                
                if (str.Length <= 0)
                    return defaultColor;
                
                if (str.Contains(","))
                {
                    try
                    {
                        // 分组整数型参数
                        var part = str.Split(',');

                        if (part.Length == 3)
                        {
                            var red = int.Parse(part[0]);
                            var green = int.Parse(part[1]);
                            var blue = int.Parse(part[2]);

                            return Color.FromArgb(red, green, blue);
                        }
                        else if (part.Length == 4)
                        {
                            var alpha = int.Parse(part[0]);
                            var red = int.Parse(part[1]);
                            var green = int.Parse(part[2]);
                            var blue = int.Parse(part[3]);

                            return Color.FromArgb(alpha, red, green, blue);
                        }
                        else
                        {
                            return defaultColor;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else
                {
                    try
                    {
                        // 名称
                        return Color.FromName(str);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }

            // convert from a int
            if (value is int)
            {
                try
                {
                    // 32 位 ARGB 值的值
                    return Color.FromArgb((int)value);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            
            return base.ConvertFrom(context, culture, value);
        }

    }
}
