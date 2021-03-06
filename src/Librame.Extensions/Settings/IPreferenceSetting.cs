﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions
{
    /// <summary>
    /// 偏好设置接口。
    /// </summary>
    /// <typeparam name="TOptions">指定的设置选项类型。</typeparam>
    public interface IPreferenceSetting<TOptions> : IPreferenceSetting
    {
        /// <summary>
        /// 设置选项。
        /// </summary>
        TOptions Options { get; }
    }


    /// <summary>
    /// 偏好设置接口。
    /// </summary>
    public interface IPreferenceSetting
    {
        /// <summary>
        /// 重置偏好设置。
        /// </summary>
        void Reset();
    }
}
