using System;
using System.ComponentModel;

namespace Librame.Models
{
    using Extensions;
    using Extensions.Data;
    using Extensions.Data.Stores;

    [Description("文章")]
    [Shardable]
    public class Article<TGenId, TIncremId, TCreatedBy> : AbstractIdentifierEntityCreation<TGenId, TCreatedBy>,
        IEquatable<Article<TGenId, TIncremId, TCreatedBy>>
        where TIncremId : IEquatable<TIncremId>
        where TGenId : IEquatable<TGenId>
        where TCreatedBy : IEquatable<TCreatedBy>
    {
        public string Title { get; set; }

        public string Descr { get; set; }

        public int CategoryId { get; set; }

        public Category<TIncremId, TGenId, TCreatedBy> Category { get; set; }


        public bool Equals(Article<TGenId, TIncremId, TCreatedBy> other)
            => Title == other?.Title;

        public override bool Equals(object obj)
            => obj is Article<TGenId, TIncremId, TCreatedBy> other && Equals(other);


        public override int GetHashCode()
            => ToString().CompatibleGetHashCode();


        public override string ToString()
            => Title;
    }
}
