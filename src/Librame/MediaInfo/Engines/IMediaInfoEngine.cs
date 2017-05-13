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
using System.Collections.Generic;

namespace Librame.MediaInfo.Engines
{
    /// <summary>
    /// MediaInfo 引擎接口。
    /// </summary>
    public interface IMediaInfoEngine : IDisposable
    {
        /// <summary>
        /// 打开媒体文件。
        /// </summary>
        /// <param name="fileName">给定的媒体文件。</param>
        /// <returns>返回指针或句柄平台特定类型。</returns>
        int Open(string fileName);


        /// <summary>
        /// 关闭媒体文件。
        /// </summary>
        void Close();


        /// <summary>
        /// 报告媒体文件。
        /// </summary>
        /// <returns>返回字符串。</returns>
        string Inform();


        /// <summary>
        /// 获取内容。
        /// </summary>
        /// <param name="streamKind">给定的流种类。</param>
        /// <param name="streamNumber">给定的流编号。</param>
        /// <param name="parameter">给定的参数。</param>
        /// <param name="kindOfInfo">给定的信息种类。</param>
        /// <param name="kindOfSearch">给定的查询信息种类。</param>
        /// <returns>返回字符串。</returns>
        string Get(StreamKind streamKind, int streamNumber, string parameter, InfoKind kindOfInfo,
            InfoKind kindOfSearch);

        /// <summary>
        /// 获取内容。
        /// </summary>
        /// <param name="streamKind">给定的流种类。</param>
        /// <param name="streamNumber">给定的流编号。</param>
        /// <param name="parameter">给定的参数。</param>
        /// <param name="kindOfInfo">给定的信息种类。</param>
        /// <returns>返回字符串。</returns>
        string Get(StreamKind streamKind, int streamNumber, int parameter, InfoKind kindOfInfo);


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
        /// <param name="streamKind">给定的流种类。</param>
        /// <param name="streamNumber">给定的流编号。</param>
        /// <returns>返回指针或句柄平台特定类型。</returns>
        int Count(StreamKind streamKind, int streamNumber);


        /// <summary>
        /// 状态信息。
        /// </summary>
        /// <returns>返回指针或句柄平台特定类型。</returns>
        int State();
    }


    /// <summary>
    /// <see cref="IMediaInfoEngine"/> 静态扩展。
    /// </summary>
    public static class MediaInfoEngineExtensions
    {
        /// <summary>
        /// 获取轨道字典集合数组。
        /// </summary>
        /// <param name="engine">给定的 MediaInfo 引擎。</param>
        /// <param name="streamKind">给定的流种类。</param>
        /// <returns>返回字典集合数组。</returns>
        public static IDictionary<string, string>[] GetTracks(this IMediaInfoEngine engine,
            StreamKind streamKind)
        {
            var tracksCount = engine.Count(streamKind);
            if (tracksCount < 1)
                return null;

            var list = new List<IDictionary<string, string>>();

            for (var i = 0; i < tracksCount; i++)
            {
                var singleTrack = GetTrack(engine, streamKind, i);

                if (singleTrack != null && singleTrack.Count > 0)
                    list.Add(singleTrack);
            }

            if (list.Count < 1)
                return null;

            return list.ToArray();
        }

        /// <summary>
        /// 获取轨道字典集合。
        /// </summary>
        /// <param name="engine">给定的 MediaInfo 引擎。</param>
        /// <param name="streamKind">给定的流种类。</param>
        /// <param name="streamIndex">给定的流索引。</param>
        /// <returns>返回字典集合。</returns>
        public static IDictionary<string, string> GetTrack(this IMediaInfoEngine engine,
            StreamKind streamKind, int streamIndex)
        {
            var dictionary = new Dictionary<string, string>();

            int i = 0;
            while (true)
            {
                string key = engine.Get(streamKind, streamIndex, i++, InfoKind.Name);
                if (string.IsNullOrEmpty(key))
                    break;

                string value = engine.Get(streamKind, streamIndex, key);
                dictionary.Add(key, value);
            }

            return dictionary;
        }


        /// <summary>
        /// 获取指定条件的字符串信息。
        /// </summary>
        /// <param name="engine">给定的 MediaInfo 引擎。</param>
        /// <param name="streamKind">给定的流种类。</param>
        /// <param name="streamNumber">给定的流编号。</param>
        /// <param name="parameter">给定的参数。</param>
        /// <returns>返回字符串。</returns>
        public static string Get(this IMediaInfoEngine engine, StreamKind streamKind, int streamNumber,
            string parameter)
        {
            return engine.Get(streamKind, streamNumber, parameter, InfoKind.Text, InfoKind.Name);
        }

        /// <summary>
        /// 获取指定条件的字符串信息。
        /// </summary>
        /// <param name="engine">给定的 MediaInfo 引擎。</param>
        /// <param name="streamKind">给定的流种类。</param>
        /// <param name="streamNumber">给定的流编号。</param>
        /// <param name="parameter">给定的参数。</param>
        /// <param name="kindOfInfo">给定的信息种类。</param>
        /// <returns>返回字符串。</returns>
        public static string Get(this IMediaInfoEngine engine, StreamKind streamKind, int streamNumber,
            string parameter, InfoKind kindOfInfo)
        {
            return engine.Get(streamKind, streamNumber, parameter, kindOfInfo, InfoKind.Name);
        }

        /// <summary>
        /// 获取指定条件的字符串信息。
        /// </summary>
        /// <param name="engine">给定的 MediaInfo 引擎。</param>
        /// <param name="streamKind">给定的流种类。</param>
        /// <param name="streamNumber">给定的流编号。</param>
        /// <param name="parameter">给定的参数。</param>
        /// <returns>返回字符串。</returns>
        public static string Get(this IMediaInfoEngine engine, StreamKind streamKind, int streamNumber,
            int parameter)
        {
            return engine.Get(streamKind, streamNumber, parameter, InfoKind.Text);
        }


        /// <summary>
        /// 选项值信息。
        /// </summary>
        /// <param name="engine">给定的 MediaInfo 引擎。</param>
        /// <param name="option">给定的值。</param>
        /// <returns>返回字符串。</returns>
        public static string Option(this IMediaInfoEngine engine, string option)
        {
            return engine.Option(option, string.Empty);
        }


        /// <summary>
        /// 统计信息。
        /// </summary>
        /// <param name="engine">给定的 MediaInfo 引擎。</param>
        /// <param name="streamKind">给定的流种类。</param>
        /// <returns>返回指针或句柄平台特定类型。</returns>
        public static int Count(this IMediaInfoEngine engine, StreamKind streamKind)
        {
            return engine.Count(streamKind, -1);
        }

    }

}
