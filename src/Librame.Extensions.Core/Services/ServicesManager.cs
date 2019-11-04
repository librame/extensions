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
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Core
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
