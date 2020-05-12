#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Librame.Extensions.Core.Identifiers
{
    using Builders;
    using Combiners;
    using Services;
    using Utilities;

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class SecurityIdentifierKeyRing : ISecurityIdentifierKeyRing
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IClockService _clock;
        private readonly FilePathCombiner _keysFilePath;


        public SecurityIdentifierKeyRing(IMemoryCache memoryCache,
            IClockService clock, CoreBuilderDependency dependency)
        {
            _memoryCache = memoryCache.NotNull(nameof(memoryCache));
            _clock = clock.NotNull(nameof(clock));

            _keysFilePath = CoreSettings.Preference.SecurityIdentifierKeyRingFilePath
                .ChangeBasePathIfEmpty(dependency?.IdentifiersDirectory);
        }


        public string this[string index]
        {
            get
            {
                var info = GetKeyInfos().FirstOrDefault(info => info.Index == index);
                if (info.IsNull() || info.Identifier.IsEmpty())
                    throw new ArgumentException($"The data protector key id '{index}' is not found.");

                return info.Identifier;
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


        private List<SecurityIdentifierKeyInfo> GetKeyInfos()
        {
            return ExtensionSettings.Preference.RunLockerResult(() =>
            {
                return _memoryCache.GetOrCreate(GetCacheKey(), entry =>
                {
                    if (_keysFilePath.Exists())
                        return _keysFilePath.ReadSecureJson<List<SecurityIdentifierKeyInfo>>();

                    var infos = GenerateKeyInfos(CoreSettings.Preference.SecurityIdentifierKeyInfosCount);
                    _keysFilePath.WriteSecureJson(infos);
                    return infos;
                });
            });

            // GenerateKeyInfos
            List<SecurityIdentifierKeyInfo> GenerateKeyInfos(int count)
            {
                var infos = new List<SecurityIdentifierKeyInfo>();

                while (infos.Count < count)
                {
                    var createdTime = _clock.GetNowOffsetAsync().ConfigureAndResult();
                    var identifier = SecurityIdentifier.New();

                    var info = SecurityIdentifierKeyInfo.Create(identifier, createdTime);
                    if (!infos.Any(p => p.Index == info.Index))
                    {
                        infos.Add(info);
                    }
                }

                return infos;
            }
        }


        private string GetCacheKey()
            => $"{nameof(SecurityIdentifierKeyRing)}|{_keysFilePath}";

    }
}
