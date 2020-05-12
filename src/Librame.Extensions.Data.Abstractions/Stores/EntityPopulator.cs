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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Stores
{
    using Core.Services;

    /// <summary>
    /// 实体填充器。
    /// </summary>
    public static class EntityPopulator
    {
        private static readonly Type _creationGenericTypeDefinition
            = typeof(ICreation<,>);

        private static readonly Type _updationGenericTypeDefinition
            = typeof(IUpdation<,>);


        /// <summary>
        /// 格式化类型名称。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <returns>返回字符串。</returns>
        public static string FormatTypeName<T>()
            => FormatTypeName(typeof(T));

        /// <summary>
        /// 格式化类型名称。
        /// </summary>
        /// <param name="referenceType">给定的引用类型。</param>
        /// <param name="viewMaxLength">查看的最大长度。</param>
        /// <returns>返回字符串。</returns>
        public static string FormatTypeName(Type referenceType, int viewMaxLength = 100)
        {
            var name = referenceType.GetDisplayNameWithNamespace();

            if (name.Length > viewMaxLength)
                name = $"{referenceType.GetGenericBodyName()}...";

            return name;
        }


        /// <summary>
        /// 填充默认租户。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="defaultTenant"/> is null.
        /// </exception>
        /// <param name="defaultTenant">给定的 <see cref="ITenant"/>。</param>
        /// <returns>返回 <see cref="ITenant"/>。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        public static ITenant PopulateDefaultTenant(ITenant defaultTenant)
        {
            defaultTenant.NotNull(nameof(defaultTenant));

            if (defaultTenant.Name.IsEmpty())
                defaultTenant.Name = "DefaultTenant";

            if (defaultTenant.Host.IsEmpty())
                defaultTenant.Host = "localhost";

            if (defaultTenant.DefaultConnectionString.IsEmpty())
                defaultTenant.DefaultConnectionString = "librame_data_default";

            if (defaultTenant.WritingConnectionString.IsEmpty())
                defaultTenant.WritingConnectionString = "librame_data_writing";

            return defaultTenant;
        }


        /// <summary>
        /// 异步填充创建属性（此方法需确保创建者属性类型为字符串类型）。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="entity"/> or <paramref name="clock"/> is null.
        /// </exception>
        /// <typeparam name="TInvoke">指定的调用类型。</typeparam>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="entity">给定的实体对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含布尔值的异步操作。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static Task<bool> PopulateCreationAsync<TInvoke>(IClockService clock, object entity,
            CancellationToken cancellationToken = default)
            => PopulateCreationAsync(clock, entity, FormatTypeName<TInvoke>(), cancellationToken);

        /// <summary>
        /// 异步填充创建属性。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="entity"/> or <paramref name="clock"/> is null.
        /// </exception>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="entity">给定的实体对象。</param>
        /// <param name="createdBy">给定的创建者对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含布尔值的异步操作。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static async Task<bool> PopulateCreationAsync(IClockService clock, object entity, object createdBy,
            CancellationToken cancellationToken = default)
        {
            entity.NotNull(nameof(entity));

            var entityType = entity.GetType();

            if (!entityType.IsImplementedInterface(_creationGenericTypeDefinition, out Type genericType))
                return false;

            clock.NotNull(nameof(clock));

            var createdByProperty = entityType.GetProperty(nameof(ICreation<string, DateTime>.CreatedBy));
            var createdTimeProperty = entityType.GetProperty(nameof(ICreation<string, DateTime>.CreatedTime));

            var createdTimeType = genericType.GetGenericArguments().Last();

            // Set CreatedBy
            createdByProperty.SetValue(entity, createdBy);

            object createdTime = null;

            if (createdTimeType == typeof(DateTime))
                createdTime = await clock.GetNowAsync(cancellationToken: cancellationToken).ConfigureAndResultAsync();

            if (createdTimeType == typeof(DateTimeOffset))
                createdTime = await clock.GetNowOffsetAsync(cancellationToken: cancellationToken).ConfigureAndResultAsync();

            if (createdTime.IsNotNull())
            {
                // Set CreatedTime
                createdTimeProperty.SetValue(entity, createdTime);

                if (entityType.IsImplementedInterface<ICreatedTimeTicks>())
                {
                    var ticks = createdTime.GetType()
                        .GetProperty(nameof(DateTime.Ticks))
                        .GetValue(createdTime);

                    entityType.GetProperty(nameof(ICreatedTimeTicks.CreatedTimeTicks))
                        .SetValue(entity, ticks);
                }
            }

            return true;
        }


        /// <summary>
        /// 异步填充更新属性（此方法需确保创建者属性类型为字符串类型）。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="entity"/> or <paramref name="clock"/> is null.
        /// </exception>
        /// <typeparam name="TInvoke">指定的调用类型。</typeparam>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="entity">给定的实体对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含布尔值的异步操作。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static Task<bool> PopulateUpdationAsync<TInvoke>(IClockService clock, object entity,
            CancellationToken cancellationToken = default)
            => PopulateUpdationAsync(clock, entity, FormatTypeName<TInvoke>(), cancellationToken);

        /// <summary>
        /// 异步填充更新属性。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="entity"/> or <paramref name="clock"/> is null.
        /// </exception>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="entity">给定的实体对象。</param>
        /// <param name="updatedBy">给定的更新者对象。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含布尔值的异步操作。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static async Task<bool> PopulateUpdationAsync(IClockService clock, object entity, object updatedBy,
            CancellationToken cancellationToken = default)
        {
            entity.NotNull(nameof(entity));

            var entityType = entity.GetType();

            if (!entityType.IsImplementedInterface(_updationGenericTypeDefinition, out Type genericType))
                return false;

            clock.NotNull(nameof(clock));

            var updatedByProperty = entityType.GetProperty(nameof(IUpdation<string, DateTime>.UpdatedBy));
            var updatedTimeProperty = entityType.GetProperty(nameof(IUpdation<string, DateTime>.UpdatedTime));
            var createdByProperty = entityType.GetProperty(nameof(IUpdation<string, DateTime>.CreatedBy));
            var createdTimeProperty = entityType.GetProperty(nameof(IUpdation<string, DateTime>.CreatedTime));

            var updatedTimeType = genericType.GetGenericArguments().Last();

            // Set UpdatedBy
            updatedByProperty.SetValue(entity, updatedBy);
            // CreatedBy = UpdatedBy
            createdByProperty.SetValue(entity, updatedBy);

            object updatedTime = null;

            if (updatedTimeType == typeof(DateTime))
                updatedTime = await clock.GetNowAsync(cancellationToken: cancellationToken).ConfigureAndResultAsync();

            if (updatedTimeType == typeof(DateTimeOffset))
                updatedTime = await clock.GetNowOffsetAsync(cancellationToken: cancellationToken).ConfigureAndResultAsync();

            if (updatedTime.IsNotNull())
            {
                // Set UpdatedTime
                updatedTimeProperty.SetValue(entity, updatedTime);
                // CreatedTime = UpdatedTime
                createdTimeProperty.SetValue(entity, updatedTime);

                if (entityType.IsImplementedInterface<IUpdatedTimeTicks>())
                {
                    var ticks = (long)updatedTime.GetType()
                        .GetProperty(nameof(DateTime.Ticks))
                        .GetValue(updatedTime);

                    entityType.GetProperty(nameof(IUpdatedTimeTicks.UpdatedTimeTicks))
                        .SetValue(entity, ticks);
                    entityType.GetProperty(nameof(IUpdatedTimeTicks.CreatedTimeTicks))
                        .SetValue(entity, ticks);
                }
            }

            return true;
        }

    }
}
