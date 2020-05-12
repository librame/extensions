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
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Security;

namespace Librame.Extensions
{
    using Resources;

    /// <summary>
    /// 压缩静态扩展。
    /// </summary>
    public static class CompressionExtensions
    {
        /// <summary>
        /// RTL 压缩字节数组。
        /// </summary>
        /// <param name="originalBuffer">给定的原生字节数组。</param>
        /// <returns>返回字节数组。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static byte[] RtlCompress(this byte[] originalBuffer)
        {
            originalBuffer.NotEmpty(nameof(originalBuffer));

            var outputBuffer = new byte[originalBuffer.Length * 6];
            var compressionFormat = (ushort)(ExtensionSettings.Preference.CompressionFormatLZNT1
                | ExtensionSettings.Preference.CompressionEngineMaximum);

            var size = SafeNativeMethods.RtlGetCompressionWorkSpaceSize(compressionFormat, out uint dwSize, out _);
            if (size != 0) return null;

            var hWork = SafeNativeMethods.LocalAlloc(0, new IntPtr(dwSize));

            size = SafeNativeMethods.RtlCompressBuffer(compressionFormat, originalBuffer, originalBuffer.Length, outputBuffer,
                outputBuffer.Length, 0, out int dstSize, hWork);
            if (size != 0) return null;

            SafeNativeMethods.LocalFree(hWork);

            Array.Resize(ref outputBuffer, dstSize);
            return outputBuffer;
        }

        /// <summary>
        /// RTL 解压缩字节数组。
        /// </summary>
        /// <param name="compressedBuffer">给定的已压缩字节数组。</param>
        /// <returns>返回字节数组。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static byte[] RtlDecompress(this byte[] compressedBuffer)
        {
            compressedBuffer.NotEmpty(nameof(compressedBuffer));

            var outputBuffer = new byte[compressedBuffer.Length * 6];
            var size = SafeNativeMethods.RtlGetCompressionWorkSpaceSize(ExtensionSettings.Preference.CompressionFormatLZNT1, out _, out _);
            if (size != 0) return null;

            size = SafeNativeMethods.RtlDecompressBuffer(ExtensionSettings.Preference.CompressionFormatLZNT1, outputBuffer, outputBuffer.Length,
                compressedBuffer, compressedBuffer.Length, out uint dwRet);
            if (size != 0) return null;

            Array.Resize(ref outputBuffer, (int)dwRet);
            return outputBuffer;
        }


        #region GZip

        /// <summary>
        /// GZip 压缩文件。
        /// </summary>
        /// <param name="originalFileInfo">给定的原始 <see cref="FileInfo"/>。</param>
        /// <returns>返回原始或压缩的 <see cref="FileInfo"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static FileInfo GZipCompress(this FileInfo originalFileInfo)
        {
            originalFileInfo.NotNull(nameof(originalFileInfo));

            if ((File.GetAttributes(originalFileInfo.FullName) & FileAttributes.Hidden)
                != FileAttributes.Hidden & !originalFileInfo.Extension.Equals(ExtensionSettings.Preference.GZipCompressedFileType, StringComparison.OrdinalIgnoreCase))
            {
                var compressedFilePath = originalFileInfo.FullName + ExtensionSettings.Preference.GZipCompressedFileType;

                using (var originalStream = originalFileInfo.OpenRead())
                using (var compressedStream = File.Create(compressedFilePath))
                {
                    originalStream.GZipCompress(compressedStream);
                }

                return new FileInfo(compressedFilePath);
            }

            return originalFileInfo;
        }

        /// <summary>
        /// GZip 解压缩文件。
        /// </summary>
        /// <exception cref="FileLoadException">
        /// Unsupported compressed file type (current: '{0}', required: '{1}').
        /// </exception>
        /// <param name="compressedFileInfo">给定的已压缩 <see cref="FileInfo"/>。</param>
        /// <returns>返回解压缩的 <see cref="FileInfo"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static FileInfo GZipDecompress(this FileInfo compressedFileInfo)
        {
            compressedFileInfo.NotNull(nameof(compressedFileInfo));

            if (!compressedFileInfo.Extension.Equals(ExtensionSettings.Preference.GZipCompressedFileType,
                StringComparison.OrdinalIgnoreCase))
            {
                throw new FileLoadException(InternalResource.FileLoadExceptionUnsupportedCompressedFileTypeFormat
                    .Format(compressedFileInfo.Extension, ExtensionSettings.Preference.GZipCompressedFileType));
            }

            var decompressedFilePath = compressedFileInfo.FullName.TrimEnd(ExtensionSettings.Preference.GZipCompressedFileType);

            using (var compressedStream = compressedFileInfo.OpenRead())
            using (var decompressedStream = File.Create(decompressedFilePath))
            {
                compressedStream.GZipDecompress(decompressedStream);
            }

            return new FileInfo(decompressedFilePath);
        }


