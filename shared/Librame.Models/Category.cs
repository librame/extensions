using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Librame.Models
{
    using Extensions.Data;

    [Description("分类")]
    public class Category : AbstractEntity<int, DateTime, DataStatus>
    {
        public Category()
        {
            CreateTime = UpdateTime = DateTime.Now;
            DataStatus = DataStatus.Public;
        }


        public string Name { get; set; }

        public IList<Article> Articles { get; set; }
            = new List<Article>();
    }
}
