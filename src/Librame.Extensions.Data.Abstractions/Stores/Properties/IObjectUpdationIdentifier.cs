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
    using Core.Identifiers;

    /// <summary>
    /// 对象更新标识符接口。
    /// </summary>
    public interface IObjectUpdationIdentifier : IObjectUpdation, IObjectIdentifier
    {
    }
}
