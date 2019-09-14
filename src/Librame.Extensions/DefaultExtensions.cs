﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace Librame.Extensions
{
    /// <summary>
    /// 默认静态扩展。
    /// </summary>
    public static class DefaultExtensions
    {
        private static byte[] _locker = new byte[0];


        /// <summary>
        /// 确保单例。
        /// </summary>
        /// <typeparam name="TSingleton">指定的单例类型。</typeparam>
        /// <param name="singleton">给定的当前单例。</param>
        /// <param name="buildFactory">用于构建单例的工厂方法。</param>
        /// <returns>返回单例。</returns>
        public static TSingleton EnsureSingleton<TSingleton>(this TSingleton singleton, Func<TSingleton> buildFactory)
            where TSingleton : class
        {
            if (singleton.IsNull())
            {
                lock (_locker)
                {
                    if (singleton.IsNull())
                        singleton = buildFactory?.Invoke();
                }
            }

            return singleton.NotNull(nameof(singleton));
        }


        #region EnsureCreate

        /// <summary>
        /// 确保创建实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <returns>返回 <typeparamref name="T"/>。</returns>
        public static T EnsureCreate<T>()
        {
            var expression = Expression.New(typeof(T));
            var factory = Expression.Lambda<Func<T>>(expression, null).Compile();
            return factory.Invoke();
        }

        /// <summary>
        /// 确保创建实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回 <typeparamref name="T"/>。</returns>
        public static T EnsureCreate<T>(this Type type)
            => (T)type.EnsureCreateObject();

        /// <summary>
        /// 确保创建对象实例。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回实例对象。</returns>
        public static object EnsureCreateObject(this Type type)
        {
            var expression = Expression.New(type);
            var factory = Expression.Lambda<Func<object>>(expression, null).Compile();
            return factory.Invoke();
        }


        private static ConcurrentDictionary<string, Func<object[], object>> _createFactories
            = new ConcurrentDictionary<string, Func<object[], object>>();

        /// <summary>
        /// 利用当前对象为构造函数的参数来确保构造实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="parameter">给定的参数对象。</param>
        /// <returns>返回 <typeparamref name="T"/>。</returns>
        public static T EnsureConstruct<T>(this object parameter)
            => EnsureCreate<T>(parameter);

        /// <summary>
        /// 利用当前对象数组为构造函数的参数数组来确保构造实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="parameters">给定的参数对象数组。</param>
        /// <returns>返回 <typeparamref name="T"/>。</returns>
        public static T EnsureConstruct<T>(this object[] parameters)
            => EnsureCreate<T>(parameters);


        /// <summary>
        /// 通过指定的对象数组为构造函数的参数数组来确保创建实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="parameters">给定的参数对象数组。</param>
        /// <returns>返回 <typeparamref name="T"/>。</returns>
        public static T EnsureCreate<T>(params object[] parameters)
            => (T)typeof(T).EnsureCreateObject(parameters);

        /// <summary>
        /// 通过指定的对象数组为构造函数的参数数组来确保创建实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="type">给定的类型。</param>
        /// <param name="parameters">给定的参数对象数组。</param>
        /// <returns>返回 <typeparamref name="T"/>。</returns>
        public static T EnsureCreate<T>(this Type type, params object[] parameters)
            => (T)type.EnsureCreateObject(parameters);

        /// <summary>
        /// 通过指定的对象数组为构造函数的参数数组来确保创建对象实例。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <param name="parameters">给定的参数对象数组。</param>
        /// <returns>返回实例对象。</returns>
        public static object EnsureCreateObject(this Type type, params object[] parameters)
        {
            var paramTypes = new Type[0];
            string key = type.FullName;

            if (parameters.IsNotNullOrEmpty())
            {
                paramTypes = parameters.Select(p => p.GetType()).ToArray();
                key = string.Concat(key, "_", string.Concat(paramTypes.Select(t => t.Name)));
            }

            var factory = _createFactories.GetOrAdd(key, k =>
            {
                var constructorInfo = type.GetConstructor(paramTypes);
                var paramsExtension = Expression.Parameter(typeof(object[]), "_parameters");
                var arguments = BuildParameters(paramTypes, paramsExtension);
                var newExpression = Expression.New(constructorInfo, arguments);

                return Expression.Lambda<Func<object[], object>>(newExpression, paramsExtension).Compile();
            });

            return factory.Invoke(parameters);

            // 建立参数表达式数组
            Expression[] BuildParameters(Type[] parameterTypes, ParameterExpression parameterExpression)
            {
                var list = new List<Expression>();

                for (int i = 0; i < parameterTypes.Length; i++)
                {
                    var expression = Expression.ArrayIndex(parameterExpression, Expression.Constant(i));
                    var targetExpression = Expression.Convert(expression, parameterTypes[i]);
                    list.Add(targetExpression);
                }

                return list.ToArray();
            }
        }


        /// <summary>
        /// 确保克隆。
        /// </summary>
        /// <param name="obj">给定要克隆的对象。</param>
        /// <param name="type">给定要克隆的类型（可选）。</param>
        /// <param name="clonedTypes">给定已克隆的类型字典（可选；外部传入以支持链式克隆）。</param>
        /// <returns>返回克隆的对象。</returns>
        public static object EnsureClone(this object obj, Type type,
            ConcurrentDictionary<Type, object> clonedTypes = null)
        {
            if (obj.IsNull()) return null;
            if (type.IsNull()) type = obj.GetType();

            // 如果是值或字符串类型
            if (type.IsValueType || type.IsStringType())
            {
                // 则直接调用 Object.MemberwiseClone() 方法
                return type.GetMethod("MemberwiseClone")?.Invoke(obj, null);
            }

            // 如果支持序列化模式
            if (type.IsSerializable)
            {
                using (var ms = new MemoryStream())
                {
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(ms, obj);

                    ms.Seek(0, SeekOrigin.Begin);
                    return formatter.Deserialize(ms);
                }
            }

            // 初始实例化
            if (clonedTypes.IsNull())
                clonedTypes = new ConcurrentDictionary<Type, object>();

            // 反射模式
            var clone = type.EnsureCreateObject();

            foreach (var field in type.GetAllFields())
            {
                var fieldValue = clonedTypes.GetOrAdd(field.FieldType, _type =>
                {
                    // 链式克隆
                    return EnsureClone(field.GetValue(obj), _type, clonedTypes);
                });

                field.SetValue(clone, fieldValue);
            }

            foreach (var property in type.GetAllProperties())
            {
                var propertyValue = clonedTypes.GetOrAdd(property.PropertyType, _type =>
                {
                    // 链式克隆
                    return EnsureClone(property.GetValue(obj), _type, clonedTypes);
                });

                property.SetValue(clone, propertyValue);
            }

            return clone;
        }


        /// <summary>
        /// 确保从源对象填入目标对象（支持所有字段与属性）。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为空或默认值。
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="target"/> 为空或默认值。
        /// </exception>
        /// <param name="source">给定的来源类型实例。</param>
        /// <param name="target">给定的目标类型实例。</param>
        public static void EnsurePopulate<TSource, TTarget>(this TSource source, TTarget target)
            where TSource : class
            where TTarget : class
        {
            source.NotNull(nameof(source));
            target.NotNull(nameof(target));

            var sourceType = typeof(TSource);
            var targetType = typeof(TTarget);

            var srcFields = new List<FieldInfo>(sourceType.GetAllFields());
            var tarFields = new List<FieldInfo>(targetType.GetAllFields());

            for (var s = 0; s < srcFields.Count; s++)
            {
                for (var t = 0; t < tarFields.Count; t++)
                {
                    var srcField = srcFields[s];
                    var tarField = tarFields[t];

                    if (srcField.Name == tarField.Name)
                    {
                        var value = srcField.GetValue(source);
                        tarField.SetValue(target, value);

                        tarFields.Remove(tarField);
                        srcFields.Remove(srcField);

                        break;
                    }
                }
            }

            var srcProperties = new List<PropertyInfo>(sourceType.GetAllProperties());
            var tarProperties = new List<PropertyInfo>(targetType.GetAllProperties());

            for (var s = 0; s < srcProperties.Count; s++)
            {
                for (var t = 0; t < tarProperties.Count; t++)
                {
                    var srcProperty = srcProperties[s];
                    var tarProperty = tarProperties[t];

                    if (srcProperty.Name == tarProperty.Name)
                    {
                        var value = srcProperty.GetValue(source);
                        tarProperty.SetValue(target, value);

                        tarProperties.Remove(tarProperty);
                        srcProperties.Remove(srcProperty);

                        break;
                    }
                }
            }
        }

        #endregion


        #region OrDefault

        /// <summary>
        /// 得到不为 NULL 或默认值。
        /// </summary>
        /// <typeparam name="TStruct">指定的值类型。</typeparam>
        /// <param name="nullable">给定的当前可空值。</param>
        /// <param name="default">给定的默认值（如果可空值为空，则返回此值）。</param>
        /// <returns>返回存在值或默认值。</returns>
        public static TStruct NotNullOrDefault<TStruct>(this TStruct? nullable, TStruct @default)
            where TStruct : struct
            => nullable.NotNullOrDefault(() => @default);

        /// <summary>
        /// 得到不为 NULL 或默认值。
        /// </summary>
        /// <typeparam name="TStruct">指定的值类型。</typeparam>
        /// <param name="nullable">给定的当前可空值。</param>
        /// <param name="defaultFactory">给定的默认值工厂方法（如果可空值为空，则调用此方法）。</param>
        /// <returns>返回当前或默认值。</returns>
        public static TStruct NotNullOrDefault<TStruct>(this TStruct? nullable, Func<TStruct> defaultFactory)
            where TStruct : struct
            => nullable.HasValue ? nullable.Value : defaultFactory.Invoke();


        /// <summary>
        /// 得到不为 NULL、空格或默认字符串。
        /// </summary>
        /// <param name="str">给定的当前字符串。</param>
        /// <param name="default">在未通过验证或验证异常时，要返回的默认字符串。</param>
        /// <param name="throwIfDefaultInvalid">如果默认字符串不满足必要条件时，是否抛出异常（可选；如果为 TRUE，表示在不满足时抛出异常；反之则不验证也不抛出异常）。</param>
        /// <returns>返回当前或默认字符串。</returns>
        public static string NotWhiteSpaceOrDefault(this string str, string @default, bool throwIfDefaultInvalid = true)
            => str.OrDefault(@default, ValidationExtensions.IsNotNullOrWhiteSpace, throwIfDefaultInvalid);

        /// <summary>
        /// 得到不为 NULL、空格或默认字符串。
        /// </summary>
        /// <param name="str">给定的当前字符串。</param>
        /// <param name="defaultFactory">在未通过验证或验证异常时，要返回的默认结果工厂方法。</param>
        /// <param name="throwIfDefaultInvalid">如果默认字符串不满足必要条件时，是否抛出异常（可选；如果为 TRUE，表示在不满足时抛出异常；反之则不验证也不抛出异常）。</param>
        /// <returns>返回当前或默认字符串。</returns>
        public static string NotWhiteSpaceOrDefault(this string str, Func<string> defaultFactory, bool throwIfDefaultInvalid = true)
            => str.OrDefault(defaultFactory, ValidationExtensions.IsNotNullOrWhiteSpace, throwIfDefaultInvalid);


        /// <summary>
        /// 得到不为 NULL、空或默认字符串。
        /// </summary>
        /// <param name="str">给定的当前字符串。</param>
        /// <param name="default">在未通过验证或验证异常时，要返回的默认字符串。</param>
        /// <param name="throwIfDefaultInvalid">如果默认字符串不满足必要条件时，是否抛出异常（可选；如果为 TRUE，表示在不满足时抛出异常；反之则不验证也不抛出异常）。</param>
        /// <returns>返回当前或默认字符串。</returns>
        public static string NotEmptyOrDefault(this string str, string @default, bool throwIfDefaultInvalid = true)
            => str.OrDefault(@default, ValidationExtensions.IsNotNullOrEmpty, throwIfDefaultInvalid);

        /// <summary>
        /// 得到不为 NULL、空或默认字符串。
        /// </summary>
        /// <param name="str">给定的当前字符串。</param>
        /// <param name="defaultFactory">在未通过验证或验证异常时，要返回的默认字符串工厂方法。</param>
        /// <param name="throwIfDefaultInvalid">如果默认字符串不满足必要条件时，是否抛出异常（可选；如果为 TRUE，表示在不满足时抛出异常；反之则不验证也不抛出异常）。</param>
        /// <returns>返回当前或默认字符串。</returns>
        public static string NotEmptyOrDefault(this string str, Func<string> defaultFactory, bool throwIfDefaultInvalid = true)
            => str.OrDefault(defaultFactory, ValidationExtensions.IsNotNullOrEmpty, throwIfDefaultInvalid);


        /// <summary>
        /// 得到不为 NULL、空或默认集合实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="current">给定的当前集合实例。</param>
        /// <param name="default">在未通过验证或验证异常时，要返回的默认结果。</param>
        /// <param name="throwIfDefaultInvalid">如果默认集合实例不满足必要条件时，是否抛出异常（可选；如果为 TRUE，表示在不满足时抛出异常；反之则不验证也不抛出异常）。</param>
        /// <returns>返回当前或默认集合实例。</returns>
        public static IEnumerable<T> NotEmptyOrDefault<T>(this IEnumerable<T> current, IEnumerable<T> @default, bool throwIfDefaultInvalid = true)
            => current.OrDefault(@default, ValidationExtensions.IsNotNullOrEmpty, throwIfDefaultInvalid);

        /// <summary>
        /// 得到不为 NULL、空或默认集合实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="current">给定的当前集合实例。</param>
        /// <param name="defaultFactory">在未通过验证或验证异常时，要返回的默认结果工厂方法。</param>
        /// <param name="throwIfDefaultInvalid">如果默认集合实例不满足必要条件时，是否抛出异常（可选；如果为 TRUE，表示在不满足时抛出异常；反之则不验证也不抛出异常）。</param>
        /// <returns>返回当前或默认集合实例。</returns>
        public static IEnumerable<T> NotEmptyOrDefault<T>(this IEnumerable<T> current, Func<IEnumerable<T>> defaultFactory, bool throwIfDefaultInvalid = true)
            => current.OrDefault(defaultFactory, ValidationExtensions.IsNotNullOrEmpty, throwIfDefaultInvalid);


        /// <summary>
        /// 得到不为 NULL、空或默认集合实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="current">给定的当前集合实例。</param>
        /// <param name="default">在未通过验证或验证异常时，要返回的默认集合实例。</param>
        /// <param name="throwIfDefaultInvalid">如果默认集合实例不满足必要条件时，是否抛出异常（可选；如果为 TRUE，表示在不满足时抛出异常；反之则不验证也不抛出异常）。</param>
        /// <returns>返回当前或默认集合实例。</returns>
        public static T NotEmptyOrDefault<T>(this T current, T @default, bool throwIfDefaultInvalid = true)
            where T : IEnumerable
            => current.OrDefault(@default, ValidationExtensions.IsNotNullOrEmpty, throwIfDefaultInvalid);

        /// <summary>
        /// 得到不为 NULL、空或默认集合实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="current">给定的当前集合实例。</param>
        /// <param name="defaultFactory">在未通过验证或验证异常时，要返回的默认集合实例工厂方法。</param>
        /// <param name="throwIfDefaultInvalid">如果默认集合实例不满足必要条件时，是否抛出异常（可选；如果为 TRUE，表示在不满足时抛出异常；反之则不验证也不抛出异常）。</param>
        /// <returns>返回当前或默认集合实例。</returns>
        public static T NotEmptyOrDefault<T>(this T current, Func<T> defaultFactory, bool throwIfDefaultInvalid = true)
            where T : IEnumerable
            => current.OrDefault(defaultFactory, ValidationExtensions.IsNotNullOrEmpty, throwIfDefaultInvalid);


        /// <summary>
        /// 得到不为 NULL 或默认实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="current">给定的当前实例。</param>
        /// <param name="default">在未通过验证或验证异常时，要返回的默认实例。</param>
        /// <param name="throwIfDefaultInvalid">如果默认实例不满足必要条件时，是否抛出异常（可选；如果为 TRUE，表示在不满足时抛出异常；反之则不验证也不抛出异常）。</param>
        /// <returns>返回当前或默认实例。</returns>
        public static T NotNullOrDefault<T>(this T current, T @default, bool throwIfDefaultInvalid = true)
            => current.OrDefault(@default, ValidationExtensions.IsNotNull, throwIfDefaultInvalid);

        /// <summary>
        /// 得到不为 NULL 或默认实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="current">给定的当前实例。</param>
        /// <param name="defaultFactory">在未通过验证或验证异常时，要返回的默认实例工厂方法。</param>
        /// <param name="throwIfDefaultInvalid">如果默认实例不满足验证条件时，是否抛出异常（可选；如果为 TRUE，表示在不满足时抛出异常；反之则不验证也不抛出异常）。</param>
        /// <returns>返回当前或默认实例。</returns>
        public static T NotNullOrDefault<T>(this T current, Func<T> defaultFactory, bool throwIfDefaultInvalid = true)
            => current.OrDefault(defaultFactory, ValidationExtensions.IsNotNull, throwIfDefaultInvalid);


        /// <summary>
        /// 得到不大于的当前或默认实例（不验证比较实例也不抛出异常）。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="current">给定的当前实例。</param>
        /// <param name="compare">给定的比较实例。</param>
        /// <param name="equals">是否比较等于（可选；默认不比较）。</param>
        /// <returns>返回当前或默认实例。</returns>
        public static T NotGreaterOrDefault<T>(this T current, T compare, bool equals = false)
            where T : IComparable<T>
            => current.OrDefault(compare, _current => _current.IsLesser(compare, equals), throwIfDefaultInvalid: false);

        /// <summary>
        /// 得到不大于的当前或默认实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="current">给定的当前结果。</param>
        /// <param name="compare">给定的比较结果。</param>
        /// <param name="default">在未通过验证或验证异常时，要返回的默认结果。</param>
        /// <param name="equals">是否比较等于（可选；默认不比较）。</param>
        /// <param name="throwIfDefaultInvalid">如果默认实例不满足验证条件时，是否抛出异常（可选；如果为 TRUE，表示在不满足时抛出异常；反之则不验证也不抛出异常）。</param>
        /// <returns>返回当前或默认实例。</returns>
        public static T NotGreaterOrDefault<T>(this T current, T compare, T @default, bool equals = false, bool throwIfDefaultInvalid = true)
            where T : IComparable<T>
            => current.OrDefault(@default, _current => _current.IsLesser(compare, equals), throwIfDefaultInvalid);

        /// <summary>
        /// 得到不大于的当前或默认实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="current">给定的当前实例。</param>
        /// <param name="compare">给定的比较实例。</param>
        /// <param name="defaultFactory">在未通过验证或验证异常时，要返回的默认实例工厂方法。</param>
        /// <param name="equals">是否比较等于（可选；默认不比较）。</param>
        /// <param name="throwIfDefaultInvalid">如果默认实例不满足验证条件时，是否抛出异常（可选；如果为 TRUE，表示在不满足时抛出异常；反之则不验证也不抛出异常）。</param>
        /// <returns>返回当前或默认实例。</returns>
        public static T NotGreaterOrDefault<T>(this T current, T compare, Func<T> defaultFactory, bool equals = false, bool throwIfDefaultInvalid = true)
            where T : IComparable<T>
            => current.OrDefault(defaultFactory, _current => _current.IsLesser(compare, equals), throwIfDefaultInvalid);


        /// <summary>
        /// 得到不小于的当前或默认实例（不验证比较实例也不抛出异常）。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="current">给定的当前实例。</param>
        /// <param name="compare">给定的比较实例。</param>
        /// <param name="equals">是否比较等于（可选；默认不比较）。</param>
        /// <returns>返回当前或默认实例。</returns>
        public static T NotLesserOrDefault<T>(this T current, T compare, bool equals = false)
            where T : IComparable<T>
            => current.OrDefault(compare, _current => _current.IsGreater(compare, equals), throwIfDefaultInvalid: false);

        /// <summary>
        /// 得到不小于的当前或默认实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="current">给定的当前实例。</param>
        /// <param name="compare">给定的比较实例。</param>
        /// <param name="default">在未通过验证或验证异常时，要返回的默认实例。</param>
        /// <param name="equals">是否比较等于（可选；默认不比较）。</param>
        /// <param name="throwIfDefaultInvalid">如果默认实例不满足验证条件时，是否抛出异常（可选；如果为 TRUE，表示在不满足时抛出异常；反之则不验证也不抛出异常）。</param>
        /// <returns>返回当前或默认实例。</returns>
        public static T NotLesserOrDefault<T>(this T current, T compare, T @default, bool equals = false, bool throwIfDefaultInvalid = true)
            where T : IComparable<T>
            => current.OrDefault(@default, _current => _current.IsGreater(compare, equals), throwIfDefaultInvalid);

        /// <summary>
        /// 得到不小于的当前或默认实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="current">给定的当前实例。</param>
        /// <param name="compare">给定的比较实例。</param>
        /// <param name="defaultFactory">在未通过验证或验证异常时，要返回的默认实例工厂方法。</param>
        /// <param name="equals">是否比较等于（可选；默认不比较）。</param>
        /// <param name="throwIfDefaultInvalid">如果默认实例不满足验证条件时，是否抛出异常（可选；如果为 TRUE，表示在不满足时抛出异常；反之则不验证也不抛出异常）。</param>
        /// <returns>返回当前或默认实例。</returns>
        public static T NotLesserOrDefault<T>(this T current, T compare, Func<T> defaultFactory, bool equals = false, bool throwIfDefaultInvalid = true)
            where T : IComparable<T>
            => current.OrDefault(defaultFactory, _current => _current.IsGreater(compare, equals), throwIfDefaultInvalid);


        /// <summary>
        /// 得到不超出范围的当前或默认实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="current">给定的当前实例。</param>
        /// <param name="compareMinimum">给定的最小比较值。</param>
        /// <param name="compareMaximum">给定的最大比较值。</param>
        /// <param name="default">在未通过验证或验证异常时，要返回的默认实例。</param>
        /// <param name="equalMinimum">是否比较等于最小值（可选；默认不比较）。</param>
        /// <param name="equalMaximum">是否比较等于最大值（可选；默认不比较）。</param>
        /// <param name="throwIfDefaultInvalid">如果默认实例不满足验证条件时，是否抛出异常（可选；如果为 TRUE，表示在不满足时抛出异常；反之则不验证也不抛出异常）。</param>
        /// <returns>返回当前或默认实例。</returns>
        public static T NotOutOfRangeOrDefault<T>(this T current, T compareMinimum, T compareMaximum,
            T @default, bool equalMinimum = false, bool equalMaximum = false, bool throwIfDefaultInvalid = true)
            where T : IComparable<T>
            => current.OrDefault(@default, _current => _current.IsNotOutOfRange(compareMinimum, compareMaximum, equalMinimum, equalMaximum), throwIfDefaultInvalid);

        /// <summary>
        /// 得到不超出范围的当前或默认实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="current">给定的当前实例。</param>
        /// <param name="compareMinimum">给定的最小比较值。</param>
        /// <param name="compareMaximum">给定的最大比较值。</param>
        /// <param name="defaultFactory">在未通过验证或验证异常时，要返回的默认实例工厂方法。</param>
        /// <param name="equalMinimum">是否比较等于最小值（可选；默认不比较）。</param>
        /// <param name="equalMaximum">是否比较等于最大值（可选；默认不比较）。</param>
        /// <param name="throwIfDefaultInvalid">如果默认实例不满足验证条件时，是否抛出异常（可选；如果为 TRUE，表示在不满足时抛出异常；反之则不验证也不抛出异常）。</param>
        /// <returns>返回当前或默认实例。</returns>
        public static T NotOutOfRangeOrDefault<T>(this T current, T compareMinimum, T compareMaximum,
            Func<T> defaultFactory, bool equalMinimum = false, bool equalMaximum = false, bool throwIfDefaultInvalid = true)
            where T : IComparable<T>
            => current.OrDefault(defaultFactory, _current => _current.IsNotOutOfRange(compareMinimum, compareMaximum, equalMinimum, equalMaximum), throwIfDefaultInvalid);


        /// <summary>
        /// 得到当前或默认实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="current">给定的当前实例。</param>
        /// <param name="default">在未通过验证或验证异常时，要返回的默认实例。</param>
        /// <param name="validFactory">验证当前（或默认）实例是否有效的工厂方法。</param>
        /// <param name="throwIfDefaultInvalid">如果默认实例不满足验证条件时，是否抛出异常（可选；如果为 TRUE，表示在不满足时抛出异常；反之则不验证也不抛出异常）。</param>
        /// <returns>返回当前或默认实例。</returns>
        public static T OrDefault<T>(this T current, T @default,
            Func<T, bool> validFactory, bool throwIfDefaultInvalid = true)
            => current.OrDefault(() => @default, validFactory, throwIfDefaultInvalid);

        /// <summary>
        /// 得到当前或默认实例。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="current">给定的当前实例。</param>
        /// <param name="defaultFactory">在未通过验证或验证异常时，要返回的默认实例工厂方法。</param>
        /// <param name="validFactory">验证当前（或默认）实例是否有效的工厂方法。</param>
        /// <param name="throwIfDefaultInvalid">如果默认实例不满足验证条件时，是否抛出异常（可选；如果为 TRUE，表示在不满足时抛出异常；反之则不验证也不抛出异常）。</param>
        /// <returns>返回当前或默认实例。</returns>
        public static T OrDefault<T>(this T current, Func<T> defaultFactory,
            Func<T, bool> validFactory, bool throwIfDefaultInvalid = true)
        {
            defaultFactory.NotNull(nameof(defaultFactory));
            validFactory.NotNull(nameof(validFactory));

            T @default;

            try
            {
                if (validFactory.Invoke(current))
                    return current;

                @default = defaultFactory.Invoke();
            }
            catch
            {
                @default = defaultFactory.Invoke();
            }

            if (throwIfDefaultInvalid && !validFactory.Invoke(@default))
                throw new ArgumentException("The default instance don't satisfy the necessary conditions.", nameof(defaultFactory));

            return @default;
        }

        #endregion

    }
}
