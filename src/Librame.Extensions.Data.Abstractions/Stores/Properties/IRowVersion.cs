#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Data.Stores
{
    /// <summary>
    /// 行版本接口。
    /// </summary>
    public interface IRowVersion
    {
        /// <summary>
        /// 行版本。
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        byte[] RowVersion { get; set; }
    }
}
