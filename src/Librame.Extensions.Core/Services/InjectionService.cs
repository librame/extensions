#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Librame.Extensions.Core
{
    class InjectionService : AbstractService, IInjectionService
    {
        Dictionary<Type, Action<object, IServiceProvider>> _injectedActions
            = new Dictionary<Type, Action<object, IServiceProvider>>();


        public InjectionService(IServiceProvider serviceProvider,
            ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            ServiceProvider = serviceProvider.NotNull(nameof(serviceProvider));
        }


        public IServiceProvider ServiceProvider { get; private set; }


        public void Inject(object service)
        {
            service.NotNull(nameof(service));

            var serviceType = service.GetType();

            if (_injectedActions.TryGetValue(serviceType, out Action<object, IServiceProvider> action))
            {
                action.Invoke(service, ServiceProvider);
                Logger.LogInformation($"Use cache action for resolve the service type: {serviceType}");

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


        public override void Dispose()
        {
            _injectedActions.Clear();

            base.Dispose();
        }

    }
}
