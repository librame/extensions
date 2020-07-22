#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core.Builders
{
    using Options;

    /// <summary>
    /// 抽象核心构建器选项。
    /// </summary>
    /// <typeparam name="TIdentifierOptions">指定的标识符选项类型。</typeparam>
    public abstract class AbstractCoreBuilderOptions<TIdentifierOptions> : IExtensionBuilderOptions
        where TIdentifierOptions : IdentifierOptions, new()
    {
        /// <summary>
        /// 标识符选项。
        /// </summary>
        public TIdentifierOptions Identifier { get; }
            = new TIdentifierOptions();
    }
}
