using System;
using System.ComponentModel;

namespace Librame.Models
{
    using Extensions.Data;
    using Extensions.Data.Stores;

    [Description("文章")]
    [Sharding]
    public class Article<TGenId, TIncremId> : AbstractEntityCreation<TGenId>
        where TIncremId : IEquatable<TIncremId>
        where TGenId : IEquatable<TGenId>
    {
        public string Title { get; set; }

        public string Descr { get; set; }

        public int CategoryId { get; set; }

        public Category<TIncremId, TGenId> Category { get; set; }


        public override string ToString()
            => $"{nameof(Title)}={Title},{nameof(Id)}={Id},{nameof(CategoryId)}={CategoryId}";
    }
}
