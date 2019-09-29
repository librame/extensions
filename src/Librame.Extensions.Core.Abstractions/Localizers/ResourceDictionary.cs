#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections.Concurrent;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 资源字典。
    /// </summary>
    public class ResourceDictionary : ConcurrentDictionary<string, object>, IResourceDictionary
    {
        /// <summary>
        /// 构造一个 <see cref="ResourceDictionary"/>。
        /// </summary>
        public ResourceDictionary()
            : base()
        {
        }
    }
}
