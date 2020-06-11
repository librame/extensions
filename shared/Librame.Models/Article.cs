using System;
using System.ComponentModel;

namespace Librame.Models
{
    using Extensions.Data;
    using Extensions.Data.Stores;

    [Description("文章")]
    [Shardable]
    public class Article<TGenId, TIncremId, TCreatedBy> : AbstractIdentifierEntityCreation<TGenId, TCreatedBy>
        where TIncremId : IEquatable<TIncremId>
        where TGenId : IEquatable<TGenId>
        where TCreatedBy : IEquatable<TCreatedBy>
    {
        public string Title { get; set; }

        public string Descr { get; set; }

        public int CategoryId { get; set; }

        public Category<TIncremId, TGenId, TCreatedBy> Category { get; set; }


        public override string ToString()
            => $"{nameof(Title)}={Title},{nameof(Id)}={Id},{nameof(CategoryId)}={CategoryId}";
    }
}
