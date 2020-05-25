#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 核心首选项。
    /// </summary>
    public static class CoreSettings
    {
        private static ICorePreferenceSetting _preference;

        /// <summary>
        /// 当前偏好设置。
        /// </summary>
        public static ICorePreferenceSetting Preference
        {
            get => _preference.EnsureSingleton(() => new CorePreferenceSetting());
            set => _preference = value.NotNull(nameof(value));
        }

    }
}
