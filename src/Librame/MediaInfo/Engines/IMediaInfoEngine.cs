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
using System.IO;

namespace Librame.MediaInfo.Engines
{
    /// <summary>
    /// MediaInfo 引擎接口。
    /// </summary>
    public interface IMediaInfoEngine : IDisposable
    {
        /// <summary>
        /// 获取 DLL 文件信息。
        /// </summary>
        FileInfo DllFileInfo { get; }


        /// <summary>
        /// 打开指定的媒体文件。
        /// </summary>
        /// <param name="FileName">给定的完整文件名。</param>
        /// <returns></returns>
        int Open(string FileName);

        /// <summary>
        /// 关闭。
        /// </summary>
        void Close();

        /// <summary>
        /// 通告。
        /// </summary>
        /// <returns></returns>
        string Inform();

        /// <summary>
        /// 获取指定条件的字符串信息。
        /// </summary>
        /// <param name="StreamKind"></param>
        /// <param name="StreamNumber"></param>
        /// <param name="Parameter"></param>
        /// <param name="KindOfInfo"></param>
        /// <param name="KindOfSearch"></param>
        /// <returns></returns>
        string Get(StreamKind StreamKind, int StreamNumber, string Parameter, InfoKind KindOfInfo, InfoKind KindOfSearch);
        /// <summary>
        /// 获取指定条件的字符串信息。
        /// </summary>
        /// <param name="StreamKind"></param>
        /// <param name="StreamNumber"></param>
        /// <param name="Parameter"></param>
        /// <param name="KindOfInfo"></param>
        /// <returns></returns>
        string Get(StreamKind StreamKind, int StreamNumber, int Parameter, InfoKind KindOfInfo);

        /// <summary>
        /// 获取指定条件的字符串信息。
        /// </summary>
        /// <param name="StreamKind"></param>
        /// <param name="StreamNumber"></param>
        /// <param name="Parameter"></param>
        /// <returns></returns>
        string Get(StreamKind StreamKind, int StreamNumber, string Parameter);

        /// <summary>
        /// 获取指定项。
        /// </summary>
        /// <param name="Option"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        string Option(string Option, string Value);
    }
}
