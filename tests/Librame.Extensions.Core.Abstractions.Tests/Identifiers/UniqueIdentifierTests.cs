using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class UniqueIdentifierTests
    {
        [Fact]
        public void AllTest()
        {
            string str = UniqueIdentifier.Empty;
            Assert.NotEmpty(str);

            var guid = Guid.NewGuid();
            var identifier = new UniqueIdentifier(guid);
            Assert.NotEmpty(identifier.ToShortString());

            var other = new UniqueIdentifier(identifier.ToString(), identifier.Converter);
            Assert.True(identifier == other);
            Assert.False(identifier != other);
            Assert.True(identifier.Equals(other));

            Assert.Equal(guid, other.RawGuid);

            Assert.Equal(identifier.GetHashCode(), other.GetHashCode());
            Assert.Equal(identifier.ToString(), other.ToString());

            var combGuid = identifier.RawGuid.AsCombGuid();
            Assert.False(guid == combGuid);
            Assert.True(guid != combGuid);

            var identifiers = UniqueIdentifier.NewArray(10);
            Assert.NotEmpty(identifiers);

            var shorts = identifiers.Select(id =>
            {
                return new KeyValuePair<Guid, string>(id.RawGuid, id.ToShortString());
            })
            .ToArray();
            Assert.NotEmpty(shorts);

            var combidShorts = identifiers.Select(id =>
            {
                return new KeyValuePair<Guid, string>(id.RawGuid.AsCombGuid(), id.ToShortString());
            })
            .ToArray();
            Assert.NotEmpty(combidShorts);
        }

    }
}
