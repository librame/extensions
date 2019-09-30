#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data
{
    using Core;

    /// <summary>
    /// 迁移访问器截面接口（通常用于后置保存变化操作）。
    /// </summary>
    public interface IMigrateAccessorAspect : IAccessorAspect
    {
        /// <summary>
        /// 时钟服务。
        /// </summary>
        IClockService Clock { get; }

        /// <summary>
        /// 标识符。
        /// </summary>
        IStoreIdentifier Identifier { get; }
    }
}
