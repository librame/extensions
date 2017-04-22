#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Data
{
    /// <summary>
    /// 仓库接口。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    public interface IRepository<T> : IRepositoryReader<T>
    {
    }
}
