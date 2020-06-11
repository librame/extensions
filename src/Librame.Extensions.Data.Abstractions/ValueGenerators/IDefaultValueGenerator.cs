#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.ValueGenerators
{
    /// <summary>
    /// 默认值生成器接口。
    /// </summary>
    /// <typeparam name="TValue">指定的存储值类型。</typeparam>
    public interface IDefaultValueGenerator<TValue> : IValueGenerator<TValue>
    {
    }
}
