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
    /// 租户存储接口。
    /// </summary>
    /// <typeparam name="TTenant">指定的租户类型。</typeparam>
    public interface ITenantStore<TTenant> : IStore
        where TTenant : class
    {
    }
}
