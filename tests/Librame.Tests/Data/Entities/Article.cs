using Librame.Data;
using Librame.Data.Descriptors;
using Librame.Utility;
using System;
using System.ComponentModel;

namespace Librame.Tests.Data.Entities
{
    public class Article : AbstractCreateDataIdDescriptor<int>
    {
        public Article()
            : base()
        {
        }


        [DisplayName("标题")]
        public virtual string Title { get; set; }

        [DisplayName("说明")]
        public virtual string Descr { get; set; }

    }
}
