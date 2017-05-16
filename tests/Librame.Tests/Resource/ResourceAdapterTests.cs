using Microsoft.VisualStudio.TestTools.UnitTesting;
using Librame.Resource;
using Librame.Utility;

namespace Librame.Tests.Resource
{
    [TestClass()]
    public class ResourceAdapterTests
    {
        private static readonly string ResourceFileName =
            TestHelper.DefaultDirectory.AppendPath("TestResource.json");

        public static readonly ResourceSourceInfo TestSourceInfo =
            ResourceHelper.CreateInfo<TestResourceSchema>(ResourceFileName);


        private readonly IResourceProvider _provider = null;

        public ResourceAdapterTests()
        {
            _provider = LibrameArchitecture.Adapters.Resource.GetProvider(TestSourceInfo,
                TestResourceSchema.Default);
        }


        [TestMethod()]
        public void ResourceTest()
        {
            Assert.IsNotNull(_provider.ExistingSchema);
            Assert.IsNotNull(_provider.Serializer);
            Assert.IsNotNull(_provider.Watcher);

            var schema = (_provider.ExistingSchema as TestResourceSchema);

            var rawName = schema.Test.Name;

            // 修改名称
            var mark = " Refresh";
            schema.Test.Name += mark;
            _provider.Save();

            var refreshName = schema.Test.Name;
            Assert.AreEqual(refreshName, rawName + mark, true);

            // 还原名称
            schema.Test.Name = refreshName.TrimEnd(mark);
            _provider.Save();

            Assert.AreEqual(rawName, schema.Test.Name);
        }

    }
}