using System;
using System.Text;

namespace Librame.Extensions.Core.Tests
{
    using Serializers;

    [NonRegistered]
    public class TestEncodingStringSerializer : AbstractStringSerializer<Encoding>
    {
        public TestEncodingStringSerializer()
            : base(f => f.AsName(), r => r.FromEncodingName())
        {
        }

    }
}
