#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.MediaInfo.Engines
{
    /// <summary>
    /// MediaInfo 列表引擎。
    /// </summary>
    public interface IMediaInfoListEngine
    {
        /// <summary>
        /// 打开媒体文件。
        /// </summary>
        /// <param name="fileName">给定的媒体文件。</param>
        /// <param name="options">给定的信息文件选项。</param>
        /// <returns>返回指针或句柄平台特定类型。</returns>
        int Open(string fileName, InfoFileOptions options);
        

        /// <summary>
        /// 关闭媒体文件。
        /// </summary>
        /// <param name="filePos">给定的文件定位。</param>
        void Close(int filePos);
        

        /// <summary>
        /// 报告媒体文件。
        /// </summary>
        /// <param name="filePos">给定的文件定位。</param>
        /// <returns>返回字符串。</returns>
        string Inform(int filePos);
        

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
        string Get(int filePos, StreamKind streamKind, int streamNumber, string parameter,
            InfoKind kindOfInfo, InfoKind kindOfSearch);

        /// <summary>
        /// 获取内容。
        /// </summary>
        /// <param name="filePos">给定的文件定位。</param>
        /// <param name="streamKind">给定的流种类。</param>
        /// <param name="streamNumber">给定的流编号。</param>
        /// <param name="parameter">给定的参数。</param>
        /// <param name="kindOfInfo">给定的信息种类。</param>
        /// <returns>返回字符串。</returns>
        string Get(int filePos, StreamKind streamKind, int streamNumber, int parameter,
            InfoKind kindOfInfo);

        /// <summary>
        /// 选项值信息。
        /// </summary>
        /// <param name="option">给定的选项。</param>
        /// <param name="value">给定的值。</param>
        /// <returns>返回字符串。</returns>
        string Option(string option, string value);


        /// <summary>
        /// 统计信息。
        /// </summary>
        /// <param name="filePos">给定的文件定位。</param>
        /// <param name="streamKind">给定的流种类。</param>
        /// <param name="streamNumber">给定的流编号。</param>
        /// <returns>返回指针或句柄平台特定类型。</returns>
        int Count(int filePos, StreamKind streamKind, int streamNumber);


        /// <summary>
        /// 状态信息。
        /// </summary>
        /// <returns>返回指针或句柄平台特定类型。</returns>
        int State();
    }


    /// <summary>
    /// <see cref="IMediaInfoListEngine"/> 静态扩展。
    /// </summary>
    public static class MediaInfoListEngineExtensions
    {
        /// <summary>
        /// 打开媒体文件。
        /// </summary>
        /// <param name="engine">给定的 MediaInfo 列表引擎。</param>
        /// <param name="fileName">给定的媒体文件。</param>
        /// <returns>返回指针或句柄平台特定类型。</returns>
        public static void Open(this IMediaInfoListEngine engine, string fileName)
        {
            engine.Open(fileName, 0);
        }


        /// <summary>
        /// 关闭媒体文件。
        /// </summary>
        /// <param name="engine">给定的 MediaInfo 列表引擎。</param>
        public static void Close(this IMediaInfoListEngine engine)
        {
            engine.Close(-1);
        }


        /// <summary>
        /// 获取内容。
        /// </summary>
        /// <param name="engine">给定的 MediaInfo 列表引擎。</param>
        /// <param name="filePos">给定的文件定位。</param>
        /// <param name="streamKind">给定的流种类。</param>
        /// <param name="streamNumber">给定的流编号。</param>
        /// <param name="parameter">给定的参数。</param>
        /// <returns>返回字符串。</returns>
        public static string Get(this IMediaInfoListEngine engine, int filePos, StreamKind streamKind,
            int streamNumber, string parameter)
        {
            return engine.Get(filePos, streamKind, streamNumber, parameter, InfoKind.Text, InfoKind.Name);
        }
        /// <summary>
        /// 获取内容。
        /// </summary>
        /// <param name="engine">给定的 MediaInfo 列表引擎。</param>
        /// <param name="filePos">给定的文件定位。</param>
        /// <param name="streamKind">给定的流种类。</param>
        /// <param name="streamNumber">给定的流编号。</param>
        /// <param name="parameter">给定的参数。</param>
        /// <param name="kindOfInfo">给定的信息种类。</param>
        /// <returns>返回字符串。</returns>
        public static string Get(this IMediaInfoListEngine engine, int filePos, StreamKind streamKind,
            int streamNumber, string parameter, InfoKind kindOfInfo)
        {
            return engine.Get(filePos, streamKind, streamNumber, parameter, kindOfInfo, InfoKind.Name);
        }

        /// <summary>
        /// 获取内容。
        /// </summary>
        /// <param name="engine">给定的 MediaInfo 列表引擎。</param>
        /// <param name="filePos">给定的文件定位。</param>
        /// <param name="streamKind">给定的流种类。</param>
        /// <param name="streamNumber">给定的流编号。</param>
        /// <param name="parameter">给定的参数。</param>
        /// <returns>返回字符串。</returns>
        public static string Get(this IMediaInfoListEngine engine, int filePos, StreamKind streamKind,
            int streamNumber, int parameter)
        {
            return engine.Get(filePos, streamKind, streamNumber, parameter, InfoKind.Text);
        }


        /// <summary>
        /// 选项值信息。
        /// </summary>
        /// <param name="engine">给定的 MediaInfo 列表引擎。</param>
        /// <param name="option">给定的选项。</param>
        /// <returns>返回字符串。</returns>
        public static string Option(this IMediaInfoListEngine engine, string option)
        {
            return engine.Option(option, string.Empty);
        }


        /// <summary>
        /// 统计信息。
        /// </summary>
        /// <param name="engine">给定的 MediaInfo 列表引擎。</param>
        /// <param name="filePos">给定的文件定位。</param>
        /// <param name="streamKind">给定的流种类。</param>
        /// <returns>返回指针或句柄平台特定类型。</returns>
        public static int Count(this IMediaInfoListEngine engine, int filePos, StreamKind streamKind)
        {
            return engine.Count(filePos, streamKind, -1);
        }

    }
}
