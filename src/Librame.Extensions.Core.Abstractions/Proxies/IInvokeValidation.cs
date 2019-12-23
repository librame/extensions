#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Reflection;

namespace Librame.Extensions.Core.Proxies
{
    /// <summary>
    /// 调用验证接口。
    /// </summary>
    public interface IInvokeValidation
    {
        /// <summary>
        /// 验证成员参数值。
        /// </summary>
        /// <param name="memberInfo">给定的 <see cref="MemberInfo"/>。</param>
        /// <param name="args">给定的参数数组。</param>
        /// <returns>返回验证是否成功的布尔值。</returns>
        bool Validate(MemberInfo memberInfo, object[] args);

        /// <summary>
        /// 验证成员参数值。
        /// </summary>
        /// <param name="memberInfo">给定的 <see cref="MemberInfo"/>。</param>
        /// <param name="args">给定的参数数组。</param>
        /// <param name="errorMessage">输出错误消息。</param>
        /// <returns>返回验证是否成功的布尔值。</returns>
        bool Validate(MemberInfo memberInfo, object[] args, out string errorMessage);
    }
}
