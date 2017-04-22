using Librame.Resource.Schema;
using System.Xml.Serialization;

namespace Librame.UnitTests.Resource
{
    [XmlRoot("Resource")]
    public class TestResourceSchema : ResourceSchema
    {
        internal static readonly TestResourceSchema Default = new TestResourceSchema(new TestSchemaSection("Test Resource"));


        public TestResourceSchema()
            : base()
        {
            Test = new TestSchemaSection();
        }
        public TestResourceSchema(TestSchemaSection section)
            : base()
        {
            Test = section;
        }


        public TestSchemaSection Test { get; set; }
    }

    public class TestSchemaSection : AbstractSchemaSection
    {
        public TestSchemaSection()
            : base()
        {
        }
        public TestSchemaSection(string name)
            : base()
        {
            Name = name;
        }

        protected override string SectionName
        {
            get { return "Test"; }
        }


        public string Name { get; set; }
    }
}
