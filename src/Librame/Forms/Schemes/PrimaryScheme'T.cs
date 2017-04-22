#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Forms.Schemes
{
    /// <summary>
    /// 主要方案。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    public class PrimaryScheme<T>
    {
        /// <summary>
        /// 构造一个 <see cref="PrimaryScheme{T}"/> 实例。
        /// </summary>
        /// <param name="primary">给定的主设定。</param>
        /// <param name="darkPrimary">给定的暗系主设定。</param>
        /// <param name="lightPrimary">给定的亮系主设定。</param>
        /// <param name="accent">给定的强调设定。</param>
        /// <param name="textShade">给定的文本阴影。</param>
        public PrimaryScheme(T primary, T darkPrimary, T lightPrimary, T accent, T textShade)
        {
            Primary = primary;
            DarkPrimary = darkPrimary;
            LightPrimary = lightPrimary;
            Accent = accent;
            TextShade = textShade;
        }


        /// <summary>
        /// 主设定。
        /// </summary>
        public T Primary { get; }

        /// <summary>
        /// 暗主设定。
        /// </summary>
        public T DarkPrimary { get; }

        /// <summary>
        /// 亮主设定。
        /// </summary>
        public T LightPrimary { get; }

        /// <summary>
        /// 强调设定。
        /// </summary>
        public T Accent { get; }

        /// <summary>
        /// 文本阴影设定。
        /// </summary>
        public T TextShade { get; }
    }
}
