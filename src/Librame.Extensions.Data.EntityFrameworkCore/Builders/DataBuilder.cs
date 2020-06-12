#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Data.Builders
{
    using Core.Builders;
    using Core.Services;
    using Data.Accessors;
    using Data.Mediators;
    using Data.Protectors;
    using Data.Stores;
    using Data.ValueGenerators;

    internal class DataBuilder : AbstractExtensionBuilder, IDataBuilder
    {
        public DataBuilder(IExtensionBuilder parentBuilder, DataBuilderDependency dependency)
            : base(parentBuilder, dependency)
        {
            Services.AddSingleton<IDataBuilder>(this);

            AddInternalServices();
        }


        public Type DatabaseDesignTimeType { get; internal set; }

        public AccessorGenericTypeArguments GenericTypeArguments { get; internal set; }


        public override ServiceCharacteristics GetServiceCharacteristics(Type serviceType)
            => DataBuilderServiceCharacteristicsRegistration.Register.GetOrDefault(serviceType);


        private void AddInternalServices()
        {
            // Mediators
            AddService(typeof(AuditNotificationHandler<,>));
            AddService(typeof(EntityNotificationHandler<>));
            AddService(typeof(MigrationNotificationHandler<>));

            // Protectors
            AddService<IPrivacyDataProtector, PrivacyDataProtector>();

            // Stores
            AddStoreIdentifierGenerator<GuidDataStoreIdentifierGenerator>();

            // ValueGenerators
            AddDefaultValueGenerator<GuidDefaultValueGenerator>();
        }


        public AccessorGenericTypeArguments FindGenericTypeArguments<TAccessor>()
            => FindGenericTypeArguments(typeof(TAccessor));

        public AccessorGenericTypeArguments FindGenericTypeArguments(Type accessorType)
            => AccessorDataBuilderExtensions.GenericTypeArguments[accessorType];


        public IDataBuilder AddGenericServiceByPopulateGenericTypeArguments(Type serviceType,
            Type implementationTypeDefinition,
            Func<Type, AccessorGenericTypeArguments, Type> populateServiceFactory = null,
            Func<Type, AccessorGenericTypeArguments, Type> populateImplementationFactory = null,
            bool addEnumerable = false, AccessorGenericTypeArguments genericTypeArguments = null)
        {
            if (false == implementationTypeDefinition?.IsGenericTypeDefinition)
                throw new NotSupportedException($"The implementation type '{implementationTypeDefinition}' only support generic type definition.");

            if (!implementationTypeDefinition.IsImplementedInterface(serviceType, out var resultType))
                throw new InvalidOperationException($"The type '{implementationTypeDefinition}' does not implement '{serviceType}' interface.");

            var characteristics = GetServiceCharacteristics(serviceType);
            if (serviceType.IsGenericTypeDefinition)
                serviceType = PopulateGenericTypeArguments(serviceType, populateServiceFactory);

            var implementationType = PopulateGenericTypeArguments(implementationTypeDefinition, populateImplementationFactory);

            // 如果不添加为可枚举集合
            if (!addEnumerable)
                Services.TryReplaceAll(serviceType, implementationType, throwIfNotFound: false);

            Services.AddByCharacteristics(serviceType, implementationType, characteristics);
            return this;

            // PopulateGenericTypeArguments
            Type PopulateGenericTypeArguments(Type populateType,
                Func<Type, AccessorGenericTypeArguments, Type> populateFactory = null)
            {
                if (populateFactory.IsNull())
                {
                    populateFactory = (type, args) => type.MakeGenericType(
                        args.AuditType,
                        args.AuditPropertyType,
                        args.EntityType,
                        args.MigrationType,
                        args.TenantType,
                        args.GenIdType,
                        args.IncremIdType,
                        args.CreatedByType);
                }

                genericTypeArguments = genericTypeArguments ?? GenericTypeArguments;
                if (genericTypeArguments.IsNull())
                    throw new InvalidOperationException("Registration builder.AddAccessor().");

                return populateFactory.Invoke(populateType, genericTypeArguments);
            }
        }


        public IDataBuilder AddDefaultValueGenerator<TGenerator>()
            where TGenerator : IValueGeneratorIndication
            => AddDefaultValueGenerator(typeof(TGenerator));

        public IDataBuilder AddDefaultValueGenerator(Type generatorType)
        {
            AddGenericService(typeof(IDefaultValueGenerator<>), generatorType);
            return this;
        }


        public IDataBuilder AddStoreIdentifierGenerator<TGenerator>()
            where TGenerator : class, IStoreIdentifierGeneratorIndication
            => AddStoreIdentifierGenerator(typeof(TGenerator));

        public IDataBuilder AddStoreIdentifierGenerator(Type generatorType)
        {
            AddGenericService(typeof(IStoreIdentifierGenerator<>), generatorType);
            return this;
        }

    }
}
