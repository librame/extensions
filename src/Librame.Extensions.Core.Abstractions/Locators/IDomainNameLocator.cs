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
    /// 域名定位器接口。
    /// </summary>
    public interface IDomainNameLocator : ILocator<string>, IEquatable<IDomainNameLocator>
    {
        /// <summary>
        /// 根域名（如：com/org...）。
        /// </summary>
        string Root { get; }

        /// <summary>
        /// 顶级/一级片段（如：top.com/top.org... 中的 top）。
        /// </summary>
        string TopLevelSegment { get; }

        /// <summary>
        /// 二级片段（如：second.top.com/second.top.org... 中的 second）。
        /// </summary>
        string SecondLevelSegment { get; }

        /// <summary>
        /// 三级片段（如：third.second.top.com/third.second.top.org... 中的 third）。
        /// </summary>
        string ThirdLevelSegment { get; }

        /// <summary>
        /// 其他级别片段集合（除三级外所有子级）。
        /// </summary>
        string[] OtherLevelSegments { get; }


        /// <summary>
        /// 顶级/一级域名（如：top.com/top.org...）。
        /// </summary>
        string TopLevel { get; }

        /// <summary>
        /// 二级域名（如：second.top.com/second.top.org...）。
        /// </summary>
        string SecondLevel { get; }

        /// <summary>
        /// 三级域名（如：third.second.top.com/third.second.top.org...）。
        /// </summary>
        string ThirdLevel { get; }


        #region Changes

        /// <summary>
        /// 改变域名。
        /// </summary>
        /// <param name="newDomainName">给定的新域名（支持包括根、一级、二级、三级、多级等在内的所有域名形式）。</param>
        /// <returns>返回 <see cref="IDomainNameLocator"/>。</returns>
        IDomainNameLocator ChangeDomainName(string newDomainName);

        /// <summary>
        /// 改变根。
        /// </summary>
        /// <param name="newRoot">给定的根（如：com/org...）。</param>
        /// <returns>返回 <see cref="IDomainNameLocator"/>。</returns>
        IDomainNameLocator ChangeRoot(string newRoot);

        /// <summary>
        /// 改变顶级/一级片段。
        /// </summary>
        /// <param name="newTopLevelSegment">给定的新顶级/一级片段（如：top.com/top.org... 中的 top）。</param>
        /// <returns>返回 <see cref="IDomainNameLocator"/>。</returns>
        IDomainNameLocator ChangeTopLevelSegment(string newTopLevelSegment);

        /// <summary>
        /// 改变二级片段。
        /// </summary>
        /// <param name="newSecondLevelSegment">给定的二级片段（如：second.top.com/second.top.org... 中的 second）。</param>
        /// <returns>返回 <see cref="IDomainNameLocator"/>。</returns>
        IDomainNameLocator ChangeSecondLevelSegment(string newSecondLevelSegment);

        /// <summary>
        /// 改变三级片段。
        /// </summary>
        /// <param name="newThirdLevelSegment">给定的三级片段（如：third.second.top.com/third.second.top.org... 中的 third）。</param>
        /// <returns>返回 <see cref="IDomainNameLocator"/>。</returns>
        IDomainNameLocator ChangeThirdLevelSegment(string newThirdLevelSegment);

        /// <summary>
        /// 改变其他级别片段集合（除三级外所有子级）。
        /// </summary>
        /// <param name="newOtherLevelSegments">给定的新其他级别片段集合（除三级外所有子级）。</param>
        /// <returns>返回 <see cref="IDomainNameLocator"/>。</returns>
        IDomainNameLocator ChangeOtherLevelSegments(params string[] newOtherLevelSegments);

        #endregion


        #region News

        /// <summary>
        /// 新建域名。
        /// </summary>
        /// <param name="newDomainName">给定的新域名（支持包括根、一级、二级、三级、多级等在内的所有域名）。</param>
        /// <returns>返回 <see cref="IDomainNameLocator"/>。</returns>
        IDomainNameLocator NewDomainName(string newDomainName);

        /// <summary>
        /// 新建根。
        /// </summary>
        /// <param name="newRoot">给定的根（如：com/org...）。</param>
        /// <returns>返回 <see cref="IDomainNameLocator"/>。</returns>
        IDomainNameLocator NewRoot(string newRoot);

        /// <summary>
        /// 新建顶级/一级片段。
        /// </summary>
        /// <param name="newTopLevelSegment">给定的新顶级/一级片段（如：top.com/top.org... 中的 top）。</param>
        /// <returns>返回 <see cref="IDomainNameLocator"/>。</returns>
        IDomainNameLocator NewTopLevelSegment(string newTopLevelSegment);

        /// <summary>
        /// 新建二级片段。
        /// </summary>
        /// <param name="newSecondLevelSegment">给定的二级片段（如：second.top.com/second.top.org... 中的 second）。</param>
        /// <returns>返回 <see cref="IDomainNameLocator"/>。</returns>
        IDomainNameLocator NewSecondLevelSegment(string newSecondLevelSegment);

        /// <summary>
        /// 新建三级片段。
        /// </summary>
        /// <param name="newThirdLevelSegment">给定的三级片段（如：third.second.top.com/third.second.top.org... 中的 third）。</param>
        /// <returns>返回 <see cref="IDomainNameLocator"/>。</returns>
        IDomainNameLocator NewThirdLevelSegment(string newThirdLevelSegment);

        /// <summary>
        /// 新建其他级别片段集合（除三级外所有子级）。
        /// </summary>
        /// <param name="newOtherLevelSegments">给定的新其他级别片段集合（除三级外所有子级）。</param>
        /// <returns>返回 <see cref="IDomainNameLocator"/>。</returns>
        IDomainNameLocator NewOtherLevelSegments(params string[] newOtherLevelSegments);

        #endregion

    }
}
