#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Librame.Extensions.Core.Identifiers
{
    using Options;

    /// <summary>
    /// <see cref="IIdentityGenerator{TIdentifier}"/> 工厂。
    /// </summary>
    public class IdentityGeneratorFactory : IIdentityGeneratorFactory
    {
        private List<IObjectIdentityGenerator> _identifierGenerators = null;


        /// <summary>
        /// 构造一个 <see cref="IdentityGeneratorFactory"/>。
        /// </summary>
        /// <param name="options">给定的 <see cref="IdentifierOptions"/>。</param>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public IdentityGeneratorFactory(IdentifierOptions options)
        {
            if (_identifierGenerators.IsNull())
            {
                options.NotNull(nameof(options));

                _identifierGenerators = new List<IObjectIdentityGenerator>();

                _identifierGenerators.Add(options.GuidIdentifierGenerator);
                _identifierGenerators.Add(options.LongIdentifierGenerator);
            }
        }


        /// <summary>
        /// 获取标识生成器。
        /// </summary>
        /// <typeparam name="TId">指定的标识类型。</typeparam>
        /// <returns>返回 <see cref="IIdentityGenerator{TId}"/>。</returns>
        public virtual IIdentityGenerator<TId> GetGenerator<TId>()
            => (IIdentityGenerator<TId>)GetGenerator(typeof(TId));

        /// <summary>
        /// 获取对象标识生成器。
        /// </summary>
        /// <param name="idType">给定的标识类型。</param>
        /// <returns>返回 <see cref="IObjectIdentityGenerator"/>。</returns>
        public virtual IObjectIdentityGenerator GetGenerator(Type idType)
        {
            idType.NotNull(nameof(idType));
            return _identifierGenerators.First(p => p.IdType == idType);
        }

    }
}
