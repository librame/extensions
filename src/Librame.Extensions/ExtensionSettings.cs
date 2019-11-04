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
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace Librame.Extensions
{
    /// <summary>
    /// 静态扩展首选项。
    /// </summary>
    public static class ExtensionSettings
    {
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


        #region Algorithm Extensions

        /// <summary>
        /// 0-9 的数字。
        /// </summary>
        public const string Digits
            = "0123456789";

        /// <summary>
        /// 26 个小写字母。
        /// </summary>
        public const string LowercaseLetters
            = "abcdefghijklmnopqrstuvwxyz";

        /// <summary>
        /// 26 个大写字母。
        /// </summary>
        public const string UppercaseLetters
            = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// 52 个大小写字母表。
        /// </summary>
        public const string AllLetters
            = LowercaseLetters + UppercaseLetters;

        /// <summary>
        /// 62 个大小写字母表和阿拉伯数字。
        /// </summary>
        public const string AllLettersAndDigits
            = AllLetters + Digits;

        /// <summary>
        /// 9 个算法特殊符号。
        /// </summary>
        public const string AlgorithmSpecialSymbols
            = "~!@#$%^&*";

        /// <summary>
        /// 算法字符集（包括 71 个大小写字母表、阿拉伯数字和算法特殊符号）。
        /// </summary>
        public const string AlgorithmChars
            = AllLettersAndDigits + AlgorithmSpecialSymbols;

        /// <summary>
        /// BASE32 字符集。
        /// </summary>
        public const string Base32Chars
            = UppercaseLetters + "234567";

        #endregion


        #region Compression Extensions

        /// <summary>
        /// LZNT1 压缩格式。
        /// </summary>
        public const ushort CompressionFormatLZNT1
            = 2;

        /// <summary>
        /// 压缩引擎最大值。
        /// </summary>
        public const ushort CompressionEngineMaximum
            = 0x100;

        /// <summary>
        /// 支持的压缩文件类型集合（默认支持 GZip 与 Deflate 压缩算法文件类型）。
        /// </summary>
        public static readonly string[] SupportedCompressedFileTypes
            = ".cmp,.gz".Split(',');

        /// <summary>
        /// Deflate 压缩文件类型。
        /// </summary>
        public static readonly string DeflateCompressedFileType
            = SupportedCompressedFileTypes[0];

        /// <summary>
        /// GZIP 压缩文件类型。
        /// </summary>
        public static readonly string GZipCompressedFileType
            = SupportedCompressedFileTypes[1];

        #endregion


        #region DateTime Extensions

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
        /// 基础日期与时间。
        /// </summary>
        public static readonly DateTime BaseDateTime
            = new DateTime(1900, 1, 1);

        #endregion


        #region Object Extensions

        /// <summary>
        /// 锁定器。
        /// </summary>
        public static readonly object Locker
            = new object();

        #endregion


        #region Path Extensions

        /// <summary>
        /// 正向目录分隔字符（/）。
        /// </summary>
        public static readonly char AltDirectorySeparatorChar
            = Path.AltDirectorySeparatorChar;

        /// <summary>
        /// 正向目录分隔符（/）。
        /// </summary>
        public static readonly string AltDirectorySeparator
            = AltDirectorySeparatorChar.ToString(CultureInfo.InvariantCulture);


        /// <summary>
        /// 反向目录分隔字符（\）。
        /// </summary>
        public static readonly char DirectorySeparatorChar
            = Path.DirectorySeparatorChar;

        /// <summary>
        /// 反向目录分隔符（/）。
        /// </summary>
        public static readonly string DirectorySeparator
            = DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture);

        #endregion


        #region Serialization Extensions

        /// <summary>
        /// 二进制格式化器。
        /// </summary>
        public static readonly Lazy<BinaryFormatter> BinaryFormatter
            = new Lazy<BinaryFormatter>(() => new BinaryFormatter());

        #endregion


        #region Type Extensions

        /// <summary>
        /// 字符串类型。
        /// </summary>
        public static readonly Type StringType
            = typeof(string);

        /// <summary>
        /// 可空类型。
        /// </summary>
        public static readonly Type NullableType
            = typeof(Nullable<>);


        /// <summary>
        /// 常用标记（包括公开、非公开、实例、静态等）。
        /// </summary>
        public static readonly BindingFlags CommonFlags
            = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        /// <summary>
        /// 常用非静态标记（包括公开、非公开、实例等）。
        /// </summary>
        public static readonly BindingFlags CommonFlagsWithoutStatic
            = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        #endregion

    }
}
