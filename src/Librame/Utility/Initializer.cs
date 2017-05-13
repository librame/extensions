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
using System.Linq;
using System.Reflection;

namespace Librame.Utility
{
    /// <summary>
    /// 初始化器。
    /// </summary>
    public class Initializer
    {
        /// <summary>
        /// 初始化对象公共属性默认值。
        /// </summary>
        /// <typeparam name="T">指定的对象类型。</typeparam>
        /// <returns>返回对象。</returns>
        public static T Initialize<T>()
            where T : new()
        {
            var obj = Activator.CreateInstance<T>();

            return (T)Initialize(obj);
        }

        /// <summary>
        /// 初始化对象公共属性默认值。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回对象。</returns>
        public static object Initialize(object obj)
        {
            obj.NotNull(nameof(obj));

            var properties = obj.GetType().GetProperties();
            if (properties == null || properties.Length < 1)
                return obj;

            foreach (var p in properties)
            {
                var value = GetDefaultValue(p);

                if (value == null)
                    value = InitializePropertyValue(p.PropertyType);

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
            var attrib = (DefaultValueAttribute)Attribute.GetCustomAttribute(property,
                    typeof(DefaultValueAttribute), false);

            return (attrib == null ? null : attrib.Value);
        }

        /// <summary>
        /// 初始化属性值。
        /// </summary>
        /// <param name="propertyType">给定的属性类型。</param>
        /// <returns>返回属性值对象。</returns>
        private static object InitializePropertyValue(Type propertyType)
        {
            switch (propertyType.FullName)
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
                    return byte.MinValue; // sbyte

                case "System.UInt16":
                    return byte.MinValue; // ushort

                case "System.UInt32":
                    return byte.MinValue; // uint

                case "System.UInt64":
                    return byte.MinValue; // ulong

                default:
                    {
                        if (propertyType.IsSubclassOf(typeof(Nullable<>)))
                        {
                            try
                            {
                                var gts = propertyType.GenericTypeArguments;
                                var parameters = gts.Select(t => InitializePropertyValue(t)).ToArray();

                                var ci = propertyType.GetConstructor(propertyType.GenericTypeArguments);
                                return ci.Invoke(parameters);
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }

                        if (propertyType.IsClass && !propertyType.IsAbstract)
                            return Activator.CreateInstance(propertyType);

                        return null;
                    }
            }
        }

    }
}
