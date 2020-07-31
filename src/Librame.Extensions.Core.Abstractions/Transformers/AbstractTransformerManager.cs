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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Librame.Extensions.Core.Transformers
{
    using Utilities;

    /// <summary>
    /// 抽象变换器管理器。
    /// </summary>
    /// <typeparam name="TTransformer">指定的变换器类型。</typeparam>
    public abstract class AbstractTransformerManager<TTransformer>
        where TTransformer : class, ITransformer
    {
        private List<TTransformer> _transformers;


        /// <summary>
        /// 构造一个 <see cref="AbstractTransformerManager{TTransformer}"/>。
        /// </summary>
        public AbstractTransformerManager()
        {
            _transformers = AssemblyUtility.CreateCurrentThirdPartyExportedInstances<TTransformer>();
        }


        private static Func<TTransformer, bool> GetPredicate(Type sourceType, Type targetType)
            => s => s.SourceType == sourceType && s.TargetType == targetType;


        #region GetBySourceAndTarget

        /// <summary>
        /// 通过来源与目标类型获取 <typeparamref name="TTransformer"/>。
        /// </summary>
        /// <param name="sourceType">给定的来源类型。</param>
        /// <param name="targetType">给定的目标类型。</param>
        /// <param name="singleFactory">如果存在多个匹配时得到单个的工厂方法（可选；默认返回集合中第一个 <typeparamref name="TTransformer"/>）。</param>
        /// <returns>返回 <see cref="ITransformer"/>。</returns>
        public TTransformer GetBySourceAndTarget(Type sourceType, Type targetType,
            Func<IEnumerable<TTransformer>, TTransformer> singleFactory = null)
        {
            sourceType.NotNull(nameof(sourceType));
            targetType.NotNull(nameof(targetType));

            if (singleFactory.IsNull())
                singleFactory = s => s.First();

            var filters = _transformers.Where(GetPredicate(sourceType, targetType));
            return singleFactory.Invoke(filters);
        }

        #endregion


        #region Get

        /// <summary>
        /// 获取指定类型的 <typeparamref name="TTransformer"/>。
        /// </summary>
        /// <param name="transformerType">给定的 <typeparamref name="TTransformer"/> 类型。</param>
        /// <returns>返回 <see cref="ITransformer"/>。</returns>
        public TTransformer Get(Type transformerType)
        {
            transformerType.NotNull(nameof(transformerType));
            return _transformers.Single(s => s.GetType() == transformerType);
        }

        #endregion


        #region Replace

        /// <summary>
        /// 替换 <typeparamref name="TTransformer"/>。
        /// </summary>
        /// <param name="oldSourceType">给定的旧 <typeparamref name="TTransformer"/> 来源类型。</param>
        /// <param name="oldTargetType">给定的旧 <typeparamref name="TTransformer"/> 目标类型。</param>
        /// <param name="changeFactory">给定的替换工厂方法。</param>
        /// <returns>返回新 <see cref="ITransformer"/>。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        public TTransformer ReplaceBySourceAndTarget(Type oldSourceType, Type oldTargetType,
            Func<TTransformer, TTransformer> changeFactory)
        {
            oldSourceType.NotNull(nameof(oldSourceType));
            oldTargetType.NotNull(nameof(oldTargetType));
            changeFactory.NotNull(nameof(changeFactory));

            var oldTransformer = _transformers.First(GetPredicate(oldSourceType, oldTargetType));
            var newTransformer = changeFactory.Invoke(oldTransformer);

            return Replace(oldTransformer, newTransformer);
        }

        /// <summary>
        /// 替换 <typeparamref name="TTransformer"/>。
        /// </summary>
        /// <param name="newTransformer">给定的新 <typeparamref name="TTransformer"/>。</param>
        /// <param name="oldSourceType">给定的旧 <typeparamref name="TTransformer"/> 来源类型（可选；默认为新 <typeparamref name="TTransformer"/> 来源类型）。</param>
        /// <param name="oldTargetType">给定的旧 <typeparamref name="TTransformer"/> 目标类型（可选；默认为新 <typeparamref name="TTransformer"/> 来源类型）。</param>
        /// <returns>返回新 <typeparamref name="TTransformer"/>。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        public TTransformer Replace(TTransformer newTransformer,
            Type oldSourceType = null, Type oldTargetType = null)
        {
            newTransformer.NotNull(nameof(newTransformer));

            var predicate = GetPredicate(oldSourceType ?? newTransformer.SourceType,
                oldTargetType ?? newTransformer.TargetType);

            var oldTransformer = _transformers.First(predicate);

            return Replace(oldTransformer, newTransformer);
        }

        /// <summary>
        /// 替换 <typeparamref name="TTransformer"/>。
        /// </summary>
        /// <param name="oldTransformer">给定的旧 <typeparamref name="TTransformer"/>。</param>
        /// <param name="newTransformer">给定的新 <typeparamref name="TTransformer"/>。</param>
        /// <returns>返回新 <typeparamref name="TTransformer"/>。</returns>
        public TTransformer Replace(TTransformer oldTransformer,
            TTransformer newTransformer)
        {
            oldTransformer.NotNull(nameof(oldTransformer));
            newTransformer.NotNull(nameof(newTransformer));

            if (oldTransformer != newTransformer)
            {
                return ExtensionSettings.Preference.RunLocker(() =>
                {
                    _transformers.Remove(oldTransformer);
                    _transformers.Add(newTransformer);

                    return newTransformer;
                });
            }

            return oldTransformer;
        }

        #endregion

    }
}
