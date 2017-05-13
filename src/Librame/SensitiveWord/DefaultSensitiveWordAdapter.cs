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
    using Utility;

    /// <summary>
    /// 默认敏感词适配器。
    /// </summary>
    public class DefaultSensitiveWordAdapter : AbstarctSensitiveWordAdapter, ISensitiveWordAdapter
    {
        /// <summary>
        /// 获取过滤器。
        /// </summary>
        public ISensitiveWordsFilter Filter
        {
            get
            {
                return SingletonManager.Resolve<ISensitiveWordsFilter>(key =>
                {
                    var fileName = AdapterConfigDirectory.AppendPath("filters.txt");

                    return new FileSensitiveWordsFilter(fileName);
                });
            }
        }

    }
}
