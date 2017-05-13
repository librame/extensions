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

namespace Librame.MediaInfo.Engines
{
    using Utility;

    /// <summary>
    /// 抽象 MediaInfo 引擎。
    /// </summary>
    public abstract class AbstractMediaInfoEngine : LibrameBase<AbstractMediaInfoEngine>
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractMediaInfoEngine"/> 实例。
        /// </summary>
        /// <param name="dllFileName">给定的动态链接库文件名。</param>
        public AbstractMediaInfoEngine(string dllFileName)
        {
            try
            {
                var dllPath = PathUtility.BinDirectory.AppendPath(dllFileName);

                DllPath = dllPath.FileExists();
            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerMessage(), ex);
            }
        }


        /// <summary>
        /// 动态链接库路径。
        /// </summary>
        public string DllPath { get; }

    }
}
