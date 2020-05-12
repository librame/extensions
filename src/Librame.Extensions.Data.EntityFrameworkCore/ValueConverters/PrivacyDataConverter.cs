#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Librame.Extensions.Data.ValueConverters
{
    using Protectors;

    /// <summary>
    /// 隐私数据转换器。
    /// </summary>
    public class PrivacyDataConverter : ValueConverter<string, string>
    {
        /// <summary>
        /// 构造一个 <see cref="PrivacyDataConverter"/>。
        /// </summary>
        /// <param name="protector">给定的 <see cref="IPrivacyDataProtector"/>。</param>
        public PrivacyDataConverter(IPrivacyDataProtector protector)
            : this(protector.NotNull(nameof(protector)), default)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="PrivacyDataConverter"/>。
        /// </summary>
        /// <param name="protector">给定的 <see cref="IPrivacyDataProtector"/>。</param>
        /// <param name="mappingHints">给定的 <see cref="ConverterMappingHints"/>。</param>
        public PrivacyDataConverter(IPrivacyDataProtector protector, ConverterMappingHints mappingHints)
            : base(s => protector.Protect(s), s => protector.Unprotect(s), mappingHints)
        {
        }

    }
}
