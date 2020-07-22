#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Librame.Extensions.Core.Tokens
{
    using Builders;
    using Combiners;
    using Services;
    using Utilities;

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class SecurityTokenKeyRing : ISecurityTokenKeyRing
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IClockService _clock;
        private readonly FilePathCombiner _keysFilePath;


        public SecurityTokenKeyRing(IMemoryCache memoryCache,
            IClockService clock, CoreBuilderDependency dependency)
        {
            _memoryCache = memoryCache.NotNull(nameof(memoryCache));
            _clock = clock.NotNull(nameof(clock));

            _keysFilePath = CoreSettings.Preference.SecurityTokenKeyRingFilePath
                .ChangeBasePathIfEmpty(dependency?.TokensDirectory);
        }


        public string this[string index]
        {
            get
            {
                var info = GetKeyInfos().FirstOrDefault(info => info.Index == index);
                if (info.IsNull() || info.Token.IsEmpty())
                    throw new ArgumentException($"The data protector key id '{index}' is not found.");

                return info.Token;
            }
        }

        public string CurrentIndex
        {
            get
            {
                var infos = GetKeyInfos();
                var index = RandomUtility.Run(r => r.Next(0, infos.Count));
                return infos[index].Index;
            }
        }


        public IEnumerable<string> GetAllIndexes()
            => GetKeyInfos().Select(info => info.Index);


        private List<SecurityTokenKeyInfo> GetKeyInfos()
        {
            return ExtensionSettings.Preference.RunLocker(() =>
            {
                return _memoryCache.GetOrCreate(GetCacheKey(), entry =>
                {
                    if (_keysFilePath.Exists())
                        return _keysFilePath.ReadSecureJson<List<SecurityTokenKeyInfo>>();

                    var infos = GenerateKeyInfos(CoreSettings.Preference.SecurityTokensCount);
                    _keysFilePath.WriteSecureJson(infos);
                    return infos;
                });
            });

            // GenerateKeyInfos
            List<SecurityTokenKeyInfo> GenerateKeyInfos(int count)
            {
                var infos = new List<SecurityTokenKeyInfo>();

                while (infos.Count < count)
                {
                    var createdTime = _clock.GetNowOffset();
                    var token = SecurityToken.New();

                    var info = SecurityTokenKeyInfo.Create(token, createdTime);
                    if (!infos.Any(p => p.Index == info.Index))
                    {
                        infos.Add(info);
                    }
                }

                return infos;
            }
        }


        private string GetCacheKey()
            => $"{nameof(SecurityTokenKeyRing)}|{_keysFilePath}";

    }
}
