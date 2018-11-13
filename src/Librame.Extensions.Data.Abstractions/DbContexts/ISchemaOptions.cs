#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 架构选项接口。
    /// </summary>
    public interface IOptions
    {
        /// <summary>
        /// 架构。
        /// </summary>
        string Schema { get; set; }
    }
}
