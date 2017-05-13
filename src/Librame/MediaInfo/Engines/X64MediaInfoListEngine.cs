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

namespace Librame.MediaInfo.Engines
{
    /// <summary>
    /// 64 位 MediaInfo 列表引擎。
    /// </summary>
    public class X64MediaInfoListEngine : AbstractMediaInfoEngine, IMediaInfoListEngine
    {
        // Import of DLL functions. DO NOT USE until you know what you do
        // (MediaInfo DLL do NOT use CoTaskMemAlloc to allocate memory)
        [DllImport(X64MediaInfoEngine.DLL_FILENAME)]
        private static extern IntPtr MediaInfoList_New();

        [DllImport(X64MediaInfoEngine.DLL_FILENAME)]
        private static extern void MediaInfoList_Delete(IntPtr handle);

        [DllImport(X64MediaInfoEngine.DLL_FILENAME)]
        private static extern IntPtr MediaInfoList_Open(IntPtr handle,
            [MarshalAs(UnmanagedType.LPWStr)] string fileName, IntPtr options);

        [DllImport(X64MediaInfoEngine.DLL_FILENAME)]
        private static extern void MediaInfoList_Close(IntPtr handle, IntPtr filePos);

        [DllImport(X64MediaInfoEngine.DLL_FILENAME)]
        private static extern IntPtr MediaInfoList_Inform(IntPtr handle, IntPtr filePos, IntPtr reserved);

        [DllImport(X64MediaInfoEngine.DLL_FILENAME)]
        private static extern IntPtr MediaInfoList_GetI(IntPtr handle, IntPtr filePos, IntPtr streamKind,
            IntPtr streamNumber, IntPtr parameter, IntPtr kindOfInfo);

        [DllImport(X64MediaInfoEngine.DLL_FILENAME)]
        private static extern IntPtr MediaInfoList_Get(IntPtr handle, IntPtr filePos, IntPtr streamKind,
            IntPtr streamNumber, [MarshalAs(UnmanagedType.LPWStr)] string parameter, IntPtr kindOfInfo,
            IntPtr kindOfSearch);

        [DllImport(X64MediaInfoEngine.DLL_FILENAME)]
        private static extern IntPtr MediaInfoList_Option(IntPtr handle,
            [MarshalAs(UnmanagedType.LPWStr)] string option, [MarshalAs(UnmanagedType.LPWStr)] string value);

        [DllImport(X64MediaInfoEngine.DLL_FILENAME)]
        private static extern IntPtr MediaInfoList_State_Get(IntPtr handle);

        [DllImport(X64MediaInfoEngine.DLL_FILENAME)]
        private static extern IntPtr MediaInfoList_Count_Get(IntPtr handle, IntPtr filePos,
            IntPtr streamKind, IntPtr streamNumber);


        private IntPtr _handle;

        /// <summary>
        /// 构造一个 <see cref="X64MediaInfoListEngine"/> 实例。
        /// </summary>
        public X64MediaInfoListEngine()
            : base(X64MediaInfoEngine.DLL_FILENAME)
        {
            _handle = MediaInfoList_New();
        }
        /// <summary>
        /// 解构一个 <see cref="X64MediaInfoListEngine"/> 实例。
        /// </summary>
        ~X64MediaInfoListEngine()
        {
            MediaInfoList_Delete(_handle);
        }


        /// <summary>
        /// 打开媒体文件。
        /// </summary>
        /// <param name="fileName">给定的媒体文件。</param>
        /// <param name="options">给定的信息文件选项。</param>
        /// <returns>返回指针或句柄平台特定类型。</returns>
        public int Open(string fileName, InfoFileOptions options)
        {
            return (int)MediaInfoList_Open(_handle, fileName, (IntPtr)options);
        }


        /// <summary>
        /// 关闭媒体文件。
        /// </summary>
        /// <param name="filePos">给定的文件定位。</param>
        public void Close(int filePos)
        {
            MediaInfoList_Close(_handle, (IntPtr)filePos);
        }


        /// <summary>
        /// 报告媒体文件。
        /// </summary>
        /// <param name="filePos">给定的文件定位。</param>
        /// <returns>返回字符串。</returns>
        public string Inform(int filePos)
        {
            return Marshal.PtrToStringUni(MediaInfoList_Inform(_handle, (IntPtr)filePos, (IntPtr)0));
        }


        /// <summary>
        /// 获取内容。
        /// </summary>
        /// <param name="filePos">给定的文件定位。</param>
        /// <param name="streamKind">给定的流种类。</param>
        /// <param name="streamNumber">给定的流编号。</param>
        /// <param name="parameter">给定的参数。</param>
        /// <param name="kindOfInfo">给定的信息种类。</param>
        /// <param name="kindOfSearch">给定的查询信息种类。</param>
        /// <returns>返回字符串。</returns>
        public string Get(int filePos, StreamKind streamKind, int streamNumber, string parameter,
            InfoKind kindOfInfo, InfoKind kindOfSearch)
        {
            return Marshal.PtrToStringUni(MediaInfoList_Get(_handle, (IntPtr)filePos, (IntPtr)streamKind,
                (IntPtr)streamNumber, parameter, (IntPtr)kindOfInfo, (IntPtr)kindOfSearch));
        }

        /// <summary>
        /// 获取内容。
        /// </summary>
        /// <param name="filePos">给定的文件定位。</param>
        /// <param name="streamKind">给定的流种类。</param>
        /// <param name="streamNumber">给定的流编号。</param>
        /// <param name="parameter">给定的参数。</param>
        /// <param name="kindOfInfo">给定的信息种类。</param>
        /// <returns>返回字符串。</returns>
        public string Get(int filePos, StreamKind streamKind, int streamNumber, int parameter,
            InfoKind kindOfInfo)
        {
            return Marshal.PtrToStringUni(MediaInfoList_GetI(_handle, (IntPtr)filePos,
                (IntPtr)streamKind, (IntPtr)streamNumber, (IntPtr)parameter, (IntPtr)kindOfInfo));
        }


        /// <summary>
        /// 选项值信息。
        /// </summary>
        /// <param name="option">给定的选项。</param>
        /// <param name="value">给定的值。</param>
        /// <returns>返回字符串。</returns>
        public string Option(string option, string value)
        {
            return Marshal.PtrToStringUni(MediaInfoList_Option(_handle, option, value));
        }


        /// <summary>
        /// 统计信息。
        /// </summary>
        /// <param name="filePos">给定的文件定位。</param>
        /// <param name="streamKind">给定的流种类。</param>
        /// <param name="streamNumber">给定的流编号。</param>
        /// <returns>返回指针或句柄平台特定类型。</returns>
        public int Count(int filePos, StreamKind streamKind, int streamNumber)
        {
            return (int)MediaInfoList_Count_Get(_handle, (IntPtr)filePos, (IntPtr)streamKind,
                (IntPtr)streamNumber);
        }


        /// <summary>
        /// 状态信息。
        /// </summary>
        /// <returns>返回指针或句柄平台特定类型。</returns>
        public int State()
        {
            return (int)MediaInfoList_State_Get(_handle);
        }

    }
}