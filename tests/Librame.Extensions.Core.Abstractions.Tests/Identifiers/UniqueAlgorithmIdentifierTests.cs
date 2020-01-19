using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Identifiers;
    using Serializers;

    public class UniqueAlgorithmIdentifierTests
    {
        [Fact]
        public void AllTest()
        {
            string str = UniqueAlgorithmIdentifier.Empty;
            Assert.NotEmpty(str);

            var guid = Guid.NewGuid();
            var identifier = new UniqueAlgorithmIdentifier(SerializableObjectHelper.CreateHexString(guid.ToByteArray()));
            Assert.NotEmpty((string)identifier);

            var other = new UniqueAlgorithmIdentifier(identifier.ReadOnlyMemory);
            Assert.True(identifier == other);
            Assert.False(identifier != other);
            Assert.True(identifier.Equals(other));

            Assert.Equal(guid, other.RawGuid);

            Assert.Equal(identifier.GetHashCode(), other.GetHashCode());
            Assert.Equal(identifier.ToString(), other.ToString());

            // CombGuid
            var timestamp = DateTimeOffset.UtcNow;
            var combGuid = identifier.RawGuid.AsCombGuid(timestamp);
            Assert.False(guid == combGuid);
            Assert.True(guid != combGuid);

            var identifiers = UniqueAlgorithmIdentifier.NewArray(10);
            Assert.NotEmpty(identifiers);

            var shorts = identifiers.Select(id =>
            {
                return new KeyValuePair<Guid, string>(id.RawGuid, id.ToShortString(timestamp));
            })
            .ToArray();
            Assert.NotEmpty(shorts);

            var combidShorts = identifiers.Select(id =>
            {
                return new KeyValuePair<Guid, string>(id.RawGuid.AsCombGuid(timestamp), id.ToShortString(timestamp));
            })
            .ToArray();
            Assert.NotEmpty(combidShorts);
        }

    }
}
