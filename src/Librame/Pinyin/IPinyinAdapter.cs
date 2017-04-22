#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Pinyin
{
    /// <summary>
    /// 拼音适配器接口。
    /// </summary>
    public interface IPinyinAdapter
    {
        /// <summary>
        /// 解析汉字为拼音。
        /// </summary>
        /// <param name="str">给定的汉字。</param> 
        /// <returns>返回 <see cref="PinyinInfo"/>。</returns> 
        PinyinInfo Parse(string str);
    }
}
