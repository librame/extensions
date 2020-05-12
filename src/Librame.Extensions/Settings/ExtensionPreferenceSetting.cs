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
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;

namespace Librame.Extensions
{
    using Resources;

    /// <summary>
    /// 扩展偏好设置。
    /// </summary>
    public class ExtensionPreferenceSetting : AbstractPreferenceSetting, IExtensionPreferenceSetting
    {
        private readonly List<object> _lockers
            = new List<object>();

        // 禁止只读限制
        private Lazy<SpinLock> _spinLock
            = new Lazy<SpinLock>(() => new SpinLock());


        /// <summary>
        /// 重置偏好设置。
        /// </summary>
        public override void Reset()
        {
            _lockers.Clear();

            ObjectExtensions.CreateFactories.Clear();

            AlgorithmExtensions.HashAlgorithms.Clear();
            AlgorithmExtensions.HmacAlgorithms.Clear();
            AlgorithmExtensions.SymmetricAlgorithms.Clear();
            AlgorithmExtensions.AsymmetricAlgorithms.Clear();
        }


        #region Lockers

        /// <summary>
        /// 最大锁定器数量（默认为 <see cref="ExtensionSettings.ProcessorCount"/>）。
        /// </summary>
        public virtual int MaxLockerCount
            => ExtensionSettings.ProcessorCount;

        /// <summary>
        /// 断定死锁的持续时长（默认为3秒）。
        /// </summary>
        public virtual TimeSpan DeadlockDuration
            => TimeSpan.FromSeconds(3);


        /// <summary>
        /// 获取锁定器。
        /// </summary>
        /// <returns>返回锁定器对象。</returns>
        protected object GetLocker()
            => GetLocker(out _);

        /// <summary>
        /// 获取锁定器。
        /// </summary>
        /// <param name="index">输出索引。</param>
        /// <returns>返回锁定器对象。</returns>
        protected object GetLocker(out int index)
        {
            lock (ExtensionSettings.Locker)
            {
                if (_lockers.IsEmpty())
                    return AddLocker(out index);

                return GetLocker(out index);
            }

            // AddLocker
            object AddLocker(out int index)
            {
                index = _lockers.Count;

                var locker = new object();
                _lockers.Add(locker);

                return locker;
            }

            // GetLocker
            object GetLocker(out int index)
            {
                Stopwatch stopwatch = null;

                while (true)
                {
                    var locker = _lockers.FirstOrDefault(obj => !Monitor.IsEntered(obj));
                    if (locker.IsNotNull())
                    {
                        index = _lockers.FindIndex(obj => ReferenceEquals(obj, locker));
                        return locker;
                    }

                    if (_lockers.Count < MaxLockerCount)
                        return AddLocker(out index);

                    if (stopwatch.IsNull())
                        stopwatch = Stopwatch.StartNew();

                    if (!stopwatch.IsRunning)
                        stopwatch.Restart();

                    if (stopwatch.Elapsed >= DeadlockDuration)
                    {
                        stopwatch.Stop();

                        throw new OverflowException(string.Format(CultureInfo.InvariantCulture,
                            InternalResource.OverflowExceptionLockersFormat,
                            MaxLockerCount));
                    }
                }
            }
        }


        /// <summary>
        /// 运行锁定器。
        /// </summary>
        /// <param name="action">给定的动作。</param>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        public virtual void RunLocker(Action action)
        {
            action.NotNull(nameof(action));

            lock (GetLocker())
            {
                action.Invoke();
            }
        }

        /// <summary>
        /// 运行锁定器。
        /// </summary>
        /// <param name="action">给定的动作（输入参数为当前锁定器索引）。</param>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        public virtual void RunLocker(Action<int> action)
        {
            action.NotNull(nameof(action));

            lock (GetLocker(out var index))
            {
                action.Invoke(index);
            }
        }


        /// <summary>
        /// 运行带返回结果的锁定器。
        /// </summary>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="func">给定的工厂方法。</param>
        /// <returns>返回 <typeparamref name="TResult"/>。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        public virtual TResult RunLockerResult<TResult>(Func<TResult> func)
        {
            func.NotNull(nameof(func));

            lock (GetLocker())
            {
                return func.Invoke();
            }
        }

        /// <summary>
        /// 运行带返回结果的锁定器。
        /// </summary>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="func">给定的工厂方法（输入参数为当前锁定器索引）。</param>
        /// <returns>返回 <typeparamref name="TResult"/>。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        public virtual TResult RunLockerResult<TResult>(Func<int, TResult> func)
        {
            func.NotNull(nameof(func));

            lock (GetLocker(out var index))
            {
                return func.Invoke(index);
            }
        }


        /// <summary>
        /// 运行短时间锁定器。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action"/> is null.
        /// </exception>
        /// <param name="action">给定的动作。</param>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        public virtual void RunSpinLocker(Action action)
        {
            action.NotNull(nameof(action));

            var lockTaken = false;
            try
            {
                _spinLock.Value.Enter(ref lockTaken);
                action.Invoke();
            }
            finally
            {
                if (lockTaken)
                    _spinLock.Value.Exit(false);
            }
        }

