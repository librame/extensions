using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace Librame.Extensions.Core.Tests
{
    internal class InternalTestBuilder : AbstractExtensionBuilder, IExtensionBuilder
    {
        public InternalTestBuilder(IServiceCollection services, IExtensionBuilderDependencyOptions dependencyOptions)
            : base(services, dependencyOptions)
        {
            Services.AddSingleton(this);
        }
    }

    public class PublicTestBuilder : AbstractExtensionBuilder, IExtensionBuilder
    {
        public PublicTestBuilder(IExtensionBuilder builder, IExtensionBuilderDependencyOptions dependencyOptions)
            : base(builder, dependencyOptions)
        {
            Services.AddSingleton(this);
        }
    }

    public class TestBuilderDependencyOptions : AbstractOptionsConfigurator, IExtensionBuilderDependencyOptions
    {
        public TestBuilderDependencyOptions()
            : base()
        {
        }

        public string OptionsName { get; set; }

        public override Type OptionsType { get; }

        public string BaseDirectory { get; set; }
            = Environment.CurrentDirectory;
    }

    public static class TestBuilderExtensions
    {
        public static IExtensionBuilder AddTest(this IServiceCollection services,
            IExtensionBuilderDependencyOptions dependencyOptions)
        {
            return new InternalTestBuilder(services, dependencyOptions);
        }

        public static IExtensionBuilder AddChild(this IExtensionBuilder builder,
            IExtensionBuilderDependencyOptions dependencyOptions)
        {
            return new PublicTestBuilder(builder, dependencyOptions);
        }
    }


    public class AbstractionExtensionBuilderExtensionsTests
    {
        [Fact]
        public void AllTest()
        {
            var dependency = new TestBuilderDependencyOptions();
            var builder = new ServiceCollection()
                .AddTest(dependency)
                .AddChild(dependency);

            if (!builder.TryGetParentBuilder(out PublicTestBuilder child))
            {
                // PublicTestBuilder is not parent builder
                Assert.Null(child);
            }

            if (builder.TryGetParentBuilder(out InternalTestBuilder parent))
            {
                Assert.NotNull(parent);
                Assert.True(parent is InternalTestBuilder);
                Assert.NotNull(parent.DependencyOptions);
            }

            Assert.NotNull(builder.DependencyOptions);
        }
    }
}
