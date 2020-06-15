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

    /// <summary>
    /// 加密构建器。
    /// </summary>
    public class EncryptionBuilder : AbstractExtensionBuilder, IEncryptionBuilder
    {
        /// <summary>
        /// 构造一个 <see cref="EncryptionBuilder"/>。
        /// </summary>
        /// <param name="parentBuilder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="dependency">给定的 <see cref="EncryptionBuilderDependency"/>。</param>
        public EncryptionBuilder(IExtensionBuilder parentBuilder, EncryptionBuilderDependency dependency)
            : base(parentBuilder, dependency)
        {
            Services.AddSingleton<IEncryptionBuilder>(this);

            AddEncryptionServices();
        }


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


        /// <summary>
        /// 获取服务特征。
        /// </summary>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <returns>返回 <see cref="ServiceCharacteristics"/>。</returns>
        public override ServiceCharacteristics GetServiceCharacteristics(Type serviceType)
            => EncryptionBuilderServiceCharacteristicsRegistration.Register.GetOrDefault(serviceType);

    }
}
