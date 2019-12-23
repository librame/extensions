#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
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
