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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Core.Serializers
{
    ///// <summary>
    ///// 序列化器注册。
    ///// </summary>
    //public static class SerializerRegistration
    //{
    //    static SerializerRegistration()
    //    {
    //        if (_serializers.IsEmpty)
    //        {
    //            AddOrUpdate<ReadOnlyMemoryBase32StringSerializer>();
    //            AddOrUpdate<ReadOnlyMemoryBase64StringSerializer>();
    //            AddOrUpdate<ReadOnlyMemoryHexStringSerializer>();
    //            AddOrUpdate<TypeStringSerializer>();
    //            AddOrUpdate<EncodingStringSerializer>();
    //        }

    //        TypeDescriptor.GetConverter(myGuid).
    //    }


    //    /// <summary>
    //    /// 所有名称集合。
    //    /// </summary>
    //    public static ICollection<string> AllNames
    //        => _serializers.Keys;

    //    /// <summary>
    //    /// 所有序列化器集合。
    //    /// </summary>
    //    public static ICollection<ISerializer> AllSerializers
    //        => _serializers.Values;

    //    /// <summary>
    //    /// 已注册的序列化器数。
    //    /// </summary>
    //    public static int Count
    //        => _serializers.Count;


    //    /// <summary>
    //    /// 获取对象字符串序列化器。
    //    /// </summary>
    //    /// <param name="name">给定的名称。</param>
    //    /// <returns>返回 <see cref="IObjectStringSerializer"/>。</returns>
    //    public static IObjectStringSerializer GetObjectString(string name)
    //    {
    //        var serializer = Get(name);

    //        if (!(serializer is IObjectStringSerializer objectStringSerializer))
    //            throw new InvalidOperationException($"Invalid '{nameof(IObjectStringSerializer)}': The name '{name}' serializer is '{serializer?.GetType()}'.");

    //        return objectStringSerializer;
    //    }


    //    /// <summary>
    //    /// 获取指定类型的序列化器。
    //    /// </summary>
    //    /// <typeparam name="TSerializer">指定实现 <see cref="ISerializer"/> 的序列化器类型。</typeparam>
    //    /// <param name="name">给定的名称。</param>
    //    /// <returns>返回 <typeparamref name="TSerializer"/>。</returns>
    //    public static TSerializer Get<TSerializer>(string name)
    //        where TSerializer : ISerializer
    //        => (TSerializer)Get(name);

    //    /// <summary>
    //    /// 获取指定名称的序列化器。
    //    /// </summary>
    //    /// <param name="name">给定的名称。</param>
    //    /// <returns>返回 <see cref="ISerializer"/>。</returns>
    //    public static ISerializer Get(string name)
    //        => _serializers[name];


    //    /// <summary>
    //    /// 添加或更新序列化器。
    //    /// </summary>
    //    /// <typeparam name="TSerializer">指定实现 <see cref="ISerializer"/> 的序列化器类型。</typeparam>
    //    /// <param name="name">给定的键名。</param>
    //    /// <param name="updateValueFactory">如果此序列化器已存在，给定用于更新的工厂方法（可选；默认强制更新）。</param>
    //    /// <returns>返回 <typeparamref name="TSerializer"/>。</returns>
    //    public static TSerializer AddOrUpdate<TSerializer>(string name,
    //        Func<string, ISerializer, ISerializer> updateValueFactory = null)
    //        where TSerializer : ISerializer
    //    {
    //        var serializerType = typeof(TSerializer);
    //        var serializer = serializerType.EnsureCreate<TSerializer>();

    //        _serializers.AddOrUpdate(name, serializer,
    //            updateValueFactory ?? ((key, value) => serializer));

    //        return serializer;
    //    }

    //    /// <summary>
    //    /// 添加或更新序列化器。
    //    /// </summary>
    //    /// <typeparam name="TSerializer">指定实现 <see cref="ISerializer"/> 的序列化器类型。</typeparam>
    //    /// <param name="nameFactory">给定的键名工厂方法（可选；默认使用序列化器名称）。</param>
    //    /// <param name="updateValueFactory">如果此序列化器已存在，给定用于更新的工厂方法（可选；默认强制更新）。</param>
    //    /// <returns>返回 <typeparamref name="TSerializer"/>。</returns>
    //    public static TSerializer AddOrUpdate<TSerializer>(Func<TSerializer, string> nameFactory = null,
    //        Func<string, ISerializer, ISerializer> updateValueFactory = null)
    //        where TSerializer : ISerializer
    //    {
    //        var serializerType = typeof(TSerializer);
    //        var serializer = serializerType.EnsureCreate<TSerializer>();

    //        if (nameFactory.IsNull())
    //            nameFactory = s => s.Name;

    //        _serializers.AddOrUpdate(nameFactory.Invoke(serializer), serializer,
    //            updateValueFactory ?? ((key, value) => serializer));

    //        return serializer;
    //    }

    //    /// <summary>
    //    /// 添加或更新序列化器。
    //    /// </summary>
    //    /// <param name="serializer">给定的 <see cref="ISerializer"/>。</param>
    //    /// <param name="updateValueFactory">如果此序列化器已存在，则给定用于更新的工厂方法（可选；默认强制更新）。</param>
    //    /// <returns>返回 <see cref="ISerializer"/>。</returns>
    //    [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "serializer")]
    //    public static ISerializer AddOrUpdate(ISerializer serializer,
    //        Func<string, ISerializer, ISerializer> updateValueFactory = null)
    //        => AddOrUpdate(serializer?.Name, serializer, updateValueFactory);

    //    /// <summary>
    //    /// 添加或更新序列化器。
    //    /// </summary>
    //    /// <param name="name">给定的键名。</param>
    //    /// <param name="serializer">给定的 <see cref="ISerializer"/>。</param>
    //    /// <param name="updateValueFactory">如果此序列化器已存在，则给定用于更新的工厂方法（可选；默认强制更新）。</param>
    //    /// <returns>返回 <see cref="ISerializer"/>。</returns>
    //    public static ISerializer AddOrUpdate(string name, ISerializer serializer,
    //        Func<string, ISerializer, ISerializer> updateValueFactory = null)
    //    {
    //        serializer.NotNull(nameof(serializer));

    //        _serializers.AddOrUpdate(name, serializer,
    //            updateValueFactory ?? ((key, value) => serializer));

    //        return serializer;
    //    }


    //    /// <summary>
    //    /// 清空所有序列化器。
    //    /// </summary>
    //    public static void Clear()
    //        => _serializers.Clear();
    //}
}