        /// <summary>
        /// 运行带返回结果的短时间锁定器。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="func"/> is null.
        /// </exception>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="func">给定的动作。</param>
        /// <returns>返回 <typeparamref name="TResult"/>。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        public virtual TResult RunSpinLockerResult<TResult>(Func<TResult> func)
        {
            func.NotNull(nameof(func));

            var lockTaken = false;
            try
            {
                _spinLock.Value.Enter(ref lockTaken);
                return func.Invoke();
            }
            finally
            {
                if (lockTaken)
                    _spinLock.Value.Exit(false);
            }
        }

        #endregion


        #region Algorithm Extensions

        /// <summary>
        /// 0-9 的数字。
        /// </summary>
        public virtual string Digits
            => "0123456789";

        /// <summary>
        /// 26 个小写字母。
        /// </summary>
        public virtual string LowercaseLetters
            => "abcdefghijklmnopqrstuvwxyz";

        /// <summary>
        /// 26 个大写字母。
        /// </summary>
        public virtual string UppercaseLetters
            => "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// 52 个大小写字母表。
        /// </summary>
        public virtual string AllLetters
            => LowercaseLetters + UppercaseLetters;

        /// <summary>
        /// 62 个大小写字母表和阿拉伯数字。
        /// </summary>
        public virtual string AllLettersAndDigits
            => AllLetters + Digits;

        /// <summary>
        /// 9 个算法特殊符号。
        /// </summary>
        public virtual string AlgorithmSpecialSymbols
            => "~!@#$%^&*";

        /// <summary>
        /// 算法字符集（包括 71 个大小写字母表、阿拉伯数字和算法特殊符号）。
        /// </summary>
        public virtual string AlgorithmChars
            => AllLettersAndDigits + AlgorithmSpecialSymbols;

        /// <summary>
        /// BASE32 字符集。
        /// </summary>
        public virtual string Base32Chars
            => UppercaseLetters + "234567";


        /// <summary>
        /// HMAC 键控短密钥。
        /// </summary>
        public virtual string HmacShortKey
            => "7Rka278mPkmw45a3gtsNWRka278mPkmw45a3gtsNWe0ZGtu/Jj5JsOOWt4LbDVntGRrbvyY+SbDjlreC2w1Z7Q==";

        /// <summary>
        /// HMAC 键控长密钥。
        /// </summary>
        public virtual string HmacLongKey
            => "7Rka278mPkmw45a3gtsNWRka278mPkmw45a3gtsNWe0ZGtu/Jj5JsOOWt4LbDVntGRrbvyY+SbDjlreC2w1Z7Rka278mPkmw45a3gtsNWe0ZGtu/Jj5JsOOWt4LbDVntGRrbvyY+SbDjlreC2w1Z7Rka278mPkmw45a3gtsNWe0=";

        /// <summary>
        /// AES 密钥。
        /// </summary>
        public virtual string AesKey
            => "JUmlxL8G806eU4R5eSU+mEmlxL8G806eU4R5eSU+mCU=";

        /// <summary>
        /// AES 向量。
        /// </summary>
        public virtual string AesVector
            => "nk7zBr/EpUmYPiV5eYRTng==";

        /// <summary>
        /// DES 密钥。
        /// </summary>
        public virtual string DesKey
            => "JUmlxL8G804=";

        /// <summary>
        /// DES 向量。
        /// </summary>
        public virtual string DesVector
            => "mD4leXmEU54=";

        /// <summary>
        /// TripleDES 密钥。
        /// </summary>
        public virtual string TripleDesKey
            => "JUmlxL8G806eU4R5eSU+mEmlxL8G806e";

        /// <summary>
        /// TripleDES 向量。
        /// </summary>
        public virtual string TripleDesVector
            => "mD4leXmEU54=";

        #endregion


        #region Compression Extensions

        /// <summary>
        /// LZNT1 压缩格式。
        /// </summary>
        public virtual ushort CompressionFormatLZNT1
            => 2;

        /// <summary>
        /// 压缩引擎最大值。
        /// </summary>
        public virtual ushort CompressionEngineMaximum
            => 0x100;

        /// <summary>
        /// 支持的压缩文件类型集合（默认支持 GZip 与 Deflate 压缩算法文件类型）。
        /// </summary>
        public virtual IReadOnlyList<string> SupportedCompressedFileTypes
            => ".cmp,.gz".Split(',');

        /// <summary>
        /// Deflate 压缩文件类型。
        /// </summary>
        public virtual string DeflateCompressedFileType
            => SupportedCompressedFileTypes[0];

        /// <summary>
        /// GZIP 压缩文件类型。
        /// </summary>
        public virtual string GZipCompressedFileType
            => SupportedCompressedFileTypes[1];

        #endregion


        #region DateTime Extensions

        /// <summary>
        /// 基础日期与时间（1911-01-01）。
        /// </summary>
        public virtual DateTime BaseDateTime
            => new DateTime(1911, 1, 1);

        /// <summary>
        /// 基础日期与时间（<see cref="BaseDateTime"/>）。
        /// </summary>
        public virtual DateTimeOffset BaseDateTimeOffset
            => new DateTimeOffset(BaseDateTime);

        #endregion


        #region Encoding Extensions

        /// <summary>
        /// 默认字符编码（UTF-8）。
        /// </summary>
        public virtual Encoding DefaultEncoding
            => Encoding.UTF8;

        #endregion

    }
}
