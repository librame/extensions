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


        //[Fact]
        //public void CreateOrDefaultTest()
        //{
        //    var i = (int)typeof(int).CreateOrDefault();
        //    Assert.Equal(default, i);

        //    var obj = (TestClass1)typeof(TestClass1).CreateOrDefault();
        //    Assert.NotNull(obj);
        //}


        [Fact]
        public void UnwrapNullableTypeTest()
        {
            var type = typeof(bool?).UnwrapNullableType();
            Assert.Equal(typeof(bool), type);
        }

    }
}
