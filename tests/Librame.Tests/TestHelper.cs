using Librame.Utility;
using System.IO;

namespace Librame.Tests
{
    public static class TestHelper
    {
        public static readonly string DefaultDirectory = null;

        static TestHelper()
        {
            if (ReferenceEquals(DefaultDirectory, null))
            {
                DefaultDirectory = PathUtility.BaseDirectory.AppendDirectoryName("_test");
            }
        }

    }
}
