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
using System.Runtime.InteropServices;

namespace Librame.Algorithm.Asymmetries
{
    /// <summary>
    /// 非对称密钥对。
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct AsymmetryKeyPair
    {
        /// <summary>
        /// 构造一个 <see cref="AsymmetryKeyPair"/> 实例。
        /// </summary>
        /// <param name="_public">给定的公钥。</param>
        /// <param name="_private">给定的私钥。</param>
        public AsymmetryKeyPair(string _public, string _private)
        {
            Public = _public;
            Private = _private;
        }

        /// <summary>
        /// 获取公钥。
        /// </summary>
        public string Public { get; }

        /// <summary>
        /// 获取私钥。
        /// </summary>
        public string Private { get; }
    }
}
