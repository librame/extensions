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

namespace Librame.SensitiveWord
{
    /// <summary>
    /// 敏感词过滤器接口。
    /// </summary>
    public interface ISensitiveWordsFilter
    {
        /// <summary>
        /// 单词列表。
        /// </summary>
        IList<string> Words { get; }

        /// <summary>
        /// 是否存在敏感词（需要执行过滤方法）。
        /// </summary>
        bool Exists { get; }


        /// <summary>
        /// 过滤内容。
        /// </summary>
        /// <param name="content">给定的要过滤的内容。</param>
        /// <param name="replacement">用于更换的字符串（可选）。</param>
        /// <returns>返回过滤后的字符串。</returns>
        string Filting(string content, string replacement = null);
    }
}
