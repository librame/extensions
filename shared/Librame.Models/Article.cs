using System.ComponentModel;

namespace Librame.Models
{
    using Extensions.Data;
    using Extensions.Data.Stores;

    [Description("文章")]
    [ShardingTable]
    public class Article : AbstractEntityCreation<string>
    {
        public string Title { get; set; }

        public string Descr { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }


        public override string ToString()
        {
            return $"{nameof(Title)}={Title},{nameof(Id)}={Id},{nameof(CreatedBy)}={CreatedBy},{nameof(CreatedTime)}={CreatedTime}";
        }
    }
}
