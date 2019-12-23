using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Builders;

    public class AbstractionBuilderExtensionsTests
    {
        private IExtensionBuilder _builder;


        public AbstractionBuilderExtensionsTests()
        {
            _builder = new ServiceCollection()
                .AddInternal(new InternalBuilderDependency())
                .AddPublic(new PublicBuilderDependency());
        }


        [Fact]
        public void ContainsParentBuilderTest()
        {
            var result = _builder.ContainsParentBuilder<ICoreBuilder>();
            Assert.False(result);
        }


        [Fact]
        public void TryGetParentBuilderTest()
        {
            if (!_builder.TryGetParentBuilder(out PublicTestBuilder @public))
            {
                // PublicTestBuilder is not parent builder
                Assert.Null(@public);
            }

            if (_builder.TryGetParentBuilder(out InternalTestBuilder @internal))
            {
                Assert.NotNull(@internal);
                Assert.True(@internal is InternalTestBuilder);
                Assert.NotNull(@internal.Dependency);
            }

            Assert.NotNull(_builder.Dependency);
        }


        [Fact]
        public void ExportDependenciesTest()
        {
            var provider = _builder.Services.BuildServiceProvider();

            var dependencies = provider.ExportDependencies(_builder.Services);
            Assert.NotEmpty(dependencies);

            //dependencies = provider.ExportDependencies(typeof(InternalTestBuilder));
            //Assert.NotEmpty(dependencies);

            //dependencies = provider.ExportDependencies<PublicTestBuilder>();
            //Assert.NotEmpty(dependencies);

            var json = JsonConvert.SerializeObject(dependencies);
            Assert.NotEmpty(json);
        }
    }


    internal class InternalTestBuilder : AbstractExtensionBuilder, IExtensionBuilder
    {
        public InternalTestBuilder(IServiceCollection services, IExtensionBuilderDependency dependencyOptions)
            : base(services, dependencyOptions)
        {
            Services.AddSingleton(this);
        }
    }

    public class PublicTestBuilder : AbstractExtensionBuilder, IExtensionBuilder
    {
        public PublicTestBuilder(IExtensionBuilder builder, IExtensionBuilderDependency dependencyOptions)
            : base(builder, dependencyOptions)
        {
            Services.AddSingleton(this);
        }
    }

    public class InternalBuilderDependency : AbstractExtensionBuilderDependency<InternalBuilderOptions>
    {
        public InternalBuilderDependency()
            : base(nameof(InternalBuilderDependency))
        {
        }
    }

    public class PublicBuilderDependency : AbstractExtensionBuilderDependency<PublicBuilderOptions>
    {
        public PublicBuilderDependency()
            : base(nameof(PublicBuilderDependency))
        {
        }
    }

    public class InternalBuilderOptions : IExtensionBuilderOptions
    {
        public string TestString { get; set; }
    }

    public class PublicBuilderOptions : IExtensionBuilderOptions
    {
        public string TestString { get; set; }
    }

    public static class TestBuilderExtensions
    {
        public static IExtensionBuilder AddInternal(this IServiceCollection services,
            IExtensionBuilderDependency dependency)
        {
            return new InternalTestBuilder(services, dependency);
        }

        public static IExtensionBuilder AddPublic(this IExtensionBuilder builder,
            IExtensionBuilderDependency dependency)
        {
            return new PublicTestBuilder(builder, dependency);
        }
    }
}
