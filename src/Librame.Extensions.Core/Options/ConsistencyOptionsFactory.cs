#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Options;
using System.Collections.Generic;
using BaseOptions = Microsoft.Extensions.Options.Options;

namespace Librame.Extensions.Core.Options
{
    /// <summary>
    /// 一致性选项依赖工厂。
    /// </summary>
    /// <typeparam name="TOptions">指定的选项类型。</typeparam>
    public class ConsistencyOptionsFactory<TOptions> : IOptionsFactory<TOptions>
        where TOptions : class, new()
    {
        private readonly IEnumerable<IConfigureOptions<TOptions>> _setups;
        private readonly IEnumerable<IPostConfigureOptions<TOptions>> _postConfigures;
        private readonly IEnumerable<IValidateOptions<TOptions>> _validations;


        /// <summary>
        /// 构造一个 <see cref="ConsistencyOptionsFactory{TOptions}"/>。
        /// </summary>
        /// <param name="setups">给定的配置选项集合。</param>
        /// <param name="postConfigures">给定的后置配置选项集合。</param>
        public ConsistencyOptionsFactory(IEnumerable<IConfigureOptions<TOptions>> setups,
            IEnumerable<IPostConfigureOptions<TOptions>> postConfigures)
            : this(setups, postConfigures, validations: null)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="ConsistencyOptionsFactory{TOptions}"/>。
        /// </summary>
        /// <param name="setups">给定的配置选项集合。</param>
        /// <param name="postConfigures">给定的后置配置选项集合。</param>
        /// <param name="validations">给定的验证选项集合。</param>
        public ConsistencyOptionsFactory(IEnumerable<IConfigureOptions<TOptions>> setups,
            IEnumerable<IPostConfigureOptions<TOptions>> postConfigures,
            IEnumerable<IValidateOptions<TOptions>> validations)
        {
            _setups = setups;
            _postConfigures = postConfigures;
            _validations = validations;
        }


        /// <summary>
        /// 创建选项实例。
        /// </summary>
        public TOptions Create(string name)
        {
            // 利用对象引用是唯一实例的特性，查找已创建的一致性选项实例
            var options = ConsistencyOptionsPool.GetOrAdd<TOptions>();

            foreach (var setup in _setups)
            {
                if (setup is IConfigureNamedOptions<TOptions> namedSetup)
                {
                    namedSetup.Configure(name, options);
                }
                else if (name == BaseOptions.DefaultName)
                {
                    setup.Configure(options);
                }
            }

            foreach (var post in _postConfigures)
            {
                post.PostConfigure(name, options);
            }

            if (_validations != null)
            {
                var failures = new List<string>();
                foreach (var validate in _validations)
                {
                    var result = validate.Validate(name, options);
                    if (result.Failed)
                    {
                        failures.AddRange(result.Failures);
                    }
                }
                if (failures.Count > 0)
                {
                    throw new OptionsValidationException(name, typeof(TOptions), failures);
                }
            }

            return options;
        }

    }
}
