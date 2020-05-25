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
using System.Text;

namespace Librame.Extensions
{
    /// <summary>
    /// 扩展偏好设置接口。
    /// </summary>
    public interface IExtensionPreferenceSetting : IPreferenceSetting
    {

        #region Lockers

        /// <summary>
        /// 最大锁定器数量。
        /// </summary>
        int MaxLockerCount { get; }

        /// <summary>
        /// 断定死锁的持续时长。
        /// </summary>
        TimeSpan DeadlockDuration { get; }


        /// <summary>
        /// 获取锁定器。
        /// </summary>
        /// <returns>返回锁定器对象。</returns>
        object GetLocker();

        /// <summary>
        /// 获取锁定器。
        /// </summary>
        /// <param name="index">输出索引。</param>
        /// <returns>返回锁定器对象。</returns>
        object GetLocker(out int index);


        /// <summary>
        /// 运行锁定器。
        /// </summary>
        /// <param name="action">给定的动作。</param>
        void RunLocker(Action action);

        /// <summary>
        /// 运行锁定器。
        /// </summary>
        /// <param name="action">给定的动作（输入参数为当前锁定器索引）。</param>
        void RunLocker(Action<int> action);

        /// <summary>
        /// 运行带返回结果的锁定器。
        /// </summary>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="func">给定的工厂方法。</param>
        /// <returns>返回 <typeparamref name="TResult"/>。</returns>
        TResult RunLockerResult<TResult>(Func<TResult> func);

        /// <summary>
        /// 运行带返回结果的锁定器。
        /// </summary>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="func">给定的工厂方法（输入参数为当前锁定器索引）。</param>
        /// <returns>返回 <typeparamref name="TResult"/>。</returns>
        TResult RunLockerResult<TResult>(Func<int, TResult> func);


        /// <summary>
        /// 运行短时间锁定器（不适用于有堵塞、调用自身可能会阻止的任何内容、同时保留多个自旋锁、进行动态调度的调用(接口和虚方法)、对任何代码进行静态调度调用、分配内存等操作）。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action"/> is null.
        /// </exception>
        /// <param name="action">给定的动作。</param>
        void RunSpinLocker(Action action);

        /// <summary>
        /// 运行带返回结果的短时间锁定器（不适用于有堵塞、调用自身可能会阻止的任何内容、同时保留多个自旋锁、进行动态调度的调用(接口和虚方法)、对任何代码进行静态调度调用、分配内存等操作）。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="func"/> is null.
        /// </exception>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="func">给定的动作。</param>
        /// <returns>返回 <typeparamref name="TResult"/>。</returns>
        TResult RunSpinLockerResult<TResult>(Func<TResult> func);

        #endregion


        #region Algorithm Extensions

        /// <summary>
        /// 0-9 的数字。
        /// </summary>
        string Digits { get; }

        /// <summary>
        /// 26 个小写字母。
        /// </summary>
        string LowercaseLetters { get; }

        /// <summary>
        /// 26 个大写字母。
        /// </summary>
        string UppercaseLetters { get; }

        /// <summary>
        /// 52 个大小写字母表。
        /// </summary>
        string AllLetters { get; }

        /// <summary>
        /// 62 个大小写字母表和阿拉伯数字。
        /// </summary>
        string AllLettersAndDigits { get; }

        /// <summary>
        /// 9 个算法特殊符号。
        /// </summary>
        string AlgorithmSpecialSymbols { get; }

        /// <summary>
        /// 算法字符集（包括 71 个大小写字母表、阿拉伯数字和算法特殊符号）。
        /// </summary>
        string AlgorithmChars { get; }

        /// <summary>
        /// BASE32 字符集。
        /// </summary>
        string Base32Chars { get; }


        /// <summary>
        /// HMAC 键控短密钥。
        /// </summary>
        string HmacShortKey { get; }

        /// <summary>
        /// HMAC 键控长密钥。
        /// </summary>
        string HmacLongKey { get; }

        /// <summary>
        /// AES 密钥。
        /// </summary>
        string AesKey { get; }

        /// <summary>
        /// AES 向量。
        /// </summary>
        string AesVector { get; }

        /// <summary>
        /// DES 密钥。
        /// </summary>
        string DesKey { get; }

        /// <summary>
        /// DES 向量。
        /// </summary>
        string DesVector { get; }

        /// <summary>
        /// TripleDES 密钥。
        /// </summary>
        string TripleDesKey { get; }

        /// <summary>
        /// TripleDES 向量。
        /// </summary>
        string TripleDesVector { get; }

        #endregion


        #region Compression Extensions

        /// <summary>
        /// LZNT1 压缩格式。
        /// </summary>
        ushort CompressionFormatLZNT1 { get; }

        /// <summary>
        /// 压缩引擎最大值。
        /// </summary>
        ushort CompressionEngineMaximum { get; }

        /// <summary>
        /// 支持的压缩文件类型集合（默认支持 GZip 与 Deflate 压缩算法文件类型）。
        /// </summary>
        IReadOnlyList<string> SupportedCompressedFileTypes { get; }

        /// <summary>
        /// Deflate 压缩文件类型。
        /// </summary>
        string DeflateCompressedFileType { get; }

        /// <summary>
        /// GZIP 压缩文件类型。
        /// </summary>
        string GZipCompressedFileType { get; }

        #endregion


        #region DateTime Extensions

        /// <summary>
        /// 基础日期与时间。
        /// </summary>
        DateTime BaseDateTime { get; }

        /// <summary>
        /// 基础日期与时间。
        /// </summary>
        DateTimeOffset BaseDateTimeOffset { get; }

        #endregion


        #region Encoding Extensions

        /// <summary>
        /// 默认字符编码。
        /// </summary>
        Encoding DefaultEncoding { get; }

        #endregion

    }
}
