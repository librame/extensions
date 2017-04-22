#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Resource.Schema
{
    /// <summary>
    /// 抽象结构部分。
    /// </summary>
    public abstract class AbstractSchemaSection
    {
        /// <summary>
        /// 部分名称。
        /// </summary>
        protected abstract string SectionName { get; }
    }
}
