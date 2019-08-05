#region License

/* **************************************************************************************
 * Copyright (c) zwbwl All rights reserved.
 * 
 * http://51zwb.com
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Collections.Concurrent;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// URI 定位器接口。
    /// </summary>
    public interface IUriLocator : ILocator<Uri>, IEquatable<IUriLocator>
    {
        /// <summary>
        /// 协议。
        /// </summary>
        string Scheme { get; }

        /// <summary>
        /// 可能包含端口号的主机。
        /// </summary>
        string Host { get; }

        /// <summary>
        /// 以 / 开始的路径。
        /// </summary>
        string Path { get; }

        /// <summary>
        /// 以 ? 开始的查询。
        /// </summary>
        string Query { get; }

        /// <summary>
        /// 以 # 开始的锚点。
        /// </summary>
        string Anchor { get; }

        /// <summary>
        /// 域名定位器。
        /// </summary>
        /// <value>返回 <see cref="IDomainNameLocator"/>。</value>
        IDomainNameLocator DomainName { get; }


        /// <summary>
        /// 改变协议。
        /// </summary>
        /// <param name="newScheme">给定的新协议。</param>
        /// <returns>返回 <see cref="IUriLocator"/>。</returns>
        IUriLocator ChangeScheme(string newScheme);

        /// <summary>
        /// 改变主机。
        /// </summary>
        /// <param name="newHost">给定可能包含端口号的新主机。</param>
        /// <returns>返回 <see cref="IUriLocator"/>。</returns>
        IUriLocator ChangeHost(string newHost);

        /// <summary>
        /// 改变路径。
        /// </summary>
        /// <param name="newPath">给定以 / 开始的新路径。</param>
        /// <returns>返回 <see cref="IUriLocator"/>。</returns>
        IUriLocator ChangePath(string newPath);

        /// <summary>
        /// 改变查询。
        /// </summary>
        /// <param name="newQuery">给定以 ? 开始的新查询。</param>
        /// <returns>返回 <see cref="IUriLocator"/>。</returns>
        IUriLocator ChangeQuery(string newQuery);

        /// <summary>
        /// 改变查询参数集合。
        /// </summary>
        /// <param name="queriesAction">给定的改变查询参数集合动作（内部支持对参数值的特殊字符进行转码处理）。</param>
        /// <returns>返回 <see cref="IUriLocator"/>。</returns>
        IUriLocator ChangeQueries(Action<ConcurrentDictionary<string, string>> queriesAction);

        /// <summary>
        /// 改变锚点。
        /// </summary>
        /// <param name="newAnchor">给定以 # 开始的新锚点。</param>
        /// <returns>返回 <see cref="IUriLocator"/>。</returns>
        IUriLocator ChangeAnchor(string newAnchor);


        /// <summary>
        /// 使用指定的新协议新建一个 <see cref="IUriLocator"/> 实例。
        /// </summary>
        /// <param name="newScheme">给定的新协议。</param>
        /// <returns>返回 <see cref="IUriLocator"/>。</returns>
        IUriLocator NewScheme(string newScheme);

        /// <summary>
        /// 使用指定的新主机新建一个 <see cref="IUriLocator"/> 实例。
        /// </summary>
        /// <param name="newHost">给定可能包含端口号的新主机。</param>
        /// <returns>返回 <see cref="IUriLocator"/>。</returns>
        IUriLocator NewHost(string newHost);

        /// <summary>
        /// 使用指定的新路径新建一个 <see cref="IUriLocator"/> 实例。
        /// </summary>
        /// <param name="newPath">给定以 / 开始的新路径。</param>
        /// <returns>返回 <see cref="IUriLocator"/>。</returns>
        IUriLocator NewPath(string newPath);

        /// <summary>
        /// 使用指定的新查询新建一个 <see cref="IUriLocator"/> 实例。
        /// </summary>
        /// <param name="newQuery">给定以 ? 开始的新查询。</param>
        /// <returns>返回 <see cref="IUriLocator"/>。</returns>
        IUriLocator NewQuery(string newQuery);

        /// <summary>
        /// 使用指定的查询参数数组新建一个 <see cref="IUriLocator"/> 实例。
        /// </summary>
        /// <param name="queriesAction">给定的改变查询参数集合动作（内部支持对参数值的特殊字符进行转码处理）。</param>
        /// <returns>返回 <see cref="IUriLocator"/>。</returns>
        IUriLocator NewQueries(Action<ConcurrentDictionary<string, string>> queriesAction);

        /// <summary>
        /// 使用指定的新锚点新建一个 <see cref="IUriLocator"/> 实例。
        /// </summary>
        /// <param name="newAnchor">给定以 # 开始的新锚点。</param>
        /// <returns>返回 <see cref="IUriLocator"/>。</returns>
        IUriLocator NewAnchor(string newAnchor);
    }
}
