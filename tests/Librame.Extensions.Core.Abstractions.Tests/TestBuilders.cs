using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Core.Tests
{
    using Builders;
    using Services;

    public class InternalBuilder : AbstractExtensionBuilder, IExtensionBuilder
    {
        public InternalBuilder(IServiceCollection services, IExtensionBuilderDependency dependency)
            : base(services, dependency)
        {
            Services.AddSingleton(this);
        }

        public override ServiceCharacteristics GetServiceCharacteristics(Type serviceType)
        {
            throw new NotImplementedException();
        }
    }

    public class PublicBuilder : AbstractExtensionBuilder, IExtensionBuilder
    {
        public PublicBuilder(IExtensionBuilder parentBuilder, IExtensionBuilderDependency dependency)
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
            var builder = new InternalBuilder(services, dependency);

            services.AddSingleton(builder);
            services.AddSingleton(dependency);

            return builder;
        }

        public static IExtensionBuilder AddPublic(this IExtensionBuilder parentBuilder)
        {
            var dependency = new PublicBuilderDependency(parentBuilder.Dependency);
            var builder = new PublicBuilder(parentBuilder, dependency);

            parentBuilder.Services.AddSingleton(builder);
            parentBuilder.Services.AddSingleton(dependency);

            return builder;
        }

    }
}
