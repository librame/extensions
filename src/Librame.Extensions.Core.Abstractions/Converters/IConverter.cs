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
    /// <typeparam name="TInput">指定的输入类型。</typeparam>
    /// <typeparam name="TOutput">指定的输出类型。</typeparam>
    public interface IConverter<TInput, TOutput>
    {
        /// <summary>
        /// 转换为输入类型实例。
        /// </summary>
        /// <param name="output">给定的 <typeparamref name="TOutput"/>。</param>
        /// <returns>返回 <typeparamref name="TInput"/>。</returns>
        TInput From(TOutput output);

        /// <summary>
        /// 转换为输出类型实例。
        /// </summary>
        /// <param name="input">给定的 <typeparamref name="TInput"/>。</param>
        /// <returns>返回 <typeparamref name="TOutput"/>。</returns>
        TOutput To(TInput input);
    }
}
