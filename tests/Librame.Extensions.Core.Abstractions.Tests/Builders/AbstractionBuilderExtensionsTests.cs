using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    using Builders;
    using Combiners;

    public class AbstractionBuilderExtensionsTests
    {
        private IExtensionBuilder _builder;


        public AbstractionBuilderExtensionsTests()
        {
            _builder = new ServiceCollection()
                .AddInternal(new InternalBuilderDependency())
                .AddPublic();
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
        public void ContainsParentDependencyTest()
        {
            var result = _builder.Dependency.ContainsParentDependency<PublicBuilderDependency>();
            Assert.False(result);

            result = _builder.Dependency.ContainsParentDependency<InternalBuilderDependency>();
            Assert.True(result);
        }

        [Fact]
        public void TryGetParentDependencyTest()
        {
            Assert.NotNull(_builder.Dependency);

            if (!_builder.Dependency.TryGetParentDependency(out PublicBuilderDependency @public))
            {
                // PublicTestDependency is not parent builder
                Assert.Null(@public);
            }

            if (_builder.Dependency.TryGetParentDependency(out InternalBuilderDependency @internal))
            {
                Assert.NotNull(@internal);
            }
        }


        [Fact]
        public void EnumerateDependenciesTest()
        {
            // Last Dependency
            var dependencies = _builder.Dependency.EnumerateDependencies();
            Assert.NotEmpty(dependencies);

            var provider = _builder.Services.BuildServiceProvider();
            var publicDependency = provider.GetRequiredService<PublicBuilderDependency>();

            var currentDependencies = publicDependency.EnumerateDependencies();
            Assert.Equal(dependencies.Count, currentDependencies.Count);

            var internalDependency = provider.GetRequiredService<InternalBuilderDependency>();
            currentDependencies = internalDependency.EnumerateDependencies();
            Assert.True(dependencies.Count > currentDependencies.Count);

            var filePath = $"{nameof(EnumerateDependenciesTest)}.json"
                .AsFilePathCombiner(publicDependency.ReportDirectory);
            filePath.WriteJson(dependencies);
        }
    }


    internal class InternalTestBuilder : AbstractExtensionBuilder, IExtensionBuilder
    {
        public InternalTestBuilder(IServiceCollection services, IExtensionBuilderDependency dependency)
            : base(services, dependency)
        {
            Services.AddSingleton(this);
        }
    }

    public class PublicTestBuilder : AbstractExtensionBuilder, IExtensionBuilder
    {
        public PublicTestBuilder(IExtensionBuilder parentBuilder, IExtensionBuilderDependency dependency)
            : base(parentBuilder, dependency)
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
        public PublicBuilderDependency(IExtensionBuilderDependency parentDependency)
            : base(nameof(PublicBuilderDependency), parentDependency)
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
            InternalBuilderDependency dependency)
        {
            var builder = new InternalTestBuilder(services, dependency);

            services.AddSingleton(builder);
            services.AddSingleton(dependency);

            return builder;
        }

        public static IExtensionBuilder AddPublic(this IExtensionBuilder parentBuilder)
        {
            var dependency = new PublicBuilderDependency(parentBuilder.Dependency);
            var builder = new PublicTestBuilder(parentBuilder, dependency);

            parentBuilder.Services.AddSingleton(builder);
            parentBuilder.Services.AddSingleton(dependency);

            return builder;
        }

    }
}
