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
using Topshelf;
using Topshelf.HostConfigurators;
using Topshelf.ServiceConfigurators;

namespace Librame.WinService
{
    /// <summary>
    /// Windows 服务适配器。
    /// </summary>
    public class DefaultWinServiceAdapter : AbstractWinServiceAdapter, IWinServiceAdapter
    {
        /// <summary>
        /// 新增服务。
        /// </summary>
        /// <typeparam name="T">指定的服务控件类型。</typeparam>
        /// <param name="serviceName">给定的服务名称。</param>
        /// <param name="displayName">给定的显示名称。</param>
        /// <param name="description">给定的服务说明。</param>
        /// <param name="isAutomatic">是否自动启动。</param>
        /// <param name="action">自定义操作方法。</param>
        public virtual void AddService<T>(string serviceName, string displayName, string description,
            bool isAutomatic = false, Action<HostConfigurator> action = null)
            where T : class, ServiceControl, new()
        {
            Action<ServiceConfigurator<T>> callback = sc =>
            {
                // 配置一个完全定制的服务,对 Topshelf 没有依赖关系
                sc.ConstructUsing(() => new T());

                // the start and stop methods for the service
                sc.WhenStarted((s, hc) => s.Start(hc));
                sc.WhenStopped((s, hc) => s.Stop(hc));

                //// optional pause/continue methods if used
                //sc.WhenPaused((s, hc) => s.Pause());
                //sc.WhenContinued((s, hc) => s.Continue());

                //// optional, when shutdown is supported
                //sc.WhenShutdown((s, hc) => s.Shutdown());
            };
            
            HostFactory.New(x =>
            {
                x.Service(callback);
                
                // 服务使用 NETWORK_SERVICE 内置帐户运行。
                // 身份标识，有好几种方式
                x.RunAsLocalSystem();
                //x.RunAsNetworkService();
                //x.RunAsPrompt();
                //x.RunAs("username", "password");

                // 服务信息
                x.SetDescription(description);
                x.SetDisplayName(displayName);
                x.SetServiceName(serviceName);

                if (isAutomatic)
                    x.StartAutomatically(); // Start the service automatically

                action?.Invoke(x);

                //x.StartAutomaticallyDelayed(); // Automatic (Delayed) -- only available on .NET 4.0 or later
                //x.StartManually(); // Start the service manually
                //x.Disabled(); // install the service as disabled

                //x.EnableServiceRecovery(r =>
                //{
                //    //you can have up to three of these
                //    r.RestartComputer(5, "message");
                //    r.RestartService(0);

                //    //the last one will act for all subsequent failures
                //    r.RunProgram(7, "ping google.com");
                //    r.RunProgram(1, "notepad.exe");

                //    //should this be true for crashed or non-zero exits
                //    r.OnCrashOnly();

                //    //number of days until the error count resets
                //    r.SetResetPeriod(1);
                //});
            });
        }

    }
}
