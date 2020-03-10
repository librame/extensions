#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core.Serializers
{
    /// <summary>
    /// 字符串序列化器接口。
    /// </summary>
    /// <typeparam name="TSource">指定的来源类型。</typeparam>
    public interface IStringSerializer<TSource> : ISerializer<TSource, string>
    {
    }
}
