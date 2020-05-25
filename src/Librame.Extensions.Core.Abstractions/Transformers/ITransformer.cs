#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;

namespace Librame.Extensions.Core.Transformers
{
    /// <summary>
    /// 变换器接口。
    /// </summary>
    public interface ITransformer
    {
        /// <summary>
        /// 来源类型。
        /// </summary>
        Type SourceType { get; }

        /// <summary>
        /// 目标类型。
        /// </summary>
        Type TargetType { get; }
    }
}
