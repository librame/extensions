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
    /// 选项动作配置器。
    /// </summary>
    public interface IOptionsActionConfigurator : IOptionsConfigurator
    {
        /// <summary>
        /// 自动配置动作。
        /// </summary>
        bool AutoConfigureAction { get; set; }

        /// <summary>
        /// 自动后置配置动作。
        /// </summary>
        bool AutoPostConfigureAction { get; set; }
    }
}
