#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.International.Converters.PinYinConverter;

namespace Librame.Pinyin
{
    /// <summary>
    /// 默认拼音适配器。
    /// </summary>
    public class DefaultPinyinAdapter : AbstractPinyinAdapter, IPinyinAdapter
    {
        /// <summary>
        /// 解析汉字为拼音。
        /// </summary>
        /// <param name="str">给定的汉字。</param> 
        /// <returns>返回 <see cref="PinyinInfo"/>。</returns> 
        public PinyinInfo Parse(string str)
        {
            if (string.IsNullOrEmpty(str))
                return null;

            var info = new PinyinInfo(str);

            for (var i = 0; i < str.Length; i++)
            {
                var ch = str[i];

                if (ChineseChar.IsValidChar(ch))
                {
                    // 将每个中文字符转拼音
                    var pinyin = ToPinyin(ch);

                    // 首字母大写
                    var first = pinyin.Substring(0, 1).ToUpper();
                    // 其余字母小写
                    var end = pinyin.Substring(1).ToLower();

                    // 绑定所有拼音单词
                    info.AllWords.Add(i, string.Format("{0}{1}", first, end));

                    // 绑定第一个汉字的首字母
                    if (i == 0) info.Initial = first;

                    // 绑定每个汉字的首字母
                    if (ReferenceEquals(info.Acronym, null))
                        info.Acronym = first;
                    else
                        info.Acronym += first;
                }
                else
                {
                    info.AllWords.Add(i, ch.ToString());
                }
            }

            return info;
        }


        /// <summary>
        /// 将中文字符转换为拼音字符串。
        /// </summary>
        /// <param name="ch">给定的中文字符。</param>
        /// <returns>返回拼音字符串。</returns>
        protected virtual string ToPinyin(char ch)
        {
            string pinyin = string.Empty;

            try
            {
                var chineseChar = new ChineseChar(ch);
                string str = chineseChar.Pinyins[0].ToString();
                pinyin += str.Substring(0, str.Length - 1);
            }
            catch
            {
                pinyin += ch.ToString();
            }

            return pinyin;
        }

    }
}
