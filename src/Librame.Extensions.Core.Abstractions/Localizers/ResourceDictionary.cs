#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections.Concurrent;

namespace Librame.Extensions.Core.Localizers
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
