#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Practices.Unity;

namespace Librame.Container
{
    using Utility;

    /// <summary>
    /// 容器助手。
    /// </summary>
    public class ContainerHelper
    {
        //container.RegisterType(typeof(IGenericClass<>), typeof(GenericClass<>));
        //<register type="IGenericClass[]" mapTo="GenericClass[]" />

        
        #region Resolver Override

        // ResolverOverride 实现类型
        //Microsoft.Practices.Unity.CompositeResolverOverride
        //Microsoft.Practices.Unity.DependencyOverride
        //Microsoft.Practices.Unity.OverrideCollection<TOverride, TKey, TValue>
        //Microsoft.Practices.Unity.ParameterOverride
        //Microsoft.Practices.Unity.PropertyOverride
        //Microsoft.Practices.Unity.TypeBasedOverride

        /// <summary>
        /// 建立参数重载（如构造参数重载）。
        /// </summary>
        /// <param name="parameterName">给定的参数名。</param>
        /// <param name="parameterValue">给定的参数值。</param>
        /// <returns>返回 <see cref="PropertyOverride"/>。</returns>
        public static ParameterOverride BuildParameterOverride(string parameterName, object parameterValue)
        {
            return new ParameterOverride(parameterName, parameterValue);
        }

        /// <summary>
        /// 建立属性重载。
        /// </summary>
        /// <param name="propertyName">给定的属性名。</param>
        /// <param name="propertyValue">给定的属性值。</param>
        /// <returns>返回 <see cref="PropertyOverride"/>。</returns>
        public static PropertyOverride BuildPropertyOverride(string propertyName, object propertyValue)
        {
            return new PropertyOverride(propertyName, propertyValue);
        }

        /// <summary>
        /// 建立依赖重载。
        /// </summary>
        /// <typeparam name="T">指定的依赖类型。</typeparam>
        /// <param name="dependencyValue">给定的依赖值。</param>
        /// <returns>返回 <see cref="DependencyOverride{T}"/>。</returns>
        public static DependencyOverride<T> BuildDependencyOverride<T>(object dependencyValue)
        {
            return new DependencyOverride<T>(dependencyValue);
        }

        #endregion


        #region Injection Member

        // InjectionMember 实现类型
        //Microsoft.Practices.Unity.InjectionConstructor
        //Microsoft.Practices.Unity.InjectionMethod
        //Microsoft.Practices.Unity.InjectionProperty

        #endregion

    }
}
