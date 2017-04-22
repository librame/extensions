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
using System.IO;

namespace Librame.Utility
{
    /// <summary>
    /// <see cref="Exception"/> 实用工具。
    /// </summary>
    public class ExceptionUtility
    {
        /// <summary>
        /// 得到内部异常消息。
        /// </summary>
        /// <param name="ex">给定的异常。</param>
        /// <returns>返回消息字符串。</returns>
        public static string AsOrInnerMessage(Exception ex)
        {
            if (!ReferenceEquals(ex.InnerException, null))
                return AsOrInnerMessage(ex.InnerException);

            return ex.Message;
        }


        /// <summary>
        /// 得到不为空的类型实例。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// item is null.
        /// </exception>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="item">给定的实例。</param>
        /// <param name="paramName">给定的参数名。</param>
        /// <returns>返回实例或抛出异常。</returns>
        public static T NotNull<T>(T item, string paramName)
        {
            GuardNull(item, paramName);

            return item;
        }


        #region Arguments

        /// <summary>
        /// 监视对象是否为空。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// obj is null.
        /// </exception>
        /// <param name="obj">给定的对象。</param>
        /// <param name="paramName">给定的参数名。</param>
        public static void GuardNull(object obj, string paramName)
        {
            if (ReferenceEquals(obj, null))
            {
                throw new ArgumentNullException(paramName);
            }
        }

        /// <summary>
        /// 监视字符串是否为空。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// str is null or empty.
        /// </exception>
        /// <param name="str">给定的字符串。</param>
        /// <param name="paramName">给定的参数名。</param>
        public static void GuardNullOrEmpty(string str, string paramName)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentNullException(paramName);
            }
        }

        /// <summary>
        /// 监视整数是否超出定义的允许值范围。
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// i is out of range.
        /// </exception>
        /// <param name="i">给定的整数。</param>
        /// <param name="min">给定的最小值。</param>
        /// <param name="max">给定的最大值。</param>
        /// <param name="paramName">给定的参数名。</param>
        public static void GuardOutOfRange(int i, int min, int max, string paramName)
        {
            if (i < min || i > max)
            {
                throw new ArgumentOutOfRangeException(paramName,
                    string.Format("The {0} value is out of range: {1} (min: {2}, max: {3})", paramName, i, min, max));
            }
        }

        /// <summary>
        /// 监视当前参数类型是否是指定类型（已集成检测对象是否为空）。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// obj is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// obj.GetType() is not requirement type.
        /// </exception>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="obj">给定的当前对象。</param>
        /// <param name="paramName">给定的参数名。</param>
        public static void GuardNotType<T>(object obj, string paramName)
            where T : class
        {
            GuardNull(obj, paramName);

            if (!(obj is T))
            {
                throw new ArgumentException(string.Format("The {0} is not requirement type {1}",
                    typeof(T).FullName), obj.GetType().FullName);
            }
        }

        /// <summary>
        /// 监视基础类型能否从指定类型中派生。
        /// </summary>
        /// <param name="baseType">给定的基础类型。</param>
        /// <param name="fromType">给定的派生类型。</param>
        public static void GuardAssignableFrom(Type baseType, Type fromType)
        {
            if (!baseType.IsAssignableFrom(fromType))
            {
                throw new ArgumentException(string.Format(Properties.Resources.TypeAssignableFromExceptionFormat,
                    baseType, fromType));
            }
        }

        #endregion


        #region IO

        /// <summary>
        /// 监视文件是否存在。
        /// </summary>
        /// <exception cref="FileNotFoundException">
        /// file not found.
        /// </exception>
        /// <param name="file">给定的文件字符串。</param>
        public static void GuardFileNotFound(string file)
        {
            if (!File.Exists(file))
            {
                throw new FileNotFoundException(file);
            }
        }

        #endregion

    }


    /// <summary>
    /// <see cref="ExceptionUtility"/> 静态扩展。
    /// </summary>
    public static class ExceptionUtilityExtensions
    {
        /// <summary>
        /// 得到内部异常消息。
        /// </summary>
        /// <param name="ex">给定的异常。</param>
        /// <returns>返回消息字符串。</returns>
        public static string AsOrInnerMessage(this Exception ex)
        {
            return ExceptionUtility.AsOrInnerMessage(ex);
        }


        /// <summary>
        /// 得到不为空的类型实例。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// item is null.
        /// </exception>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="item">给定的实例。</param>
        /// <param name="paramName">给定的参数名。</param>
        /// <returns>返回实例或抛出异常。</returns>
        public static T NotNull<T>(this T item, string paramName)
        {
            return ExceptionUtility.NotNull(item, paramName);
        }


        #region Arguments

        /// <summary>
        /// 监视对象是否为空。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// obj is null.
        /// </exception>
        /// <param name="obj">给定的对象。</param>
        /// <param name="paramName">给定的参数名。</param>
        public static void GuardNull(this object obj, string paramName)
        {
            ExceptionUtility.GuardNull(obj, paramName);
        }

        /// <summary>
        /// 监视字符串是否为空。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// str is null or empty.
        /// </exception>
        /// <param name="str">给定的字符串。</param>
        /// <param name="paramName">给定的参数名。</param>
        public static void GuardNullOrEmpty(this string str, string paramName)
        {
            ExceptionUtility.GuardNullOrEmpty(str, paramName);
        }
        
        /// <summary>
        /// 监视整数是否超出定义的允许值范围。
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// i is out of range.
        /// </exception>
        /// <param name="i">给定的整数。</param>
        /// <param name="min">给定的最小值。</param>
        /// <param name="max">给定的最大值。</param>
        /// <param name="paramName">给定的参数名。</param>
        public static void GuardOutOfRange(this int i, int min, int max, string paramName)
        {
            ExceptionUtility.GuardOutOfRange(i, min, max, paramName);
        }
        
        /// <summary>
        /// 监视当前参数类型是否是指定类型（已集成检测对象是否为空）。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// obj is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// obj.GetType() is not requirement type.
        /// </exception>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="obj">给定的当前对象。</param>
        /// <param name="paramName">给定的参数名。</param>
        public static void GuardNotType<T>(this object obj, string paramName)
            where T : class
        {
            ExceptionUtility.GuardNotType<T>(obj, paramName);
        }

        /// <summary>
        /// 监视基础类型能否从指定类型中派生。
        /// </summary>
        /// <param name="baseType">给定的基础类型。</param>
        /// <param name="fromType">给定的派生类型。</param>
        public static void GuardAssignableFrom(this Type baseType, Type fromType)
        {
            ExceptionUtility.GuardAssignableFrom(baseType, fromType);
        }

        #endregion


        #region IO

        /// <summary>
        /// 监视文件是否存在。
        /// </summary>
        /// <exception cref="FileNotFoundException">
        /// file not found.
        /// </exception>
        /// <param name="file">给定的文件字符串。</param>
        public static void GuardFileNotFound(this string file)
        {
            ExceptionUtility.GuardFileNotFound(file);
        }

        #endregion

    }
}
