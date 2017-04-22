#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Authorization.Descriptors
{
    using Data;

    /// <summary>
    /// 令牌描述符接口。
    /// </summary>
    public interface ITokenDescriptor : IEntityAutomapping
    {
        /// <summary>
        /// 名称。
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 票根。
        /// </summary>
        string Ticket { get; }
    }
}
