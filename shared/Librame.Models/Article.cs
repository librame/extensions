using System;
using System.ComponentModel;

namespace Librame.Models
{
    using Extensions;
    using Extensions.Core.Identifiers;
    using Extensions.Data;
    using Extensions.Data.Stores;

    [Description("文章")]
    [Shardable]
    public class Article<TGenId, TCategoryId, TCreatedBy> : AbstractIdentifierEntityCreation<TGenId, TCreatedBy>,
        IGenerativeIdentifier<TGenId>,
        IEquatable<Article<TGenId, TCategoryId, TCreatedBy>>
        where TCategoryId : IEquatable<TCategoryId>
        where TGenId : IEquatable<TGenId>
        where TCreatedBy : IEquatable<TCreatedBy>
    {
        public TCategoryId CategoryId { get; set; }

        public string Title { get; set; }

        public string Descr { get; set; }


        // 除主键外的唯一索引相等比较（参见实体映射的唯一索引配置）。
        public bool Equals(Article<TGenId, TCategoryId, TCreatedBy> other)
        {
            if (other.IsNull())
                return false;

            return CategoryId.Equals(other.CategoryId) && Title == other.Title;
        }

        public override bool Equals(object obj)
            => obj is Article<TGenId, TCategoryId, TCreatedBy> other && Equals(other);


        public override int GetHashCode()
            => CategoryId.GetHashCode() ^ Title.CompatibleGetHashCode();


        public override string ToString()
            => $"{base.ToString()};{nameof(CategoryId)}={CategoryId};{nameof(Title)}={Title}";
    }
}
