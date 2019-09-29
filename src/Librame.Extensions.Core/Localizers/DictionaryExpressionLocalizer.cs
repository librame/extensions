#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 字典表达式定位器。
    /// </summary>
    /// <typeparam name="TResource">指定的资源类型。</typeparam>
    public class DictionaryExpressionLocalizer<TResource> : ExpressionLocalizer<TResource>, IDictionaryExpressionLocalizer<TResource>
        where TResource : class
    {
        /// <summary>
        /// 构造一个 <see cref="ExpressionLocalizer{TResource}"/>。
        /// </summary>
        /// <param name="factory">给定的 <see cref="IDictionaryStringLocalizerFactory"/>。</param>
        public DictionaryExpressionLocalizer(IDictionaryStringLocalizerFactory factory)
            : base(factory)
        {
        }
    }
}
