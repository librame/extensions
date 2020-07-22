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
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Data.Protectors
{
    using Core;
    using Core.Tokens;

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class PrivacyDataProtector : IPrivacyDataProtector
    {
        private readonly ISecurityTokenKeyRing _keyRing;
        private readonly ISecurityTokenProtector _encryptor;


        public PrivacyDataProtector(ISecurityTokenKeyRing keyRing, ISecurityTokenProtector protector)
        {
            _keyRing = keyRing.NotNull(nameof(keyRing));
            _encryptor = protector.NotNull(nameof(keyRing));
        }


        public string Protect(string data)
        {
            var current = _keyRing.CurrentIndex;
            return $"{current}{CoreSettings.Preference.KeySeparator}{_encryptor.Protect(current, data)}";
        }

        public string Unprotect(string data)
        {
            var pair = data.SplitPair(CoreSettings.Preference.KeySeparator);
            if (pair.Key.IsEmpty() || pair.Value.IsEmpty())
                throw new InvalidOperationException($"Malformed data '{data}'.");

            return _encryptor.Unprotect(pair.Key, pair.Value);
        }

    }
}
