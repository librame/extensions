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

namespace Librame.Extensions.Core.Serializers
{
    using Converters;

    /// <summary>
    /// 可序列化对象。
    /// </summary>
    /// <typeparam name="TSource">指定的来源类型。</typeparam>
    public class SerializableObject<TSource> : IEquatable<SerializableObject<TSource>>
    {
        private string _value;


        /// <summary>
        /// 构造一个 <see cref="SerializableObject{TSource}"/>。
        /// </summary>
        /// <param name="value">给定的字符串值。</param>
        /// <param name="name">给定已注册的转换器名称（默认支持框架原生的类型转换器。详情参考：<see cref="TypeConverterHelper.Register(TypeNamedKey, TypeConverter)"/>）。</param>
        public SerializableObject(string value, string name)
            : this(new TypeNamedKey<TSource>(name))
        {
            ChangeValue(value);
        }
        /// <summary>
        /// 构造一个 <see cref="SerializableObject{TSource}"/>。
        /// </summary>
        /// <param name="value">给定的字符串值。</param>
        /// <param name="key">给定的 <see cref="TypeNamedKey"/>（默认支持框架原生的类型转换器。详情参考：<see cref="TypeConverterHelper.Register(TypeNamedKey, TypeConverter)"/>）。</param>
        public SerializableObject(string value, TypeNamedKey key)
            : this(key)
        {
            ChangeValue(value);
        }

        /// <summary>
        /// 构造一个 <see cref="SerializableObject{TSource}"/>。
        /// </summary>
        /// <param name="source">给定的来源实例。</param>
        /// <param name="name">给定已注册的转换器名称（默认支持框架原生的类型转换器。详情参考：<see cref="TypeConverterHelper.Register(TypeNamedKey, TypeConverter)"/>）。</param>
        public SerializableObject(TSource source, string name)
            : this(source, new TypeNamedKey<TSource>(name))
        {
        }
        /// <summary>
        /// 构造一个 <see cref="SerializableObject{TSource}"/>。
        /// </summary>
        /// <param name="source">给定的来源实例。</param>
        /// <param name="key">给定的 <see cref="TypeNamedKey"/>（默认支持框架原生的类型转换器。详情参考：<see cref="TypeConverterHelper.Register(TypeNamedKey, TypeConverter)"/>）。</param>
        public SerializableObject(TSource source, TypeNamedKey key)
            : this(key)
        {
            ChangeSource(source);
        }

        private SerializableObject(TypeNamedKey key)
        {
            Key = key.NotNull(nameof(key));

            if (!TypeConverterHelper.TryGet(key, out TypeConverter converter))
                throw new ArgumentException($"The key '{key}' converter is not found.");

            Converter = converter;
        }


        /// <summary>
        /// 类型命名键。
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public TypeNamedKey Key { get; }

        /// <summary>
        /// 类型转换器。
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public TypeConverter Converter { get; }

        /// <summary>
        /// 来源实例。
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public TSource Source { get; private set; }


        /// <summary>
        /// 字符串值。
        /// </summary>
        public string Value
        {
            get => _value;
            set => ChangeValue(value);
        }


        /// <summary>
        /// 改变字符串值。
        /// </summary>
        /// <param name="newValue">给定的新字符串值。</param>
        /// <returns>返回 <see cref="SerializableObject{TSource}"/>。</returns>
        public SerializableObject<TSource> ChangeValue(string newValue)
        {
            Source = (TSource)Converter.ConvertFrom(newValue);
            _value = newValue;

            return this;
        }

        /// <summary>
        /// 改变来源实例。
        /// </summary>
        /// <param name="newSource">给定的新 <typeparamref name="TSource"/>。</param>
        /// <returns>返回 <see cref="SerializableObject{TSource}"/>。</returns>
        public SerializableObject<TSource> ChangeSource(TSource newSource)
        {
            _value = (string)Converter.ConvertTo(newSource, typeof(string));
            Source = newSource;

            return this;
        }


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
            => obj is SerializableObject<TSource> other ? Equals(other) : false;

        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="other">给定的 <see cref="SerializableObject{TSource}"/>。</param>
        /// <returns>返回布尔值。</returns>
        public bool Equals(SerializableObject<TSource> other)
            => other?.Value == Value;


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回 32 位整数。</returns>
        public override int GetHashCode()
            => ToString().CompatibleGetHashCode();


        /// <summary>
        /// 转换为字符串形式。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
            => Value;


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="left">给定的 <see cref="SerializableObject{TSource}"/>。</param>
        /// <param name="right">给定的 <see cref="SerializableObject{TSource}"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator ==(SerializableObject<TSource> left, SerializableObject<TSource> right)
            => (bool)left?.Equals(right);

        /// <summary>
        /// 是否不等。
        /// </summary>
        /// <param name="left">给定的 <see cref="SerializableObject{TSource}"/>。</param>
        /// <param name="right">给定的 <see cref="SerializableObject{TSource}"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator !=(SerializableObject<TSource> left, SerializableObject<TSource> right)
            => !(left == right);


        /// <summary>
        /// 隐式转换为字符串形式。
        /// </summary>
        /// <param name="value">给定的 <see cref="SerializableObject{TSource}"/>。</param>
        public static implicit operator string(SerializableObject<TSource> value)
            => value?.ToString();
    }
}
