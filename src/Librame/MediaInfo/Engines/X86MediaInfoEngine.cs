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
using System.Runtime.InteropServices;

namespace Librame.MediaInfo.Engines
{
    using Utility;

    /// <summary>
    /// 32 位 MediaInfo 引擎。
    /// </summary>
    public class X86MediaInfoEngine : IMediaInfoEngine
    {
        /// <summary>
        /// 默认 DLL 文件名。
        /// </summary>
        private const string DEFAULT_DLL_FILENAME = "MediaInfo_i386.dll";


        //Import of DLL functions. DO NOT USE until you know what you do (MediaInfo DLL do NOT use CoTaskMemAlloc to allocate memory)
        [DllImport(DEFAULT_DLL_FILENAME)]
        private static extern IntPtr MediaInfo_New();
        
        [DllImport(DEFAULT_DLL_FILENAME)]
        private static extern void   MediaInfo_Delete(IntPtr Handle);
        
        [DllImport(DEFAULT_DLL_FILENAME)]
        private static extern IntPtr MediaInfo_Open(IntPtr Handle, [MarshalAs(UnmanagedType.LPWStr)] string FileName);
        
        [DllImport(DEFAULT_DLL_FILENAME)]
        private static extern IntPtr MediaInfoA_Open(IntPtr Handle, IntPtr FileName);
        
        [DllImport(DEFAULT_DLL_FILENAME)]
        private static extern IntPtr MediaInfo_Open_Buffer_Init(IntPtr Handle, Int64 File_Size, Int64 File_Offset);
        
        [DllImport(DEFAULT_DLL_FILENAME)]
        private static extern IntPtr MediaInfoA_Open(IntPtr Handle, Int64 File_Size, Int64 File_Offset);
        
        [DllImport(DEFAULT_DLL_FILENAME)]
        private static extern IntPtr MediaInfo_Open_Buffer_Continue(IntPtr Handle, IntPtr Buffer, IntPtr Buffer_Size);
        
        [DllImport(DEFAULT_DLL_FILENAME)]
        private static extern IntPtr MediaInfoA_Open_Buffer_Continue(IntPtr Handle, Int64 File_Size, byte[] Buffer, IntPtr Buffer_Size);
        
        [DllImport(DEFAULT_DLL_FILENAME)]
        private static extern Int64  MediaInfo_Open_Buffer_Continue_GoTo_Get(IntPtr Handle);
        
        [DllImport(DEFAULT_DLL_FILENAME)]
        private static extern Int64  MediaInfoA_Open_Buffer_Continue_GoTo_Get(IntPtr Handle);

        [DllImport(DEFAULT_DLL_FILENAME)]
        private static extern IntPtr MediaInfo_Open_Buffer_Finalize(IntPtr Handle);
        
        [DllImport(DEFAULT_DLL_FILENAME)]
        private static extern IntPtr MediaInfoA_Open_Buffer_Finalize(IntPtr Handle);
        
        [DllImport(DEFAULT_DLL_FILENAME)]
        private static extern void   MediaInfo_Close(IntPtr Handle);
        
        [DllImport(DEFAULT_DLL_FILENAME)]
        private static extern IntPtr MediaInfo_Inform(IntPtr Handle, IntPtr Reserved);
        
        [DllImport(DEFAULT_DLL_FILENAME)]
        private static extern IntPtr MediaInfoA_Inform(IntPtr Handle, IntPtr Reserved);
        
        [DllImport(DEFAULT_DLL_FILENAME)]
        private static extern IntPtr MediaInfo_GetI(IntPtr Handle, IntPtr StreamKind, IntPtr StreamNumber,
            IntPtr Parameter, IntPtr KindOfInfo);

        [DllImport(DEFAULT_DLL_FILENAME)]
        private static extern IntPtr MediaInfoA_GetI(IntPtr Handle, IntPtr StreamKind, IntPtr StreamNumber,
            IntPtr Parameter, IntPtr KindOfInfo);
        
        [DllImport(DEFAULT_DLL_FILENAME)]
        private static extern IntPtr MediaInfo_Get(IntPtr Handle, IntPtr StreamKind, IntPtr StreamNumber,
            [MarshalAs(UnmanagedType.LPWStr)] string Parameter, IntPtr KindOfInfo, IntPtr KindOfSearch);
        
