using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Librame.Models
{
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


        public override string ToString()
            => $"{nameof(Name)}={Name},{nameof(Id)}={Id}";
    }
}
