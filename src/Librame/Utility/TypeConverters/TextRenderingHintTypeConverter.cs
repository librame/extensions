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
using System.Drawing.Text;
using System.Globalization;

namespace Librame.Utility.TypeConverters
{
    /// <summary>
    /// <see cref="TextRenderingHint"/> 类型转换器。
    /// </summary>
    public class TextRenderingHintTypeConverter : TypeConverter
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
        /// <param name="value">给定的 <see cref="TextRenderingHint"/>。</param>
        /// <param name="destinationType">给定的目标类型。</param>
        /// <returns>返回字符串。</returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value != null)
            {
                if (!(value is TextRenderingHint))
                    throw new Exception(string.Format(Properties.Resources.TypeNoSupportConversionExceptionFormat, value.GetType()));
            }

            // convert to a string
            if (destinationType == typeof(string))
            {
                // no value so we return an empty string
                if (value == null)
                    return string.Empty;

                // strongly typed
                var trh = (TextRenderingHint)value;

                // convert to a string and return
                return Enum.GetName(typeof(TextRenderingHint), trh);
            }

            // call the base converter
            return base.ConvertTo(context, culture, value, destinationType);
        }


        /// <summary>
        /// 能否还原为 <see cref="TextRenderingHint"/>。
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
        /// 还原为 <see cref="TextRenderingHint"/>。
        /// </summary>
        /// <param name="context">给定的 <see cref="ITypeDescriptorContext"/>。</param>
        /// <param name="culture">给定的 <see cref="CultureInfo"/>。</param>
        /// <param name="value">给定的字符串值。</param>
        /// <returns>返回 <see cref="TextRenderingHint"/>。</returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var defaultTRH = TextRenderingHint.SystemDefault;

            // no value so we return a new TextRenderingHint instance
            if (value == null)
                return defaultTRH;

            // convert from a string
            if (value is string)
            {
                // get strongly typed value
                string str = (value as string);

                // empty string so we return a new TextRenderingHint instance
                if (str.Length <= 0)
                    return defaultTRH;

                try
                {
                    // create a new TextRenderingHint instance with these values and return it
                    return (TextRenderingHint)Enum.Parse(typeof(TextRenderingHint), str);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            // otherwise call the base converter
            return base.ConvertFrom(context, culture, value);
        }

    }
}
