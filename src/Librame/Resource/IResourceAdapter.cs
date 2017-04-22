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
    /// <summary>
    /// 资源适配器接口。
    /// </summary>
    public interface IResourceAdapter : Adaptation.IAdapter
    {
        /// <summary>
        /// 获取序列化器。
        /// </summary>
        Serialization.IResourceSerializer Serializer { get; }

        /// <summary>
        /// 获取监视器。
        /// </summary>
        IResourceWatcher Watcher { get; }


        /// <summary>
        /// 获取指定来源信息的资源管道。
        /// </summary>
        /// <param name="sourceInfo">给定的 <see cref="ResourceSourceInfo"/>。</param>
        /// <param name="defaultSchema">给定的默认 <see cref="Schema.ResourceSchema"/>（可选）。</param>
        /// <returns>返回 <see cref="IResourceProvider"/>。</returns>
        IResourceProvider GetProvider(ResourceSourceInfo sourceInfo, Schema.ResourceSchema defaultSchema = null);
        
        /// <summary>
        /// 获取指定来源信息的资源结构。
        /// </summary>
        /// <typeparam name="TSchema">指定的资源结构类型。</typeparam>
        /// <param name="sourceInfo">给定的 <see cref="ResourceSourceInfo"/>。</param>
        /// <param name="defaultSchema">给定的默认资源结构（可选）。</param>
        /// <returns>返回资源结构。</returns>
        TSchema GetSchema<TSchema>(ResourceSourceInfo sourceInfo, TSchema defaultSchema = default(TSchema)) where TSchema : Schema.ResourceSchema;

    }
}
