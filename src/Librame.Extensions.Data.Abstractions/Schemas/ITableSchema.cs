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
    /// <summary>
    /// 表架构接口。
    /// </summary>
    public interface ITableSchema : ISchema
    {
        /// <summary>
        /// 名称。
        /// </summary>
        string Name { get; }
    }
}
