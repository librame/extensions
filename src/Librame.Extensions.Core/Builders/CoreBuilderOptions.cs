#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Text;

namespace Librame.Extensions.Core.Builders
{
    using Identifiers;
    using Options;
    using Serializers;

    /// <summary>
    /// 核心构建器选项。
    /// </summary>
    public class CoreBuilderOptions : CoreBuilderOptions<IdentifierOptions>
    {
        /// <summary>
        /// 字符编码（默认为 UTF8）。
        /// </summary>
        public SerializableString<Encoding> Encoding { get; }
            = new SerializableString<Encoding>(ExtensionSettings.Preference.DefaultEncoding);

        /// <summary>
        /// 解决时钟回流的偏移量（默认为 1）。
        /// </summary>
        public int ClockRefluxOffset { get; set; }
            = 1;

        /// <summary>
        /// 是 UTC 时钟。
        /// </summary>
        public bool IsUtcClock { get; set; }
            = true;
    }


    /// <summary>
    /// 核心构建器选项。
    /// </summary>
    /// <typeparam name="TIdentifierOptions">指定的标识符选项类型。</typeparam>
    public class CoreBuilderOptions<TIdentifierOptions>
        : AbstractCoreBuilderOptions<TIdentifierOptions>
        where TIdentifierOptions : IdentifierOptions, new()
    {
        /// <summary>
        /// 构造一个核心构建器选项。
        /// </summary>
        public CoreBuilderOptions()
        {
            Identifier.GuidIdentifierGenerator = CombIdentityGenerator.SQLServer;
            Identifier.LongIdentifierGenerator = SnowflakeIdentityGenerator.Default;
        }

    }
}
