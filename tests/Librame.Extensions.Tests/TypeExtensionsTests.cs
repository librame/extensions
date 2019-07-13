using System.Collections.Generic;
using Xunit;

namespace Librame.Extensions.Tests
{
    public class TypeExtensionsTests
    {
        class TestClass
        {
            static string _field1;
            string _field2;
            public string Field3 = string.Empty;

            static string Property1 { get; set; }
            string Property2 { get; set; }
            public string Property3 { get; set; }
        }


        [Fact]
        public void GetAllFieldsTest()
        {
            var type = typeof(TestClass);

            var fields = type.GetAllFields();
            Assert.Equal(6, fields.Length);

            fields = type.GetAllFieldsWithoutStatic();
            Assert.Equal(4, fields.Length);
        }


        [Fact]
        public void GetAllPropertiesTest()
        {
            var type = typeof(TestClass);

            var properties = type.GetAllProperties();
            Assert.Equal(3, properties.Length);

            properties = type.GetAllPropertiesWithoutStatic();
            Assert.Equal(2, properties.Length);
        }


        [Fact]
        public void GetQualifiedNameTest()
        {
            var listType = typeof(IList<string>);
            var qualifiedName = listType.GetQualifiedName();
            Assert.Equal("System.Runtime, System.Collections.Generic.IList`1[System.String]", qualifiedName);
        }


        [Fact]
        public void GetBodyNameTest()
        {
            var dictType = typeof(IDictionary<string, IList<string>>);
            var bodyName = dictType.GetBodyName();
            Assert.Equal("IDictionary", bodyName);
        }


        [Fact]
        public void GetStringTest()
        {
            var listType = typeof(IList<string>);
            var dictType = typeof(IDictionary<string, IList<string>>);

            // GetName
            var listTypeName = listType.GetName();
            var dictTypeName = dictType.GetName();
            Assert.Equal("IList`1[String]", listTypeName);
            Assert.Equal("IDictionary`2[String, IList`1[String]]", dictTypeName);

            // GetFullName
            listTypeName = listType.GetFullName();
            dictTypeName = dictType.GetFullName();
            Assert.Equal("System.Collections.Generic.IList`1[System.String]", listTypeName);
            Assert.Equal("System.Collections.Generic.IDictionary`2[System.String, System.Collections.Generic.IList`1[System.String]]", dictTypeName);
        }


        [Fact]
        public void UnwrapNullableTypeTest()
        {
            var type = typeof(bool?).UnwrapNullableType();
            Assert.Equal(typeof(bool), type);
        }

    }
}
