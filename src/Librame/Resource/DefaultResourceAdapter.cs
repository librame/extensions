#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Resource
{
    using Utility;

    /// <summary>
    /// 默认资源适配器。
    /// </summary>
    public class DefaultResourceAdapter : AbstractResourceAdapter, IResourceAdapter
    {
        /// <summary>
        /// 获取序列化器。
        /// </summary>
        /// <remarks>
        /// 默认使用 <see cref="Serialization.JsonResourceSerializer"/>。
        /// </remarks>
        public virtual Serialization.IResourceSerializer Serializer
        {
            get { return SingletonManager.Resolve(key => new Serialization.JsonResourceSerializer(Settings)); }
        }

        /// <summary>
        /// 获取监视器。
        /// </summary>
        public virtual IResourceWatcher Watcher
        {
            get { return SingletonManager.Resolve(key => new ResourceWatcher(Settings)); }
        }


        /// <summary>
        /// 获取指定来源信息的资源管道。
        /// </summary>
        /// <param name="sourceInfo">给定的 <see cref="ResourceSourceInfo"/>。</param>
        /// <param name="defaultSchema">给定的默认 <see cref="Schema.ResourceSchema"/>（可选）。</param>
        /// <returns>返回 <see cref="IResourceProvider"/>。</returns>
        public virtual IResourceProvider GetProvider(ResourceSourceInfo sourceInfo, Schema.ResourceSchema defaultSchema = null)
        {
            return new ResourceProvider(Settings, Serializer, Watcher, sourceInfo, defaultSchema);
        }

        /// <summary>
        /// 获取指定来源信息的资源结构。
        /// </summary>
        /// <typeparam name="TSchema">指定的资源结构类型。</typeparam>
        /// <param name="sourceInfo">给定的 <see cref="ResourceSourceInfo"/>。</param>
        /// <param name="defaultSchema">给定的默认资源结构（可选）。</param>
        /// <returns>返回资源结构。</returns>
        public virtual TSchema GetSchema<TSchema>(ResourceSourceInfo sourceInfo, TSchema defaultSchema = default(TSchema))
            where TSchema : Schema.ResourceSchema
        {
            var provider = GetProvider(sourceInfo, defaultSchema);
            return (TSchema)provider.ExistingSchema;
        }

    }
}
