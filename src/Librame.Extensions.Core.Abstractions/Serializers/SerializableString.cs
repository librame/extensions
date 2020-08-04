#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;

namespace Librame.Extensions.Core.Serializers
{
    /// <summary>
    /// 可序列化字符串。
    /// </summary>
    /// <typeparam name="TSource">指定的来源类型。</typeparam>
    public class SerializableString<TSource> : IEquatable<SerializableString<TSource>>
    {
        private string _value;


        /// <summary>
        /// 构造一个 <see cref="SerializableString{TSource}"/>。
        /// </summary>
        /// <param name="value">给定的字符串值。</param>
        public SerializableString(string value)
        {
            ChangeValue(value);
        }

        /// <summary>
        /// 构造一个 <see cref="SerializableString{TSource}"/>。
        /// </summary>
        /// <param name="source">给定的来源实例。</param>
        public SerializableString(TSource source)
        {
            ChangeSource(source);
        }


        /// <summary>
        /// 类型转换器。
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual IStringSerializer<TSource> Serializer
            => SerializerManager.GetBySource<TSource>();

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
        /// <returns>返回 <see cref="SerializableString{TSource}"/>。</returns>
        public SerializableString<TSource> ChangeValue(string newValue)
        {
            ExtensionSettings.Preference.RunLocker(() =>
            {
                Source = Serializer.Deserialize(newValue);
                _value = newValue;
            });

            return this;
        }

        /// <summary>
        /// 改变来源实例。
        /// </summary>
        /// <param name="newSource">给定的新 <typeparamref name="TSource"/>。</param>
        /// <returns>返回 <see cref="SerializableString{TSource}"/>。</returns>
        public SerializableString<TSource> ChangeSource(TSource newSource)
        {
            ExtensionSettings.Preference.RunLocker(() =>
            {
                _value = Serializer.Serialize(newSource);
                Source = newSource;
            });
            
            return this;
        }


        /// <summary>
        /// 转换为来源实例。
        /// </summary>
        /// <returns>返回 <typeparamref name="TSource"/>。</returns>
        public virtual TSource ToTSource()
            => Source;


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="other">给定的 <see cref="SerializableString{TSource}"/>。</param>
        /// <returns>返回布尔值。</returns>
        public virtual bool Equals(SerializableString<TSource> other)
            => Value == other?.Value;

        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
            => obj is SerializableString<TSource> other && Equals(other);


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回 32 位整数。</returns>
        public override int GetHashCode()
            => Value.CompatibleGetHashCode();


        /// <summary>
        /// 转换为字符串形式。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
            => Value;


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="left">给定的 <see cref="SerializableString{TSource}"/>。</param>
        /// <param name="right">给定的 <see cref="SerializableString{TSource}"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator ==(SerializableString<TSource> left, SerializableString<TSource> right)
            => (bool)left?.Equals(right);

        /// <summary>
        /// 是否不等。
        /// </summary>
        /// <param name="left">给定的 <see cref="SerializableString{TSource}"/>。</param>
        /// <param name="right">给定的 <see cref="SerializableString{TSource}"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator !=(SerializableString<TSource> left, SerializableString<TSource> right)
            => !(left == right);


        /// <summary>
        /// 隐式转换为来源实例。
        /// </summary>
        /// <param name="serializable">给定的 <see cref="SerializableString{TSource}"/>。</param>
        public static implicit operator TSource(SerializableString<TSource> serializable)
            => serializable.NotNull(nameof(serializable)).ToTSource();

        /// <summary>
        /// 隐式转换为字符串形式。
        /// </summary>
        /// <param name="serializable">给定的 <see cref="SerializableString{TSource}"/>。</param>
        public static implicit operator string(SerializableString<TSource> serializable)
            => serializable?.ToString();
    }
}
