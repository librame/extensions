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
using System.Text;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 抽象构建器选项字符编码。
    /// </summary>
    public abstract class AbstractBuilderOptionsEncoding : IEncoding
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractBuilderOptionsEncoding"/> 实例。
        /// </summary>
        /// <param name="coreOptions">给定的 <see cref="IOptions{CoreBuilderOptions}"/>。</param>
        protected AbstractBuilderOptionsEncoding(IOptions<CoreBuilderOptions> coreOptions)
        {
            CoreOptions = coreOptions.NotNull(nameof(coreOptions)).Value;
            Encoding = CoreOptions.Encoding;
        }


        /// <summary>
        /// 核心构建器选项。
        /// </summary>
        protected CoreBuilderOptions CoreOptions { get; }

        /// <summary>
        /// 字符编码（默认使用构建器选项配置）。
        /// </summary>
        public Encoding Encoding { get; set; }
    }
}
