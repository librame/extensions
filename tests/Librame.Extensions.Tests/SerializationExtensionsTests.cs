using System;
using System.IO;
using Xunit;

namespace Librame.Extensions.Tests
{
    public class SerializationExtensionsTests
    {
        [Serializable]
        public class TestSerialization : IEquatable<TestSerialization>
        {
            [NonSerialized]
            private static readonly string _staticReadonlyString
                = nameof(_staticReadonlyString);


            public TestSerialization(string none)
            {
                None = none;
            }


            public DateTimeOffset UtcNow { get; set; }
                = DateTimeOffset.UtcNow;

            public string None;


            public bool Equals(TestSerialization other)
                => UtcNow == other?.UtcNow && None == other?.None;

            public override bool Equals(object obj)
                => obj is TestSerialization other && Equals(other);

            public override int GetHashCode()
                => UtcNow.GetHashCode() ^ None.GetHashCode();
        }


        [Fact]
        public void AllTest()
        {
            var test = new TestSerialization(string.Empty);

            // Byte[]
            var buffer = test.SerializeBinary();
            Assert.NotEmpty(buffer);

            var obj = buffer.DeserializeBinary();
            Assert.Equal(test, obj);

            // File
            var fileFullName = Path.Combine(Path.GetTempPath(), test.UtcNow.ToFileTime() + ".txt");
            test.SerializeBinary(fileFullName);
            Assert.True(File.Exists(fileFullName));

            obj = fileFullName.DeserializeBinary();
            Assert.Equal(test, obj);
            File.Delete(fileFullName);
        }

    }
}
