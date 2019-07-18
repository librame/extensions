using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    internal class InternalTestBuilder : AbstractExtensionBuilder, IExtensionBuilder
    {
        public InternalTestBuilder(IServiceCollection services)
            : base(services)
        {
            Services.AddSingleton(this);
        }
    }

    public class PublicTestBuilder : AbstractExtensionBuilder, IExtensionBuilder
    {
        public PublicTestBuilder(IExtensionBuilder builder)
            : base(builder)
        {
            Services.AddSingleton(this);
        }
    }

    public static class TestBuilderExtensions
    {
        public static IExtensionBuilder AddTest(this IServiceCollection services)
        {
            return new InternalTestBuilder(services);
        }

        public static IExtensionBuilder AddChild(this IExtensionBuilder builder)
        {
            return new PublicTestBuilder(builder);
        }
    }


    public class AbstractionExtensionBuilderExtensionsTests
    {
        [Fact]
        public void AllTest()
        {
            var builder = new ServiceCollection()
                .AddTest()
                .AddChild();

            if (!builder.TryGetParentBuilder(out PublicTestBuilder child))
            {
                // PublicTestBuilder is not parent builder
                Assert.Null(child);
            }

            if (builder.TryGetParentBuilder(out InternalTestBuilder parent))
            {
                Assert.NotNull(parent);
                Assert.True(parent is InternalTestBuilder);
            }
        }
    }
}
