using System.Collections.Generic;
using System.ComponentModel;

namespace Librame.Models
{
    using Extensions.Data;

    [Description("分类")]
    public class Category : AbstractEntity<int>
    {
        public string Name { get; set; }

        public IList<Article> Articles { get; set; }
            = new List<Article>();
    }
}
