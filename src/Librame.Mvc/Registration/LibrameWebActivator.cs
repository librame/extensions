#region License

/* **************************************************************************************
 * Copyright (c) zwbwl All rights reserved.
 * 
 * http://51zwb.com
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Linq;
using System.Web.Mvc;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Librame.Mvc.LibrameWebActivator), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(Librame.Mvc.LibrameWebActivator), "Shutdown")]

namespace Librame.Mvc
{
    /// <summary>
    /// Librame Web 激活器。
    /// </summary>
    public static class LibrameWebActivator
    {
        /// <summary>
        /// 应用程序启动时由容器适配器整合依赖注入。
        /// </summary>
        public static void Start()
        {
            var containerAdapter = LibrameArchitecture.ContainerAdapter;

            FilterProviders.Providers.Remove(FilterProviders.Providers.OfType<FilterAttributeFilterProvider>().First());
            FilterProviders.Providers.Add(new ContainerFilterAttributeFilterProvider(containerAdapter));
            
            LibrameWebRegistration.SetResolver(containerAdapter);
            //DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            // TODO: Uncomment if you want to use PerRequestLifetimeManager
            // Microsoft.Web.Infrastructure.DynamicModuleHelper.DynamicModuleUtility.RegisterModule(typeof(UnityPerRequestHttpModule));
        }
        

        /// <summary>
        /// 当应用程序关闭时释放容器适配器资源。
        /// </summary>
        public static void Shutdown()
        {
            var containerAdapter = LibrameArchitecture.ContainerAdapter;

            containerAdapter.Dispose();
        }

    }
}
