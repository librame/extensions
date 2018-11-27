#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Services
{
    using Builders;

    /// <summary>
    /// 服务接口。
    /// </summary>
    /// <typeparam name="TBuilderOptions">指定的构建器选项类型。</typeparam>
    public interface IService<TBuilderOptions> : IService
        where TBuilderOptions : class, IBuilderOptions, new()
    {
        /// <summary>
        /// 构建器选项。
        /// </summary>
        TBuilderOptions BuilderOptions { get; }
    }


    /// <summary>
    /// 服务接口。
    /// </summary>
    public interface IService
    {
    }
}
