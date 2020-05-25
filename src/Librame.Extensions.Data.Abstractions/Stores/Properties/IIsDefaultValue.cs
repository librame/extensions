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
    /// 是默认值接口。
    /// </summary>
    public interface IIsDefaultValue
    {
        /// <summary>
        /// 是默认值。
        /// </summary>
        bool IsDefaultValue { get; set; }
    }
}
