#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 迁移审计。
    /// </summary>
    [Description("迁移审计")]
    public class MigrationAudit : AbstractId<int>
    {
        /// <summary>
        /// 构造一个 <see cref="MigrationAudit"/> 实例。
        /// </summary>
        public MigrationAudit()
        {
            CreateTime = DateTimeOffset.Now;
        }


        /// <summary>
        /// 命令文本。
        /// </summary>
        [Display(Name = nameof(CommandText), ResourceType = typeof(MigrationAuditResource))]
        public string CommandText { get; set; }

        /// <summary>
        /// 命令哈希。
        /// </summary>
        [Display(Name = nameof(CommandHash), ResourceType = typeof(MigrationAuditResource))]
        public string CommandHash { get; set; }

        /// <summary>
        /// 创建时间。
        /// </summary>
        [Display(Name = nameof(CreateTime), ResourceType = typeof(AbstractEntityResource))]
        [DataType(DataType.DateTime)]
        public virtual DateTimeOffset CreateTime { get; set; }
    }
}
