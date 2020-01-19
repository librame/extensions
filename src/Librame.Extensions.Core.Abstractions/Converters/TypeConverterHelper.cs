#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Core.Converters
{
    /// <summary>
    /// 类型转换器助手。
    /// </summary>
    public static class TypeConverterHelper
    {
        private static readonly ConcurrentDictionary<TypeNamedKey, TypeConverter> _converters
            = new ConcurrentDictionary<TypeNamedKey, TypeConverter>();


        static TypeConverterHelper()
        {
            Register(TypeStringConverter.Key, TypeStringConverter.Default);
            Register(EncodingStringConverter.Key, EncodingStringConverter.Default);
            Register(HexStringConverter.Key, HexStringConverter.Default);
            Register(Base32StringConverter.Key, Base32StringConverter.Default);
            Register(Base64StringConverter.Key, Base64StringConverter.Default);
        }


        /// <summary>
        /// 获取类型转换器（支持框架原生的类型转换器）。
        /// </summary>
        /// <param name="key">给定的 <see cref="TypeNamedKey"/>。</param>
        /// <returns>返回 <see cref="TypeConverter"/>。</returns>
        public static TypeConverter Get(TypeNamedKey key)
            => TryGet(key, out TypeConverter converter) ? converter : null;


        /// <summary>
        /// 尝试获取类型转换器（支持框架原生的类型转换器）。
        /// </summary>
        /// <param name="key">给定的 <see cref="TypeNamedKey"/>。</param>
        /// <param name="converter">输出 <see cref="TypeConverter"/>。</param>
        /// <returns>返回布尔值。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "key")]
        public static bool TryGet(TypeNamedKey key, out TypeConverter converter)
        {
            // 自定义转换器集合优先
            if (_converters.TryGetValue(key, out converter))
                return true;
            
            // 支持框架原生的类型转换器
            if (key.Type.IsNotNull())
            {
                converter = TypeDescriptor.GetConverter(key.Type);
                return converter.IsNotNull();
            }

            return false;
        }


        /// <summary>
        /// 尝试移除已注册的类型转换器。
        /// </summary>
        /// <param name="key">给定的 <see cref="TypeNamedKey"/>。</param>
        /// <param name="converter">输出 <see cref="TypeConverter"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool TryRemove(TypeNamedKey key, out TypeConverter converter)
            => _converters.TryRemove(key, out converter);


        /// <summary>
        /// 注册类型转换器（默认支持框架原生转换器，即不用手动注册诸如 GuidConverter、DateTimeConverter 等框架原生转换器）。
        /// </summary>
        /// <param name="key">给定的类型。</param>
        /// <param name="converter">给定的 <see cref="TypeConverter"/>。</param>
        /// <returns>返回 <see cref="TypeConverter"/>。</returns>
        public static TypeConverter Register(TypeNamedKey key, TypeConverter converter)
            => _converters.AddOrUpdate(key, converter, (key, value) => converter);
    }
}
