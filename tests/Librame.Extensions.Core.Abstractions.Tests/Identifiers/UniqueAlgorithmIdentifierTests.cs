using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class UniqueAlgorithmIdentifierTests
    {
        [Fact]
        public void AllTest()
        {
            string str = UniqueAlgorithmIdentifier.Empty;
            Assert.NotEmpty(str);

            var guid = Guid.NewGuid();
            var identifier = new UniqueAlgorithmIdentifier(guid, HexAlgorithmConverter.Default);
            Assert.NotEmpty((string)identifier);

            var other = new UniqueAlgorithmIdentifier(identifier, identifier.Converter);
            Assert.True(identifier == other);
            Assert.False(identifier != other);
            Assert.True(identifier.Equals(other));

            Assert.Equal(guid, other.RawGuid);

            Assert.Equal(identifier.GetHashCode(), other.GetHashCode());
            Assert.Equal(identifier.ToString(), other.ToString());

            // Serialization
            var buffer = identifier.SerializeBinary().Compress();
            var base64String = buffer.AsBase64String();
            Assert.NotEmpty(base64String);

            buffer = base64String.FromBase64String().Decompress();
            var obj = buffer.DeserializeBinary();
            Assert.True(identifier.Equals(obj));

            // CombGuid
            var timestamp = DateTimeOffset.UtcNow;
            var combGuid = identifier.RawGuid.AsCombGuid(timestamp);
            Assert.False(guid == combGuid);
            Assert.True(guid != combGuid);

            var identifiers = UniqueAlgorithmIdentifier.NewArray(10, identifier.Converter);
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
