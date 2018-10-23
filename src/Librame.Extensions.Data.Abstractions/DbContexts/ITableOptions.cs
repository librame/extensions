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
    /// 表选项接口。
    /// </summary>
    public interface ITableOptions : ISchemaOptions
    {
        /// <summary>
        /// 名称。
        /// </summary>
        string Name { get; }
    }
}
