#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame;
using Librame.Data;
using Librame.SensitiveWord;
using Librame.Utility;
using System.ComponentModel;

namespace System.Web.Mvc
{
    /// <summary>
    /// 敏感词过滤模型绑定程序。
    /// </summary>
    public class SensitiveWordsFilterModelBinder : DefaultModelBinder
    {
        private readonly ISensitiveWordsFilter _filter
            = LibrameArchitecture.AdapterManager.SensitiveWord.Filter;


        /// <summary>
        /// 设定属性。
        /// </summary>
        /// <param name="controllerContext">给定的控制器上下文。</param>
        /// <param name="bindingContext">给定的模型绑定上下文。</param>
        /// <param name="propertyDescriptor">给定的属性描述符。</param>
        /// <param name="value">给定的属性值。</param>
        protected override void SetProperty(ControllerContext controllerContext,
            ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor, object value)
        {
            if (propertyDescriptor.PropertyType == typeof(string))
                value = _filter.Filting(value as string);

            base.SetProperty(controllerContext, bindingContext, propertyDescriptor, value);
        }

        /// <summary>
        /// 绑定模型。
        /// </summary>
        /// <param name="controllerContext">给定的控制器上下文。</param>
        /// <param name="bindingContext">给定的绑定上下文。</param>
        /// <returns>返回模型对象。</returns>
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = base.BindModel(controllerContext, bindingContext);
            if (bindingContext.ModelType == typeof(string))
                value = _filter.Filting(value as string);

            return value;
        }


        /// <summary>
        /// 注册敏感词过滤模型绑定程序。
        /// </summary>
        /// <param name="modelBinder">给定的敏感词过滤模型绑定程序（可选）。</param>
        /// <returns>返回敏感词过滤模型绑定程序。</returns>
        public static SensitiveWordsFilterModelBinder Register(SensitiveWordsFilterModelBinder modelBinder = null)
        {
            if (modelBinder == null)
                modelBinder = new SensitiveWordsFilterModelBinder();
            
            var assemblies = DataHelper.GetMappingAssemblies(LibrameArchitecture.AdapterManager.DataSettings.AssemblyStrings);
            assemblies.Invoke(a =>
            {
                var types = TypeUtility.GetAssignableTypes<IEntityAutomapping>(a);

                types.Invoke(t =>
                {
                    ModelBinders.Binders.Add(t, modelBinder);
                });
            });
            
            return modelBinder;
        }

    }
}
