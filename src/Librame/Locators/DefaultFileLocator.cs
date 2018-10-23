#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Locators
{
    /// <summary>
    /// 默认文件定位器。
    /// </summary>
    public class DefaultFileLocator : AbstractFileLocator, IFileLocator
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractFileLocator"/> 实例。
        /// </summary>
        /// <param name="fileName">给定的文件名。</param>
        public DefaultFileLocator(string fileName)
            : base(fileName)
        {
        }

    }
}
