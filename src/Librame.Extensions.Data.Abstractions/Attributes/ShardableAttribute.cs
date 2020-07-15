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

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 可分表特性（如果在实体中标记此特性，则表示在模型迁移中，当检测到表映射的名称发生差异时，默认视为分表迁移操作）。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class ShardableAttribute : Attribute
    {
    }
}
