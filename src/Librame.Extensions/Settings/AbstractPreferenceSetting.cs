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
using System.IO;

namespace Librame.Extensions
{
    /// <summary>
    /// 抽象偏好设置。
    /// </summary>
    /// <typeparam name="TOptions">指定的设置选项类型。</typeparam>
    public abstract class AbstractPreferenceSetting<TOptions> : AbstractPreferenceSetting, IPreferenceSetting<TOptions>
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractPreferenceSetting{TOptions}"/>。
        /// </summary>
        /// <param name="filePath">给定的设置选项文件路径。</param>
        /// <param name="defaultOptionsFactory">给定的默认设置选项工厂方法。</param>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        public AbstractPreferenceSetting(string filePath, Func<TOptions> defaultOptionsFactory)
        {
            if (File.Exists(filePath))
            {
                Options = filePath.ReadJson<TOptions>();
            }
            else
            {
                defaultOptionsFactory.NotNull(nameof(defaultOptionsFactory));

                Options = defaultOptionsFactory.Invoke();
                filePath.WriteJson(Options);
            }
        }


        /// <summary>
        /// 设置选项。
        /// </summary>
        public TOptions Options { get; }
    }


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
