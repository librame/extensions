using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Librame.Extensions.Tests
{
    public class TypeExtensionsTests
    {
        [Fact]
        public void IsIntegerTypeTest()
        {
            var type = typeof(int);
            Assert.True(type.IsIntegerType());

            type = typeof(long?);
            Assert.True(type.IsIntegerType());

            type = typeof(sbyte?);
            Assert.True(type.IsIntegerType());

            type = typeof(Guid?);
            Assert.False(type.IsIntegerType());
        }


        [Fact]
        public void IsNullableTypeTest()
        {
            Assert.True(typeof(bool?).IsNullableType());
            Assert.False(typeof(bool).IsNullableType());
        }


        [Fact]
        public void IsAssignableFromTargetTypeTest()
        {
            var baseType = typeof(ITestA<string>);
            var testType = typeof(TestClass);
            var testSubType = typeof(TestSubClass);

            Assert.True(baseType.IsAssignableFromTargetType(testType));
            Assert.True(baseType.IsAssignableFromTargetType(testSubType));
            Assert.True(testType.IsAssignableFromTargetType(testSubType));

            Assert.True(testType.IsAssignableToBaseType(baseType));
            Assert.True(testSubType.IsAssignableToBaseType(baseType));
            Assert.False(testType.IsAssignableToBaseType(testSubType));
        }


        [Fact]
        public void GetBaseTypesTest()
        {
            var type = typeof(TestSubClass);
            var baseTypes = type.GetBaseTypes();
            Assert.NotEmpty(baseTypes);
        }


        [Fact]
        public void UnwrapNullableTypeTest()
        {
            var type = typeof(bool?).UnwrapNullableType();
            Assert.Equal(typeof(bool), type);
        }


        #region GetDisplayName

        [Fact]
        public void GetGenericBodyNameTest()
        {
            var dictType = typeof(IDictionary<string, IList<string>>);
            var bodyName = dictType.GetGenericBodyName();
            Assert.Equal("IDictionary", bodyName);
        }

        [Fact]
        public void GetDisplayNameTest()
        {
            var listType = typeof(IList<string>);
            var dictType = typeof(IDictionary<string, IList<string>>);

            // GetSimpleName
            var listTypeName = listType.GetDisplayName();
            var dictTypeName = dictType.GetDisplayName();
            Assert.Equal("IList`1[String]", listTypeName);
            Assert.Equal("IDictionary`2[String, IList`1[String]]", dictTypeName);

            // GetFullName
            listTypeName = listType.GetDisplayNameWithNamespace();
            dictTypeName = dictType.GetDisplayNameWithNamespace();
            Assert.Equal("System.Collections.Generic.IList`1[System.String]", listTypeName);
            Assert.Equal("System.Collections.Generic.IDictionary`2[System.String, System.Collections.Generic.IList`1[System.String]]", dictTypeName);
        }

        [Fact]
        public void GetAssemblyQualifiedNameWithoutVersionTest()
        {
            var listType = typeof(IList<string>);
            var qualifiedName = listType.GetAssemblyQualifiedNameWithoutVersion();

            var targetQualifiedName = $"System.Collections.Generic.IList`1[System.String], {listType.Assembly.GetDisplayName()}";
            Assert.Equal(targetQualifiedName, qualifiedName);
        }

        #endregion


        #region GetMemberInfos

        [Fact]
        public void GetAllFieldsTest()
        {
            var type = typeof(TestClass);

            var fields = type.GetAllFields();
            Assert.Equal(5, fields.Length);

            fields = type.GetAllFieldsWithoutStatic();
            Assert.Equal(3, fields.Length);
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

        #endregion


        [Fact]
        public void InvokeTypesTest()
        {
            var count = typeof(TypeExtensionsTests).Assembly.InvokeTypes(type =>
            {
                Assert.NotNull(type);
            });
            Assert.True(count > 0);
        }


        [Fact]
        public void PropertyValuesEqualsTest()
        {
            var source = new TestClass();
            var compare = ObjectExtensions.EnsureCreate<TestClass>();

            Assert.True(source.PropertyValuesEquals(compare));
            Assert.True(source.YieldEnumerable().SequencePropertyValuesEquals(compare.YieldEnumerable()));
        }


        [Fact]
        public void SetPropertyTest()
        {
            var tc = new TestSetProperty();

            tc.SetProperty(p => p.PublicProperty, "123");
            Assert.Equal("123", tc.PublicProperty);

            tc.SetProperty(p => p.ProtectedProperty, 1);
            Assert.Equal(1, tc.ProtectedProperty);

            tc.SetProperty(p => p.InternalProperty, true);
            Assert.True(tc.InternalProperty);

            var now = DateTime.Now;
            tc.SetProperty(p => p.PrivateProperty, now);
            Assert.Equal(now, tc.PrivateProperty);


            ITestSetProperty ti = new TestSetProperty();

            ti.SetProperty(p => p.PublicProperty, "123");
            Assert.Equal("123", tc.PublicProperty);

            ti.SetProperty(p => p.ProtectedProperty, 1);
            Assert.Equal(1, tc.ProtectedProperty);

            ti.SetProperty(p => p.InternalProperty, true);
            Assert.True(tc.InternalProperty);

            ti.SetProperty(p => p.PrivateProperty, now);
            Assert.Equal(now, tc.PrivateProperty);
        }


        [Fact]
        public void IsImplementedInterfaceTypeTest()
        {
            Assert.True(typeof(TestClass).IsImplementedInterfaceType<ITestB>());
            Assert.True(typeof(ITestB).IsImplementedInterfaceType<ITestA<string>>());

            Assert.True(typeof(TestClass).IsImplementedInterfaceType(typeof(ITestA<>), out Type resultType));
            Assert.True(resultType.GetGenericArguments().Single() == typeof(string));
        }


        [Fact]
        public void IsImplementedBaseTypeTest()
        {
            Assert.True(typeof(TestSubClass).IsImplementedBaseType<TestClass>());
            Assert.ThrowsAny<NotSupportedException>(() =>
            {
                typeof(TestSubClass).IsImplementedBaseType<ITestB>();
            });

            Assert.True(typeof(TestSubClassReference).IsImplementedBaseType(typeof(TestClass<>), out Type resultType));
            Assert.True(resultType.GetGenericArguments().Single() == typeof(string));
        }

    }
}
