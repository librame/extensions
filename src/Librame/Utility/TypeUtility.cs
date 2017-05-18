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
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Librame.Utility
{
    /// <summary>
    /// <see cref="Type"/> 实用工具。
    /// </summary>
    public static class TypeUtility
    {
        /// <summary>
        /// 获取指定类型的程序集限定名，但不包括版本、文化、公钥标记等信息（如 Librame.Utility.TypeUtility, Librame）。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <returns>返回字符串。</returns>
        public static string AssemblyQualifiedNameWithoutVCP<T>()
        {
            return AssemblyQualifiedNameWithoutVCP(typeof(T));
        }
        /// <summary>
        /// 获取指定类型的程序集限定名，但不包括版本、文化、公钥标记等信息（如 Librame.Utility.TypeUtility, Librame）。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回字符串。</returns>
        public static string AssemblyQualifiedNameWithoutVCP(this Type type)
        {
            return (type.FullName + ", " + type.Assembly.GetName().Name);
        }


        /// <summary>
        /// 如果指定对象的属性值。
        /// </summary>
        /// <param name="obj">给定要获取属性值的对象。</param>
        /// <param name="propertyName">给定的属性名。</param>
        /// <returns>返回属性值。</returns>
        public static object AsPropertyValue(this object obj, string propertyName)
        {
            obj.NotNull(nameof(obj));

            var pi = obj.GetType().GetProperty(propertyName);
            return pi?.GetValue(obj);
        }


        /// <summary>
        /// 创建实例集合。
        /// </summary>
        /// <param name="types">给定的类型集合。</param>
        /// <returns>返回 <see cref="IList{T}"/>。</returns>
        public static IEnumerable<object> AsInstances(this IEnumerable<Type> types)
        {
            return types.Select(t => Activator.CreateInstance(t));
        }


        #region AsOrDefault

        /// <summary>
        /// 返回当前实例或默认值。
        /// </summary>
        /// <typeparam name="TSource">指定的源类型。</typeparam>
        /// <param name="source">给定的源对象。</param>
        /// <param name="defaultValue">给定的默认值。</param>
        /// <returns>返回实例或默认值。</returns>
        public static TSource AsOrDefault<TSource>(this TSource source, TSource defaultValue = default(TSource))
        {
            return source.AsOrDefault(c => c, defaultValue);
        }

        /// <summary>
        /// 转换类型或返回默认值。
        /// </summary>
        /// <typeparam name="TSource">指定的源类型。</typeparam>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="source">给定的源对象。</param>
        /// <param name="convertFactory">给定的转换方法。</param>
        /// <param name="defaultValue">给定的默认值。</param>
        /// <returns>返回结果对象。</returns>
        public static TResult AsOrDefault<TSource, TResult>(this TSource source, Func<TSource, TResult> convertFactory,
            TResult defaultValue = default(TResult))
        {
            if (source == null || convertFactory == null)
                return defaultValue;

            try
            {
                return convertFactory(source);
            }
            catch
            {
                return defaultValue;
            }
        }

        #endregion


        #region AsPairsString

        /// <summary>
        /// 将对象转换为键值对集合的字符串形式。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <param name="searchMode">给定的搜索模式（可选；默认不进行成员名称筛选）。</param>
        /// <param name="searchNames">要排除的成员名称数组。</param>
        /// <returns>返回字符串。</returns>
        public static string AsPairsString(this object obj, SearchMode searchMode = SearchMode.Default,
            params string[] searchNames)
        {
            obj.NotNull(nameof(obj));

            var properties = obj.GetType().SearchProperties(searchMode, searchNames);

            // 如果有需要处理的属性信息集合
            if (properties.Length > 0)
            {
                var sb = new StringBuilder();

                var last = properties.Last();
                foreach (var p in properties)
                {
                    var o = p.GetValue(obj);
                    var value = string.Empty;

                    if (o != null)
                    {
                        // 字符串不属于值类型
                        if (p.PropertyType.IsValueType || p.PropertyType.IsAnsiClass)
                            value = o.ToString();
                        else
                            value = o.AsPairsString(SearchMode.Default);
                    }

                    sb.AppendFormat("{0}={1}", p.Name, value);

                    if (p.Name != last.Name)
                        sb.Append(",");
                }

                return sb.ToString();
            }

            return string.Empty;
        }


        /// <summary>
        /// 将键值对集合的字符串还原为对象。
        /// </summary>
        /// <typeparam name="T">指定的对象类型。</typeparam>
        /// <param name="str">给定的字符串。</param>
        /// <param name="searchMode">给定的搜索模式（可选；默认不进行成员名称筛选）。</param>
        /// <param name="searchNames">要排除的成员名称数组。</param>
        /// <returns>返回对象。</returns>
        public static T FromPairsString<T>(this string str, SearchMode searchMode = SearchMode.Default,
            params string[] searchNames)
        {
            return (T)str.FromPairsString(typeof(T), searchMode, searchNames);
        }
        /// <summary>
        /// 将键值对集合的字符串还原为对象。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="type">给定的对象类型。</param>
        /// <param name="searchMode">给定的搜索模式（可选；默认不进行成员名称筛选）。</param>
        /// <param name="searchNames">要排除的成员名称数组。</param>
        /// <returns>返回对象。</returns>
        public static object FromPairsString(this string str, Type type, SearchMode searchMode = SearchMode.Default,
            params string[] searchNames)
        {
            str.NotEmpty(nameof(str));
            type.NotNull(nameof(type));

            var obj = Activator.CreateInstance(type);

            var properties = type.SearchProperties(searchMode, searchNames);

            // 如果有需要处理的属性信息集合
            if (properties.Length > 0)
            {
                var pairs = str.Split(',');
                foreach (var pair in pairs)
                {
                    if (string.IsNullOrEmpty(pair))
                        continue;

                    var part = pair.Split('=');
                    if (part.Length < 1 || part.Length > 2)
                        continue; // 不标准的键值对

                    var name = part[0];
                    var value = part[1];
                    if (string.IsNullOrEmpty(value))
                        continue; // 空字符串值

                    var pi = properties.First(p => p.Name == name);
                    var o = value.AsOrDefault(pi.PropertyType);// 转换值
                    if (o != null)
                        pi.SetValue(obj, o); // 更新值
                }
            }

            return obj;
        }


        /// <summary>
        /// 搜索指定类型的属性集合。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <param name="searchMode">给定的搜索模式（可选；默认不进行成员名称筛选）。</param>
        /// <param name="searchNames">要排除的成员名称数组。</param>
        /// <returns>返回属性数组。</returns>
        public static PropertyInfo[] SearchProperties(this Type type, SearchMode searchMode = SearchMode.Default,
            params string[] searchNames)
        {
            var properties = type.GetProperties();

            // 如果需要成员名称筛选
            if (searchNames.Length > 0)
            {
                // 排除模式
                if (searchMode == SearchMode.Exclude)
                {
                    foreach (var key in searchNames)
                    {
                        // 越来越少
                        properties = properties.Where(f => f.Name != key).ToArray();
                    }
                }
                // 包含模式
                else if (searchMode == SearchMode.Include)
                {
                    var list = new List<PropertyInfo>();

                    foreach (var name in searchNames)
                    {
                        var p = properties.Where(f => f.Name == name);
                        list.AddRange(p);
                    }

                    // 移除可能存在的重复项
                    properties = list.Distinct().ToArray();
                }
                else
                {
                    // 默认不操作
                }
            }

            return properties;
        }

        #endregion


        #region CopyTo

        /// <summary>
        /// 将源类型实例复制到新实例。
        /// </summary>
        /// <param name="source">给定的源类型实例。</param>
        /// <returns>返回新实例。</returns>
        public static T CopyToCreate<T>(this T source)
            where T : class, new()
        {
            var target = Activator.CreateInstance<T>();
            CopyTo(source, target);

            return target;
        }
        /// <summary>
        /// 将源类型实例复制到目标类型实例。
        /// </summary>
        /// <param name="source">给定的源类型实例。</param>
        /// <param name="target">给定的目标类型实例。</param>
        public static void CopyTo<T>(this T source, T target)
            where T : class
        {
            source.NotNull(nameof(source));
            target.NotNull(nameof(target));

            SetProperties(typeof(T), source, target);
        }

        /// <summary>
        /// 将源对象复制到新对象。
        /// </summary>
        /// <param name="source">给定的源对象。</param>
        /// <returns>返回新对象。</returns>
        public static object CopyToCreate(this object source)
        {
            var target = Activator.CreateInstance(source?.GetType());
            CopyTo(source, target);

            return target;
        }
        /// <summary>
        /// 将源对象复制到目标对象。
        /// </summary>
        /// <param name="source">给定的源对象。</param>
        /// <param name="target">给定的目标对象。</param>
        public static void CopyTo(this object source, object target)
        {
            source = source.NotNull(nameof(source));
            target = target.NotNull(nameof(target));

            var sourceType = source.GetType();
            var targetType = target.GetType();
            if (sourceType != targetType)
            {
                throw new ArgumentException(string.Format("源类型 {0} 与目标类型 {1} 不一致",
                    sourceType.Name, targetType.Name));
            }

            SetProperties(sourceType, source, target);
        }

        private static void SetProperties(Type propertyType, object source, object target)
        {
            var properties = propertyType.GetProperties();
            if (properties.Length < 1)
                return;

            properties.Invoke(pi =>
            {
                var sourceValue = pi.GetValue(source);
                if (sourceValue != null)
                    pi.SetValue(target, sourceValue);
            });
        }

        #endregion


        #region Attribute

        /// <summary>
        /// 获取类属性。
        /// </summary>
        /// <typeparam name="T">指定要获取的类型。</typeparam>
        /// <typeparam name="TAttribute">指定要获取的属性类型。</typeparam>
        /// <param name="inherit">搜索此成员的继承链以查找这些属性，则为 true；否则为 false。</param>
        /// <returns>返回属性对象。</returns>
        public static TAttribute GetClassAttribute<T, TAttribute>(bool inherit = false)
            where TAttribute : Attribute
        {
            return GetClassAttribute<TAttribute>(typeof(T), inherit);
        }
        /// <summary>
        /// 获取类属性。
        /// </summary>
        /// <typeparam name="TAttribute">指定要获取的属性类型。</typeparam>
        /// <param name="type">给定的类型。</param>
        /// <param name="inherit">搜索此成员的继承链以查找这些属性，则为 true；否则为 false。</param>
        /// <returns>返回属性对象。</returns>
        public static TAttribute GetClassAttribute<TAttribute>(this Type type, bool inherit = false)
            where TAttribute : Attribute
        {
            type.NotNull(nameof(type));

            var attribs = type.GetCustomAttributes(typeof(TAttribute), inherit);

            return (TAttribute)attribs?.FirstOrDefault();
        }


        /// <summary>
        /// 获取此成员包含的自定义属性。
        /// </summary>
        /// <typeparam name="TAttribute">指定的属性类型。</typeparam>
        /// <param name="member">指定的成员信息。</param>
        /// <param name="inherit">指定是否搜索该成员的继承链以查找这些特性。</param>
        /// <returns>返回自定义属性对象。</returns>
        public static TAttribute GetMemberAttribute<TAttribute>(this MemberInfo member, bool inherit = false)
            where TAttribute : Attribute
        {
            member.NotNull(nameof(member));
            
            var attribs = member.GetCustomAttributes(typeof(TAttribute), inherit);

            return (TAttribute)attribs?.FirstOrDefault();
        }

        #endregion


        #region AssignableTypes

        /// <summary>
        /// 从当前程序集中获取限定类型的派生类型集合。
        /// </summary>
        /// <typeparam name="TBase">指定的基础类型。</typeparam>
        /// <param name="withInstantiable">仅支持可实例化类型（可选）。</param>
        /// <param name="withoutBaseType">排除基础类型自身（可选）。</param>
        /// <returns>返回类型数组。</returns>
        public static Type[] GetAssignableTypes<TBase>(bool withInstantiable = true,
            bool withoutBaseType = true)
        {
            return GetAssignableTypes(typeof(TBase), withInstantiable, withoutBaseType);
        }
        /// <summary>
        /// 从指定程序集中获取限定基础类型的派生类型集合。
        /// </summary>
        /// <param name="baseType">给定的基础类型。</param>
        /// <param name="withInstantiable">仅支持可实例化类型（可选）。</param>
        /// <param name="withoutBaseType">排除基础类型自身（可选）。</param>
        /// <returns>返回类型数组。</returns>
        public static Type[] GetAssignableTypes(this Type baseType,
            bool withInstantiable = true, bool withoutBaseType = true)
        {
            var assembly = Assembly.GetCallingAssembly();

            return GetAssignableTypes(assembly, baseType, withInstantiable, withoutBaseType);
        }

        /// <summary>
        /// 从已加载到此应用程序域的程序集集合中获取限定类型的派生类型集合。
        /// </summary>
        /// <typeparam name="TBase">指定的基础类型。</typeparam>
        /// <param name="assembly">给定的程序集。</param>
        /// <param name="withInstantiable">仅支持可实例化类型（可选）。</param>
        /// <param name="withoutBaseType">排除基础类型自身（可选）。</param>
        /// <returns>返回类型数组。</returns>
        public static Type[] GetAssignableTypes<TBase>(this Assembly assembly,
            bool withInstantiable = true, bool withoutBaseType = true)
        {
            return GetAssignableTypes(assembly, typeof(TBase), withInstantiable, withoutBaseType);
        }
        /// <summary>
        /// 从指定程序集中获取限定基础类型的派生类型集合。
        /// </summary>
        /// <param name="assembly">给定的程序集。</param>
        /// <param name="baseType">给定的基础类型。</param>
        /// <param name="withInstantiable">仅支持可实例化类型（可选）。</param>
        /// <param name="withoutBaseType">排除基础类型自身（可选）。</param>
        /// <returns>返回类型数组。</returns>
        public static Type[] GetAssignableTypes(this Assembly assembly, Type baseType,
            bool withInstantiable = true, bool withoutBaseType = true)
        {
            baseType.NotNull(nameof(baseType));
            assembly.NotNull(nameof(assembly));

            try
            {
                // 获取定义的公共类型
                var allTypes = assembly.GetExportedTypes();
                if (ReferenceEquals(allTypes, null) || allTypes.Length < 1)
                    return null;

                // 加载所有派生类型集合
                var types = allTypes.Where(t => baseType.IsAssignableFrom(t));

                // 仅包含可实例化类（排除抽象类）
                if (withInstantiable)
                    types = types.Where(t => !t.IsAbstract);

                // 移除自身基类
                if (withoutBaseType)
                    types = types.Where(t => t.FullName != baseType.FullName);

                return types.ToArray();
            }
            catch
            {
                return null;
            }
        }

        #endregion


        #region Instantiation

        /// <summary>
        /// 实例化类型，并初始化对象公共属性的默认值。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <returns>返回类型实例。</returns>
        public static T Instantiate<T>()
        {
            return (T)Initialize(Activator.CreateInstance<T>());
        }

        /// <summary>
        /// 实例化类型，初始化对象公共属性的默认值。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回对象。</returns>
        public static object Instantiate(this Type type)
        {
            return Initialize(Activator.CreateInstance(type));
        }

        /// <summary>
        /// 初始化对象公共属性的默认值。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回对象。</returns>
        public static object Initialize(this object obj)
        {
            var properties = obj.NotNull(nameof(obj)).GetType().GetProperties();
            if (properties == null || properties.Length < 1)
                return obj;

            foreach (var p in properties)
            {
                var value = GetDefaultValue(p);

                if (value == null)
                    value = p.PropertyType.AsDefault();

                p.SetValue(obj, value, null);
            }

            return obj;
        }
        /// <summary>
        /// 获取默认值属性特性。
        /// </summary>
        /// <param name="property">给定的属性信息。</param>
        /// <returns>返回默认值对象。</returns>
        private static object GetDefaultValue(PropertyInfo property)
        {
            var attrib = (DefaultValueAttribute)property.GetCustomAttribute(typeof(DefaultValueAttribute), false);
            return attrib?.Value;
        }


        /// <summary>
        /// 转换对象的默认值。
        /// </summary>
        /// <param name="type">给定的转换类型。</param>
        /// <returns>返回对象。</returns>
        public static object AsDefault(this Type type)
        {
            switch (type.FullName)
            {
                case "System.Boolean":
                    return false;

                case "System.Decimal":
                    return decimal.One;

                case "System.Double":
                    return double.NaN;

                case "System.DateTime":
                    return DateTime.Now;

                case "System.Guid":
                    return Guid.Empty;

                case "System.String":
                    return string.Empty;

                case "System.TimeSpan":
                    return TimeSpan.Zero;

                // Int
                case "System.Byte":
                    return byte.MinValue; // byte

                case "System.Int16":
                    return byte.MinValue; // short

                case "System.Int32":
                    return byte.MinValue; // int

                case "System.Int64":
                    return byte.MinValue; // long

                case "System.SByte":
                    return sbyte.MinValue; // sbyte

                case "System.UInt16":
                    return byte.MinValue; // ushort

                case "System.UInt32":
                    return byte.MinValue; // uint

                case "System.UInt64":
                    return byte.MinValue; // ulong

                default:
                    {
                        if (type.IsGenericType)
                        {
                            try
                            {
                                var gts = type.GenericTypeArguments;
                                // 链式转换
                                var parameters = gts.Select(t => AsDefault(t)).ToArray();

                                var ci = type.GetConstructor(type.GenericTypeArguments);
                                return ci.Invoke(parameters);
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }

                        if (type.IsClass && !type.IsAbstract)
                            return Activator.CreateInstance(type);

                        return null;
                    }
            }
        }

        #endregion

    }
}
