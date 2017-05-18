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
using System.Drawing;
using System.Web.Caching;

namespace Librame.Utility
{
    /// <summary>
    /// <see cref="Cache"/> 实用工具。
    /// </summary>
    public static class CacheUtility
    {
        private const string CAPTCHA_CHARS = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string CAPTCHA_KEY = LibrameAssemblyConstants.NAME + "Captcha";


        /// <summary>
        /// 验证图片验证码。
        /// </summary>
        /// <param name="cache">给定的缓存。</param>
        /// <param name="compareCaptcha">给定用于对比的验证码。</param>
        /// <returns>返回是否通过验证的布尔值。</returns>
        public static bool ValidateCaptcha(this Cache cache, string compareCaptcha)
        {
            var captcha = (string)cache[CAPTCHA_KEY];

            return (captcha == compareCaptcha);
        }


        /// <summary>
        /// 生成图片验证码。
        /// </summary>
        /// <param name="cache">给定的缓存。</param>
        /// <param name="length">给定的验证码长度（可选；默认 5 位）。</param>
        /// <param name="captchaFactory">给定的验证码工厂方法（可选）。</param>
        /// <returns>返回图像。</returns>
        public static Image GenerateCaptcha(this Cache cache, int length = 5, Func<int, string> captchaFactory = null)
        {
            if (ReferenceEquals(captchaFactory, null))
                captchaFactory = GenerateCaptcha;

            // 调用工厂方法生成验证码
            var captcha = captchaFactory.Invoke(length);

            // 缓存验证码
            cache[CAPTCHA_KEY] = captcha;

            // 绘制图像
            return ImageUtility.Captcha(captcha, new Point(3, 3));
        }
        private static string GenerateCaptcha(int length)
        {
            var chars = CAPTCHA_CHARS.ToCharArray();
            var captcha = string.Empty;

            var r = new Random();
            for (int i = 0; i < length; i++)
            {
                captcha += chars[r.Next(chars.Length)];
            }

            return captcha;
        }

    }
}
