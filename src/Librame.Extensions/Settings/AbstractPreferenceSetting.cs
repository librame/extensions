#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions
{
    /// <summary>
    /// 抽象偏好设置。
    /// </summary>
    public abstract class AbstractPreferenceSetting : IPreferenceSetting
    {
        /// <summary>
        /// 重置偏好设置。
        /// </summary>
        public virtual void Reset()
        {
        }

    }
}
