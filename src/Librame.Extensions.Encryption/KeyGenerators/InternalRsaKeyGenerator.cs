#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Librame.Extensions.Encryption
{
    using RsaKeySerializers;
    using Services;

    /// <summary>
    /// 内部 RSA 密钥生成器。
    /// </summary>
    internal class InternalRsaKeyGenerator : AbstractService<InternalRsaKeyGenerator>, IRsaKeyGenerator
    {
        private IEnumerable<IRsaKeySerializer> _serializers;
        private RsaKeyParameters _parameters;


        /// <summary>
        /// 构造一个 <see cref="InternalRsaKeyGenerator"/> 实例。
        /// </summary>
        /// <param name="serializers">给定的 <see cref="IEnumerable{IRsaKeySerializer}"/>。</param>
        /// <param name="logger">给定的 <see cref="ILogger{InternalRsaKeyGeneratorService}"/>。</param>
        public InternalRsaKeyGenerator(IEnumerable<IRsaKeySerializer> serializers, ILogger<InternalRsaKeyGenerator> logger)
            : base(logger)
        {
            _serializers = serializers.NotEmpty(nameof(serializers));
        }


        /// <summary>
        /// 生成密钥参数。
        /// </summary>
        /// <param name="rsaKeyFileName">给定的 RSA 密钥文件名（可选；默认不使用）。</param>
        /// <param name="forceRegen">强制重新生成（可选；默认不强制）。</param>
        /// <returns>返回 <see cref="RsaKeyParameters"/>。</returns>
        public RsaKeyParameters GenerateKeyParameters(string rsaKeyFileName = null, bool forceRegen = false)
        {
            if (_parameters.IsDefault() || forceRegen)
            {
                if (rsaKeyFileName.IsEmpty())
                {
                    _parameters = RsaKeyParameters.Create(RSA.Create().ExportParameters(true));
                    Logger.LogDebug($"Generate rsa key from RSA.Create().ExportParameters(true): KeyId={_parameters.KeyId}");
                }
                else
                {
                    var fileExtension = Path.GetExtension(rsaKeyFileName);
                    var serializer = _serializers.First(s => s.FileExtension == fileExtension);
                    _parameters = serializer.Deserialize(rsaKeyFileName);
                    Logger.LogDebug($"Generate rsa key from IRsaKeySerializer.Deserialize(fileName): KeyId={_parameters.KeyId}, Filename={rsaKeyFileName}");
                }
            }

            return _parameters;
        }

    }
}
