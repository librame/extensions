using System;
using System.ComponentModel;

namespace Librame.Models
{
    using Extensions;
    using Extensions.Core.Identifiers;
    using Extensions.Data.Stores;

    [Description("分类")]
    public class Category<TIncremId, TCreatedBy> : AbstractEntityCreation<TIncremId, TCreatedBy>,
        IIncrementalIdentifier<TIncremId>,
        IEquatable<Category<TIncremId, TCreatedBy>>
        where TIncremId : IEquatable<TIncremId>
        where TCreatedBy : IEquatable<TCreatedBy>
    {
        public string Name { get; set; }


        // 除主键外的唯一索引相等比较（参见实体映射的唯一索引配置）。
        public bool Equals(Category<TIncremId, TCreatedBy> other)
            => Name == other?.Name;

        public override bool Equals(object obj)
            => obj is Category<TIncremId, TCreatedBy> other && Equals(other);


        public override int GetHashCode()
            => Name.CompatibleGetHashCode();


        public override string ToString()
            => $"{base.ToString()};{nameof(Name)}={Name}";
    }
}
