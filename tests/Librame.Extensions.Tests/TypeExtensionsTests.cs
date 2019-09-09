using System.Collections.Generic;
using Xunit;

namespace Librame.Extensions.Tests
{
    public class TypeExtensionsTests
    {
        class TestClass
        {
            static string _field1 = null;
            string _field2 = null;
            public string Field3 = string.Empty;

            static string Property1 { get; set; }
            string Property2 { get; set; }
            public string Property3 { get; set; }

            public void TestMethod()
            {
                Assert.Null(_field1);
                Assert.Null(_field2);
            }
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
        public void GetBodyNameTest()
        {
            var dictType = typeof(IDictionary<string, IList<string>>);
            var bodyName = dictType.GetBodyName();
            Assert.Equal("IDictionary", bodyName);
        }


        [Fact]
        public void GetSimpleAssemblyQualifiedNameTest()
        {
            var listType = typeof(IList<string>);
            var qualifiedName = listType.GetSimpleAssemblyQualifiedName();
            Assert.Equal("System.Collections.Generic.IList`1[System.String], System.Private.CoreLib", qualifiedName);
        }


        [Fact]
        public void GetSimpleNameTest()
        {
            var listType = typeof(IList<string>);
            var dictType = typeof(IDictionary<string, IList<string>>);

            // GetSimpleName
            var listTypeName = listType.GetSimpleName();
            var dictTypeName = dictType.GetSimpleName();
            Assert.Equal("IList`1[String]", listTypeName);
            Assert.Equal("IDictionary`2[String, IList`1[String]]", dictTypeName);

            // GetFullName
            listTypeName = listType.GetSimpleFullName();
            dictTypeName = dictType.GetSimpleFullName();
            Assert.Equal("System.Collections.Generic.IList`1[System.String]", listTypeName);
            Assert.Equal("System.Collections.Generic.IDictionary`2[System.String, System.Collections.Generic.IList`1[System.String]]", dictTypeName);
        }


        [Fact]
        public void UnwrapNullableTypeTest()
        {
            var type = typeof(bool?).UnwrapNullableType();
            Assert.Equal(typeof(bool), type);
        }


        [Fact]
        public void InvokeTypesTest()
        {
            var count = typeof(TypeExtensionsTests).Assembly.InvokeTypes(type =>
            {
                Assert.NotNull(type);
            });

            Assert.True(count > 0);
        }

    }
}
