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

namespace Librame.Extensions.Data
{
    ///// <summary>
    ///// 数据迁移。
    ///// </summary>
    //[Description("数据迁移")]
    //public class DataMigration : AbstractCreation<string>, IEquatable<DataMigration>
    //{
    //    /// <summary>
    //    /// 特性标识。
    //    /// </summary>
    //    public virtual string AttributeId { get; set; }

    //    /// <summary>
    //    /// 产品版本。
    //    /// </summary>
    //    public virtual string ProductVersion { get; set; }

    //    /// <summary>
    //    /// 模型主体。
    //    /// </summary>
    //    public virtual byte[] ModelBody { get; set; }

    //    /// <summary>
    //    /// 模型散列。
    //    /// </summary>
    //    public virtual string ModelHash { get; set; }


    //    /// <summary>
    //    /// 是否相等。
    //    /// </summary>
    //    /// <param name="other">给定的其他 <see cref="DataMigration"/>。</param>
    //    /// <returns>返回布尔值。</returns>
    //    public bool Equals(DataMigration other)
    //    {
    //        if (other.IsNull())
    //            return false;

    //        return AttributeId == other.AttributeId && ProductVersion == other.ProductVersion;
    //    }

    //    /// <summary>
    //    /// 重写是否相等。
    //    /// </summary>
    //    /// <param name="obj">给定要比较的对象。</param>
    //    /// <returns>返回布尔值。</returns>
    //    public override bool Equals(object obj)
    //    {
    //        if (obj is DataMigration other)
    //            return Equals(other);

    //        return false;
    //    }


    //    /// <summary>
    //    /// 获取哈希码。
    //    /// </summary>
    //    /// <returns>返回 32 位整数。</returns>
    //    public override int GetHashCode()
    //        => AttributeId.GetHashCode() ^ ProductVersion.GetHashCode();
    //}
}
