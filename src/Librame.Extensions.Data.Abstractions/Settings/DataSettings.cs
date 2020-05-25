#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 数据首选项。
    /// </summary>
    public static class DataSettings
    {
        private static IDataPreferenceSetting _preference;

        /// <summary>
        /// 当前偏好设置。
        /// </summary>
        public static IDataPreferenceSetting Preference
        {
            get => _preference.EnsureSingleton(() => new DataPreferenceSetting());
            set => _preference = value.NotNull(nameof(value));
        }

    }
}
