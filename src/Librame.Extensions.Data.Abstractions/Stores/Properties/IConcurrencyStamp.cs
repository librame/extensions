#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Stores
{
    /// <summary>
    /// 并发标记接口。
    /// </summary>
    public interface IConcurrencyStamp
    {
        /// <summary>
        /// 并发标记。
        /// </summary>
        string ConcurrencyStamp { get; set; }
    }
}
