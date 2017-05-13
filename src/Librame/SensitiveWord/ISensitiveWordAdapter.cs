#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.SensitiveWord
{
    /// <summary>
    /// 敏感词适配器接口。
    /// </summary>
    public interface ISensitiveWordAdapter : Adaptation.IAdapter
    {
        /// <summary>
        /// 获取过滤器。
        /// </summary>
        ISensitiveWordsFilter Filter { get; }
    }
}
