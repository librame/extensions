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
            get
            {
                if (null == _preference)
                {
                    ExtensionSettings.Preference.RunLocker(() =>
                    {
                        if (null == _preference)
                        {
                            _preference = new CorePreferenceSetting();
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
