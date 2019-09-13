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

namespace Librame.Extensions
{
    /// <summary>
    /// 压缩静态扩展。
    /// </summary>
    /// <remarks>https://blog.csdn.net/mengdong_zy/article/details/28636469</remarks>
    public static class CompressionExtensions
    {
        /// <summary>
        /// 压缩格式。
        /// </summary>
        public const ushort COMPRESSION_FORMAT_LZNT1
            = 2;

        /// <summary>
        /// 压缩引擎最大值。
        /// </summary>
        public const ushort COMPRESSION_ENGINE_MAXIMUM
            = 0x100;

        private const string NTDLL
            = "ntdll.dll";

        private const string KERNEL32DLL
            = "kernel32.dll";


        /// <summary>
        /// 获取压缩工作区大小。
        /// </summary>
        /// <param name="dCompressionFormat"></param>
        /// <param name="dNeededBufferSize"></param>
        /// <param name="dUnknown"></param>
        /// <returns></returns>
        [DllImport(NTDLL)]
        public static extern uint RtlGetCompressionWorkSpaceSize(ushort dCompressionFormat, out uint dNeededBufferSize, out uint dUnknown);

        /// <summary>
        /// 压缩缓冲区。
        /// </summary>
        /// <param name="dCompressionFormat"></param>
        /// <param name="dSourceBuffer"></param>
        /// <param name="dSourceBufferLength"></param>
        /// <param name="dDestinationBuffer"></param>
        /// <param name="dDestinationBufferLength"></param>
        /// <param name="dUnknown"></param>
        /// <param name="dDestinationSize"></param>
        /// <param name="dWorkspaceBuffer"></param>
        /// <returns></returns>
        [DllImport(NTDLL)]
        public static extern uint RtlCompressBuffer(ushort dCompressionFormat, byte[] dSourceBuffer, int dSourceBufferLength,
            byte[] dDestinationBuffer, int dDestinationBufferLength, uint dUnknown, out int dDestinationSize, IntPtr dWorkspaceBuffer);

        /// <summary>
        /// 解压缩缓冲区。
        /// </summary>
        /// <param name="dCompressionFormat"></param>
        /// <param name="dDestinationBuffer"></param>
        /// <param name="dDestinationBufferLength"></param>
        /// <param name="dSourceBuffer"></param>
        /// <param name="dSourceBufferLength"></param>
        /// <param name="dDestinationSize"></param>
        /// <returns></returns>
        [DllImport(NTDLL)]
        public static extern uint RtlDecompressBuffer(ushort dCompressionFormat, byte[] dDestinationBuffer, int dDestinationBufferLength,
            byte[] dSourceBuffer, int dSourceBufferLength, out uint dDestinationSize);

        /// <summary>
        /// 从堆中分配指定数量的内存。
        /// </summary>
        /// <param name="uFlags"></param>
        /// <param name="sizetdwBytes"></param>
        /// <returns></returns>
        [DllImport(KERNEL32DLL, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr LocalAlloc(int uFlags, IntPtr sizetdwBytes);

        /// <summary>
        /// 释放局部内存对象并使句柄失效。
        /// </summary>
        /// <param name="hMem"></param>
        /// <returns></returns>
        [DllImport(KERNEL32DLL, SetLastError = true)]
        public static extern IntPtr LocalFree(IntPtr hMem);


        /// <summary>
        /// 压缩字节数组。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] Compress(this byte[] buffer)
        {
            var outBuffer = new byte[buffer.Length * 6];
            var size = RtlGetCompressionWorkSpaceSize(COMPRESSION_FORMAT_LZNT1 | COMPRESSION_ENGINE_MAXIMUM, out uint dwSize, out _);
            if (size != 0) return null;

            var hWork = LocalAlloc(0, new IntPtr(dwSize));

            size = RtlCompressBuffer(COMPRESSION_FORMAT_LZNT1 | COMPRESSION_ENGINE_MAXIMUM, buffer, buffer.Length, outBuffer,
                outBuffer.Length, 0, out int dstSize, hWork);
            if (size != 0) return null;

            LocalFree(hWork);

            Array.Resize(ref outBuffer, dstSize);
            return outBuffer;
        }

        /// <summary>
        /// 解压缩字节数组。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] Decompress(this byte[] buffer)
        {
            var outBuffer = new byte[buffer.Length * 6];
            var size = RtlGetCompressionWorkSpaceSize(COMPRESSION_FORMAT_LZNT1, out _, out _);
            if (size != 0) return null;

            size = RtlDecompressBuffer(COMPRESSION_FORMAT_LZNT1, outBuffer, outBuffer.Length, buffer, buffer.Length, out uint dwRet);
            if (size != 0) return null;

            Array.Resize(ref outBuffer, (int)dwRet);
            return outBuffer;
        }

    }
}
