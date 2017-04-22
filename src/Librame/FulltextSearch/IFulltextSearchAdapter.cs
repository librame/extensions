#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.FulltextSearch
{
    /// <summary>
    /// 全文检索适配器接口。
    /// </summary>
    public interface IFulltextSearchAdapter : Adaptation.IAdapter
    {
        /// <summary>
        /// 索引目录。
        /// </summary>
        string IndexDirectory { get; }


        /// <summary>
        /// 分词。
        /// </summary>
        /// <param name="text">给定的文本。</param>
        /// <returns>返回字词数组。</returns>
        string[] Token(string text);
    }
}