        /// <summary>
        /// GZip 压缩字节数组。
        /// </summary>
        /// <param name="originalBuffer">给定的原始字节数组。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] GZipCompress(this byte[] originalBuffer)
        {
            using (var compressedStream = new MemoryStream())
            {
                originalBuffer.GZipCompress(compressedStream);
                return compressedStream.ToArray();
            }
        }

        /// <summary>
        /// GZip 解压缩字节数组。
        /// </summary>
        /// <param name="compressedBuffer">给定的已压缩字节数组。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] GZipDecompress(this byte[] compressedBuffer)
        {
            using (var decompressedStream = new MemoryStream())
            {
                compressedBuffer.GZipDecompress(decompressedStream);
                return decompressedStream.ToArray();
            }
        }


        /// <summary>
        /// GZip 压缩字节数组。
        /// </summary>
        /// <param name="originalBuffer">给定的原始字节数组。</param>
        /// <param name="compressedStream">给定的已压缩流。</param>
        public static void GZipCompress(this byte[] originalBuffer, Stream compressedStream)
        {
            using (var originalStream = new MemoryStream(originalBuffer))
            {
                originalStream.GZipCompress(compressedStream);
            }
        }

        /// <summary>
        /// GZip 解压缩字节数组。
        /// </summary>
        /// <param name="compressedBuffer">给定的已压缩字节数组。</param>
        /// <param name="decompressedStream">给定的已解压缩流。</param>
        /// <returns>返回字节数组。</returns>
        public static void GZipDecompress(this byte[] compressedBuffer, Stream decompressedStream)
        {
            using (var compressedStream = new MemoryStream(compressedBuffer))
            {
                compressedStream.GZipDecompress(decompressedStream);
            }
        }


        /// <summary>
        /// GZip 压缩。
        /// </summary>
        /// <param name="originalStream">给定的原始流。</param>
        /// <param name="compressedStream">给定的已压缩流。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static void GZipCompress(this Stream originalStream, Stream compressedStream)
        {
            originalStream.NotNull(nameof(originalStream));

            using (var compressionStream = new GZipStream(compressedStream, CompressionMode.Compress, true))
            {
                originalStream.CopyTo(compressionStream);
            }
        }

        /// <summary>
        /// GZip 解压缩。
        /// </summary>
        /// <param name="compressedStream">给定的已压缩流。</param>
        /// <param name="decompressedStream">给定的已解压缩流。</param>
        public static void GZipDecompress(this Stream compressedStream, Stream decompressedStream)
        {
            using (var decompressionStream = new GZipStream(compressedStream, CompressionMode.Decompress, true))
            {
                decompressionStream.CopyTo(decompressedStream);
            }
        }

        #endregion


        #region Deflate

        /// <summary>
        /// Deflate 压缩文件。
        /// </summary>
        /// <param name="originalFileInfo">给定的原始 <see cref="FileInfo"/>。</param>
        /// <returns>返回原始或压缩的 <see cref="FileInfo"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static FileInfo DeflateCompress(this FileInfo originalFileInfo)
        {
            originalFileInfo.NotNull(nameof(originalFileInfo));

            if ((File.GetAttributes(originalFileInfo.FullName) & FileAttributes.Hidden)
                != FileAttributes.Hidden & !originalFileInfo.Extension.Equals(ExtensionSettings.Preference.DeflateCompressedFileType, StringComparison.OrdinalIgnoreCase))
            {
                var compressedFilePath = originalFileInfo.FullName + ExtensionSettings.Preference.DeflateCompressedFileType;

                using (var originalStream = originalFileInfo.OpenRead())
                using (var compressedStream = File.Create(compressedFilePath))
                {
                    originalStream.DeflateCompress(compressedStream);
                }

                return new FileInfo(compressedFilePath);
            }

            return originalFileInfo;
        }

        /// <summary>
        /// Deflate 解压缩文件。
        /// </summary>
        /// <exception cref="FileLoadException">
        /// Unsupported compressed file type (current: '{0}', required: '{1}').
        /// </exception>
        /// <param name="compressedFileInfo">给定的已压缩 <see cref="FileInfo"/>。</param>
        /// <returns>返回解压缩的 <see cref="FileInfo"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static FileInfo DeflateDecompress(this FileInfo compressedFileInfo)
        {
            compressedFileInfo.NotNull(nameof(compressedFileInfo));

            if (!compressedFileInfo.Extension.Equals(ExtensionSettings.Preference.DeflateCompressedFileType,
                StringComparison.OrdinalIgnoreCase))
            {
                throw new FileLoadException(InternalResource.FileLoadExceptionUnsupportedCompressedFileTypeFormat
                    .Format(compressedFileInfo.Extension, ExtensionSettings.Preference.DeflateCompressedFileType));
            }

            var decompressedFilePath = compressedFileInfo.FullName.TrimEnd(ExtensionSettings.Preference.DeflateCompressedFileType);

            using (var compressedStream = compressedFileInfo.OpenRead())
            using (var decompressedStream = File.Create(decompressedFilePath))
            {
                compressedStream.DeflateDecompress(decompressedStream);
            }

            return new FileInfo(decompressedFilePath);
        }


