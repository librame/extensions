#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 转换器接口。
    /// </summary>
    /// <typeparam name="TFrom">指定的还原类型。</typeparam>
    /// <typeparam name="TTo">指定的转换类型。</typeparam>
    public interface IConverter<TFrom, TTo>
    {
        /// <summary>
        /// 还原类型。
        /// </summary>
        /// <param name="to">给定的 <typeparamref name="TTo"/>。</param>
        /// <returns>返回 <typeparamref name="TFrom"/>。</returns>
        TFrom From(TTo to);

        /// <summary>
        /// 转换类型。
        /// </summary>
        /// <param name="from">给定的 <typeparamref name="TFrom"/>。</param>
        /// <returns>返回 <typeparamref name="TTo"/>。</returns>
        TTo To(TFrom from);
    }
}
