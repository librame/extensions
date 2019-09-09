#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 表名架构静态扩展。
    /// </summary>
    public static class TableNameSchemaExtensions
    {
        /// <summary>
        /// 改变为内部前缀表名。
        /// </summary>
        /// <param name="descriptor">给定的 <see cref="TableNameDescriptor"/>。</param>
        /// <param name="options">给定的 <see cref="TableNameSchemaOptions"/>。</param>
        /// <returns>返回 <see cref="TableNameDescriptor"/>。</returns>
        public static TableNameDescriptor ChangeInternalPrefix(this TableNameDescriptor descriptor, TableNameSchemaOptions options)
        {
            return descriptor
                .Change(options)
                .ChangePrefix(options.InternalPrefix);
        }

        /// <summary>
        /// 改变为私有前缀表名。
        /// </summary>
        /// <param name="descriptor">给定的 <see cref="TableNameDescriptor"/>。</param>
        /// <param name="options">给定的 <see cref="TableNameSchemaOptions"/>。</param>
        /// <returns>返回 <see cref="TableNameDescriptor"/>。</returns>
        public static TableNameDescriptor ChangePrivatePrefix(this TableNameDescriptor descriptor, TableNameSchemaOptions options)
        {
            return descriptor
                .Change(options)
                .ChangePrefix(options.PrivatePrefix);
        }

        /// <summary>
        /// 改变表名。
        /// </summary>
        /// <param name="descriptor">给定的 <see cref="TableNameDescriptor"/>。</param>
        /// <param name="options">给定的 <see cref="TableNameSchemaOptions"/>。</param>
        /// <returns>返回 <see cref="TableNameDescriptor"/>。</returns>
        public static TableNameDescriptor Change(this TableNameDescriptor descriptor, TableNameSchemaOptions options)
        {
            descriptor.NotNull(nameof(descriptor));
            options.NotNull(nameof(options));

            if (options.DefaultConnector.IsNotNullOrEmpty())
                descriptor.ChangeConnector(options.DefaultConnector);

            return descriptor;
        }

    }
}
