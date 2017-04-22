using Librame.Data;
using Librame.Data.Descriptors;
using Librame.Utility;
using System;
using System.ComponentModel;

namespace Librame.UnitTests.Data.Entities
{
    public class Article : AbstractUpdateAndCreateIdDescriptor<int>, IEntityAutomapping
    {
        public Article()
            : base()
        {
        }


        [DisplayName("名称")]
        public virtual string Name { get; set; }

        [DisplayName("说明")]
        public virtual string Descr { get; set; }

    }
}
