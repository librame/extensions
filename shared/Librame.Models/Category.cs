using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Librame.Models
{
    using Extensions.Data.Stores;

    [Description("分类")]
    public class Category<TIncremId, TGenId> : AbstractEntityCreation<TIncremId>
        where TIncremId : IEquatable<TIncremId>
        where TGenId : IEquatable<TGenId>
    {
        public string Name { get; set; }

        public IList<Article<TGenId, TIncremId>> Articles { get; set; }
            = new List<Article<TGenId, TIncremId>>();


        public override string ToString()
            => $"{nameof(Name)}={Name},{nameof(Id)}={Id}";
    }
}
