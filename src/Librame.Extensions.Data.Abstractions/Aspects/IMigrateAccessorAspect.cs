#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Aspects
{
    /// <summary>
    /// 迁移访问器截面接口（通常用于后置保存变化操作）。
    /// </summary>
    public interface IMigrateAccessorAspect : IAccessorAspect
    {
        /// <summary>
        /// 需要保存更改。
        /// </summary>
        bool RequiredSaveChanges { get; set; }
    }
}