        [DllImport(DEFAULT_DLL_FILENAME)]
        private static extern IntPtr MediaInfoA_Get(IntPtr Handle, IntPtr StreamKind, IntPtr StreamNumber,
            IntPtr Parameter, IntPtr KindOfInfo, IntPtr KindOfSearch);
        
        [DllImport(DEFAULT_DLL_FILENAME)]
        private static extern IntPtr MediaInfo_Option(IntPtr Handle, [MarshalAs(UnmanagedType.LPWStr)] string Option,
            [MarshalAs(UnmanagedType.LPWStr)] string Value);
        
        [DllImport(DEFAULT_DLL_FILENAME)]
        private static extern IntPtr MediaInfoA_Option(IntPtr Handle, IntPtr Option,  IntPtr Value);
        
        [DllImport(DEFAULT_DLL_FILENAME)]
        private static extern IntPtr MediaInfo_State_Get(IntPtr Handle);
        
        [DllImport(DEFAULT_DLL_FILENAME)]
        private static extern IntPtr MediaInfo_Count_Get(IntPtr Handle, IntPtr StreamKind, IntPtr StreamNumber);
        

        /// <summary>
        /// 构造一个 <see cref="X86MediaInfoEngine"/> 对象。
        /// </summary>
        public X86MediaInfoEngine()
        {
            DllFileInfo = new FileInfo(PathUtility.BinDirectory.AppendPath(DEFAULT_DLL_FILENAME));
            if (!DllFileInfo.Exists)
                throw new FileNotFoundException(DllFileInfo.FullName);

            Handle = MediaInfo_New();
            
            if (Environment.OSVersion.ToString().IndexOf("Windows")==-1)
                MustUseAnsi=true;
            else
                MustUseAnsi=false;
        }

        /// <summary>
        /// 解构一个 <see cref="X86MediaInfoEngine"/> 对象。
        /// </summary>
        ~X86MediaInfoEngine()
        {
            MediaInfo_Delete(Handle);
        }


        /// <summary>
        /// 获取 DLL 文件信息。
        /// </summary>
        public FileInfo DllFileInfo { get; private set; }


        /// <summary>
        /// 打开指定的媒体文件。
        /// </summary>
        /// <param name="FileName">给定的完整文件名。</param>
        /// <returns></returns>
        public int Open(String FileName)
        {
            if (MustUseAnsi)
            {
                IntPtr FileName_Ptr = Marshal.StringToHGlobalAnsi(FileName);
                int ToReturn = (int)MediaInfoA_Open(Handle, FileName_Ptr);
                Marshal.FreeHGlobal(FileName_Ptr);
                return ToReturn;
            }
            else
                return (int)MediaInfo_Open(Handle, FileName);
        }
        
        private int Open_Buffer_Init(Int64 File_Size, Int64 File_Offset)
        {
            return (int)MediaInfo_Open_Buffer_Init(Handle, File_Size, File_Offset);
        }
        
        private int Open_Buffer_Continue(IntPtr Buffer, IntPtr Buffer_Size)
        {
            return (int)MediaInfo_Open_Buffer_Continue(Handle, Buffer, Buffer_Size);
        }
        
        private Int64 Open_Buffer_Continue_GoTo_Get()
        {
            return (int)MediaInfo_Open_Buffer_Continue_GoTo_Get(Handle);
        }
        
        private int Open_Buffer_Finalize()
        {
            return (int)MediaInfo_Open_Buffer_Finalize(Handle);
        }


        /// <summary>
        /// 关闭。
        /// </summary>
        public void Close()
        {
            MediaInfo_Close(Handle);
        }


        /// <summary>
        /// 通告。
        /// </summary>
        /// <returns></returns>
        public String Inform()
        {
            if (MustUseAnsi)
                return Marshal.PtrToStringAnsi(MediaInfoA_Inform(Handle, (IntPtr)0));
            else
                return Marshal.PtrToStringUni(MediaInfo_Inform(Handle, (IntPtr)0));
        }


