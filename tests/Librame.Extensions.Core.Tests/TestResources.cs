using System;

namespace Librame.Extensions.Core.Tests
{
    using Localizers;
    using Resources;

    public class TestResource : IResource
    {
        public string Name { get; set; }
    }

    public class TestResource_en_US : ResourceDictionary
    {
        public TestResource_en_US()
            : base()
        {
            var name = DictionaryStringLocalizerTests.DefaultNames[0];
            AddOrUpdate("Name", name, (key, value) => name);
        }
    }

    public class TestResource_zh_CN : ResourceDictionary
    {
        public TestResource_zh_CN()
            : base()
        {
            var name = DictionaryStringLocalizerTests.DefaultNames[1];
            AddOrUpdate("Name", name, (key, value) => name);
        }
    }

    public class TestResource_zh_TW : ResourceDictionary
    {
        public TestResource_zh_TW()
            : base()
        {
            var name = DictionaryStringLocalizerTests.DefaultNames[2];
            AddOrUpdate("Name", name, (key, value) => name);
        }
    }
}
