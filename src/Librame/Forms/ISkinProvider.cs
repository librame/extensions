#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Forms
{
    using Schemes;

    /// <summary>
    /// 皮肤提供程序接口。
    /// </summary>
    public interface ISkinProvider
    {
        /// <summary>
        /// 获取 <see cref="Settings"/>。
        /// </summary>
        FormsSettings Settings { get; }

        /// <summary>
        /// 获取 <see cref="ISchemeBuilder"/>。
        /// </summary>
        ISchemeBuilder Scheme { get; }


        /// <summary>
        /// 名称。
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 描述。
        /// </summary>
        string Description { get; }
    }
}
