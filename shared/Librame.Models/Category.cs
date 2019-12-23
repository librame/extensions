using System.Collections.Generic;
using System.ComponentModel;

namespace Librame.Models
{
    using Extensions.Data.Stores;

    [Description("分类")]
    public class Category : AbstractEntityCreation<int>
    {
        public string Name { get; set; }

        public IList<Article> Articles { get; set; }
            = new List<Article>();


        public override string ToString()
        {
            return $"{nameof(Name)}={Name},{nameof(Id)}={Id},{nameof(CreatedBy)}={CreatedBy},{nameof(CreatedTime)}={CreatedTime}";
        }
    }
}
