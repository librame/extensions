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

namespace Librame.Extensions.Core.Converters
{
    /// <summary>
    /// 类型字符串转换器。
    /// </summary>
    public class TypeStringConverter : AbstractStringConverter
    {
        /// <summary>
        /// 键名。
        /// </summary>
        public static readonly TypeNamedKey Key
            = new TypeNamedKey<Type>(nameof(TypeStringConverter));
        
        /// <summary>
        /// 默认实例。
        /// </summary>
        public static readonly TypeStringConverter Default
            = new TypeStringConverter();


        /// <summary>
        /// 自定义还原字符串为对象。
        /// </summary>
        /// <param name="destination">给定的目标字符串。</param>
        /// <returns>返回还原后的对象。</returns>
        protected override object CustomConvertFrom(string destination)
            => Type.GetType(destination, true);


        /// <summary>
        /// 自定义转换对象为字符串。
        /// </summary>
        /// <param name="source">给定的来源对象。</param>
        /// <returns>返回转换后的对象。</returns>
        protected override object CustomConvertTo(object source)
        {
            if (source is Type type)
                return type.GetAssemblyQualifiedNameWithoutVersion();

            return base.CustomConvertTo(source);
        }

    }
}
