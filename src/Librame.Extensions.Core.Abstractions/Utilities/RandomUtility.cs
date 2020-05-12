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
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace Librame.Extensions.Core.Utilities
{
    /// <summary>
    /// <see cref="Random"/> 实用工具。
    /// </summary>
    public static class RandomUtility
    {
        private static int _seed
            = Environment.TickCount;

        // 支持多线程，各线程维持独立的随机实例
        private static readonly ThreadLocal<Random> _random
            = new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref _seed)));

        // 支持多线程，各线程维持独立的随机数生成器实例
        private static readonly ThreadLocal<RandomNumberGenerator> _generator
            = new ThreadLocal<RandomNumberGenerator>(() => RandomNumberGenerator.Create());


        /// <summary>
        /// 运行伪随机数生成器。
        /// </summary>
        /// <param name="action">给定的动作。</param>
        public static void Run(Action<Random> action)
            => action.NotNull(nameof(action)).Invoke(_random.Value);

        /// <summary>
        /// 运行伪随机数生成器，并返回结果值。
        /// </summary>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="valueFactory">给定的值工厂方法。</param>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        public static TValue Run<TValue>(Func<Random, TValue> valueFactory)
            => valueFactory.NotNull(nameof(valueFactory)).Invoke(_random.Value);


        /// <summary>
        /// 运行更具安全性的随机数生成器。
        /// </summary>
        /// <param name="action">给定的动作。</param>
        public static void RunSecurity(Action<RandomNumberGenerator> action)
            => action.NotNull(nameof(action)).Invoke(_generator.Value);

        /// <summary>
        /// 运行更具安全性的随机数生成器，并返回结果值。
        /// </summary>
        /// <typeparam name="TValue">指定的值类型。</typeparam>
        /// <param name="valueFactory">给定的值工厂方法。</param>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        public static TValue RunSecurity<TValue>(Func<RandomNumberGenerator, TValue> valueFactory)
            => valueFactory.NotNull(nameof(valueFactory)).Invoke(_generator.Value);


        /// <summary>
        /// 生成指定长度的随机数字节数组。
        /// </summary>
        /// <param name="length">给定的字节数组元素长度。</param>
        /// <returns>返回生成的字节数组。</returns>
        public static byte[] GenerateByteArray(int length)
        {
            return RunSecurity(rng =>
            {
                var buffer = new byte[length];
                rng.GetBytes(buffer);

                return buffer;
            });
        }


        /// <summary>
        /// 生成指定个数与单个长度的字符串字典集合。
        /// </summary>
        /// <param name="number">给定要生成的随机字符串个数（如 100 个）。</param>
        /// <param name="length">给定单个随机字符串的长度（可选；默认 8 位长度）。</param>
        /// <param name="encodeFactory">给定单个随机字符串的编码工厂方法（可选；默认使用 MD5 编码）。</param>
        /// <param name="hasSpecial">是否包含部分特殊符号（可选；默认不包含）。</param>
        /// <returns>返回 <see cref="Dictionary{String, String}"/>。</returns>
        public static Dictionary<string, string> GenerateStrings(int number, int length = 8,
            Func<string, string> encodeFactory = null, bool hasSpecial = false)
        {
            if (encodeFactory.IsNull())
                encodeFactory = s => s.Md5Base64String(Encoding.UTF8);

            var pairs = new Dictionary<string, string>();
            var chars = hasSpecial
                ? ExtensionSettings.Preference.AlgorithmChars
                : ExtensionSettings.Preference.AllLettersAndDigits;

            Run(r =>
            {
                var offset = 0;
                for (int j = 0; j < number + offset; j++)
                {
                    var str = string.Empty;
                    for (int i = 0; i < length; i++)
                    {
                        str += chars[r.Next(chars.Length)];
                    }

                    if (str.IsLetter())
                    {
                        offset++;
                        continue; // 如果全是字母则重新生成
                    }

                    if (str.IsDigit())
                    {
                        offset++;
                        continue; // 如果全是数字则重新生成
                    }

                    if (hasSpecial && (!str.HasAlgorithmSpecial() || str.IsAlgorithmSpecial()))
                    {
                        offset++;
                        continue; // 如果没有或全是特殊符号则重新生成
                    }

                    pairs.Add(str, encodeFactory.Invoke(str));
                }
            });

            return pairs;
        }

    }
}
