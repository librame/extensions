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
using System.ComponentModel.Design.Serialization;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Librame.Extensions.Core.Converters
{
    /// <summary>
    /// 抽象字符串转换器。
    /// </summary>
    public abstract class AbstractStringConverter : TypeConverter
    {
        /// <summary>
        /// 默认对象。
        /// </summary>
        protected virtual object DefaultObject { get; }
            = null;


        /// <summary>
        /// 还原字符串为对象。
        /// </summary>
        /// <param name="context">给定的 <see cref="ITypeDescriptorContext"/>。</param>
        /// <param name="culture">给定的 <see cref="CultureInfo"/>。</param>
        /// <param name="value">给定的字符串对象。</param>
        /// <returns>返回还原的对象。</returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string destination)
            {
                if (destination.IsEmpty())
                    return DefaultObject;

                return CustomConvertFrom(destination);
            }

            if (value is InstanceDescriptor instanceDescriptor)
                return instanceDescriptor.Invoke();

            throw GetConvertFromException(value);
        }

        /// <summary>
        /// 自定义还原字符串为对象。
        /// </summary>
        /// <param name="destination">给定的目标字符串。</param>
        /// <returns>返回还原的对象。</returns>
        protected abstract object CustomConvertFrom(string destination);


        /// <summary>
        /// 转换对象为字符串。
        /// </summary>
        /// <param name="context">给定的 <see cref="ITypeDescriptorContext"/>。</param>
        /// <param name="culture">给定的 <see cref="CultureInfo"/>。</param>
        /// <param name="value">给定的对象。</param>
        /// <param name="destinationType">给定的目标类型。</param>
        /// <returns>返回转换的字符串。</returns>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            destinationType.NotNull(nameof(destinationType));

            if (destinationType == ExtensionSettings.StringType)
            {
                if (value.IsNull())
                    return DefaultObject;

                if (culture.IsNotNull() && culture != CultureInfo.CurrentCulture && value is IFormattable formattable)
                    return formattable.ToString(format: null, formatProvider: culture);

                return CustomConvertTo(value);
            }

            throw GetConvertToException(value, destinationType);
        }

        /// <summary>
        /// 自定义转换对象为字符串。
        /// </summary>
        /// <param name="source">给定的来源对象。</param>
        /// <returns>返回转换的字符串。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "source")]
        protected virtual object CustomConvertTo(object source)
            => source.ToString();
    }
}
