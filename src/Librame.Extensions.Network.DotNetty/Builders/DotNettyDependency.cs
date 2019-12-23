#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Network.Builders
{
    using Core.Builders;

    /// <summary>
    /// DotNetty 依赖。
    /// </summary>
    public class DotNettyDependency : AbstractExtensionBuilderDependency<DotNettyOptions>
    {
        /// <summary>
        /// 构造一个 <see cref="DotNettyDependency"/>。
        /// </summary>
        public DotNettyDependency()
            : base(nameof(DotNettyDependency))
        {
        }

    }
}
