﻿#region License

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
    /// 行版本接口。
    /// </summary>
    public interface IRowVersion
    {
        /// <summary>
        /// 行版本。
        /// </summary>
        byte[] RowVersion { get; set; }
    }
}
