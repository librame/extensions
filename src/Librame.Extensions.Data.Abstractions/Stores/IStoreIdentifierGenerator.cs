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
    using Core.Services;

    /// <summary>
    /// 存储标识符生成器接口。
    /// </summary>
    public interface IStoreIdentifierGenerator : IService
    {
        /// <summary>
        /// 时钟服务。
        /// </summary>
        IClockService Clock { get; }
    }
}
