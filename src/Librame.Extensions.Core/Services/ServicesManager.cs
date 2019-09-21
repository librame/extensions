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

namespace Librame.Extensions.Core
{
    class ServicesManager<TService, TDefault> : AbstractServicesManager<TService, TDefault>
        where TService : IService
        where TDefault : TService
    {
        public ServicesManager(IEnumerable<TService> services)
            : base(services)
        {
        }

    }


    class ServicesManager<TService> : AbstractServicesManager<TService>
        where TService : IService
    {
        public ServicesManager(IEnumerable<TService> services)
            : base(services)
        {
        }

    }

}
