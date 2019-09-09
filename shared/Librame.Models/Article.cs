using System.ComponentModel;

namespace Librame.Models
{
    using Extensions.Data;

    [Description("文章")]
    [TableNameSchema(IsSharding = true)]
    public class Article : AbstractEntity<string>
    {
        public string Title { get; set; }

        public string Descr { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }
}