        /// <summary>
        /// 获取指定条件的字符串信息。
        /// </summary>
        /// <param name="StreamKind"></param>
        /// <param name="StreamNumber"></param>
        /// <param name="Parameter"></param>
        /// <param name="KindOfInfo"></param>
        /// <param name="KindOfSearch"></param>
        /// <returns></returns>
        public String Get(StreamKind StreamKind, int StreamNumber, String Parameter, InfoKind KindOfInfo, InfoKind KindOfSearch)
        {
            if (MustUseAnsi)
            {
                IntPtr Parameter_Ptr=Marshal.StringToHGlobalAnsi(Parameter);
                String ToReturn=Marshal.PtrToStringAnsi(MediaInfoA_Get(Handle, (IntPtr)StreamKind, (IntPtr)StreamNumber, Parameter_Ptr, (IntPtr)KindOfInfo, (IntPtr)KindOfSearch));
                Marshal.FreeHGlobal(Parameter_Ptr);
                return ToReturn;
            }
            else
                return Marshal.PtrToStringUni(MediaInfo_Get(Handle, (IntPtr)StreamKind, (IntPtr)StreamNumber, Parameter, (IntPtr)KindOfInfo, (IntPtr)KindOfSearch));
        }
        /// <summary>
        /// 获取指定条件的字符串信息。
        /// </summary>
        /// <param name="StreamKind"></param>
        /// <param name="StreamNumber"></param>
        /// <param name="Parameter"></param>
        /// <param name="KindOfInfo"></param>
        /// <returns></returns>
        public String Get(StreamKind StreamKind, int StreamNumber, int Parameter, InfoKind KindOfInfo)
        {
            if (MustUseAnsi)
                return Marshal.PtrToStringAnsi(MediaInfoA_GetI(Handle, (IntPtr)StreamKind, (IntPtr)StreamNumber, (IntPtr)Parameter, (IntPtr)KindOfInfo));
            else
                return Marshal.PtrToStringUni(MediaInfo_GetI(Handle, (IntPtr)StreamKind, (IntPtr)StreamNumber, (IntPtr)Parameter, (IntPtr)KindOfInfo));
        }


        /// <summary>
        /// 获取指定项。
        /// </summary>
        /// <param name="Option"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public String Option(String Option, String Value)
        {
            if (MustUseAnsi)
            {
                IntPtr Option_Ptr=Marshal.StringToHGlobalAnsi(Option);
                IntPtr Value_Ptr=Marshal.StringToHGlobalAnsi(Value);
                String ToReturn=Marshal.PtrToStringAnsi(MediaInfoA_Option(Handle, Option_Ptr, Value_Ptr));
                Marshal.FreeHGlobal(Option_Ptr);
                Marshal.FreeHGlobal(Value_Ptr);
                return ToReturn;
            }
            else
                return Marshal.PtrToStringUni(MediaInfo_Option(Handle, Option, Value));
        }
        
        private int State_Get()
        {
            return (int)MediaInfo_State_Get(Handle);
        }
        
        private int Count_Get(StreamKind StreamKind, int StreamNumber)
        {
            return (int)MediaInfo_Count_Get(Handle, (IntPtr)StreamKind, (IntPtr)StreamNumber);
        }

        private IntPtr Handle;
        private bool MustUseAnsi;
        //Default values, if you know how to set default values in C#, say me
        
        private String Get(StreamKind StreamKind, int StreamNumber, String Parameter, InfoKind KindOfInfo)
        {
            return Get(StreamKind, StreamNumber, Parameter, KindOfInfo, InfoKind.Name);
        }

        /// <summary>
        /// 获取指定条件的字符串信息。
        /// </summary>
        /// <param name="StreamKind"></param>
        /// <param name="StreamNumber"></param>
        /// <param name="Parameter"></param>
        /// <returns></returns>
        public String Get(StreamKind StreamKind, int StreamNumber, String Parameter)
        {
            return Get(StreamKind, StreamNumber, Parameter, InfoKind.Text, InfoKind.Name);
        }
        
        private String Get(StreamKind StreamKind, int StreamNumber, int Parameter)
        {
            return Get(StreamKind, StreamNumber, Parameter, InfoKind.Text);
        }
        
        private String Option(String Option_)
        {
            return Option(Option_, "");
        }
        
        private int Count_Get(StreamKind StreamKind)
        {
            return Count_Get(StreamKind, -1);
        }


