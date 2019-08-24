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

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 实体错误。
    /// </summary>
    public class EntityError
    {
        /// <summary>
        /// 构造一个 <see cref="EntityError"/> 默认实例。
        /// </summary>
        public EntityError()
        {
        }

        /// <summary>
        /// 构造一个 <see cref="EntityError"/>。
        /// </summary>
        /// <param name="code">给定的代码。</param>
        /// <param name="description">给定的描述。</param>
        public EntityError(string code, string description)
        {
            Code = code;
            Description = description;
        }


        /// <summary>
        /// 代码。
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 描述。
        /// </summary>
        public string Description { get; set; }


        /// <summary>
        /// 转换为实体错误。
        /// </summary>
        /// <param name="exception">给定的 <see cref="Exception"/>。</param>
        /// <returns>返回 <see cref="EntityError"/>。</returns>
        public static EntityError ToError(Exception exception)
        {
            exception.NotNull(nameof(exception));

            return new EntityError
            {
                Code = exception.HResult.ToString(),
                Description = exception.AsInnerMessage()
            };
        }
    }
}
