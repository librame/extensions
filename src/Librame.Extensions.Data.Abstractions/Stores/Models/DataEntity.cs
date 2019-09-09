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
    /// 数据实体。
    /// </summary>
    [Description("数据实体")]
    public class DataEntity : AbstractCreation<string>, IEquatable<DataEntity>
    {
        /// <summary>
        /// 架构。
        /// </summary>
        [Display(Name = nameof(Schema), ResourceType = typeof(EntityResource))]
        public virtual string Schema { get; set; }

        /// <summary>
        /// 名称。
        /// </summary>
        [Display(Name = nameof(Name), ResourceType = typeof(EntityResource))]
        public virtual string Name { get; set; }

        /// <summary>
        /// 描述。
        /// </summary>
        [Display(Name = nameof(Name), ResourceType = typeof(EntityResource))]
        public virtual string Description { get; set; }

        /// <summary>
        /// 实体类型名。
        /// </summary>
        [Display(Name = nameof(EntityName), ResourceType = typeof(EntityResource))]
        public virtual string EntityName { get; set; }

        /// <summary>
        /// 程序集名。
        /// </summary>
        [Display(Name = nameof(EntityName), ResourceType = typeof(EntityResource))]
        public virtual string AssemblyName { get; set; }


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="other">给定的其他 <see cref="DataEntity"/>。</param>
        /// <returns>返回布尔值。</returns>
        public bool Equals(DataEntity other)
        {
            if (other.IsNull())
                return false;

            return Schema == other.Schema && Name == other.Name;
        }

        /// <summary>
        /// 重写是否相等。
        /// </summary>
        /// <param name="obj">给定要比较的对象。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
        {
            if (obj is DataEntity other)
                return Equals(other);

            return false;
        }


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回 32 位整数。</returns>
        public override int GetHashCode()
            => Schema.GetHashCode() ^ Name.GetHashCode();
    }
}
