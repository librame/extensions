#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;

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
            get
            {
                if (null == _preference)
                {
                    ExtensionSettings.Preference.RunLocker(() =>
                    {
                        if (null == _preference)
                        {
                            _preference = new DataPreferenceSetting();
                        }
                    });
                }

                return _preference;
            }
            set
            {
                if (null == value)
                    throw new ArgumentNullException(nameof(value));

                ExtensionSettings.Preference.RunLocker(() =>
                {
                    _preference = value;
                });
            }
        }

    }
}
