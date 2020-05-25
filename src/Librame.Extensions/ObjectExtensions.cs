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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace Librame.Extensions
{
    /// <summary>
    /// 对象静态扩展。
    /// </summary>
    public static class ObjectExtensions
    {
        // 放在 ExtensionContext 中时，在 net48 环境下可能会提示内部解析异常的错误
        internal static ConcurrentDictionary<string, Func<object[], object>> CreateFactories
            = new ConcurrentDictionary<string, Func<object[], object>>();


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
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static object EnsureCreateObject(this Type type, params object[] parameters)
        {
            type.NotNull(nameof(type));

            var paramTypes = Array.Empty<Type>();
            string key = type.FullName;

            if (parameters.IsNotEmpty())
            {
                paramTypes = parameters.Select(p => p.GetType()).ToArray();
                key = string.Concat(key, "_", string.Concat(paramTypes.Select(t => t.Name)));
            }

            var factory = CreateFactories.GetOrAdd(key, @params =>
            {
                var constructorInfo = type.GetConstructor(paramTypes);
                var argsExpression = Expression.Parameter(typeof(object[]), "args");
                var paramExpressions = BuildParameterExpressions(argsExpression);
                var newExpression = Expression.New(constructorInfo, paramExpressions);

                var result = Expression.Lambda<Func<object[], object>>(newExpression, argsExpression);
                return result.Compile();
            });

            return factory.Invoke(parameters);

            // BuildParameterExpressions
            Expression[] BuildParameterExpressions(ParameterExpression argsExpression)
            {
                var list = new List<Expression>();

                for (int i = 0; i < paramTypes.Length; i++)
                {
                    var expression = Expression.ArrayIndex(argsExpression, Expression.Constant(i));
                    var targetExpression = Expression.Convert(expression, paramTypes[i]);
                    list.Add(targetExpression);
                }

                return list.ToArray();
            }
        }

        #endregion


        /// <summary>
        /// 确保克隆。
        /// </summary>
        /// <param name="source">给定要克隆的源对象。</param>
        /// <param name="type">给定要克隆的类型（可选）。</param>
        /// <param name="clonedTypes">给定已克隆的类型字典（可选；外部传入以支持链式克隆）。</param>
        /// <returns>返回克隆的对象。</returns>
        public static object EnsureClone(this object source, Type type,
            ConcurrentDictionary<Type, object> clonedTypes = null)
        {
            if (source.IsNull()) return null;
            if (type.IsNull()) type = source.GetType();

            // 如果是值或字符串类型
            if (type.IsValueType || type.IsStringType())
            {
                // 则直接调用 Object.MemberwiseClone() 方法
                return type.GetMethod("MemberwiseClone")?.Invoke(source, null);
            }

            // 如果支持序列化模式
            if (type.IsSerializable)
            {
                using (var ms = new MemoryStream())
                {
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(ms, source);

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
                    return EnsureClone(field.GetValue(source), _type, clonedTypes);
                });

                field.SetValue(clone, fieldValue);
            }

            foreach (var property in type.GetAllProperties())
            {
                var propertyValue = clonedTypes.GetOrAdd(property.PropertyType, _type =>
                {
                    // 链式克隆
                    return EnsureClone(property.GetValue(source), _type, clonedTypes);
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
                ExtensionSettings.Preference.RunLocker(() =>
                {
                    if (singleton.IsNull())
                        singleton = buildFactory?.Invoke();
                });
            }

            return singleton.NotNull(nameof(singleton));
        }

    }
}
