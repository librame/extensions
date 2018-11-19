#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections.Generic;
using System.Reflection;

namespace Librame.Extensions
{
    /// <summary>
    /// 对象静态扩展。
    /// </summary>
    public static class TypeExtensions
    {

        /// <summary>
        /// 填入属性集合。
        /// </summary>
        /// <param name="source">给定的来源类型实例。</param>
        /// <param name="target">给定的目标类型实例。</param>
        public static void PopulateProperties<TSource, TTarget>(this TSource source, TTarget target)
        {
            source.NotDefault(nameof(source));
            target.NotDefault(nameof(target));

            var srcProperties = new List<PropertyInfo>(typeof(TSource).GetProperties());
            var trgtProperties = new List<PropertyInfo>(typeof(TTarget).GetProperties());

            for (var s = 0; s < srcProperties.Count; s++)
            {
                for (var t = 0; t < trgtProperties.Count; t++)
                {
                    var srcProperty = srcProperties[s];
                    var trgtProperty = trgtProperties[t];

                    if (srcProperty.Name == trgtProperty.Name)
                    {
                        var value = srcProperty.GetValue(source);
                        trgtProperty.SetValue(target, value);

                        trgtProperties.Remove(trgtProperty);
                        srcProperties.Remove(srcProperty);
                        
                        break;
                    }
                }
            }
        }

    }
}
