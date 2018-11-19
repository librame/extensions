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

namespace Librame.Options
{
    /// <summary>
    /// 增强型选项工厂。
    /// </summary>
    public class EnhancedOptionsFactory<TOptions> : IOptionsFactory<TOptions>
        where TOptions : class, new()
    {
        private readonly IEnumerable<IConfigureOptions<TOptions>> _setups;
        private readonly IEnumerable<IPostConfigureOptions<TOptions>> _postConfigures;


        /// <summary>
        /// 构造一个 <see cref="EnhancedOptionsFactory{TOptions}"/> 实例。
        /// </summary>
        /// <param name="setups">给定的配置选项集合。</param>
        /// <param name="postConfigures">给定的后置配置选项集合。</param>
        public EnhancedOptionsFactory(IEnumerable<IConfigureOptions<TOptions>> setups, IEnumerable<IPostConfigureOptions<TOptions>> postConfigures)
        {
            _setups = setups;
            _postConfigures = postConfigures;
        }


        /// <summary>
        /// 创建选项。
        /// </summary>
        /// <param name="name">给定的配置名称。</param>
        /// <returns>返回选项实例。</returns>
        public TOptions Create(string name)
        {
            var options = new TOptions();

            foreach (var setup in _setups)
            {
                if (setup is IConfigureNamedOptions<TOptions> namedSetup)
                {
                    namedSetup.Configure(name, options);
                }
                else if (name == Microsoft.Extensions.Options.Options.DefaultName)
                {
                    setup.Configure(options);
                }
            }

            foreach (var post in _postConfigures)
            {
                post.PostConfigure(name, options);
            }

            return options;
        }

    }
}
