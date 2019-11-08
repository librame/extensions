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
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace Librame.Extensions.Core
{
    using Resources;

    /// <summary>
    /// 调用验证。
    /// </summary>
    public class InvokeValidation : IInvokeValidation
    {
        private static readonly Type _baseAttributeType
            = typeof(ValidationAttribute);


        /// <summary>
        /// 验证成员参数值。
        /// </summary>
        /// <param name="memberInfo">给定的 <see cref="MemberInfo"/>。</param>
        /// <param name="args">给定的参数数组。</param>
        /// <returns>返回验证是否成功的布尔值。</returns>
        public bool Validate(MemberInfo memberInfo, object[] args)
            => Validate(memberInfo, args, out _);

        /// <summary>
        /// 验证成员参数值。
        /// </summary>
        /// <param name="memberInfo">给定的 <see cref="MemberInfo"/>。</param>
        /// <param name="args">给定的参数数组。</param>
        /// <param name="errorMessage">输出错误消息。</param>
        /// <returns>返回验证是否成功的布尔值。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "memberInfo")]
        public bool Validate(MemberInfo memberInfo, object[] args, out string errorMessage)
        {
            memberInfo.NotNull(nameof(memberInfo));

            var attributeDatas = memberInfo.CustomAttributes.Where(attrib
                => attrib.AttributeType.IsAssignableToBaseType(_baseAttributeType)).ToList();

            if (attributeDatas.Count > 0)
            {
                var value = args.Length > 0 ? args[0] : args;
                foreach (var attributeData in attributeDatas)
                {
                    var validation = (ValidationAttribute)memberInfo.GetCustomAttribute(attributeData.AttributeType);
                    if (!validation.IsValid(value))
                    {
                        errorMessage = GetValidationErrorMessage(validation, memberInfo, value);
                        return false;
                    }
                }
            }

            errorMessage = null;
            return true;
        }

        /// <summary>
        /// 获取验证错误消息。
        /// </summary>
        /// <param name="validation">给定的 <see cref="ValidationAttribute"/>。</param>
        /// <param name="memberInfo">给定的 <see cref="MemberInfo"/>。</param>
        /// <param name="value">给定的参数值。</param>
        /// <returns>返回字符串。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        protected virtual string GetValidationErrorMessage(ValidationAttribute validation, MemberInfo memberInfo, object value)
        {
            if (validation.ErrorMessage.IsNotEmpty())
                return validation.ErrorMessage;

            var errorMessage = validation.FormatErrorMessage(memberInfo.Name);
            if (errorMessage.IsNotEmpty())
                return errorMessage;

            errorMessage = value?.ToString() ?? "null";
            return InternalResource.ValidationErrorMessageFormat.Format(memberInfo.MemberType.ToString().AsCamelCasing(),
                errorMessage, validation.GetType().GetSimpleFullName());
        }

    }
}
