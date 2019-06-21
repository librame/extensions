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
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 内部注入服务。
    /// </summary>
    internal class InternalInjectionService : AbstractDisposable<InternalInjectionService>, IInjectionService
    {
        Dictionary<Type, Action<object, IServiceProvider>> _injectedActions
            = new Dictionary<Type, Action<object, IServiceProvider>>();


        /// <summary>
        /// 构造一个 <see cref="InternalInjectionService"/> 实例。
        /// </summary>
        /// <param name="serviceProvider">给定的 <see cref="IServiceProvider"/>。</param>
        public InternalInjectionService(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider.NotNull(nameof(serviceProvider));
        }


        /// <summary>
        /// 服务提供程序。
        /// </summary>
        public IServiceProvider ServiceProvider { get; private set; }


        /// <summary>
        /// 服务注入。
        /// </summary>
        /// <param name="service">给定的服务对象。</param>
        public void Inject(object service)
        {
            service.NotNull(nameof(service));

            var serviceType = service.GetType();

            if (_injectedActions.TryGetValue(serviceType, out Action<object, IServiceProvider> action))
            {
                action(service, ServiceProvider);
                return;
            }

            var setExpressions = new List<Expression>();

            // 参数
            var objExpression = Expression.Parameter(typeof(object), "obj");
            var spExpression = Expression.Parameter(typeof(IServiceProvider), "sp");

            var obj = Expression.Convert(objExpression, serviceType);
            var serviceMethod = typeof(IServiceProvider).GetMethod(nameof(IServiceProvider.GetService));

            // 字段注入
            var fields = serviceType.GetAllFieldsWithoutStatic().Where(fi => fi.IsDefined<InjectionServiceAttribute>());
            foreach (var field in fields)
            {
                var fieldExpression = Expression.Field(obj, field);
                var getServiceExpression = Expression.Call(spExpression, serviceMethod, Expression.Constant(field.FieldType));
                var setExpression = Expression.Assign(fieldExpression, Expression.Convert(getServiceExpression, field.FieldType));
                setExpressions.Add(setExpression);
            }

            // 属性注入
            var properties = serviceType.GetAllPropertiesWithoutStatic().Where(pi => pi.IsDefined<InjectionServiceAttribute>());
            foreach (var property in properties)
            {
                var propExpression = Expression.Property(obj, property);
                var getServiceExpression = Expression.Call(spExpression, serviceMethod, Expression.Constant(property.PropertyType));
                var setExpression = Expression.Assign(propExpression, Expression.Convert(getServiceExpression, property.PropertyType));
                setExpressions.Add(setExpression);
            }

            var bodyExpression = Expression.Block(setExpressions);
            var injectAction = Expression.Lambda<Action<object, IServiceProvider>>(bodyExpression, objExpression, spExpression).Compile();

            _injectedActions[serviceType] = injectAction;
            injectAction.Invoke(service, ServiceProvider);
        }


        /// <summary>
        /// 释放资源。
        /// </summary>
        public override void Dispose()
        {
            _injectedActions.Clear();

            base.Dispose();
        }

    }
}
