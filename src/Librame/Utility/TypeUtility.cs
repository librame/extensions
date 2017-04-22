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
using System.Linq;
using System.Reflection;

namespace Librame.Utility
{
    /// <summary>
    /// <see cref="Type"/> 实用工具。
    /// </summary>
    public class TypeUtility
    {
        /// <summary>
        /// 如果指定对象的属性值。
        /// </summary>
        /// <param name="obj">给定要获取属性值的对象。</param>
        /// <param name="propertyName">给定的属性名。</param>
        /// <returns>返回属性值。</returns>
        public static object AsPropertyValue(object obj, string propertyName)
        {
            if (ReferenceEquals(obj, null))
                return null;

            var pi = obj.GetType().GetProperty(propertyName);
            return pi?.GetValue(obj);
        }


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
        public static string AssemblyQualifiedNameWithoutVCP(Type type)
        {
            return (type.FullName + ", " + type.Assembly.GetName().Name);
        }


        /// <summary>
        /// 创建实例集合。
        /// </summary>
        /// <param name="types">给定的类型集合。</param>
        /// <returns>返回 <see cref="IList{T}"/>。</returns>
        public static IList<object> CreateInstances(IEnumerable<Type> types)
        {
            if (ReferenceEquals(types, null))
                return null;

            var list = new List<object>();

            foreach (var t in types)
            {
                object obj = null;

                try
                {
                    if (t.IsClass && !t.IsAbstract)
                        obj = Activator.CreateInstance(t);
                }
                catch { }

                if (!ReferenceEquals(obj, null))
                    list.Add(obj);
            }

            return list;
        }


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
        public static TAttribute GetClassAttribute<TAttribute>(Type type, bool inherit = false)
            where TAttribute : Attribute
        {
            type.GuardNull(nameof(type));

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
        public static TAttribute GetMemberAttribute<TAttribute>(MemberInfo member, bool inherit = false)
            where TAttribute : Attribute
        {
            member.GuardNull(nameof(member));
            
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
        public static Type[] GetAssignableTypes(Type baseType,
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
        public static Type[] GetAssignableTypes<TBase>(Assembly assembly,
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
        public static Type[] GetAssignableTypes(Assembly assembly, Type baseType,
            bool withInstantiable = true, bool withoutBaseType = true)
        {
            baseType.GuardNull(nameof(baseType));
            assembly.GuardNull(nameof(assembly));

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

    }


    /// <summary>
    /// <see cref="TypeUtility"/> 静态扩展。
    /// </summary>
    public static class TypeUtilityExtensions
    {
        /// <summary>
        /// 如果指定对象的属性值。
        /// </summary>
        /// <param name="obj">给定要获取属性值的对象。</param>
        /// <param name="propertyName">给定的属性名。</param>
        /// <returns>返回属性值。</returns>
        public static object AsPropertyValue(this object obj, string propertyName)
        {
            return TypeUtility.AsPropertyValue(obj, propertyName);
        }


        /// <summary>
        /// 获取指定类型的程序集限定名，但不包括版本、文化、公钥标记等信息（如 Librame.Utility.TypeUtility, Librame）。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回字符串。</returns>
        public static string AssemblyQualifiedNameWithoutVCP(Type type)
        {
            return TypeUtility.AssemblyQualifiedNameWithoutVCP(type);
        }


        /// <summary>
        /// 创建实例集合。
        /// </summary>
        /// <param name="types">给定的类型集合。</param>
        /// <returns>返回 <see cref="IList{T}"/>。</returns>
        public static IList<object> CreateInstances(this IEnumerable<Type> types)
        {
            return TypeUtility.CreateInstances(types);
        }

    }
}
