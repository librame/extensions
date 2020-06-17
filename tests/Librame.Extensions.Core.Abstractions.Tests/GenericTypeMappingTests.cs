using System.Linq;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Mediators;

    public class GenericTypeMappingTests
    {
        [Fact]
        public void PopulateArrayTest()
        {
            var baseType = typeof(INotificationHandler<>);
            var implType = typeof(INotificationHandler<PingNotification>);

            var parameters = GenericTypeMapping.PopulateArray(baseType, implType);
            
            Assert.Single(parameters);
            Assert.Equal("TNotification", parameters[0].ParameterName);
            Assert.Equal(nameof(PingNotification), parameters[0].ArgumentName);


            baseType = typeof(IRequestHandler<,>);
            implType = typeof(IRequestHandler<PingRequest, Pong>);

            parameters = GenericTypeMapping.PopulateArray(baseType, implType);

            Assert.Equal(2, parameters.Length);
            Assert.Equal("TRequest", parameters[0].ParameterName);
            Assert.Equal("TResponse", parameters[1].ParameterName);
            Assert.Equal(nameof(PingRequest), parameters[0].ArgumentName);
            Assert.Equal(nameof(Pong), parameters[1].ArgumentName);
        }

        [Fact]
        public void PopulateDictionaryTest()
        {
            var baseType = typeof(INotificationHandler<>);
            var implType = typeof(INotificationHandler<PingNotification>);

            var parameters = GenericTypeMapping.PopulateDictionary(baseType, implType);

            Assert.Single(parameters);
            Assert.Equal("TNotification", parameters.Keys.First());
            Assert.Equal(nameof(PingNotification), parameters.Values.First().ArgumentName);


            baseType = typeof(IRequestHandler<,>);
            implType = typeof(IRequestHandler<PingRequest, Pong>);

            parameters = GenericTypeMapping.PopulateDictionary(baseType, implType);

            Assert.Equal(2, parameters.Count);
            Assert.Equal("TRequest", parameters.Keys.First());
            Assert.Equal("TResponse", parameters.Keys.Last());
            Assert.Equal(nameof(PingRequest), parameters.Values.First().ArgumentName);
            Assert.Equal(nameof(Pong), parameters.Values.Last().ArgumentName);
        }

    }
}
