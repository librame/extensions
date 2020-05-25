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

namespace Librame.Extensions.Encryption.Builders
{
    using Core.Builders;
    using Core.Services;
    using Encryption.Generators;
    using Encryption.Services;

    internal class EncryptionBuilder : AbstractExtensionBuilder, IEncryptionBuilder
    {
        public EncryptionBuilder(IExtensionBuilder parentBuilder, EncryptionBuilderDependency dependency)
            : base(parentBuilder, dependency)
        {
            Services.AddSingleton<IEncryptionBuilder>(this);

            AddEncryptionServices();
        }


        public override ServiceCharacteristics GetServiceCharacteristics(Type serviceType)
            => EncryptionBuilderServiceCharacteristicsRegistration.Register.GetOrDefault(serviceType);


        private void AddEncryptionServices()
        {
            // Generators
            AddService<IKeyGenerator, KeyGenerator>();
            AddService<IVectorGenerator, VectorGenerator>();

            // Services
            AddService<IHashService, HashService>();
            AddService<IKeyedHashService, KeyedHashService>();
            AddService<IRsaService, RsaService>();
            AddService<ISymmetricService, SymmetricService>();
        }

    }
}
