using System;

namespace Librame.Extensions.Tests
{
    public class TestSetProperty : ITestSetProperty
    {
        public string PublicProperty { get; set; }

        public int ProtectedProperty { get; protected set; }

        public bool InternalProperty { get; internal set; }

        public DateTime PrivateProperty { get; private set; }
    }


    public interface ITestSetProperty
    {
        public string PublicProperty { get; set; }

        public int ProtectedProperty { get; }

        public bool InternalProperty { get; }

        public DateTime PrivateProperty { get; }
    }
}
