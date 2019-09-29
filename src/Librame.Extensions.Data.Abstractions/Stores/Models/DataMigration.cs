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
using System.Linq.Expressions;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 数据迁移。
    /// </summary>
    [Description("数据迁移")]
    public class DataMigration : AbstractCreation<string>, IEquatable<DataMigration>
    {
        /// <summary>
        /// 访问器类型名。
        /// </summary>
        [Display(Name = nameof(AccessorName), ResourceType = typeof(DataMigrationResource))]
        public virtual string AccessorName { get; set; }

        /// <summary>
        /// 模型快照类型名。
        /// </summary>
        [Display(Name = nameof(ModelSnapshotName), ResourceType = typeof(DataMigrationResource))]
        public virtual string ModelSnapshotName { get; set; }

        /// <summary>
        /// 模型散列。
        /// </summary>
        [Display(Name = nameof(ModelHash), ResourceType = typeof(DataMigrationResource))]
        public virtual string ModelHash { get; set; }

        /// <summary>
        /// 模型主体。
        /// </summary>
        [Display(Name = nameof(ModelBody), ResourceType = typeof(DataMigrationResource))]
        public virtual byte[] ModelBody { get; set; }


        /// <summary>
        /// 唯一索引是否相等。
        /// </summary>
        /// <param name="other">给定的其他 <see cref="DataMigration"/>。</param>
        /// <returns>返回布尔值。</returns>
        public bool Equals(DataMigration other)
            => ModelHash == other?.ModelHash;

        /// <summary>
        /// 重写是否相等。
        /// </summary>
        /// <param name="obj">给定要比较的对象。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
            => (obj is DataMigration other) ? Equals(other) : false;


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回 32 位整数。</returns>
        public override int GetHashCode()
            => ToString().GetHashCode();


        /// <summary>
        /// 转换为字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
            => ModelHash;


        /// <summary>
        /// 获取唯一索引表达式。
        /// </summary>
        /// <typeparam name="TMigration">指定的迁移类型。</typeparam>
        /// <param name="modelHash">给定的模型哈希。</param>
        /// <returns>返回查询表达式。</returns>
        public static Expression<Func<TMigration, bool>> GetUniqueIndexExpression<TMigration>(string modelHash)
            where TMigration : DataMigration
        {
            modelHash.NotEmpty(nameof(modelHash));

            return p => p.ModelHash == modelHash;
        }
    }
}
