#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Resources
{
    using Core.Resources;

    /// <summary>
    /// 数据实体资源。
    /// </summary>
    public class DataEntityResource : IResource
    {
        /// <summary>
        /// 架构。
        /// </summary>
        public string Schema { get; set; }

        /// <summary>
        /// 名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否分表。
        /// </summary>
        public string IsSharding { get; set; }

        /// <summary>
        /// 描述。
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 实体类型名。
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// 程序集名。
        /// </summary>
        public string AssemblyName { get; set; }
    }
}