        /// <summary>
        /// Deflate 压缩字节数组。
        /// </summary>
        /// <param name="originalBuffer">给定的原始字节数组。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] DeflateCompress(this byte[] originalBuffer)
        {
            using (var compressedStream = new MemoryStream())
            {
                originalBuffer.DeflateCompress(compressedStream);
                return compressedStream.ToArray();
            }
        }

        /// <summary>
        /// Deflate 解压缩字节数组。
        /// </summary>
        /// <param name="compressedBuffer">给定的已压缩字节数组。</param>
        /// <returns>返回字节数组。</returns>
        public static byte[] DeflateDecompress(this byte[] compressedBuffer)
        {
            using (var decompressedStream = new MemoryStream())
            {
                compressedBuffer.DeflateDecompress(decompressedStream);
                return decompressedStream.ToArray();
            }
        }


        /// <summary>
        /// Deflate 压缩字节数组。
        /// </summary>
        /// <param name="originalBuffer">给定的原始字节数组。</param>
        /// <param name="compressedStream">给定的已压缩流。</param>
        public static void DeflateCompress(this byte[] originalBuffer, Stream compressedStream)
        {
            using (var originalStream = new MemoryStream(originalBuffer))
            {
                originalStream.DeflateCompress(compressedStream);
            }
        }

        /// <summary>
        /// Deflate 解压缩字节数组。
        /// </summary>
        /// <param name="compressedBuffer">给定的已压缩字节数组。</param>
        /// <param name="decompressedStream">给定的已解压缩流。</param>
        /// <returns>返回字节数组。</returns>
        public static void DeflateDecompress(this byte[] compressedBuffer, Stream decompressedStream)
        {
            using (var compressedStream = new MemoryStream(compressedBuffer))
            {
                compressedStream.DeflateDecompress(decompressedStream);
            }
        }


        /// <summary>
        /// Deflate 压缩。
        /// </summary>
        /// <param name="originalStream">给定的原始流。</param>
        /// <param name="compressedStream">给定的已压缩流。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static void DeflateCompress(this Stream originalStream, Stream compressedStream)
        {
            originalStream.NotNull(nameof(originalStream));

            using (var compressionStream = new DeflateStream(compressedStream, CompressionMode.Compress, true))
            {
                originalStream.CopyTo(compressionStream);
            }
        }

        /// <summary>
        /// Deflate 解压缩。
        /// </summary>
        /// <param name="compressedStream">给定的已压缩流。</param>
        /// <param name="decompressedStream">给定的已解压缩流。</param>
        public static void DeflateDecompress(this Stream compressedStream, Stream decompressedStream)
        {
            using (var decompressionStream = new DeflateStream(compressedStream, CompressionMode.Decompress, true))
            {
                decompressionStream.CopyTo(decompressedStream);
            }
        }

        #endregion

    }


    [SuppressUnmanagedCodeSecurity]
    static class SafeNativeMethods
    {
        [DllImport(ExtensionSettings.NtDllFileName, CharSet = CharSet.Auto, ExactSpelling = true)]
        internal static extern uint RtlGetCompressionWorkSpaceSize(ushort dCompressionFormat, out uint dNeededBufferSize, out uint dUnknown);

        [DllImport(ExtensionSettings.NtDllFileName, CharSet = CharSet.Auto, ExactSpelling = true)]
        internal static extern uint RtlCompressBuffer(ushort dCompressionFormat, byte[] dSourceBuffer, int dSourceBufferLength,
            byte[] dDestinationBuffer, int dDestinationBufferLength, uint dUnknown, out int dDestinationSize, IntPtr dWorkspaceBuffer);

        [DllImport(ExtensionSettings.NtDllFileName, CharSet = CharSet.Auto, ExactSpelling = true)]
        internal static extern uint RtlDecompressBuffer(ushort dCompressionFormat, byte[] dDestinationBuffer, int dDestinationBufferLength,
            byte[] dSourceBuffer, int dSourceBufferLength, out uint dDestinationSize);

        [DllImport(ExtensionSettings.Kernel32DllFileName, CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
        internal static extern IntPtr LocalAlloc(int uFlags, IntPtr sizetdwBytes);

        [DllImport(ExtensionSettings.Kernel32DllFileName, CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
        internal static extern IntPtr LocalFree(IntPtr hMem);
    }
}
