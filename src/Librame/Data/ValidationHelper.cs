#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Text.RegularExpressions;

namespace Librame.Data
{
    /// <summary>
    /// 数据验证助手。
    /// </summary>
    public class ValidationHelper
    {
        /// <summary>
        /// 是否为邮箱帐户。
        /// </summary>
        /// <param name="email">给定的名称。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsEmail(string email)
        {
            var r = new Regex(@"\w{1,}@\w{1,}\.\w{1,}");
            return r.IsMatch(email);
        }

        /// <summary>
        /// 是否为身份证号码。
        /// </summary>
        /// <param name="number">给定的号码。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsIdCard(string number)
        {
            var r = new Regex(@"(^\d{18}$)|(^\d{15}$)");
            return r.IsMatch(number);
        }

        /// <summary>
        /// 是否为电话（支持座机与手机号码）。
        /// </summary>
        /// <param name="number">给定的号码。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsPhone(string number)
        {
            // 如果不是座机电话
            if (!IsLandlinePhone(number))
                return IsMobilePhone(number);

            return true;
        }
        /// <summary>
        /// 是否为座机电话。
        /// </summary>
        /// <param name="number">给定的号码。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsLandlinePhone(string number)
        {
            var r = new Regex(@"^(\d{3,4}-)?\d{6,8}$");
            return r.IsMatch(number);
        }
        /// <summary>
        /// 是否为移动电话。
        /// </summary>
        /// <param name="number">给定的号码。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsMobilePhone(string number)
        {
            var r = new Regex(@"([1]+[3,5,7]+\d{9})");
            return r.IsMatch(number);
        }

        /// <summary>
        /// 是否为 HTTP 或 HTTPS 格式 URL。
        /// </summary>
        /// <param name="url">给定的 URL。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsHttpOrHttpsUrl(string url)
        {
            var r = new Regex(@"http(s)?://([/w-]+/.)+[/w-]+(/[/w- ./?%&=]*)?");
            return r.IsMatch(url);
        }

        /// <summary>
        /// 是否为 IPv4 地址。
        /// </summary>
        /// <param name="ip">给定的 IP 地址。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsIPv4(string ip)
        {
            string num = "(25[0-5]|2[0-4]//d|[0-1]//d{2}|[1-9]?//d)";
            return Regex.IsMatch(ip, ("^" + num + "//." + num + "//." + num + "//." + num + "$"));
        }


        /// <summary>
        /// 验证数值范围。
        /// </summary>
        /// <param name="number">给定的数值。</param>
        /// <param name="max">给定的最大值。</param>
        /// <param name="min">给定的最小值。</param>
        /// <returns>返回数值。</returns>
        public static int Range(int number, int max, int min)
        {
            // 不超过最大值
            if (number > max)
                return max;

            // 不低于最小值
            if (number < min)
                return min;

            return number;
        }

        /// <summary>
        /// 最大范围值。
        /// </summary>
        /// <param name="number">给定的数值。</param>
        /// <param name="max">给定的最大值。</param>
        /// <returns>返回数值。</returns>
        public static int RangeMax(int number, int max)
        {
            // 不超过最大值
            if (number > max)
                return max;

            return number;
        }

        /// <summary>
        /// 最小范围值。
        /// </summary>
        /// <param name="number">给定的数值。</param>
        /// <param name="min">给定的最小值。</param>
        /// <returns>返回数值。</returns>
        public static int RangeMin(int number, int min)
        {
            // 不低于最小值
            if (number < min)
                return min;

            return number;
        }

    }

    /// <summary>
    /// <see cref="ValidationHelper"/> 静态扩展。
    /// </summary>
    public static class ValidationHelperExtensions
    {
        /// <summary>
        /// 是否为邮箱帐户。
        /// </summary>
        /// <param name="email">给定的名称。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsEmail(this string email)
        {
            return ValidationHelper.IsEmail(email);
        }

        /// <summary>
        /// 是否为身份证号码。
        /// </summary>
        /// <param name="number">给定的号码。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsIdCard(this string number)
        {
            return ValidationHelper.IsIdCard(number);
        }

        /// <summary>
        /// 是否为电话（支持座机与手机号码）。
        /// </summary>
        /// <param name="number">给定的号码。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsPhone(this string number)
        {
            return ValidationHelper.IsPhone(number);
        }
        /// <summary>
        /// 是否为座机电话。
        /// </summary>
        /// <param name="number">给定的号码。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsLandlinePhone(this string number)
        {
            return ValidationHelper.IsLandlinePhone(number);
        }
        /// <summary>
        /// 是否为移动电话。
        /// </summary>
        /// <param name="number">给定的号码。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsMobilePhone(this string number)
        {
            return ValidationHelper.IsMobilePhone(number);
        }

        /// <summary>
        /// 是否为 HTTP 或 HTTPS 格式 URL。
        /// </summary>
        /// <param name="url">给定的 URL。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsHttpOrHttpsUrl(this string url)
        {
            return ValidationHelper.IsHttpOrHttpsUrl(url);
        }

        /// <summary>
        /// 是否为 IPv4 地址。
        /// </summary>
        /// <param name="ip">给定的 IP 地址。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsIPv4(this string ip)
        {
            return ValidationHelper.IsIPv4(ip);
        }


        /// <summary>
        /// 验证数值范围。
        /// </summary>
        /// <param name="number">给定的数值。</param>
        /// <param name="max">给定的最大值。</param>
        /// <param name="min">给定的最小值。</param>
        /// <returns>返回数值。</returns>
        public static int Range(this int number, int max, int min)
        {
            return ValidationHelper.Range(number, max, min);
        }

        /// <summary>
        /// 最大范围值。
        /// </summary>
        /// <param name="number">给定的数值。</param>
        /// <param name="max">给定的最大值。</param>
        /// <returns>返回数值。</returns>
        public static int RangeMax(this int number, int max)
        {
            return ValidationHelper.RangeMax(number, max);
        }

        /// <summary>
        /// 最小范围值。
        /// </summary>
        /// <param name="number">给定的数值。</param>
        /// <param name="min">给定的最小值。</param>
        /// <returns>返回数值。</returns>
        public static int RangeMin(this int number, int min)
        {
            return ValidationHelper.RangeMin(number, min);
        }

    }
}
