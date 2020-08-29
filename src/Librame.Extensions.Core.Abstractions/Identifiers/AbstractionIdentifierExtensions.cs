#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Core.Identifiers
{
    /// <summary>
    /// 标识符静态扩展。
    /// </summary>
    public static class AbstractionIdentifierExtensions
    {
        /// <summary>
        /// 异步设置标识。
        /// </summary>
        /// <typeparam name="TId">指定的标识类型（兼容各种引用与值类型标识）。</typeparam>
        /// <param name="identifier">给定的 <see cref="IIdentifier{TId}"/>。</param>
        /// <param name="newIdFactory">给定的新 <typeparamref name="TId"/> 工厂方法。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TId"/> （兼容各种引用与值类型标识）的异步操作。</returns>
        public static ValueTask<TId> SetIdAsync<TId>(this IIdentifier<TId> identifier,
            Func<TId, TId> newIdFactory, CancellationToken cancellationToken = default)
            where TId : IEquatable<TId>
        {
            identifier.NotNull(nameof(identifier));
            newIdFactory.NotNull(nameof(newIdFactory));

            return cancellationToken.RunOrCancelValueAsync(()
                => identifier.Id = newIdFactory.Invoke(identifier.Id));
        }


        /// <summary>
        /// 异步设置对象标识。
        /// </summary>
        /// <param name="identifier">给定的 <see cref="IObjectIdentifier"/>。</param>
        /// <param name="newIdFactory">给定的新对象标识工厂方法。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含标识（兼容各种引用与值类型标识）的异步操作。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static async ValueTask<object> SetObjectIdAsync(this IObjectIdentifier identifier,
            Func<object, object> newIdFactory, CancellationToken cancellationToken = default)
        {
            identifier.NotNull(nameof(identifier));
            newIdFactory.NotNull(nameof(newIdFactory));

            var newId = await identifier.GetObjectIdAsync(cancellationToken).ConfigureAwait();
            return await identifier.SetObjectIdAsync(newIdFactory.Invoke(newId), cancellationToken)
                .ConfigureAwait();
        }


        /// <summary>
        /// 导入生成式标识（默认支持 <see cref="Guid"/>、<see cref="long"/>、<see cref="string"/> 等类型标识的字符串形式）。
        /// </summary>
        /// <typeparam name="TGenId">指定的生成式标识类型（如：<see cref="Guid"/>、<see cref="long"/>、<see cref="string"/> 等类型）。</typeparam>
        /// <param name="identifier">给定的 <see cref="IGenerativeIdentifier{TGenId}"/>。</param>
        /// <param name="id">给定的字符串标识。</param>
        /// <param name="provider">给定的 <see cref="IFormatProvider"/>（可选；默认使用 <see cref="CultureInfo.InvariantCulture"/>）。</param>
        /// <returns>返回 <typeparamref name="TGenId"/>。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static TGenId ImportId<TGenId>(this IGenerativeIdentifier<TGenId> identifier,
            string id, IFormatProvider provider = null)
            where TGenId : IEquatable<TGenId>
        {
            identifier.NotNull(nameof(identifier));

            identifier.Id = (TGenId)id.ToGenerativeId(identifier.IdType, provider);
            return identifier.Id;
        }

        /// <summary>
        /// 导入增量式标识（默认支持所有整数类型标识的字符串形式）。
        /// </summary>
        /// <typeparam name="TIncremId">指定的增量式标识类型（如：整数型标识）。</typeparam>
        /// <param name="identifier">给定的 <see cref="IIncrementalIdentifier{TIncremId}"/>。</param>
        /// <param name="id">给定的字符串标识。</param>
        /// <param name="provider">给定的 <see cref="IFormatProvider"/>（可选；默认使用 <see cref="CultureInfo.InvariantCulture"/>）。</param>
        /// <returns>返回 <typeparamref name="TIncremId"/>。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static TIncremId ImportId<TIncremId>(this IIncrementalIdentifier<TIncremId> identifier,
            string id, IFormatProvider provider = null)
            where TIncremId : IEquatable<TIncremId>
        {
            identifier.NotNull(nameof(identifier));

            identifier.Id = (TIncremId)id.ToIncrementalId(identifier.IdType, provider);
            return identifier.Id;
        }


        /// <summary>
        /// 转为生成式标识（支持 <see cref="Guid"/>、<see cref="long"/>、<see cref="string"/> 等类型标识的字符串形式）。
        /// </summary>
        /// <typeparam name="TGenId">指定的生成式标识类型（支持 <see cref="Guid"/>、<see cref="long"/>、<see cref="string"/> 等类型）。</typeparam>
        /// <param name="id">给定的字符串标识。</param>
        /// <param name="provider">给定的 <see cref="IFormatProvider"/>（可选；默认使用 <see cref="CultureInfo.InvariantCulture"/>）。</param>
        /// <returns>返回 <typeparamref name="TGenId"/>。</returns>
        public static TGenId ToGenerativeId<TGenId>(this string id, IFormatProvider provider = null)
            where TGenId : IEquatable<TGenId>
            => (TGenId)id.ToGenerativeId(typeof(TGenId), provider);

        /// <summary>
        /// 转为生成式标识对象（支持 <see cref="Guid"/>、<see cref="long"/>、<see cref="string"/> 等类型标识的字符串形式）。
        /// </summary>
        /// <param name="id">给定的字符串标识。</param>
        /// <param name="idType">给定的标识类型（支持 <see cref="Guid"/>、<see cref="long"/>、<see cref="string"/> 等类型）。</param>
        /// <param name="provider">给定的 <see cref="IFormatProvider"/>（可选；默认使用 <see cref="CultureInfo.InvariantCulture"/>）。</param>
        /// <returns>返回标识对象。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static object ToGenerativeId(this string id, Type idType, IFormatProvider provider = null)
        {
            idType.NotNull(nameof(idType));

            if (provider.IsNull())
                provider = CultureInfo.InvariantCulture;

            object obj = idType.Name switch
            {
                "Guid" => Guid.Parse(id),
                "Int64" => long.Parse(id, provider),
                "String" => id,
                _ => new NotSupportedException()
            };

            return obj;
        }


        /// <summary>
        /// 转为增量式标识（支持所有整数类型标识的字符串形式）。
        /// </summary>
        /// <typeparam name="TIncremId">指定的增量式标识类型（支持所有整数类型）。</typeparam>
        /// <param name="id">给定的字符串标识。</param>
        /// <param name="provider">给定的 <see cref="IFormatProvider"/>（可选；默认使用 <see cref="CultureInfo.InvariantCulture"/>）。</param>
        /// <returns>返回 <typeparamref name="TIncremId"/>。</returns>
        public static TIncremId ToIncrementalId<TIncremId>(this string id, IFormatProvider provider = null)
            where TIncremId : IEquatable<TIncremId>
            => (TIncremId)id.ToIncrementalId(typeof(TIncremId), provider);

        /// <summary>
        /// 转为增量式标识对象（支持所有整数类型标识的字符串形式）。
        /// </summary>
        /// <param name="id">给定的字符串标识。</param>
        /// <param name="idType">给定的标识类型（支持所有整数类型）。</param>
        /// <param name="provider">给定的 <see cref="IFormatProvider"/>（可选；默认使用 <see cref="CultureInfo.InvariantCulture"/>）。</param>
        /// <returns>返回标识对象。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static object ToIncrementalId(this string id, Type idType, IFormatProvider provider = null)
        {
            idType.NotNull(nameof(idType));

            if (provider.IsNull())
                provider = CultureInfo.InvariantCulture;

            object obj = idType.Name switch
            {
                "SByte" => sbyte.Parse(id, provider),
                "Byte" => byte.Parse(id, provider),
                "Int16" => short.Parse(id, provider),
                "UInt16" => ushort.Parse(id, provider),
                "Int32" => int.Parse(id, provider),
                "UInt32" => uint.Parse(id, provider),
                "Int64" => long.Parse(id, provider),
                "UInt64" => ulong.Parse(id, provider),
                _ => new NotSupportedException()
            };

            return obj;
        }

    }
}
