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

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 新 URI 组合器。
    /// </summary>
    public class NewUriCombiner : AbstractCombiner<Uri>, IUriOptional
    {
        private readonly IUriOptional _optional;


        /// <summary>
        /// 构造一个新 URI 组合器。
        /// </summary>
        /// <param name="configureAction">给定的配置动作。</param>
        public NewUriCombiner(Action<IUriOptional> configureAction)
            : base(configureAction.Configure<IUriOptional, UriOptional>(out IUriOptional optional).ToUri())
        {
            _optional = optional;
        }


        /// <summary>
        /// 协议。
        /// </summary>
        public string Scheme
        {
            get => _optional.Scheme;
            set => _optional.Scheme = value;
        }

        /// <summary>
        /// 可能包含端口号的主机。
        /// </summary>
        public string Host
        {
            get => _optional.Host;
            set => _optional.Host = value;
        }

        /// <summary>
        /// 以 / 开始的路径。
        /// </summary>
        public string Path
        {
            get => _optional.Path;
            set => _optional.Path = value;
        }

        /// <summary>
        /// 以 ? 开始的查询。
        /// </summary>
        public string Query
        {
            get => _optional.Query;
            set => _optional.Query = value;
        }

        /// <summary>
        /// 以 # 开始的锚点。
        /// </summary>
        public string Anchor
        {
            get => _optional.Anchor;
            set => _optional.Anchor = value;
        }


        /// <summary>
        /// 是否相等（忽略大小写）。
        /// </summary>
        /// <param name="other">给定的域名。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(Uri other)
            => Source.ToString().Equals(other?.ToString(), StringComparison.OrdinalIgnoreCase);


        /// <summary>
        /// 转换为 URI。
        /// </summary>
        /// <returns>返回 <see cref="Uri"/>。</returns>
        public Uri ToUri()
            => _optional.ToUri();
    }


    /// <summary>
    /// URI 可选配置接口。
    /// </summary>
    public interface IUriOptional : IOptional
    {
        /// <summary>
        /// 协议。
        /// </summary>
        string Scheme { get; set; }

        /// <summary>
        /// 可能包含端口号的主机。
        /// </summary>
        string Host { get; set; }

        /// <summary>
        /// 以 / 开始的路径。
        /// </summary>
        string Path { get; set; }

        /// <summary>
        /// 以 ? 开始的查询。
        /// </summary>
        string Query { get; set; }

        /// <summary>
        /// 以 # 开始的锚点。
        /// </summary>
        string Anchor { get; set; }


        /// <summary>
        /// 转换为 URI。
        /// </summary>
        /// <returns>返回 <see cref="Uri"/>。</returns>
        Uri ToUri();
    }


    class UriOptional : IUriOptional
    {
        public string Scheme { get; set; }

        public string Host { get; set; }

        public string Path { get; set; }

        public string Query { get; set; }

        public string Anchor { get; set; }

        public Uri ToUri()
        {
            Scheme.NotEmpty(nameof(Scheme));
            Host.NotEmpty(nameof(Host));

            return new Uri($"{Scheme}{Uri.SchemeDelimiter}{Host}{Path}{Query}{Anchor}");
        }
    }
}