        /// <summary>
        /// 释放资源。
        /// </summary>
        public void Dispose()
        {
            Close();
        }

    }
    
    ///// <summary>
    ///// MediaInfoStream 列表。
    ///// </summary>
    //class MediaInfoStreamList
    //{
    //    //Import of DLL functions. DO NOT USE until you know what you do (MediaInfo DLL do NOT use CoTaskMemAlloc to allocate memory)
    //    [DllImport(MediaInfoX86Provider.DllFileName)]
    //    private static extern IntPtr MediaInfoList_New();
        
    //    [DllImport(MediaInfoX86Provider.DllFileName)]
    //    private static extern void MediaInfoList_Delete(IntPtr Handle);
        
    //    [DllImport(MediaInfoX86Provider.DllFileName)]
    //    private static extern IntPtr MediaInfoList_Open(IntPtr Handle, [MarshalAs(UnmanagedType.LPWStr)] string FileName, IntPtr Options);
        
    //    [DllImport(MediaInfoX86Provider.DllFileName)]
    //    private static extern void MediaInfoList_Close(IntPtr Handle, IntPtr FilePos);
        
    //    [DllImport(MediaInfoX86Provider.DllFileName)]
    //    private static extern IntPtr MediaInfoList_Inform(IntPtr Handle, IntPtr FilePos, IntPtr Reserved);
        
    //    [DllImport(MediaInfoX86Provider.DllFileName)]
    //    private static extern IntPtr MediaInfoList_GetI(IntPtr Handle, IntPtr FilePos, IntPtr StreamKind,
    //        IntPtr StreamNumber, IntPtr Parameter, IntPtr KindOfInfo);
        
    //    [DllImport(MediaInfoX86Provider.DllFileName)]
    //    private static extern IntPtr MediaInfoList_Get(IntPtr Handle, IntPtr FilePos, IntPtr StreamKind,
    //        IntPtr StreamNumber, [MarshalAs(UnmanagedType.LPWStr)] string Parameter, IntPtr KindOfInfo, IntPtr KindOfSearch);
        
    //    [DllImport(MediaInfoX86Provider.DllFileName)]
    //    private static extern IntPtr MediaInfoList_Option(IntPtr Handle, [MarshalAs(UnmanagedType.LPWStr)] string Option,
    //        [MarshalAs(UnmanagedType.LPWStr)] string Value);
        
    //    [DllImport(MediaInfoX86Provider.DllFileName)]
    //    private static extern IntPtr MediaInfoList_State_Get(IntPtr Handle);
        
    //    [DllImport(MediaInfoX86Provider.DllFileName)]
    //    private static extern IntPtr MediaInfoList_Count_Get(IntPtr Handle, IntPtr FilePos, IntPtr StreamKind, IntPtr StreamNumber);
        
    //    /// <summary>
    //    /// MediaInfoStream 列表。
    //    /// </summary>
    //    public MediaInfoStreamList()
    //    {
    //        Handle = MediaInfoList_New();
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    ~MediaInfoStreamList()
    //    {
    //        MediaInfoList_Delete(Handle);
    //    }
        
    //    /// <summary>
    //    /// 打开指定的媒体文件。
    //    /// </summary>
    //    /// <param name="FileName"></param>
    //    /// <param name="Options"></param>
    //    /// <returns></returns>
    //    public int Open(String FileName, InfoFileOptions Options)
    //    {
    //        return (int)MediaInfoList_Open(Handle, FileName, (IntPtr)Options);
    //    }
        
