#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Core.Services
{
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class ServicesManager<TService, TDefault> : AbstractServicesManager<TService, TDefault>
        where TService : ISortableService
        where TDefault : TService
    {
        public ServicesManager(IEnumerable<TService> services)
            : base(services)
        {
        }
    }


    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class ServicesManager<TService> : AbstractServicesManager<TService>
        where TService : ISortableService
    {
        public ServicesManager(IEnumerable<TService> services)
            : base(services)
        {
        }
    }
}
