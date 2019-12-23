#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Builders
{
    using Core.Builders;

    /// <summary>
    /// 数据构建器依赖选项。
    /// </summary>
    public class DataBuilderDependency : AbstractExtensionBuilderDependency<DataBuilderOptions>
    {
        /// <summary>
        /// 构造一个 <see cref="DataBuilderDependency"/>。
        /// </summary>
        public DataBuilderDependency()
            : base(nameof(DataBuilderDependency))
        {
        }

    }
}
