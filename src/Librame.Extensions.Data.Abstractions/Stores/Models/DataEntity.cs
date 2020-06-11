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
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Librame.Extensions.Data.Stores
{
    using Resources;

    /// <summary>
    /// 数据实体。
    /// </summary>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    /// <typeparam name="TCreatedBy">指定的创建者类型。</typeparam>
    [Description("数据实体")]
    public class DataEntity<TGenId, TCreatedBy> : AbstractCreation<TGenId, TCreatedBy>, IEquatable<DataEntity<TGenId, TCreatedBy>>
        where TGenId : IEquatable<TGenId>
        where TCreatedBy : IEquatable<TCreatedBy>
    {
        /// <summary>
        /// 架构。
        /// </summary>
        [Display(Name = nameof(Schema), ResourceType = typeof(DataEntityResource))]
        public virtual string Schema { get; set; }

        /// <summary>
        /// 名称。
        /// </summary>
        [Display(Name = nameof(Name), ResourceType = typeof(DataEntityResource))]
        public virtual string Name { get; set; }

        /// <summary>
        /// 是否分表。
        /// </summary>
        [Display(Name = nameof(IsSharding), ResourceType = typeof(DataEntityResource))]
        public virtual bool IsSharding { get; set; }

        /// <summary>
        /// 描述。
        /// </summary>
        [Display(Name = nameof(Name), ResourceType = typeof(DataEntityResource))]
        public virtual string Description { get; set; }

        /// <summary>
        /// 实体类型名。
        /// </summary>
        [Display(Name = nameof(EntityName), ResourceType = typeof(DataEntityResource))]
        public virtual string EntityName { get; set; }

        /// <summary>
        /// 程序集名。
        /// </summary>
        [Display(Name = nameof(AssemblyName), ResourceType = typeof(DataEntityResource))]
        public virtual string AssemblyName { get; set; }


        /// <summary>
        /// 唯一索引是否相等。
        /// </summary>
        /// <param name="other">给定的 <see cref="DataEntity{TGenId, TCreatedBy}"/>。</param>
        /// <returns>返回布尔值。</returns>
        public bool Equals(DataEntity<TGenId, TCreatedBy> other)
            => Schema == other?.Schema && Name == other?.Name;

        /// <summary>
        /// 重写是否相等。
        /// </summary>
        /// <param name="obj">给定要比较的对象。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
            => obj is DataEntity<TGenId, TCreatedBy> other && Equals(other);


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回 32 位整数。</returns>
        public override int GetHashCode()
            => ToString().CompatibleGetHashCode();


        /// <summary>
        /// 转换为字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
            => new TableDescriptor(Name, Schema);
    }
}
