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

namespace Librame.Extensions.Data.Builders
{
    using Core.Identifiers;
    using Data.Stores;

    /// <summary>
    /// 数据构建器选项。
    /// </summary>
    public class DataBuilderOptions : DataBuilderOptionsBase
    {
        /// <summary>
        /// 构造一个 <see cref="DataBuilderOptions"/>。
        /// </summary>
        public DataBuilderOptions()
            : base(new DataTenant<Guid>()) // 使用 GUID 作为生成式标识类型
        {
        }


        /// <summary>
        /// 使用 COMB 标识符生成器方案（默认使用符合 SQL Server 排序规则的有序 <see cref="Guid"/> 标识）。
        /// </summary>
        public IIdentifierGenerator<Guid> IdentifierGenerator { get; set; }
            = CombIdentifierGenerator.SQLServer;
    }
}
