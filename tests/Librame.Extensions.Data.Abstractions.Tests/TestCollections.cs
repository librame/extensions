using System;

namespace Librame.Extensions.Data.Tests
{
    using Stores;

    public class TestTreeing : AbstractParentIdentifier<int>
    {
        public string Name { get; set; }
    }

    public class TestPaging
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
