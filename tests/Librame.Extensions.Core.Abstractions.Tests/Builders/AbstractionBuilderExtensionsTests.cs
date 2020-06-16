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
        public void ContainsBuilderTest()
        {
            var result = _builder.ContainsBuilder<PublicBuilder>(excludeCurrentBuilder: true);
            Assert.False(result);

            result = _builder.ContainsBuilder<PublicBuilder>();
            Assert.True(result);

            result = _builder.ContainsBuilder<InternalBuilder>();
            Assert.True(result);
        }

        [Fact]
        public void TryGetBuilderTest()
        {
            if (!_builder.TryGetBuilder(out PublicBuilder @public, excludeCurrentBuilder: true))
            {
                // PublicBuilder is not parent builder
                Assert.Null(@public);
            }

            if (_builder.TryGetBuilder(out InternalBuilder @internal))
            {
                Assert.NotNull(@internal);
            }
        }

        [Fact]
        public void EnumerateBuildersTest()
        {
            var builders = _builder.EnumerateBuilders();
            Assert.NotEmpty(builders);

            var provider = _builder.Services.BuildServiceProvider();
            var publicBuilder = provider.GetRequiredService<PublicBuilder>();

            var currentBuilders = publicBuilder.EnumerateBuilders();
            Assert.Equal(builders.Count, currentBuilders.Count);

            var internalBuilder = provider.GetRequiredService<InternalBuilder>();
            currentBuilders = internalBuilder.EnumerateBuilders();
            Assert.True(builders.Count > currentBuilders.Count);

            var filePath = $"{nameof(EnumerateBuildersTest)}.json"
                .AsFilePathCombiner(publicBuilder.Dependency.ReportDirectory);
            filePath.WriteJson(builders);
        }


        [Fact]
        public void ContainsDependencyTest()
        {
            var result = _builder.Dependency.ContainsDependency<PublicBuilderDependency>(excludeCurrentDependency: true);
            Assert.False(result);

            result = _builder.Dependency.ContainsDependency<PublicBuilderDependency>();
            Assert.True(result);

            result = _builder.Dependency.ContainsDependency<InternalBuilderDependency>();
            Assert.True(result);
        }

        [Fact]
        public void TryGetDependencyTest()
        {
            Assert.NotNull(_builder.Dependency);

            if (!_builder.Dependency.TryGetDependency(out PublicBuilderDependency @public, excludeCurrentDependency: true))
            {
                // PublicBuilderDependency is not parent builder dependency
                Assert.Null(@public);
            }

            if (_builder.Dependency.TryGetDependency(out InternalBuilderDependency @internal))
            {
                Assert.NotNull(@internal);
            }
        }

        [Fact]
        public void EnumerateDependenciesTest()
        {
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

}
