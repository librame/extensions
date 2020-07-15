using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Librame.Models
{
    using Extensions;
    using Extensions.Data.Stores;

    [Description("分类")]
    public class Category<TIncremId, TGenId, TCreatedBy> : AbstractIdentifierEntityCreation<TIncremId, TCreatedBy>
        where TIncremId : IEquatable<TIncremId>
        where TGenId : IEquatable<TGenId>
        where TCreatedBy : IEquatable<TCreatedBy>
    {
        public string Name { get; set; }

        public IList<Article<TGenId, TIncremId, TCreatedBy>> Articles { get; set; }
            = new List<Article<TGenId, TIncremId, TCreatedBy>>();


        public bool Equals(Category<TIncremId, TGenId, TCreatedBy> other)
            => Name == other?.Name;

        public override bool Equals(object obj)
            => obj is Category<TIncremId, TGenId, TCreatedBy> other && Equals(other);


        public override int GetHashCode()
            => ToString().CompatibleGetHashCode();


        public override string ToString()
            => Name;
    }
}
