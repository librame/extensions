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
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

namespace Librame.Extensions
{
    /// <summary>
    /// 扩展首选项。
    /// </summary>
    public static class ExtensionSettings
    {
        /// <summary>
        /// 仅供 <see cref="IExtensionPreferenceSetting"/> 实例与 <see cref="ExtensionPreferenceSetting.GetLocker()"/> 使用。
        /// </summary>
        internal static readonly object Locker = new object();

        private static IExtensionPreferenceSetting _preference;

        /// <summary>
        /// 当前偏好设置。
        /// </summary>
        public static IExtensionPreferenceSetting Preference
        {
            get
            {
                if (null == _preference)
                {
                    lock (Locker)
                    {
                        if (null == _preference)
                        {
                            _preference = new ExtensionPreferenceSetting();
                        }
                    }
                }

                return _preference;
            }
            set
            {
                if (null == value)
                    throw new ArgumentNullException(nameof(value));

                lock (Locker)
                {
                    _preference = value;
                }
            }
        }


        /// <summary>
        /// 处理器数（即处理器线程数）。
        /// </summary>
        public static readonly int ProcessorCount
            = Environment.ProcessorCount;

        /// <summary>
        /// <see cref="Encoding.UTF8"/> 字符编码。
        /// </summary>
        public static readonly Encoding UTF8Encoding
            = Encoding.UTF8;


        #region Compression Extensions

        /// <summary>
        /// ntdll.dll 文件名。
        /// </summary>
        public const string NtDllFileName
            = "ntdll.dll";

        /// <summary>
        /// kernel32.dll 文件名。
        /// </summary>
        public const string Kernel32DllFileName
            = "kernel32.dll";

        #endregion


        #region Path Extensions

        /// <summary>
        /// 正向目录分隔字符（/）。
        /// </summary>
        public static readonly char AltDirectorySeparatorChar
            = Path.AltDirectorySeparatorChar;

        /// <summary>
        /// 反向目录分隔字符（\）。
        /// </summary>
        public static readonly char DirectorySeparatorChar
            = Path.DirectorySeparatorChar;


        /// <summary>
        /// 正向目录分隔符（/）。
        /// </summary>
        public static readonly string AltDirectorySeparator
            = AltDirectorySeparatorChar.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        /// 反向目录分隔字符（\）。
        /// </summary>
        public static readonly string DirectorySeparator
            = DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture);

        #endregion


        #region Type Extensions

        /// <summary>
        /// <see cref="DateTime"/> 类型。
        /// </summary>
        public static readonly Type DateTimeType
            = typeof(DateTime);

        /// <summary>
        /// <see cref="DateTimeOffset"/> 类型。
        /// </summary>
        public static readonly Type DateTimeOffsetType
            = typeof(DateTimeOffset);

        /// <summary>
        /// 字符串类型。
        /// </summary>
        public static readonly Type StringType
            = typeof(string);

        /// <summary>
        /// 可空类型定义。
        /// </summary>
        public static readonly Type NullableTypeDefinition
            = typeof(Nullable<>);

        internal const BindingFlags AllFlags
            = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        internal const BindingFlags AllFlagsWithoutStatic
            = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        #endregion

    }
}