    //    /// <summary>
    //    /// 关闭当前媒体文件。
    //    /// </summary>
    //    /// <param name="FilePos"></param>
    //    public void Close(int FilePos)
    //    {
    //        MediaInfoList_Close(Handle, (IntPtr)FilePos);
    //    }
        
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="FilePos"></param>
    //    /// <returns></returns>
    //    public String Inform(int FilePos)
    //    {
    //        return Marshal.PtrToStringUni(MediaInfoList_Inform(Handle, (IntPtr)FilePos, (IntPtr)0));
    //    }
        
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="FilePos"></param>
    //    /// <param name="StreamKind"></param>
    //    /// <param name="StreamNumber"></param>
    //    /// <param name="Parameter"></param>
    //    /// <param name="KindOfInfo"></param>
    //    /// <param name="KindOfSearch"></param>
    //    /// <returns></returns>
    //    public String Get(int FilePos, StreamKind StreamKind, int StreamNumber, String Parameter,
    //        InfoKind KindOfInfo, InfoKind KindOfSearch)
    //    {
    //        return Marshal.PtrToStringUni(MediaInfoList_Get(Handle, (IntPtr)FilePos, (IntPtr)StreamKind,
    //            (IntPtr)StreamNumber, Parameter, (IntPtr)KindOfInfo, (IntPtr)KindOfSearch));
    //    }
        
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="FilePos"></param>
    //    /// <param name="StreamKind"></param>
    //    /// <param name="StreamNumber"></param>
    //    /// <param name="Parameter"></param>
    //    /// <param name="KindOfInfo"></param>
    //    /// <returns></returns>
    //    public String Get(int FilePos, StreamKind StreamKind, int StreamNumber, int Parameter, InfoKind KindOfInfo)
    //    {
    //        return Marshal.PtrToStringUni(MediaInfoList_GetI(Handle, (IntPtr)FilePos, (IntPtr)StreamKind,
    //            (IntPtr)StreamNumber, (IntPtr)Parameter, (IntPtr)KindOfInfo));
    //    }
        
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="Option"></param>
    //    /// <param name="Value"></param>
    //    /// <returns></returns>
    //    public String Option(String Option, String Value)
    //    {
    //        return Marshal.PtrToStringUni(MediaInfoList_Option(Handle, Option, Value));
    //    }
        
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <returns></returns>
    //    public int State_Get()
    //    {
    //        return (int)MediaInfoList_State_Get(Handle);
    //    }
        
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="FilePos"></param>
    //    /// <param name="StreamKind"></param>
    //    /// <param name="StreamNumber"></param>
    //    /// <returns></returns>
    //    public int Count_Get(int FilePos, StreamKind StreamKind, int StreamNumber)
    //    {
    //        return (int)MediaInfoList_Count_Get(Handle, (IntPtr)FilePos, (IntPtr)StreamKind, (IntPtr)StreamNumber);
    //    }
        
    //    private IntPtr Handle;
        
    //    //Default values, if you know how to set default values in C#, say me
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="FileName"></param>
    //    public void Open(String FileName)
    //    {
    //        Open(FileName, 0);
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public void Close()
    //    {
    //        Close(-1);
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="FilePos"></param>
    //    /// <param name="StreamKind"></param>
    //    /// <param name="StreamNumber"></param>
    //    /// <param name="Parameter"></param>
    //    /// <param name="KindOfInfo"></param>
    //    /// <returns></returns>
    //    public String Get(int FilePos, StreamKind StreamKind, int StreamNumber, String Parameter, InfoKind KindOfInfo)
    //    {
    //        return Get(FilePos, StreamKind, StreamNumber, Parameter, KindOfInfo, InfoKind.Name);
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="FilePos"></param>
    //    /// <param name="StreamKind"></param>
    //    /// <param name="StreamNumber"></param>
    //    /// <param name="Parameter"></param>
    //    /// <returns></returns>
    //    public String Get(int FilePos, StreamKind StreamKind, int StreamNumber, String Parameter)
    //    {
    //        return Get(FilePos, StreamKind, StreamNumber, Parameter, InfoKind.Text, InfoKind.Name);
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="FilePos"></param>
    //    /// <param name="StreamKind"></param>
    //    /// <param name="StreamNumber"></param>
    //    /// <param name="Parameter"></param>
    //    /// <returns></returns>
    //    public String Get(int FilePos, StreamKind StreamKind, int StreamNumber, int Parameter)
    //    {
    //        return Get(FilePos, StreamKind, StreamNumber, Parameter, InfoKind.Text);
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="Option_"></param>
    //    /// <returns></returns>
    //    public String Option(String Option_)
    //    {
    //        return Option(Option_, "");
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="FilePos"></param>
    //    /// <param name="StreamKind"></param>
    //    /// <returns></returns>
    //    public int Count_Get(int FilePos, StreamKind StreamKind)
    //    {
    //        return Count_Get(FilePos, StreamKind, -1);
    //    }

    //}
}
