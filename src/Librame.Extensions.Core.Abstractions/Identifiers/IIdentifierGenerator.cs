#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core.Identifiers
{
    using Services;

    /// <summary>
    /// 标识符生成器接口。
    /// </summary>
    /// <typeparam name="TIdentifier">标识符类型。</typeparam>
    public interface IIdentifierGenerator<TIdentifier>
    {
        /// <summary>
        /// 生成标识符。
        /// </summary>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <returns>返回 <typeparamref name="TIdentifier"/>。</returns>
        TIdentifier Generate(IClockService clock);
    }
}
