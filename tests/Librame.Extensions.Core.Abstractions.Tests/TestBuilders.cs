using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Core.Tests
{
    using Builders;
    using Services;

    public class InternalTestBuilder : AbstractExtensionBuilder, IExtensionBuilder
    {
        public InternalTestBuilder(IServiceCollection services, IExtensionBuilderDependency dependency)
            : base(services, dependency)
        {
            Services.AddSingleton(this);
        }

        public override ServiceCharacteristics GetServiceCharacteristics(Type serviceType)
        {
            throw new NotImplementedException();
        }
    }

    public class PublicTestBuilder : AbstractExtensionBuilder, IExtensionBuilder
    {
        public PublicTestBuilder(IExtensionBuilder parentBuilder, IExtensionBuilderDependency dependency)
            : base(parentBuilder, dependency)
        {
            Services.AddSingleton(this);
        }

        public override ServiceCharacteristics GetServiceCharacteristics(Type serviceType)
        {
            throw new NotImplementedException();
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
