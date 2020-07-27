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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Librame.Extensions
{
    /// <summary>
    /// 对象静态扩展。
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// 是否为具实类型（非接口与抽象类型，即可实例化类型）。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回布尔值。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static bool IsConcreteType(this Type type)
            => type.IsNotNull() && !type.IsAbstract && !type.IsInterface;

        /// <summary>
        /// 是否为开放式泛型（泛类型定义或包含泛型参数集合）。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回布尔值。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static bool IsOpenGenericType(this Type type)
        {
            // 如泛型 List<string>，则 GenericTypeDefinition 为 List<T>，GenericParameters 为 string
            return type.IsNotNull()
                && (type.IsGenericTypeDefinition || type.ContainsGenericParameters);
        }

        /// <summary>
        /// 是否为可空类型。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回布尔值。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static bool IsNullableType(this Type type)
        {
            // 如可空泛型 int?，则 GenericTypeDefinition() 为 Nullable<T>
            return type.IsNotNull()
                && type.IsGenericType
                && type.GetGenericTypeDefinition() == ExtensionSettings.NullableTypeDefinition;
        }

        /// <summary>
        /// 是整数类型（整数类型配置参考 <see cref="IExtensionPreferenceSetting.IntegerTypes"/>）。
        /// </summary>
        /// <param name="type">给定的 <see cref="Type"/>。</param>
        /// <returns>返回布尔值。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static bool IsIntegerType(this Type type)
        {
            type.NotNull(nameof(type));

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                type = type.UnwrapNullableType();

            return ExtensionSettings.Preference.IntegerTypes.Contains(type);
        }

        /// <summary>
        /// 是否为字符串类型。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsStringType(this Type type)
            => type.IsNotNull() && type == ExtensionSettings.StringType;

        /// <summary>
        /// 是否可以从目标类型分配。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="baseType"/> 或 <paramref name="targetType"/> 为空。
        /// </exception>
        /// <remarks>
        /// 详情参考 <see cref="Type.IsAssignableFrom(Type)"/>。
        /// </remarks>
        /// <param name="baseType">给定的基础类型。</param>
        /// <param name="targetType">给定的目标类型。</param>
        /// <returns>返回布尔值。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static bool IsAssignableFromTargetType(this Type baseType, Type targetType)
        {
            baseType.NotNull(nameof(baseType));
            targetType.NotNull(nameof(targetType));

            // 对泛型类型定义提供支持
            if (baseType.IsGenericTypeDefinition)
            {
                if (baseType.IsInterface)
                    return targetType.IsImplementedInterfaceType(baseType);

                return targetType.IsImplementedBaseType(baseType);
            }

            return baseType.IsAssignableFrom(targetType);
        }

        /// <summary>
        /// 是否可以分配给基础类型。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="baseType"/> 或 <paramref name="targetType"/> 为空。
        /// </exception>
        /// <remarks>
        /// 与 <see cref="IsAssignableFromTargetType(Type, Type)"/> 参数相反。
        /// </remarks>
        /// <param name="targetType">给定的目标类型。</param>
        /// <param name="baseType">给定的基础类型。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsAssignableToBaseType(this Type targetType, Type baseType)
            => baseType.IsAssignableFromTargetType(targetType);

        /// <summary>
        /// 获取基础类型集合。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回 <see cref="IEnumerable{Type}"/>。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static IEnumerable<Type> GetBaseTypes(this Type type)
        {
            type.NotNull(nameof(type));

            // 当前基类（Object 为最顶层基类，接口会直接返回 NULL）
            type = type.BaseType;

            while (type != null)
            {
                yield return type;

                type = type.BaseType;
            }
        }

        /// <summary>
        /// 解开可空类型。
        /// </summary>
        /// <param name="nullableType">给定的可空类型。</param>
        /// <returns>返回基础类型或可空类型本身。</returns>
        public static Type UnwrapNullableType(this Type nullableType)
            => Nullable.GetUnderlyingType(nullableType) ?? nullableType;


        #region GetDisplayName

        /// <summary>
        /// 获取泛型主体名称，普通类型直接返回类型名称（如：泛类型 IDictionary{string, IList{string}} 的主体名称为 IDictionary）。
        /// </summary>
        /// <param name="type">给定的名称。</param>
        /// <returns>返回字符串。</returns>
        public static string GetGenericBodyName(this Type type)
        {
            return GetOnlyName(type?.Name);

            string GetOnlyName(string typeName)
            {
                if (typeName.CompatibleContains('`'))
                    return typeName.SplitPair('`').Key;

                return typeName;
            }
        }


        /// <summary>
        /// 获取显示名称（参考 <see cref="AssemblyName.Name"/>）。
        /// </summary>
        /// <param name="assembly">给定的 <see cref="Assembly"/>。</param>
        /// <returns>返回字符串。</returns>
        public static string GetDisplayName(this Assembly assembly)
            => assembly?.GetName()?.Name;

        /// <summary>
        /// 获取程序集显示名称（参考 <see cref="AssemblyName.Name"/>）。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回字符串。</returns>
        public static string GetAssemblyDisplayName(this Type type)
            => type?.Assembly.GetDisplayName();


        /// <summary>
        /// 获取带命名空间的程序集限定名，不包含版本及版本号（格式：System.Collections.Generic.IList`1[System.String], System.Private.CoreLib）。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回字符串。</returns>
        public static string GetAssemblyQualifiedNameWithoutVersion(this Type type)
        {
            type.NotNull(nameof(type));
            return Assembly.CreateQualifiedName(type.GetAssemblyDisplayName(), type.GetDisplayNameWithNamespace());
        }


        /// <summary>
        /// 获取显示名称（即 Type.Name。如：泛类型 IDictionary{string, IList{string}} 的名称为 IDictionary`2[String, IList`1[String]]）。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <param name="onlyGenericBodyName">仅有泛型主体名称，如果指定类型为泛型时可用（可选；默认 False。如 IDictionary`2[String, IList`1[String]] 的泛型主体名称为 IDictionary）。</param>
        /// <returns>返回字符串。</returns>
        public static string GetDisplayName(this Type type, bool onlyGenericBodyName = false)
            => type.GetDisplayString(t => onlyGenericBodyName ? t.GetGenericBodyName() : t.Name);

        /// <summary>
        /// 获取带命名空间的显示名称（如：泛类型 IDictionary{string, IList{string}} 的名称为 System.Collections.Generic.IDictionary`2[System.String, System.Collections.Generic.IList`1[System.String]]）。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <param name="onlyGenericBodyName">仅有泛型主体名称，如果指定类型为泛型时可用（可选；默认 False。如 IDictionary`2[String, IList`1[String]] 的泛型主体名称为 IDictionary）。</param>
        /// <returns>返回字符串。</returns>
        public static string GetDisplayNameWithNamespace(this Type type, bool onlyGenericBodyName = false)
        {
            return type.GetDisplayString(t =>
            {
                var name = onlyGenericBodyName ? t.GetGenericBodyName() : t.Name;

                if (!t.IsNested)
                    return $"{t.Namespace}.{name}";

                // 支持嵌套类型：t.ReflectedType == t.DeclaringType
                return $"{t.Namespace}.{t.ReflectedType.GetDisplayName()}+{name}";
            });
        }

        /// <summary>
        /// 获取自定义字符串。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <param name="factory">给定的类型字符串工厂方法。</param>
        /// <returns>返回字符串。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static string GetDisplayString(this Type type, Func<Type, string> factory)
        {
            type.NotNull(nameof(type));

            var sb = new StringBuilder();
            sb.Append(factory.Invoke(type));

            if (type.IsGenericType)
            {
                var argumentTypes = type.GetGenericArguments();
                sb.Append("[");

                argumentTypes.ForEach((argType, i) =>
                {
                    sb.Append(argType.GetDisplayString(factory));

                    if (i < argumentTypes.Length - 1)
                        sb.Append(", ");
                });

                sb.Append("]");
            }

            return sb.ToString();
        }

        #endregion


        #region GetMemberInfos

        /// <summary>
        /// 获取所有字段集合（包括公开、非公开、实例、静态等）。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回字段信息数组。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static FieldInfo[] GetAllFields(this Type type)
            => type.NotNull(nameof(type)).GetFields(ExtensionSettings.AllFlags);

        /// <summary>
        /// 获取所有非静态字段集合（包括公开、非公开、实例等）。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回字段信息数组。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static FieldInfo[] GetAllFieldsWithoutStatic(this Type type)
            => type.NotNull(nameof(type)).GetFields(ExtensionSettings.AllFlagsWithoutStatic);


        /// <summary>
        /// 获取所有属性集合（包括公开、非公开、实例、静态等）。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回字段信息数组。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static PropertyInfo[] GetAllProperties(this Type type)
            => type.NotNull(nameof(type)).GetProperties(ExtensionSettings.AllFlags);

        /// <summary>
        /// 获取所有非静态属性集合（包括公开、非公开、实例等）。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回字段信息数组。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static PropertyInfo[] GetAllPropertiesWithoutStatic(this Type type)
            => type.NotNull(nameof(type)).GetProperties(ExtensionSettings.AllFlagsWithoutStatic);

        #endregion


        #region InvokeTypes and ExportedTypes

        /// <summary>
        /// 调用类型集合。
        /// </summary>
        /// <param name="assembly">给定的 <see cref="Assembly"/>。</param>
        /// <param name="action">给定的注册动作。</param>
        /// <param name="filterTypes">给定的类型过滤工厂方法（可选）。</param>
        /// <returns>返回已调用的类型集合数。</returns>
        /// <returns>返回已调用的类型集合数。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static int InvokeTypes(this Assembly assembly,
            Action<Type> action, Func<IEnumerable<Type>, IEnumerable<Type>> filterTypes = null)
        {
            assembly.NotNull(nameof(assembly));

            var allTypes = assembly.GetExportedTypes();

            if (filterTypes.IsNotNull())
                allTypes = filterTypes.Invoke(allTypes).ToArray();

            foreach (var type in allTypes)
                action.Invoke(type);

            return allTypes.Length;
        }

        /// <summary>
        /// 调用类型集合。
        /// </summary>
        /// <param name="assemblies">给定的 <see cref="IEnumerable{Assembly}"/>。</param>
        /// <param name="action">给定的注册动作。</param>
        /// <param name="filterTypes">给定的类型过滤工厂方法（可选）。</param>
        /// <returns>返回已调用的类型集合数。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static int InvokeTypes(this IEnumerable<Assembly> assemblies,
            Action<Type> action, Func<IEnumerable<Type>, IEnumerable<Type>> filterTypes = null)
        {
            action.NotNull(nameof(action));

            var allTypes = assemblies.ExportedTypes(filterTypes);

            foreach (var type in allTypes)
                action.Invoke(type);

            return allTypes.Count;
        }


        /// <summary>
        /// 导出类型列表。
        /// </summary>
        /// <param name="assemblies">给定的 <see cref="IEnumerable{Assembly}"/>。</param>
        /// <param name="filterTypes">给定的类型过滤工厂方法（可选）。</param>
        /// <returns>返回 <see cref="IReadOnlyList{Type}"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static List<Type> ExportedTypes(this IEnumerable<Assembly> assemblies,
            Func<IEnumerable<Type>, IEnumerable<Type>> filterTypes = null)
        {
            assemblies.NotEmpty(nameof(assemblies));

            var allTypes = new List<Type>();

            foreach (var assembly in assemblies)
            {
                try
                {
                    // 解决不支持的程序集抛出“System.NotSupportedException: 动态程序集中不支持已调用的成员。”的异常
                    allTypes.AddRange(assembly.GetExportedTypes());
                }
                catch (NotSupportedException)
                {
                    continue;
                }
            }

            if (filterTypes.IsNotNull())
                allTypes = filterTypes.Invoke(allTypes).ToList();

            return allTypes;
        }

        #endregion


        #region IsImplemented

        /// <summary>
        /// 是否已实现某个接口类型。
        /// </summary>
        /// <typeparam name="TInterface">指定的接口类型（支持泛型类型定义）。</typeparam>
        /// <param name="type">给定的当前类型。</param>
        /// <returns>返回布尔值。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static bool IsImplementedInterfaceType<TInterface>(this Type type)
            => type.IsImplementedInterfaceType(typeof(TInterface), out _);

        /// <summary>
        /// 是否已实现某个接口类型。
        /// </summary>
        /// <typeparam name="TInterface">指定的接口类型（支持泛型类型定义）。</typeparam>
        /// <param name="type">给定的当前类型。</param>
        /// <param name="resultType">输出此结果类型（当接口类型为泛型定义时，可用于得到泛型参数等操作）。</param>
        /// <returns>返回布尔值。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static bool IsImplementedInterfaceType<TInterface>(this Type type, out Type resultType)
            => type.IsImplementedInterfaceType(typeof(TInterface), out resultType);

        /// <summary>
        /// 是否已实现某个接口类型。
        /// </summary>
        /// <param name="type">给定的当前类型。</param>
        /// <param name="interfaceType">给定的接口类型（支持泛型类型定义）。</param>
        /// <returns>返回布尔值。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static bool IsImplementedInterfaceType(this Type type, Type interfaceType)
            => type.IsImplementedInterfaceType(interfaceType, out _);

        /// <summary>
        /// 是否已实现某个接口类型。
        /// </summary>
        /// <param name="type">给定的当前类型。</param>
        /// <param name="interfaceType">给定的接口类型（支持泛型类型定义）。</param>
        /// <param name="resultType">输出此结果类型（当接口类型为泛型定义时，可用于得到泛型参数等操作）。</param>
        /// <returns>返回布尔值。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static bool IsImplementedInterfaceType(this Type type, Type interfaceType, out Type resultType)
        {
            type.NotNull(nameof(type));
            interfaceType.NotNull(nameof(interfaceType));

            var allInterfaceTypes = type.GetInterfaces();

            // 如果判定的接口类型是泛型定义
            if (interfaceType.IsGenericTypeDefinition)
            {
                resultType = allInterfaceTypes
                    .Where(type => type.IsGenericType)
                    .FirstOrDefault(type => type.GetGenericTypeDefinition() == interfaceType);

                return resultType.IsNotNull();
            }

            resultType = allInterfaceTypes.FirstOrDefault(type => type == interfaceType);
            return resultType.IsNotNull();
        }


        /// <summary>
        /// 是否已实现某个基础（非接口）类型。
        /// </summary>
        /// <typeparam name="TBase">指定的基础类型（支持泛型类型定义）。</typeparam>
        /// <param name="type">给定的当前类型。</param>
        /// <returns>返回布尔值。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static bool IsImplementedBaseType<TBase>(this Type type)
            => type.IsImplementedBaseType(typeof(TBase), out _);

        /// <summary>
        /// 是否已实现某个基础（非接口）类型。
        /// </summary>
        /// <typeparam name="TBase">指定的基础类型（支持泛型类型定义）。</typeparam>
        /// <param name="type">给定的当前类型。</param>
        /// <param name="resultType">输出此结果类型（当基础类型为泛型定义时，可用于得到泛型参数等操作）。</param>
        /// <returns>返回布尔值。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static bool IsImplementedBaseType<TBase>(this Type type, out Type resultType)
            => type.IsImplementedBaseType(typeof(TBase), out resultType);

        /// <summary>
        /// 是否已实现某个基础（非接口）类型。
        /// </summary>
        /// <param name="type">给定的当前类型。</param>
        /// <param name="baseType">给定的基础类型（支持泛型类型定义）。</param>
        /// <returns>返回布尔值。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static bool IsImplementedBaseType(this Type type, Type baseType)
            => type.IsImplementedBaseType(baseType, out _);

        /// <summary>
        /// 是否已实现某个基础（非接口）类型。
        /// </summary>
        /// <param name="type">给定的当前类型。</param>
        /// <param name="baseType">给定的基础类型（支持泛型类型定义）。</param>
        /// <param name="resultType">输出此结果类型（当基础类型为泛型定义时，可用于得到泛型参数等操作）。</param>
        /// <returns>返回布尔值。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static bool IsImplementedBaseType(this Type type, Type baseType, out Type resultType)
        {
            baseType.NotNull(nameof(baseType));

            if (baseType.IsInterface)
                throw new NotSupportedException($"The base type '{baseType}' does not support interface.");

            var allBaseTypes = type.GetBaseTypes();

            // 如果判定的基础类型是泛型定义
            if (baseType.IsGenericTypeDefinition)
            {
                resultType = allBaseTypes
                    .Where(type => type.IsGenericType)
                    .FirstOrDefault(type => type.GetGenericTypeDefinition() == baseType);

                return resultType.IsNotNull();
            }

            resultType = allBaseTypes.FirstOrDefault(type => type == baseType);
            return resultType.IsNotNull();
        }

        #endregion


        #region PropertyValuesEquals

        /// <summary>
        /// 序列元素属性值集合相等比较。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="sources">给定的源类型实例集合。</param>
        /// <param name="compares">给定的比较类型实例集合。</param>
        /// <param name="bindingFlags">给定的 <see cref="BindingFlags"/>（可选；默认为公共属性标记）。</param>
        /// <returns>返回布尔值。</returns>
        public static bool SequencePropertyValuesEquals<T>(this IEnumerable<T> sources, IEnumerable<T> compares,
            BindingFlags bindingFlags = BindingFlags.Public)
        {
            sources.NotEmpty(nameof(sources));
            compares.NotEmpty(nameof(compares));

            var sequenceCount = sources.Count();
            if (sequenceCount != compares.Count())
                return false;

            var propertyInfos = typeof(T).GetProperties(bindingFlags);
            for (var i = 0; i < sequenceCount; i++)
            {
                var source = sources.ElementAt(i);
                var compare = compares.ElementAt(i);

                foreach (var info in propertyInfos)
                {
                    if (!info.GetValue(source).Equals(info.GetValue(compare)))
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 属性值集合相等比较。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="source">给定的源类型实例。</param>
        /// <param name="compare">给定的比较类型实例。</param>
        /// <param name="bindingFlags">给定的 <see cref="BindingFlags"/>（可选；默认为公共属性标记）。</param>
        /// <returns>返回布尔值。</returns>
        public static bool PropertyValuesEquals<T>(this T source, T compare, BindingFlags bindingFlags = BindingFlags.Public)
            where T : class
        {
            source.NotNull(nameof(source));
            compare.NotNull(nameof(compare));

            var propertyInfos = typeof(T).GetProperties(bindingFlags);
            foreach (var info in propertyInfos)
            {
                if (!info.GetValue(source).Equals(info.GetValue(compare)))
                    return false;
            }

            return true;
        }

        #endregion


        #region SetProperty

        /// <summary>
        /// 设置属性。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <typeparam name="TProperty">指定的属性类型。</typeparam>
        /// <param name="element">给定的 <typeparamref name="T"/>。</param>
        /// <param name="propertyExpression">给定的属性表达式。</param>
        /// <param name="newProperty">给定的 <typeparamref name="TProperty"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static T SetProperty<T, TProperty>(this T element,
            Expression<Func<T, TProperty>> propertyExpression, TProperty newProperty)
            where T : class
            => element.SetProperty(propertyExpression.AsPropertyName(), newProperty);

        /// <summary>
        /// 设置属性。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="element">给定的 <typeparamref name="T"/>。</param>
        /// <param name="propertyName">给定的属性名称。</param>
        /// <param name="newPropertyValue">给定的新属性值。</param>
        /// <returns>返回 <typeparamref name="T"/>。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        public static T SetProperty<T>(this T element, string propertyName, object newPropertyValue)
            where T : class
        {
            element.NotNull(nameof(element));

            // 如果 T 为接口、抽象等类型，typeof(T) 方法获取的属性可能因没有定义 set 方法导致参数异常
            var property = element.GetType().GetProperty(propertyName);
            property.NotNull(nameof(property));

            property.SetValue(element, newPropertyValue);
            return element;
        }

        /// <summary>
        /// 设置属性。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <param name="propertyName">给定的属性名称。</param>
        /// <param name="newPropertyValue">给定的新属性值。</param>
        /// <returns>返回对象。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        public static object SetProperty(this object obj, string propertyName, object newPropertyValue)
        {
            obj.NotNull(nameof(obj));

            var property = obj.GetType().GetProperty(propertyName);
            property.NotNull(nameof(property));

            property.SetValue(obj, newPropertyValue);
            return obj;
        }

        #endregion

    }
}
