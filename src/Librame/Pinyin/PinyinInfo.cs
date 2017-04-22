#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections.Generic;
using System.Text;

namespace Librame.Pinyin
{
    /// <summary>
    /// 拼音信息。
    /// </summary>
    public class PinyinInfo
    {
        /// <summary>
        /// 汉语名称。
        /// </summary>
        public string ChineseName { get; }

        /// <summary>
        /// 构造一个 <see cref="PinyinInfo"/> 实例。
        /// </summary>
        public PinyinInfo(string chineseName)
        {
            ChineseName = chineseName;

            AllWords = new Dictionary<int, string>();
        }


        /// <summary>
        /// 所有解析的索引字词集合。
        /// </summary>
        public IDictionary<int, string> AllWords { get; set; }

        /// <summary>
        /// 首字母缩略词。
        /// </summary>
        public string Acronym { get; set; }

        /// <summary>
        /// 初始首字母。
        /// </summary>
        public string Initial { get; set; }

        /// <summary>
        /// 名称（全拼）。
        /// </summary>
        public string Name
        {
            get { return ToString(); }
        }
        

        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="obj">给定的 <see cref="PinyinInfo"/>。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is PinyinInfo))
                return false;

            return (Name == (obj as PinyinInfo).Name);
        }

        /// <summary>
        /// 获取哈希值。
        /// </summary>
        /// <returns>返回哈希代码。</returns>
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        /// <summary>
        /// 转换为字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            int i = 0;
            foreach (var n in AllWords)
            {
                // 获取拼音值
                sb.Append(n.Value);

                if (i != AllWords.Count - 1)
                    sb.Append(" ");

                i++;
            }

            return sb.ToString();
        }

    }
}
