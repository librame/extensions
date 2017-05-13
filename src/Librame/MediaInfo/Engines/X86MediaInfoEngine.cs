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
    /// 32 位 MediaInfo 引擎。
    /// </summary>
    public class X86MediaInfoEngine : AbstractMediaInfoEngine, IMediaInfoEngine
    {
        /// <summary>
        /// 默认 DLL 文件名。
        /// </summary>
        internal const string DLL_FILENAME = "MediaInfo_i386.dll";


        // Import of DLL functions. DO NOT USE until you know what you do
        // (MediaInfo DLL do NOT use CoTaskMemAlloc to allocate memory)
        [DllImport(DLL_FILENAME)]
        private static extern IntPtr MediaInfo_New();
        
        [DllImport(DLL_FILENAME)]
        private static extern void   MediaInfo_Delete(IntPtr handle);
        
        [DllImport(DLL_FILENAME)]
        private static extern IntPtr MediaInfo_Open(IntPtr handle,
            [MarshalAs(UnmanagedType.LPWStr)] string fileName);
        
        [DllImport(DLL_FILENAME)]
        private static extern IntPtr MediaInfoA_Open(IntPtr handle, IntPtr fileName);
        
        [DllImport(DLL_FILENAME)]
        private static extern IntPtr MediaInfo_Open_Buffer_Init(IntPtr handle, Int64 fileSize,
            Int64 fileOffset);
        
        [DllImport(DLL_FILENAME)]
        private static extern IntPtr MediaInfoA_Open(IntPtr handle, Int64 fileSize, Int64 fileOffset);
        
        [DllImport(DLL_FILENAME)]
        private static extern IntPtr MediaInfo_Open_Buffer_Continue(IntPtr handle, IntPtr buffer,
            IntPtr bufferSize);
        
        [DllImport(DLL_FILENAME)]
        private static extern IntPtr MediaInfoA_Open_Buffer_Continue(IntPtr handle, Int64 fileSize,
            byte[] buffer, IntPtr bufferSize);
        
        [DllImport(DLL_FILENAME)]
        private static extern Int64  MediaInfo_Open_Buffer_Continue_GoTo_Get(IntPtr handle);
        
        [DllImport(DLL_FILENAME)]
        private static extern Int64  MediaInfoA_Open_Buffer_Continue_GoTo_Get(IntPtr handle);

        [DllImport(DLL_FILENAME)]
        private static extern IntPtr MediaInfo_Open_Buffer_Finalize(IntPtr handle);
        
        [DllImport(DLL_FILENAME)]
        private static extern IntPtr MediaInfoA_Open_Buffer_Finalize(IntPtr handle);
        
        [DllImport(DLL_FILENAME)]
        private static extern void   MediaInfo_Close(IntPtr handle);
        
        [DllImport(DLL_FILENAME)]
        private static extern IntPtr MediaInfo_Inform(IntPtr handle, IntPtr reserved);
        
        [DllImport(DLL_FILENAME)]
        private static extern IntPtr MediaInfoA_Inform(IntPtr handle, IntPtr reserved);
        
        [DllImport(DLL_FILENAME)]
        private static extern IntPtr MediaInfo_GetI(IntPtr handle, IntPtr streamKind, IntPtr streamNumber,
            IntPtr parameter, IntPtr kindOfInfo);

        [DllImport(DLL_FILENAME)]
        private static extern IntPtr MediaInfoA_GetI(IntPtr handle, IntPtr streamKind, IntPtr streamNumber,
            IntPtr parameter, IntPtr kindOfInfo);
        
        [DllImport(DLL_FILENAME)]
        private static extern IntPtr MediaInfo_Get(IntPtr handle, IntPtr streamKind, IntPtr streamNumber,
            [MarshalAs(UnmanagedType.LPWStr)] string parameter, IntPtr kindOfInfo, IntPtr kindOfSearch);
        
        [DllImport(DLL_FILENAME)]
        private static extern IntPtr MediaInfoA_Get(IntPtr handle, IntPtr streamKind, IntPtr streamNumber,
            IntPtr parameter, IntPtr kindOfInfo, IntPtr kindOfSearch);
        
        [DllImport(DLL_FILENAME)]
        private static extern IntPtr MediaInfo_Option(IntPtr handle,
            [MarshalAs(UnmanagedType.LPWStr)] string option,
            [MarshalAs(UnmanagedType.LPWStr)] string value);
        
        [DllImport(DLL_FILENAME)]
        private static extern IntPtr MediaInfoA_Option(IntPtr handle, IntPtr option,  IntPtr value);
        
        [DllImport(DLL_FILENAME)]
        private static extern IntPtr MediaInfo_State_Get(IntPtr handle);
        
        [DllImport(DLL_FILENAME)]
        private static extern IntPtr MediaInfo_Count_Get(IntPtr handle, IntPtr streamKind, IntPtr streamNumber);


        private IntPtr _handle;
        private bool _mustUseAnsi;

        /// <summary>
        /// 构造一个 <see cref="X86MediaInfoEngine"/> 实例。
        /// </summary>
        public X86MediaInfoEngine()
            : base(DLL_FILENAME)
        {
            try
            {
                _handle = MediaInfo_New();
            }
            catch
            {
                _handle = (IntPtr)0;
            }
            
            if (Environment.OSVersion.ToString().IndexOf("Windows")==-1)
                _mustUseAnsi=true;
            else
                _mustUseAnsi=false;
        }
        /// <summary>
        /// 解构一个 <see cref="X86MediaInfoEngine"/> 实例。
        /// </summary>
        ~X86MediaInfoEngine()
        {
            if (_handle == (IntPtr)0)
                return;

            MediaInfo_Delete(_handle);
        }


        /// <summary>
        /// 打开媒体文件。
        /// </summary>
        /// <param name="fileName">给定的媒体文件。</param>
        /// <returns>返回指针或句柄平台特定类型。</returns>
        public int Open(string fileName)
        {
            if (_handle == (IntPtr)0)
                return 0;

            if (_mustUseAnsi)
            {
                IntPtr FileName_Ptr = Marshal.StringToHGlobalAnsi(fileName);
                int ToReturn = (int)MediaInfoA_Open(_handle, FileName_Ptr);
                Marshal.FreeHGlobal(FileName_Ptr);
                return ToReturn;
            }
            else
                return (int)MediaInfo_Open(_handle, fileName);
        }
        
        private int Open_Buffer_Init(Int64 fileSize, Int64 fileOffset)
        {
            return (int)MediaInfo_Open_Buffer_Init(_handle, fileSize, fileOffset);
        }
        
        private int Open_Buffer_Continue(IntPtr buffer, IntPtr bufferSize)
        {
            return (int)MediaInfo_Open_Buffer_Continue(_handle, buffer, bufferSize);
        }
        
        private Int64 Open_Buffer_Continue_GoTo_Get()
        {
            return (int)MediaInfo_Open_Buffer_Continue_GoTo_Get(_handle);
        }
        
        private int Open_Buffer_Finalize()
        {
            return (int)MediaInfo_Open_Buffer_Finalize(_handle);
        }


        /// <summary>
        /// 关闭媒体文件。
        /// </summary>
        public void Close()
        {
            if (_handle == (IntPtr)0)
                return;

            MediaInfo_Close(_handle);
        }


        /// <summary>
        /// 报告媒体文件。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public string Inform()
        {
            if (_handle == (IntPtr)0)
                return "Unable to load MediaInfo library";

            if (_mustUseAnsi)
                return Marshal.PtrToStringAnsi(MediaInfoA_Inform(_handle, (IntPtr)0));
            else
                return Marshal.PtrToStringUni(MediaInfo_Inform(_handle, (IntPtr)0));
        }


        /// <summary>
        /// 获取内容。
        /// </summary>
        /// <param name="streamKind">给定的流种类。</param>
        /// <param name="streamNumber">给定的流编号。</param>
        /// <param name="parameter">给定的参数。</param>
        /// <param name="kindOfInfo">给定的信息种类。</param>
        /// <param name="kindOfSearch">给定的查询信息种类。</param>
        /// <returns>返回字符串。</returns>
        public string Get(StreamKind streamKind, int streamNumber, string parameter, InfoKind kindOfInfo,
            InfoKind kindOfSearch)
        {
            if (_handle == (IntPtr)0)
                return "Unable to load MediaInfo library";

            if (_mustUseAnsi)
            {
                IntPtr Parameter_Ptr=Marshal.StringToHGlobalAnsi(parameter);
                string ToReturn=Marshal.PtrToStringAnsi(MediaInfoA_Get(_handle, (IntPtr)streamKind,
                    (IntPtr)streamNumber, Parameter_Ptr, (IntPtr)kindOfInfo, (IntPtr)kindOfSearch));
                Marshal.FreeHGlobal(Parameter_Ptr);
                return ToReturn;
            }
            else
            {
                return Marshal.PtrToStringUni(MediaInfo_Get(_handle, (IntPtr)streamKind, (IntPtr)streamNumber,
                    parameter, (IntPtr)kindOfInfo, (IntPtr)kindOfSearch));
            }
        }

        /// <summary>
        /// 获取内容。
        /// </summary>
        /// <param name="streamKind">给定的流种类。</param>
        /// <param name="streamNumber">给定的流编号。</param>
        /// <param name="parameter">给定的参数。</param>
        /// <param name="kindOfInfo">给定的信息种类。</param>
        /// <returns>返回字符串。</returns>
        public string Get(StreamKind streamKind, int streamNumber, int parameter, InfoKind kindOfInfo)
        {
            if (_handle == (IntPtr)0)
                return "Unable to load MediaInfo library";

            if (_mustUseAnsi)
            {
                return Marshal.PtrToStringAnsi(MediaInfoA_GetI(_handle, (IntPtr)streamKind,
                    (IntPtr)streamNumber, (IntPtr)parameter, (IntPtr)kindOfInfo));
            }
            else
            {
                return Marshal.PtrToStringUni(MediaInfo_GetI(_handle, (IntPtr)streamKind,
                    (IntPtr)streamNumber, (IntPtr)parameter, (IntPtr)kindOfInfo));
            }
        }


        /// <summary>
        /// 选项值信息。
        /// </summary>
        /// <param name="option">给定的选项。</param>
        /// <param name="value">给定的值。</param>
        /// <returns>返回字符串。</returns>
        public string Option(string option, string value)
        {
            if (_handle == (IntPtr)0)
                return "Unable to load MediaInfo library";

            if (_mustUseAnsi)
            {
                IntPtr Option_Ptr=Marshal.StringToHGlobalAnsi(option);
                IntPtr Value_Ptr=Marshal.StringToHGlobalAnsi(value);
                string ToReturn=Marshal.PtrToStringAnsi(MediaInfoA_Option(_handle, Option_Ptr, Value_Ptr));
                Marshal.FreeHGlobal(Option_Ptr);
                Marshal.FreeHGlobal(Value_Ptr);
                return ToReturn;
            }
            else
                return Marshal.PtrToStringUni(MediaInfo_Option(_handle, option, value));
        }


        /// <summary>
        /// 统计信息。
        /// </summary>
        /// <param name="streamKind">给定的流种类。</param>
        /// <param name="streamNumber">给定的流编号。</param>
        /// <returns>返回指针或句柄平台特定类型。</returns>
        public int Count(StreamKind streamKind, int streamNumber)
        {
            if (_handle == (IntPtr)0)
                return 0;

            return (int)MediaInfo_Count_Get(_handle, (IntPtr)streamKind, (IntPtr)streamNumber);
        }


        /// <summary>
        /// 状态信息。
        /// </summary>
        /// <returns>返回指针或句柄平台特定类型。</returns>
        public int State()
        {
            if (_handle == (IntPtr)0)
                return 0;

            return (int)MediaInfo_State_Get(_handle);
        }
        

        /// <summary>
        /// 释放资源。
        /// </summary>
        public void Dispose()
        {
            Close();
        }

    }
}
