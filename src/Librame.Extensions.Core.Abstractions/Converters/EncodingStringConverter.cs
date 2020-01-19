#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Text;

namespace Librame.Extensions.Core.Converters
{
    /// <summary>
    /// Encoding 字符串转换器。
    /// </summary>
    public class EncodingStringConverter : AbstractStringConverter
    {
        /// <summary>
        /// 键名。
        /// </summary>
        public static readonly TypeNamedKey Key
            = new TypeNamedKey<Encoding>(nameof(EncodingStringConverter));

        /// <summary>
        /// 默认实例。
        /// </summary>
        public static readonly EncodingStringConverter Default
            = new EncodingStringConverter();


        /// <summary>
        /// 默认对象。
        /// </summary>
        protected override object DefaultObject
            => Encoding.UTF8;


        /// <summary>
        /// 自定义还原字符串为对象。
        /// </summary>
        /// <param name="destination">给定的目标字符串。</param>
        /// <returns>返回还原后的对象。</returns>
        protected override object CustomConvertFrom(string destination)
            => destination.FromEncodingName();


        /// <summary>
        /// 自定义转换对象为字符串。
        /// </summary>
        /// <param name="source">给定的来源对象。</param>
        /// <returns>返回转换后的对象。</returns>
        protected override object CustomConvertTo(object source)
        {
            if (source is Encoding encoding)
                return encoding.AsName();

            return base.CustomConvertTo(source);
        }

    }
}
