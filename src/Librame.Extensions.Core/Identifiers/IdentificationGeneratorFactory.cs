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
    /// <see cref="IIdentificationGenerator{TId}"/> 工厂。
    /// </summary>
    public class IdentificationGeneratorFactory : IIdentificationGeneratorFactory
    {
        private List<IObjectIdentificationGenerator> _generators = null;


        /// <summary>
        /// 构造一个 <see cref="IdentificationGeneratorFactory"/>。
        /// </summary>
        /// <param name="options">给定的 <see cref="IdentifierOptions"/>。</param>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public IdentificationGeneratorFactory(IdentifierOptions options)
        {
            if (_generators.IsNull())
            {
                options.NotNull(nameof(options));

                _generators = new List<IObjectIdentificationGenerator>();

                foreach (var property in typeof(IdentifierOptions).GetProperties())
                {
                    var value = property.GetValue(options);
                    if (value.IsNotNull() && value is IObjectIdentificationGenerator generator)
                        _generators.Add(generator);
                }
            }
        }


        /// <summary>
        /// 获取标识生成器。
        /// </summary>
        /// <typeparam name="TId">指定的标识类型。</typeparam>
        /// <returns>返回 <see cref="IIdentificationGenerator{TId}"/>。</returns>
        public virtual IIdentificationGenerator<TId> GetIdGenerator<TId>()
            => (IIdentificationGenerator<TId>)GetIdGenerator(typeof(TId));

        /// <summary>
        /// 获取对象标识生成器。
        /// </summary>
        /// <param name="idType">给定的标识类型。</param>
        /// <returns>返回 <see cref="IObjectIdentificationGenerator"/>。</returns>
        public virtual IObjectIdentificationGenerator GetIdGenerator(Type idType)
        {
            idType.NotNull(nameof(idType));
            return _generators.First(p => p.IdType == idType);
        }

    }
}
